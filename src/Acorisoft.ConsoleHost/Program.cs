using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acorisoft.ConsoleHost;
// ReSharper disable ReplaceSubstringWithRangeIndexer
// ReSharper disable MergeIntoNegatedPattern

namespace Acorisoft.ConsoleHost
{
    
    class Program
    {
        private static HttpListener _server;

        static void Main(string[] args)
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://localhost:8008/");
            _server.Start();
            Task.Run(Loop);
            Console.ReadLine();
        }

        static async void Loop()
        {
            var array = new List<byte>();
            var buffer = new byte[4096];
        
            while (true)
            {
                array.Clear();
            
                var context = await _server.GetContextAsync();
            
            }

        }
    }
    
    
}
