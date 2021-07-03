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
using LiteDB;

// ReSharper disable ReplaceSubstringWithRangeIndexer
// ReSharper disable MergeIntoNegatedPattern

namespace Acorisoft.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new LiteDatabase(@"test.dat");
            database.FileStorage.Upload("1", @"E:\壁纸\06.jpg");
            database.Dispose();
            Thread.Sleep(2000);
            database = new LiteDatabase(@"test.dat");
            database.FileStorage.Download("1", @"D:\1.png", true);
        }

    }
    
    
}
