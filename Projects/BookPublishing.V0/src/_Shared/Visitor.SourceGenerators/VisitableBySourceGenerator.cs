using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Visitor.SourceGenerators
{
    [Generator]
    public class VisitableBySourceGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace Visitor.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VisitableByAttribute<T> : Attribute
    {
    }
}
";
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("VisitableByAttribute.g.cs", SourceText.From(attributeText, Encoding.UTF8));
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
#endif 
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
                var classNodes = syntaxTree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>();
                foreach (var classNode in classNodes)
                {
                    var classSymbol = semanticModel.GetDeclaredSymbol(classNode);
                    
                    var visitableByAttribute = classSymbol
                        .GetAttributes()
                        .FirstOrDefault(x => x.AttributeClass?.Name == "VisitableBy");
                    if (visitableByAttribute?.AttributeClass is null)
                        continue;
                    var attributeClass = visitableByAttribute.AttributeClass;
                    if (!attributeClass.IsGenericType ||
                        attributeClass.TypeArguments.Length != 1)
                        continue;
                    var visitorType = attributeClass.TypeArguments[0].Name;
                    var classType = classSymbol.Name;
                    string newClassCode =
$$"""
using BookWriting.Domain.Visitors;
namespace BookWriting.Domain.Elements
{
    public partial class {{classType}}
    {
        public void Accept({{visitorType}} visitor)
        {
            visitor.Visit{{classType}}(this);
        }
    }
}
""";
                    context.AddSource($"{classType}.g.cs", 
                        SourceText.From(newClassCode, Encoding.UTF8));

                }

            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
