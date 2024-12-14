using Spectre.Console.Cli;

namespace Silverfly.TreeVisualizer;

public class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp<TreeCommand>();
        return app.Run(args);
    }
}
