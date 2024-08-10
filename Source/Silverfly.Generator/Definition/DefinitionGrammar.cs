using Silverfly.Parselets;
using Silverfly.Parselets.Literals;

namespace Silverfly.Generator.Definition;

public class DefinitionGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchString("'", "'");
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Group("<", ">", "nonterminal");
        def.Prefix("_", tag: "keyword");

        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.EOL);

        def.Register(PredefinedSymbols.Name, new NameParselet());
        def.Register(PredefinedSymbols.String, new StringLiteralParselet());

        def.InfixLeft(":", "Sum");
    }
}
