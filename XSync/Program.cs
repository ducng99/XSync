using System;
using System.Collections.Generic;
using System.IO;

namespace XSync
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                List<string> FolderPaths = new();

                foreach (string arg in args)
                {
                    if (Directory.Exists(arg))
                    {
                        FolderPaths.Add(Path.GetFullPath(arg));
                    }
                }

                var App = new App(FolderPaths);
                App.Sync();
            }
            else
            {
                Console.WriteLine("Please supply 2 folders or more to sync");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
