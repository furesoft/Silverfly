using Sample.JSON.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.JSON.Parselets;

public class ObjectParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        // Erstellt eine leere Map für die Schlüssel-Wert-Paare
        var objectMembers = new Dictionary<string, AstNode>();

        // Schleife über die Objektmitglieder
        while (!parser.Match("}"))
        {
            var keyToken = parser.Consume(PredefinedSymbols.String);
            var key = keyToken.Text.ToString();
            
            parser.Consume(":");
            
            var value = parser.ParseExpression();
            
            objectMembers[key] = value;
            
            if (!parser.Match(","))
            {
                break;
            }
        }
        
        parser.Consume("}");
        
        return new JsonObject(objectMembers);
    }
}

