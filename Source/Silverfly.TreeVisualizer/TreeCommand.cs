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
        [Description("Path to the assembly containing the parsers")]
        [CommandArgument(0, "<assembly>")]
        public string? AssemblyPath { get; set; }

        [CommandOption("-p|--parser")]
        [Description("Specify the parser to use")]
        public string? Parser { get; set; }

        [Description("The source to parse")]
        [CommandOption("-s|--source")]
        public string Source { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var parserAssembly = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, settings.AssemblyPath));
        var parserType = parserAssembly.GetType(settings.Parser);
        var parserInstance = (Parser)Activator.CreateInstance(parserType);

        var parsed = parserInstance.Parse(settings.Source);

        parserInstance.PrintMessages();

        var root = new Tree(parsed.Tree.GetType().Name);

        BuildTree(root, parsed.Tree);

        AnsiConsole.Write(root);

        return 0;
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
                childNode.AddNode($"{property.Name}={token}");
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
