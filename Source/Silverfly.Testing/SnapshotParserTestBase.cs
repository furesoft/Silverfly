using System.Text;
using System.Threading.Tasks;
using Argon;
using Silverfly.Testing.Converter;
using VerifyTests;
using static VerifyTests.VerifierSettings;

namespace Silverfly.Testing;

/// <summary>
/// Base class for snapshot parser tests using a specific parser type <typeparamref name="TParser"/>.
/// </summary>
/// <typeparam name="TParser">The type of the parser being tested.</typeparam>
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

    public TParser Parser { get; } = new();
    public TranslationUnit Parse(string src) => Parser.Parse(src, _options.Filename);

    public Task Test(string source)
    {
        var parsed = Parse(source);

        object result = parsed.Tree;
        if (_options.OutputMode == OutputMode.Small)
        {
            var builder = new StringBuilder();
            PrintListener.Listener.Listen(builder, parsed.Tree);
            result = new TestResult(builder.ToString(), parsed.Document);
        }

        return VerifyNUnit.Verifier.Verify(result, Settings);
    }
}
