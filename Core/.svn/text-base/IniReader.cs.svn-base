using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Butterfly.Core
{
    class IniReader
    {
        internal static Dictionary<string, string> ReadFile(string path)
        {
            Dictionary<string, string>  values = new Dictionary<string, string>();
            string[] texts = File.ReadAllLines(path);

            foreach (string text in texts)
            {
                if (text.Length != 0 && text.Contains("=") && text.Substring(0, 1) != "#" && text.Substring(0, 1) != "[")
                {
                    string[] parsedText = text.Split('=');
                    values.Add(parsedText[0], parsedText[1]);
                }
            }

            return values;
        }

        internal static Dictionary<int, string> ReadFileWithInt(string path)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            string[] texts = File.ReadAllLines(path);

            foreach (string text in texts)
            {
                if (text.Length != 0 && text.Contains("=") && text.Substring(0, 1) != "#" && text.Substring(0, 1) != "[")
                {
                    string[] parsedText = text.Split('=');
                    values.Add(int.Parse(parsedText[1]), parsedText[0]);
                }
            }

            return values;
        }
    }
}
