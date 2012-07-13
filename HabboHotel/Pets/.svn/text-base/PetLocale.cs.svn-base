using System.Collections.Generic;
using Butterfly.Core;

namespace Butterfly.HabboHotel.Pets
{
    class PetLocale
    {
        private static Dictionary<string, string[]> values;

        internal static void Init()
        {
            Dictionary<string, string> unparsedValues = IniReader.ReadFile(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/locale.pets.ini"));
            values = new Dictionary<string, string[]>();

            foreach (KeyValuePair<string, string> pair in unparsedValues)
            {
                values.Add(pair.Key, pair.Value.Split(','));
            }
        }

        internal static string[] GetValue(string key)
        {
            string[] value;
            if (values.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return new string[] { "NO KEY FOUND FOR " + key };
            }
        }
    }
}
