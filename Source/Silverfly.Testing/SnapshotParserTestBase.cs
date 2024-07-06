using Argon;
using Silverfly.Testing.Converter;
using static VerifyTests.VerifierSettings;

namespace Silverfly.Testing;

public class SnapshotParserTestBase<TParser>
    where TParser : Parser, new()
{
    public static readonly VerifySettings Settings = new();
    private static TestOptions _options;

    public static void Init(TestOptions options)
    {
        _options = options;

        AddExtraSettings(_ =>
        {
            _.Converters.Add(new SymbolConverter());
            _.Converters.Add(new DocumentConverter());
            _.Converters.Add(new RangeConverter());
            _.Converters.Add(new ReadonlyMemoryConverter());

            _.TypeNameHandling = TypeNameHandling.All;
        });

        Settings.UseDirectory("TestResults");
    }

    public static TranslationUnit Parse(string src) => Parser.Parse<TParser>(src,
            useStatementsAtToplevel: _options.UseStatementsAtToplevel,
            filename: _options.Filename);

    public static Task Test(string source)
    {
        var parsed = Parse(source);

        object result = parsed.Tree;
        if (_options.OutputMode == OutputMode.Small)
        {
            result = new TestResult(parsed.Tree.Accept(new PrintVisitor()), parsed.Document);
        }

        return Verify(result, Settings);
    }
}
