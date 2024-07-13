using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;

namespace Silverfly.Generator;

[Generator]
public class VisitorRegistrationGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var attributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(VisitorAttribute).FullName);

        var classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any()));

        foreach (SyntaxTree tree in classWithAttributes)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);

            foreach (var declaredClass in tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(cd => cd.DescendantNodes().OfType<AttributeSyntax>().Any()))
            {
                var nodes = declaredClass
                .DescendantNodes()
                .OfType<AttributeSyntax>()
                .FirstOrDefault(a => a.DescendantTokens().Any(dt => dt.IsKind(SyntaxKind.IdentifierToken) && semanticModel.GetTypeInfo(dt.Parent).Type.Name == attributeSymbol.Name))
                ?.DescendantTokens()
                ?.Where(dt => dt.IsKind(SyntaxKind.IdentifierToken))
                ?.ToList();

                if (nodes == null)
                {
                    continue;
                }

                var relatedClass = semanticModel.GetTypeInfo(nodes.Last().Parent);

                var generatedClass = GenerateClass(relatedClass);

                foreach (MethodDeclarationSyntax classMethod in declaredClass.Members.Where(m => m.IsKind(SyntaxKind.MethodDeclaration)).OfType<MethodDeclarationSyntax>())
                {
                    GenerateMethod(declaredClass.Identifier, relatedClass, classMethod, ref generatedClass);
                }

                CloseClass(generatedClass);

                context.AddSource($"{declaredClass.Identifier}_{relatedClass.Type.Name}.g.cs", SourceText.From(generatedClass.ToString(), Encoding.UTF8));
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to do here
    }

    private void GenerateMethod(SyntaxToken moduleName, TypeInfo relatedClass, MethodDeclarationSyntax methodDeclaration, ref StringBuilder builder)
    {
        var signature = $"{methodDeclaration.Modifiers} {relatedClass.Type.Name} {methodDeclaration.Identifier}(";

        var parameters = methodDeclaration.ParameterList.Parameters.Skip(1);

        signature += string.Join(", ", parameters.Select(p => p.ToString())) + ")";

        var methodCall = $"return this._wrapper.{moduleName}.{methodDeclaration.Identifier}(this, {string.Join(", ", parameters.Select(p => p.Identifier.ToString()))});";

        builder.AppendLine(@"
        " + signature + @"
        {
            " + methodCall + @"
        }");
    }

    private StringBuilder GenerateClass(TypeInfo relatedClass)
    {
        var sb = new StringBuilder();

        sb.Append($@"
using System;

namespace {relatedClass.Type.ContainingNamespace};

    public partial class {relatedClass.Type.Name}");

        sb.Append(@"
    {");

        return sb;
    }
    private void CloseClass(StringBuilder generatedClass)
    {
        generatedClass.Append('}');
    }
}