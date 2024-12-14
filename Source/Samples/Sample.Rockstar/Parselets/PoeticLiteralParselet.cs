using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Parselets;

namespace Silverfly.Sample.Rockstar.Parselets;

public class PoeticLiteralParselet : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        AstNode value;
        if (parser.IsMatch(PredefinedSymbols.Name))
        {
            List<string> tmp = [];
            while (!parser.Match(PredefinedSymbols.EOL, PredefinedSymbols.EOF))
            {
                tmp.AddRange(parser.Consume().Text.ToString().Split(' '));
            }

            var numValue = ConvertPoeticNumber(tmp);

            value = new LiteralNode(numValue, token);
        }
        else
        {
            value = parser.ParseExpression();
        }

        return new BinaryOperatorNode(left, token.Rewrite("="), value);
    }

    public int GetBindingPower()
    {
        return 100;
    }

    private static double ConvertPoeticNumber(List<string> tmp)
    {
        var numValue = 0.0;
        var decimalPointEncountered = false;
        var decimalMultiplier = 0.1;

        // Iterate over words after the variable name and 'is/was/are/were'
        for (var i = 0; i < tmp.Count; i++)
        {
            var word = tmp[i];

            // Remove non-alphabetical characters
            var cleanedWord = new string(word.Where(char.IsLetter).ToArray());

            if (cleanedWord.Length > 0)
            {
                // Handle the period (decimal point)
                if (word.Contains('.'))
                {
                    decimalPointEncountered = true;
                    continue;
                }

                // Calculate the digit
                var digit = cleanedWord.Length % 10;

                // Append the digit to the number
                if (decimalPointEncountered)
                {
                    numValue += digit * decimalMultiplier;
                    decimalMultiplier *= 0.1;
                }
                else
                {
                    numValue = (numValue * 10) + digit;
                }
            }
        }

        return numValue;
    }
}
