using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Acorisoft.Platform.Generators
{
    internal class ViewModelParingWalker : CSharpSyntaxWalker
    {
        private readonly List<string> _paring;

        public ViewModelParingWalker()
        {
            _paring = new List<string>();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            foreach (var attr in  node.AttributeLists)
            {
                var paring = attr.Attributes.FirstOrDefault(x => x.Name.ToFullString() == "ViewModelParing");
                if (paring?.ArgumentList == null)
                {
                    continue;
                }

                var args = paring.ArgumentList.Arguments;
                if (args.Count != 2)
                {
                    continue;
                }

                var vmNameRawStr = args[0].Expression.ToFullString();
                var vNameRawStr = args[1].Expression.ToFullString();

                //
                // 注册
                _paring.Add(
                    string.Format("container.Register<IViewFor<{0}>,{1}>();\n",
                    vmNameRawStr.Substring(7, vmNameRawStr.Length - 8),
                    vNameRawStr.Substring(7, vNameRawStr.Length - 8)));

                //
                // 注册视图模型
                _paring.Add(string.Format("container.Register<{0}>();\n",
                    vmNameRawStr.Substring(7, vmNameRawStr.Length - 8)));
            }           
        }
        public IReadOnlyCollection<string> Paring => _paring;
    }

    internal class NamespaceWalker : CSharpSyntaxWalker
    {
        private readonly HashSet<string> _namespace;
        private string _root;
        private int _minLength;

        public NamespaceWalker()
        {
            _minLength = 1000;
            _root = string.Empty;
            _namespace = new HashSet<string>();
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var @namespace = node.Name.ToFullString().Replace("\r", "").Replace("\n", "").Trim();
            _namespace.Add($"using {@namespace};\n");
            _root = @namespace.Length <= _minLength ? @namespace : _root;
            _minLength = @namespace.Length <= _minLength ? @namespace.Length : _minLength;

        }

        public IReadOnlyCollection<string> Namespace => _namespace;
        public string RootNamespace => _root;
    }

    [Generator]
    public class ViewModelGenerator : ISourceGenerator
    {
        private const string FileHeader = @"
            using DryIoc;
            using ReactiveUI;
            {0}
            namespace {1}
            {{  
                public partial class ViewModelGenerated
                {{
                    public static partial void RegisterViewModelsAndViews(IContainer container)
                    {{
                        {2}
                    }}
                }}
            }}
        ";


        public void Initialize(GeneratorInitializationContext context)
        {
            // no initialize
            //
            // Attach Visual Studio Debugger
            Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var walker = new ViewModelParingWalker();
            var walker1 = new NamespaceWalker();
            
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                walker.Visit(root);
                walker1.Visit(root);
            }

            var builder = new StringBuilder();
            var builder1 = new StringBuilder();

            foreach (var info in walker.Paring)
            {
                builder.Append(info);
            }

            foreach (var ns in walker1.Namespace)
            {
                builder1.Append(ns);
            }

            var usingDec = builder1.ToString();
            var header = builder.ToString();
            var code = string.Format(FileHeader, usingDec, walker1.RootNamespace, header);
            context.AddSource("ViewModelGenerated.cs", SourceText.From(code, Encoding.UTF8));
            Console.WriteLine(code);
        }
    }
}