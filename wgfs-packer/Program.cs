using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wgfs_packer
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("WGFS-Packer - Weird Game File System Packer v1.0");

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <this program> 'directory to pack' 'string table'");
                Console.ReadKey(true);
                return 0;
            }

            string dir = args[0];
            string jsonpath = args[1];
            var strm = new FileStream("data.dat", FileMode.Create, FileAccess.Write, FileShare.Read);
            Console.WriteLine("Packing files...");

            var fs = new Assets();
            foreach (var file in Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine("Adding " + file);

                fs.AddFile(new File {
                    name = Path.GetFileName(file),
                    data = System.IO.File.ReadAllBytes(file)
                });
            }

            var table = JObject.Parse(System.IO.File.ReadAllText(jsonpath));
            Console.WriteLine("Packing strings...");
            foreach (var item in table)
            {
                string v = item.Value.Value<string>();
                fs.AddString(item.Key, v);
                Console.WriteLine($"{item.Key}|{v}");
            }

            Console.WriteLine("Serializing...");
            fs.Serialize(strm);
            strm.Flush(true);
            strm.Dispose();

            Console.WriteLine("Done!");
            Console.ReadKey(true);

            return 0;
        }
    }
}
