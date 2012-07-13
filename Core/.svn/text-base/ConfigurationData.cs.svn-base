using System;
using System.Collections.Generic;
using System.IO;

namespace Butterfly.Core
{
    class ConfigurationData
    {
        internal Dictionary<string, string> data;
        internal bool fileHasBeenRead = false;

        internal ConfigurationData(string filePath, bool maynotexist = false)
        {
            data = new Dictionary<string, string>();

            if (!File.Exists(filePath))
            {
                if (!maynotexist)
                    throw new ArgumentException("Unable to locate configuration file at '" + filePath + "'.");
                else
                    return;
            }
            fileHasBeenRead = true;

            try
            {
                using (StreamReader stream = new StreamReader(filePath))
                {
                    string line = "";

                    while ((line = stream.ReadLine()) != null)
                    {
                        if (line.Length < 1 || line.StartsWith("#"))
                        {
                            continue;
                        }

                        int delimiterIndex = line.IndexOf('=');

                        if (delimiterIndex != -1)
                        {
                            string key = line.Substring(0, delimiterIndex);
                            string val = line.Substring(delimiterIndex + 1);

                            data.Add(key, val);
                        }
                    }

                    stream.Close();
                }
            }

            catch (Exception e)
            {
                throw new ArgumentException("Could not process configuration file: " + e.Message);
            }
        }
    }
}
