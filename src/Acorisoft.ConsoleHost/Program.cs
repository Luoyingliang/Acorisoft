using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.ConsoleHost;
using Acorisoft.Morisa.Core;

// ReSharper disable ReplaceSubstringWithRangeIndexer
// ReSharper disable MergeIntoNegatedPattern

namespace Acorisoft.ConsoleHost
{
    public class Name
    {
        public void Execute()
        {
            Program.GetCallee();
        }
    }

    public class Name1 : Name
    {
        
    }
    
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static void GetCallee()
        {
            var stackTrace = new StackTrace(true);
            
            //
            // 获取最顶上的堆栈信息
            var methodInfo = stackTrace.GetFrame(1)!.GetMethod();

            //
            //
            var acl = methodInfo?.DeclaringType!.AssemblyQualifiedName;

            Console.WriteLine(acl);
        }
    }
    
    
}
