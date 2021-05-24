
using System.Collections.Generic;
using System.Data;

namespace MKMTool
{
  // Defines price as a formula (string) made of any number of operands and operations.
  // Supported operands are: numbers (double), price guides (codes according to MKMHelpers.PriceGuides)
  // Supported operations are: * for multiplication, + for addition and - for subtraction.
  // Any white spaces are stripped from the formula before processing.
  // Operations are evaluated in the order they are written, not based on classic operator precedence.
  public class MKMPriceAsFormula
  {
    /// Sets this from string.
    /// <param name="formula">The formula as string.</param>
    /// <returns>True if parsing finished successfully, false if some of the operands were neither numbers nor price guides.</returns>
    public bool Parse(string formula)
    {
      string formulaOrig = formula;
      formula = formula.Trim();
      var separators = new char[] { '*', '-', '+' };
      // parse the first operand, there must be at least one and the formula cannot start with an operand
      string operand = formula.Split(separators, 2)[0];
      if (!parseAndAddOperand(operand.Trim()))
      {
        MKMHelpers.LogError("parsing price formula", "failed to parse operand " + operand + " in formula " + formulaOrig
          + ", it cannot be used.", true);
        return false;
      }
      formula = formula.Substring(operand.Length);
      while (formula.Length > 0)
      {
        char oper = formula[0]; // the next symbol is surely the operand
        switch (oper)
        {
          case '*':
            operators.Add(mult);
            break;
          case '-':
            operators.Add(subtract);
            break;
          case '+':
            operators.Add(adding);
            break;
        }
        formula = formula.Substring(1);
        // there must be another operand after operator, otherwise it is an error
        operand = formula.Split(separators, 2)[0];
        if (!parseAndAddOperand(operand.Trim()))
        {
          MKMHelpers.LogError("parsing price formula", "failed to parse operand " + operand + " in formula " + formulaOrig
            + ", it cannot be used.", false);
          return false;
        }
        formula = formula.Substring(operand.Length);
        formula.Trim();
      }
      return true;
    }

    /// Uses the provided price guides to compute the price according to the formula.
    /// <param name="priceGuides">Row from the priceGuides table containing guides for the card for which to evaluate this formula.
    /// If this formula UsesPriceGuides, must be set to contain all the columns, otherwise exception is thrown.</param>
    /// <param name="currentPrice">Our current sale price of the card (can be used as a guide).</param>
    /// <returns>The final price this formula evaluates to. NaN if the necessary guides are not found.</returns>
    public double Evaluate(DataRow priceGuides, double currentPriceSingle)
    {
      double term;
      foreach (var guideTerm in guidesToResolve)
      {
        if (guideTerm.Value == "CURRENT")
          term = currentPriceSingle;
        else
          term = MKMHelpers.ConvertDoubleAnySep(priceGuides[guideTerm.Value].ToString());
        if (double.IsNaN(term))
          return term;
        operands[guideTerm.Key] = term;
      }
      double val = operands[0];
      for (int i = 0; i < operators.Count; i++)
      {
        val = operators[i](val, operands[i + 1]);
      }
      return val;
    }

    /// Determines if this formula uses price guides.
    /// <returns>True if at least one operand is a price guide, otherwise false.</returns>
    public bool UsesPriceGuides()
    {
      return guidesToResolve.Count > 0;
    }

    /// Converts the operand to number or price guide and stores it
    /// <param name="operand">Trimmed string representation of the operand.</param>
    /// <returns>False if the parsing failed.</returns>
    private bool parseAndAddOperand(string operand)
    {
      double number = MKMHelpers.ConvertDoubleAnySep(operand);
      if (double.IsNaN(number))
      {
        foreach (var guide in MKMHelpers.PriceGuides)
        {
          if (guide.Value.Code == operand)
          {
            guidesToResolve.Add(new KeyValuePair<int, string>(operands.Count, operand));
            operands.Add(-9999);
            return true;
          }
        }
      }
      else
      {
        operands.Add(number);
        return true;
      }
      return false;
    }

    private delegate double formulaOperation(double lhs, double rhs);

    private double adding(double lhs, double rhs) { return lhs + rhs; }
    private double subtract(double lhs, double rhs) { return lhs - rhs; }
    private double mult(double lhs, double rhs) { return lhs * rhs; }

    // the indices of operands in the formula that are guide codes and need to be resolved
    private readonly List<KeyValuePair<int, string>> guidesToResolve = new List<KeyValuePair<int, string>>();
    private readonly List<double> operands = new List<double>();
    private readonly List<formulaOperation> operators = new List<formulaOperation>();
  }
}
