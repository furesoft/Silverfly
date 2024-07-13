using Sample.FuncLanguage.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.FuncLanguage.Parselets;

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

        return expr;
    }
}