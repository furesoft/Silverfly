namespace Silverfly.Generator.Definition;

public class DefinitionGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Group("<", ">", "nonterminal");
        def.Prefix("_", tag: "keyword");

        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.EOL);
    }
}
