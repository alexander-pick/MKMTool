using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static MKMTool.MKMHelpers;

namespace MKMTool
{
    /// <summary>
    /// Helper class for processing CSV files.
    /// </summary>
    class MKMCsvUtils
    {

        /// <summary>
        /// Parses a row in a CSV file.
        /// </summary>
        /// <param name="rowToParse">The row from a CSV file. Each value can be enclosed by double quotes (i.e. the character ")
        /// and if a double quote is part of the value, it is escaped by another double quote.
        /// If a field contains an even number of double quotes and nothing else, it will be considered as not enclosed, so a field that contains """""" will
        /// be parsed as """. The only exception is when there are exactly two double quotes, in that case it will be considered an enclosed empty string.
        /// This is done as in practice, excel and similar might enclose the empty fields and the user probably wants to keep them empty.</param>
        /// <param name="separator">The character used as separator between columns.</param>
        /// <returns>A list of the individual parsed values in the order they appear on the row. All enclosing quotes are trimmed
        /// from the value and all escape characters are removed.</returns>
        private static List<string> parseCSVRow(string rowToParse, char separator)
        {
            List<string> ret = new List<string>();
            string[] split = rowToParse.Split(separator);

            for (int i = 0; i < split.Length; i++)
            {
                string columnValue = split[i];
                // we need to account for entires that have commas in their own name (so the split will split them among multiple 
                // columns, even though they should be in one) and also for entries that can contain quotes.
                // MKM escapes the double quotes by another double quote, so the ending double quote is really ending only if it is not preceded by
                // something else.
                // So far worst case scenario is a card from the Force of Will game called: "I", the pilot. This in the database looks like this:
                // "304732","""I"", the Pilot","1018","Force of Will Single","1775","229401","2017-10-04 17:48:59"
                // so it has a comma, double quote precedes it, so it looks like the entry should end there, but it does not, it is in the middle of the name.
                // in general, double quote is to be considered opening/ending only if there is an odd number of them, otherwise they are escaped
                bool quoteEnclosed = false;
                for (int j = 0; j < columnValue.Length; j++)
                {
                    if (columnValue[j] == '"')
                        quoteEnclosed = !quoteEnclosed;
                    else break;
                }
                if (quoteEnclosed) // starts by a double quote -> can contain comma itself, merge until the last double quote is found
                {
                    while (true)
                    {
                        quoteEnclosed = false;
                        for (int j = columnValue.Length - 1; j >= 0; j--) // check if it ends with an odd number of double quotes
                        {
                            if (columnValue[j] == '"')
                                quoteEnclosed = !quoteEnclosed;
                            else break;
                        }
                        if (!quoteEnclosed) // the closing quote was not found yet
                            columnValue += separator + split[++i]; // append the next column value
                        else break;
                    }
                }
                if (quoteEnclosed)
                    columnValue = columnValue.Substring(1, columnValue.Length - 2);
                columnValue = columnValue.Replace("\"\"", "\""); // un-escape double quotes
                // let's handle one corner case: if somebody is exporting a list from excel and says "enclose each field in double quotes",
                // empty fields will have the value "". Since that is an even number of double quotes, our algorithm will evaluate it as 
                // not being quoteEnclosed even though it actually is an enclosed empty string. After the above replacement, we will now
                // have a string that is a single double quote. If that is the case, replace it with actual empty string.
                // Note that this still does not handle all corner cases, in general, if there is an even number X of double quotes in the field and no other text,
                // it can either be X/2 not-enclosed double quotes, or (X-1)/2 enclosed double quotes, there is no way to tell since we are allowing
                // mixed format (i.e. enclosed and unenclosed in the same file). Hopefully this never has any practical impact.
                if (!quoteEnclosed && columnValue == "\"")
                    columnValue = "";
                ret.Add(columnValue);
            }
            return ret;
        }

