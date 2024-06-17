
using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Builder;
using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser;

public partial class Parser
{
    /// <summary>Add a dynamic parselet to parse an expression based on a declarative definition</summary>
    public void ExprBuilder<TNode>(SyntaxElement definition)
        where TNode : AstNode
    {
        Builder<TNode>(definition, NodeType.Expression);
    }

    /// <summary>Add a dynamic parselet to parse a statement based on a declarative definition</summary>
    public void StmtBuilder<TNode>(SyntaxElement definition)
        where TNode : AstNode
    {
        Builder<TNode>(definition, NodeType.Statement);
    }

    private static void GetKeywordsFromDefinition(SyntaxElement definition, List<string> result)
    {
        if (definition is KeywordElement kw)
        {
            result.Add(kw.Keyword);

            return;
        }

        if (definition is BinaryElement and)
        {
            GetKeywordsFromDefinition(and.First, result);
            GetKeywordsFromDefinition(and.Second, result);
        }
    }

    private static bool TryMatchUnaryOperatorKind<TFirst, TSecond>(SyntaxElement definition, out TFirst first, out TSecond second)
    {
        if (definition is AndElement andElement)
        {
            if (andElement.First is TFirst firstElement && andElement.Second is TSecond secondElement)
            {
                first = firstElement;
                second = secondElement;

                return true;
            }
        }

        first = default;
        second = default;

        return false;
    }

    //ToDo: add ability to define infix operators and ternary operators
    protected void Operator(SyntaxElement definition, string precedenceName = null)
    {
        if (TryMatchUnaryOperatorKind<KeywordElement, ExprElement>(definition, out var prefixKw, out var prefixExpr))
        {
            _lexer.AddSymbol(prefixKw.Keyword);
            Prefix(prefixKw.Keyword, precedenceName ?? "Prefix");
        }
        else if (TryMatchUnaryOperatorKind<ExprElement, KeywordElement>(definition, out var postfixExpr, out var postfixKw))
        {
            _lexer.AddSymbol(postfixKw.Keyword);
            Postfix(postfixKw.Keyword, precedenceName ?? "Postfix");
        }
    }

    private void Builder<TNode>(SyntaxElement definition, NodeType type)
        where TNode : AstNode
    {
        AddSymbolsFromBuilderToLexer(definition);

        var parselet = new BuilderParselet<TNode>(BindingPowers.Get("Product"), definition); //Todo: enable custom binding power

        if (definition is AndElement andElement && andElement.First is KeywordElement kw && andElement.Second is ExprElement)
        {
            Prefix(kw.Keyword);
        }

        var recognitionKeywords = new List<string>();
        GetRecognitionKeywords(definition, recognitionKeywords);

        foreach (var keyword in recognitionKeywords)
        {
            switch (type)
            {
                default:
                case NodeType.Expression:
                    Register(keyword, (IPrefixParselet)parselet);
                    break;
                case NodeType.Statement:
                    Register(keyword, (IStatementParselet)parselet);
                    break;
            }
        }
    }

    private void AddSymbolsFromBuilderToLexer(SyntaxElement definition)
    {
        var allSymbols = new List<string>();
        GetKeywordsFromDefinition(definition, allSymbols);

        _lexer.AddSymbols([.. allSymbols]);
    }

    static void GetRecognitionKeywords(SyntaxElement element, List<string> keywords)
    {
        if (element is KeywordElement keywordElement)
        {
            keywords.Add(keywordElement.Keyword);
        }

        if (element is AndElement and)
        {
            GetRecognitionKeywords(and.First, keywords);
        }
        else if (element is OrElement or)
        {
            GetRecognitionKeywords(or.First, keywords);
            GetRecognitionKeywords(or.Second, keywords);
        }
    }
}