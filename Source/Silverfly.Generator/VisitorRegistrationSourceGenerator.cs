using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Silverfly.Generator;

[Generator]
public class VisitorRegistrationSourceGenerator : IIncrementalGenerator
{
    private const string Namespace = "Silverfly.Generator";
    private const string VisitorAttributeName = "VisitorAttribute";
    private const string VisitorIgnoreAttributeName = "VisitorIgnoreAttribute";
    private const string VisitorConditionAttributeName = "VisitorConditionAttribute";

    private const string VisitorAttributeSourceCode = $@"// <auto-generated/>

namespace {Namespace};

[System.AttributeUsage(System.AttributeTargets.Class)]
internal class {VisitorAttributeName} : System.Attribute
{{
}}
";
    
    private const string VisitorIgnoreAttributeSourceCode = $@"// <auto-generated/>

namespace {Namespace};

[System.AttributeUsage(System.AttributeTargets.Method)]
internal class {VisitorIgnoreAttributeName} : System.Attribute
{{
}}
";
    
    private const string VisitorConditionAttributeSourceCode = $@"// <auto-generated/>

namespace {Namespace};

[System.AttributeUsage(System.AttributeTargets.Method)]
internal class {VisitorConditionAttributeName}(string condition) : System.Attribute
{{
    public string Condition {{ get; }} = condition;
}}
";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation.
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "VisitorAttribute.g.cs",
            SourceText.From(VisitorAttributeSourceCode, Encoding.UTF8)));
        
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "VisitorIgnoreAttribute.g.cs",
            SourceText.From(VisitorIgnoreAttributeSourceCode, Encoding.UTF8)));
        
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "VisitorConditioneAttribute.g.cs",
            SourceText.From(VisitorConditionAttributeSourceCode, Encoding.UTF8)));
        
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => s is ClassDeclarationSyntax,
                (ctx, _) => GetClassDeclarationForSourceGen(ctx))
            .Where(t => t.reportAttributeFound)
            .Select((t, _) => t.Item1);

        // Generate the source code.
        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            ((ctx, t) => GenerateCode(ctx, t.Left, t.Right)));
    }
    
    private static (ClassDeclarationSyntax, bool reportAttributeFound) GetClassDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // Go through all attributes of the class.
        foreach (var attributeSyntax in classDeclarationSyntax.AttributeLists.SelectMany(attributeListSyntax => attributeListSyntax.Attributes))
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the [Report] attribute.
            if (attributeName == $"{Namespace}.{VisitorAttributeName}")
                return (classDeclarationSyntax, true);
        }

        return (classDeclarationSyntax, false);
    }
    
    private void GenerateCode(SourceProductionContext context, Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classDeclarations)
    {
        // Go through all filtered class declarations.
        foreach (var classDeclarationSyntax in classDeclarations)
        {
            // We need to get semantic model of the class to retrieve metadata.
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            // Symbols allow us to get the compile-time information.
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
                continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // 'Identifier' means the token of the node. Get class name from the syntax node.
            var className = classDeclarationSyntax.Identifier.Text;

            // Go through all class members with a particular type (property) to generate method lines.
            string GenerateFor(IMethodSymbol p)
            {
                var conditionAttribute = p.GetAttributes()
                    .FirstOrDefault(a => a.AttributeClass.Name == VisitorConditionAttributeName);

                var call = $"        For<{p.Parameters.First().Type.ToDisplayString()}>({p.Name}";
                
                if (conditionAttribute != null)
                {
                    return $"{call}, _ => {conditionAttribute.ConstructorArguments[0].Value.ToString().Replace('\'', '"')});";
                }
                
                
                return $"{call});";
            }

            var methodBody = string.Join('\n', classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.Name != ".ctor" && m.Name != ".cctor")
                .Where(m => !m.IsAbstract || !m.IsVirtual)
                .Where(m => IsVisitorMethod(context, m, classSymbol, classDeclarationSyntax))
                .Select(GenerateFor));

            // Build up the source code
            var code = $@"// <auto-generated/>

using System;
using System.Collections.Generic;

namespace {namespaceName};

public partial class {className}
{{
    public {className}()
    {{
{methodBody}
    }}
}}
";
            
            context.AddSource($"{className}.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }

    private bool IsVisitorMethod(SourceProductionContext context, IMethodSymbol method, INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDeclarationSyntax)
    {
        // Check if the class inherits from TaggedNodeVisitor
        var baseType = Utils.InheritsFrom(classSymbol, "NodeVisitorBase");

        if (baseType == null) {
            return false; // Not inheriting from NodeVisitorBase
        }
        
        foreach (var attribute in method.GetAttributes())
        {
            if (attribute?.AttributeClass?.Name == VisitorIgnoreAttributeName)
                return false;
        }

        if (method.IsAbstract || method.IsOverride)
        {
            return false;
        }

        return true;
    }
}