        /// <summary>
        /// Writes the table as CSV.
        /// </summary>
        /// <param name="filePath">Path to the file as which to write the table.</param>
        /// <param name="dt">The data table to write.</param>
        public static void WriteTableAsCSV(string filePath, DataTable dt)
        {
            try
            {
                using (StreamWriter exp = new StreamWriter(filePath))
                {
                    // we know there will be at least one column, otherwise there would be no valid imported items and therefore no export enabled
                    string row = "\"" + (dt.Columns[0].ColumnName).Replace("\"", "\"\"") + "\""; // don't forget to escape all " by doubling them
                    for (int i = 1; i < dt.Columns.Count; i++)
                        row += ",\"" + (dt.Columns[i].ColumnName).Replace("\"", "\"\"") + "\"";
                    exp.WriteLine(row);
                    foreach (DataRow card in dt.Rows)
                    {
                        row = "\"" + card[0].ToString().Replace("\"", "\"\"") + "\"";
                        for (int i = 1; i < dt.Columns.Count; i++)
                            row += ",\"" + card[i].ToString().Replace("\"", "\"\"") + "\"";
                        exp.WriteLine(row);
                    }
                }
            }
            catch (Exception eError)
            {
                LogError("writing CSV file " + filePath, eError.Message, true);
            }
        }

        /// <summary>
        /// Converts the csv to DataTable. <seealso cref="ConvertCSVtoDataTable(StreamReader sr)"/>
        /// </summary>
        /// <param name="strFilePath">The string file path.</param>
        /// <returns>Each row of the file as a row in the returned DataTable.</returns>
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            using (var sr = new StreamReader(strFilePath))
            {
                return ConvertCSVtoDataTable(sr);
            }
        }

        /// <summary>
        /// Converts the csv to DataTable. <seealso cref="ConvertCSVtoDataTable(StreamReader sr)"/>
        /// </summary>
        /// <param name="data">Raw data containing the csv file.</param>
        /// <returns>Each row of the file as a row in the returned DataTable.</returns>
        public static DataTable ConvertCSVtoDataTable(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            using (var sr = new StreamReader(ms))
            {
                return ConvertCSVtoDataTable(sr);
            }
        }

        /// <summary>
        /// Converts a CSV file to a DataTable.
        /// http://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable     
        /// </summary>
        /// <param name="sr">Stream with the CSV file. It is assumed that the file has a header on the first line with names of the columns.
        /// See parseCSVRow on details on the format of the CSV.</param>
        /// <returns>Each row of the file as a row in the returned DataTable.</returns>
        /// <exception cref="FormatException">
        /// Wrong format of the header of CSV file " + strFilePath + ": " + eError.Message
        /// or
        /// Wrong format of the CSV file on row " + (dt.Rows.Count + 1) + ": " + eError.Message
        /// </exception>
        public static DataTable ConvertCSVtoDataTable(StreamReader sr)
        {
            DataTable dt = new DataTable();
            char separator = ',';
            try
            {
                // detect the separator - this assumes it's ether semicolon or comma and that semicolon cannot be part of column names
                string firstLine = sr.ReadLine();
                if (firstLine.Contains(';'))
                    separator = ';';
                List<string> headers = parseCSVRow(firstLine, separator);
                foreach (string header in headers)
                    dt.Columns.Add(header);
            }
            catch (Exception eError)
            {
                throw new FormatException("Wrong format of the header of CSV file: " + eError.Message);
            }
            while (!sr.EndOfStream)
            {
                try
                {
                    List<string> row = parseCSVRow(sr.ReadLine(), separator);
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.Count; i++)
                        dr[i] = row[i];
                    dt.Rows.Add(dr);
                }
                catch (Exception eError)
                {
                    // technically it is the (dt.Rows.Count + 1)th row, but in the file the first row is the header so this should
                    // give the user the number of the row in the actual file
                    throw new FormatException("Wrong format of the CSV file on row " + (dt.Rows.Count + 2) + ": " + eError.Message);
                }
            }
            return dt;
        }
    }
}
