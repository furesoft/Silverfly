using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Silverfly.Nodes;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Silverfly.TreeVisualizer;

internal sealed class TreeCommand : Command<TreeCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("-p|--parser")]
        [Description("Specify the parser to use")]
        public string? Parser { get; set; }

        [Description("The source to parse")]
        [CommandArgument(0, "<source>")]
        public string Source { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var parserInstance = FindParser(settings);

        var parsed = parserInstance.Parse(settings.Source);

        parserInstance.PrintMessages();

        var root = new Tree(parsed.Tree.GetType().Name);

        BuildTree(root, parsed.Tree);

        AnsiConsole.Write(root);

        return 0;
    }

    private Parser FindParser(Settings settings)
    {
        foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);

                if (assembly == typeof(Parser).Assembly || assembly.FullName!.StartsWith("Microsoft"))
                {
                    continue;
                }

                var types = assembly.GetTypes();

                if (settings.Parser == null)
                {
                    foreach (var type in types)
                    {
                        if (type.IsSubclassOf(typeof(Parser)))
                        {
                            return (Parser)Activator.CreateInstance(type)!;
                        }
                    }

                    continue;
                }

                var parserType = assembly.GetTypes().ToList()
                    .FirstOrDefault(t => t.IsSubclassOf(typeof(Parser)) && t.FullName == settings.Parser);

                if (parserType == null)
                {
                    continue;
                }

                return (Parser)Activator.CreateInstance(parserType)!;
            }
            catch
            {
            }
        }

        AnsiConsole.WriteLine("Could not find parser");
        Environment.Exit(1);

        return null;
    }

    private string[] ignorePropNames = ["Tag", "Range", "Parent"];

    private void BuildTree(IHasTreeNodes node, AstNode parsedTree)
    {
        if (parsedTree is BlockNode block)
        {
            foreach (var child in block.Children)
            {
                BuildTreeChild(node, child);
            }
        }
        else
        {
            BuildTreeChild(node, parsedTree);
        }
    }

    private void BuildTreeChild(IHasTreeNodes node, object child)
    {
        var childType = child.GetType();
        var childNode = node.AddNode(childType.Name);

        foreach (var property in childType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (ignorePropNames.Contains(property.Name))
            {
                continue;
            }

            var propValue = property.GetValue(child);

            if (propValue is LiteralNode literal)
            {
                childNode.AddNode($"{property.Name}={literal.Value}");
                continue;
            }
            if (propValue is NameNode nameNode)
            {
                childNode.AddNode($"{property.Name}={nameNode.Token}");
                continue;
            }

            if (propValue is Token token)
            {
                childNode.AddNode($"{property.Name} '{token}'");
                continue;
            }
            else if (propValue is IEnumerable enumerable)
            {
                var itemNode = childNode.AddNode($"{property.Name}");
                foreach (var item in enumerable)
                {
                    BuildTreeChild(itemNode, item);
                }
            }

            if (propValue is AstNode childBlock)
            {
                BuildTreeChild(childNode, childBlock);
            }
        }
    }
}
