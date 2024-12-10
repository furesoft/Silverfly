using Sample.Brainfuck.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.Brainfuck.Parselets;

public class OperationParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        AstNode node = default!;
        if (token.Type == ".")
        {
            node = new PrintNode(token);
        }
        else if (token.Type == ",")
        {
            node = new ReadNode(token);
        }
        else if (token.Type == ">")
        {
            node = new IncrementNode(token);
        }
        else if (token.Type == "<")
        {
            node = new DecrementNode(token);
        }
        else if (token.Type == "+")
        {
            node = new IncrementCellNode(token);
        }
        else if (token.Type == "-")
        {
            node = new DecrementCellNode(token);
        }

        return node.WithRange(token);
    }
}
