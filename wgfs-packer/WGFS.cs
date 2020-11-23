using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wgfs_packer
{
    public static class Utils
    {
        public static void WriteASCII(BinaryWriter bw, string value)
        {
            byte[] nameAsASCII = Encoding.ASCII.GetBytes(value);
            bw.Write(nameAsASCII);
            bw.Write((byte)0x00); // null terminate the string.
        }
    }

    public struct File
    {
        public string name;
        public byte[] data;

        public void Serialize(BinaryWriter w)
        {
            Utils.WriteASCII(w, name);
            w.Write(data.Length);
            w.Write(data);
        }
    }

    public class Assets
    {
        private List<File> Files { get; set; } = new List<File>();

        private Dictionary<string, string> StringTable { get; set; } = new Dictionary<string, string>();

        private const int WGFS_HEADER = 1397114711;
        private const int WEND_HEADER = 1145980247;
        private const int FILE_HEADER = 1162627398;
        private const int STRG_HEADER = 1196577875;
        private const int WGFS_VERSION = 1;

        private void SerializeFiles(BinaryWriter bw)
        {
            bw.Write(FILE_HEADER); // 'FILE'
            bw.Write(Files.Count);
            foreach (var file in Files)
            {
                file.Serialize(bw);
            }
        }

        private void SerializeStrings(BinaryWriter bw)
        {
            bw.Write(STRG_HEADER); // 'STRG'
            bw.Write(StringTable.Count);
            foreach (var kvp in StringTable)
            {
                Utils.WriteASCII(bw, kvp.Key);
                Utils.WriteASCII(bw, kvp.Value);
            }
        }

        private void SerializeAll(BinaryWriter bw)
        {
            bw.Write(WGFS_HEADER); // 'WGFS' header
            bw.Write(WGFS_VERSION);
            this.SerializeFiles(bw);
            this.SerializeStrings(bw);
            bw.Write(WEND_HEADER); // 'WEND'
            // TODO: more stuff here?
        }

        public void Serialize(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            SerializeAll(bw);
            //bw.Dispose();
        }

        public void AddFile(File f)
        {
            Files.Add(f);
        }

        public void AddString(string key, string value)
        {
            StringTable.Add(key, value);
        }
    }
}
