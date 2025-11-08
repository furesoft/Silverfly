using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
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

    private readonly string[] _ignorePropNames = ["Tag", "Range", "Parent", "Properties"];

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

        IHasTreeNodes childNode;
        if (child is not LiteralNode and not Token and not NameNode)
        {
            childNode = node.AddNode(childType.Name);
        }
        else
        {
            childNode = node;
        }

        if (child is AstNode cnode)
        {
            foreach (var property in cnode.EnumerateProperties())
            {
                if (_ignorePropNames.Contains(property.Key))
                {
                    continue;
                }

                var propValue = property.Value;

                if (propValue is LiteralNode literal)
                {
                    childNode.AddNode($"{property.Key}={literal.Value}");
                    continue;
                }
                if (propValue is NameNode nameNode)
                {
                    childNode.AddNode($"{property.Key}={nameNode.Token}");
                    continue;
                }

                if (propValue is Token token)
                {
                    childNode.AddNode($"{property.Key} '{token}'");
                }
              /*  else if (propValue is IEnumerable enumerable)
                {
                    var itemNode = childNode.AddNode($"{property.Key}");
                    foreach (var item in enumerable)
                    {
                        BuildTreeChild(itemNode, item);
                    }
                }*/
            }

            foreach (var c in cnode.Children)
            {
                BuildTreeChild(childNode, c);
            }
        }
    }
}
