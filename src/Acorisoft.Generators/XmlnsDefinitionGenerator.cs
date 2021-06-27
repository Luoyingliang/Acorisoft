using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Acorisoft.Platform.Generators
{
    [Generator]
    public class XmlnsDefinitionGenerator : ISourceGenerator
    {
        private class AssemblyAttributeWalker : CSharpSyntaxWalker
        {
            public AssemblyAttributeWalker()
            {
                SkipNamespaces = new List<string>();
            }
            
            public override void VisitAttributeList(AttributeListSyntax node)
            {
                //
                //
                var genDef = node.Attributes.FirstOrDefault(x => x.Name.ToFullString() == "GenerateDefinition");
                
                //
                //
                if (genDef?.ArgumentList != null)
                {
                    Xmlns = genDef.ArgumentList.Arguments[0].Expression.ToFullString();
                }
                
                //
                //
                var skips = node.Attributes.Where(x => x.Name.ToFullString() == "SkipGenerateDefinition");
                
                //
                //
                foreach (var skip in skips)
                {
                    if (skip.ArgumentList != null)
                    {
                        continue;
                    }

                    var argList = skip.ArgumentList;
                    
                    //
                    //
                    SkipNamespaces.Add(argList?.Arguments[0].Expression.ToFullString());
                }
                
                base.VisitAttributeList(node);
            }
            
            public string Xmlns { get; private set; }
            public IList<string> SkipNamespaces { get; private set; }
        }
        
        public void Initialize(GeneratorInitializationContext context)
        {
            Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.Compilation.SyntaxTrees.Any(x => x.FilePath.Contains("AssemblyInfo")))
            {
                return;
            }
            
            //
            // 初始化
            var visitor = new NamespaceWalker();
            var assemblyInfoVisitor = new AssemblyAttributeWalker();
            var set = new HashSet<string>();
            var builder = new StringBuilder();
                
            //
            // 遍历
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                var root = syntaxTree.GetRoot();
                visitor.Visit(root);
                assemblyInfoVisitor.Visit(root);
            }

            //
            // 添加命名空间
            foreach (var ns in visitor.Namespace)
            {
                set.Add(ns);
            }

            //
            // 移除需要跳过的
            foreach (var skip in assemblyInfoVisitor.SkipNamespaces)
            {
                set.Remove(skip);
            }

            const string attrPattern = "[assembly: XmlnsDefinition({0},{1}{2}{3})]\n";
            const char sep = '"';
            //
            // 生成
            foreach (var ns in set)
            {
                builder.Append(string.Format(
                    attrPattern,
                    assemblyInfoVisitor.Xmlns,
                    sep, ns, sep));
            }

            const string pattern = @"
                using System.Windows.Markup;
                {0}
                ";

            var code = string.Format(pattern, builder);


            context.AddSource("AssemblyInfo.Generated.cs", SourceText.From(code, Encoding.UTF8));
        }
    }
}