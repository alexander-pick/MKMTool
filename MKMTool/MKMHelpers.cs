/*
	This file is part of MKMTool

    MKMTool is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MKMTool is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    Fubar wird in der Hoffnung, dass es nützlich sein wird, aber
    OHNE JEDE GEWÄHRLEISTUNG, bereitgestellt; sogar ohne die implizite
    Gewährleistung der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Details.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MKMTool
{
    public static class MKMHelpers
    {
        // My origin country (to find domnestic deals)
        public const string sMyOwnCountry = "D";

        // treashold price - bulk cards tend to be 0.02 but I hate shipping them at this price, 
        // some people will order 100+ cards for 2 cts making this incredibly tiresome
        public const float fAbsoluteMinPrice = 0.05F;

        private static DataTable dt = new DataTable();

        public static Dictionary<string, string> dLanguages = new Dictionary<string, string>
        {
            {
                "",
                "All"
            }
            ,
            {
                "1",
                "English"
            }
            ,
            {
                "2",
                "French"
            }
            ,
            {
                "3",
                "German"
            }
            ,
            {
                "4",
                "Spanish"
            }
            ,
            {
                "5",
                "Italian"
            },
            {
                "6",
                "Simplified Chinese"
            }
            ,
            {
                "7",
                "Japanese"
            }
            ,
            {
                "8",
                "Portuguese"
            }
            ,
            {
                "9",
                "Russian"
            }
            ,
            {
                "10",
                "Korean"
            }
            ,
            {
                "11",
                "Traditional Chinese"
            }
        };

        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        // 
        // http://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            using (var sr = new StreamReader(strFilePath))
            {
                var headers = sr.ReadLine().Split(',');

                foreach (var header in headers)
                {
                    dt.Columns.Add(header.Replace("\"", ""));
                }

                while (!sr.EndOfStream)
                {
                    var rows = sr.ReadLine().Split(',');
                    var dr = dt.NewRow();

                    for (var i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].Replace("\"", "");
                    }

                    dt.Rows.Add(dr);
                }
            }

            dt = dt.Select("[Category ID] = '1'").CopyToDataTable(); // grab only MTG Singles

            return dt;
        }

        // Reference:
        // http://stackoverflow.com/questions/665754/inner-join-of-datatables-in-c-sharp

        public static DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            var result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2)) return false;
                    }
                    return true;
                });
                foreach (var fromRow in joinRows)
                {
                    var insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                    {
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    }
                    foreach (DataColumn col2 in t2.Columns)
                    {
                        insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    }
                    result.Rows.Add(insertRow);
                }
            }
            return result;
        }

        public static byte[] gzDecompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (var stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    var count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    } while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}