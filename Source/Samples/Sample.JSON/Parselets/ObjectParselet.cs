using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.JSON.Nodes;

namespace Silverfly.Sample.JSON.Parselets;

public class ObjectParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var objectMembers = new Dictionary<string, AstNode>();
        
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
