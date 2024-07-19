using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

public class AnnotationParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        //@name
        //@name()
        //@name(1)
        var call = parser.ParseExpression();

        if (call is NameNode name)
        {
            call = new CallNode(call, []);
        }

        var expr = parser.ParseExpression();
        if (expr is AnnotatedNode a)
        {
            a.Annotations = a.Annotations.Add((CallNode)call);
        }
        else
        {
            expr.AddMessage("Annotation cannot be used for " + expr.GetType());
        }

        return expr;
    }
}
