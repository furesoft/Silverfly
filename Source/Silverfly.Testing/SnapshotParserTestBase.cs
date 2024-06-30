using Argon;
using Silverfly.Testing.Converter;
using static VerifyTests.VerifierSettings;

namespace Silverfly.Testing;

public class SnapshotParserTestBase<TParser>
    where TParser : Parser, new()
{
    private static readonly VerifySettings _settings = new();
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

        _settings.UseDirectory("TestResults");
    }

    public static Task Test(string source)
    {
        var parsed = Parser.Parse<TParser>(source,
            useStatementsAtToplevel: _options.UseStatementsAtToplevel,
            filename: _options.Filename);

        var result = new TestResult(parsed.Tree.Accept(new PrintVisitor()), parsed.Document);

        return Verify(result, _settings);
    }
}
