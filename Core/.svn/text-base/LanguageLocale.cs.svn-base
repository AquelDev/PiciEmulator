using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Data;
using System.Text.RegularExpressions;

namespace Butterfly.Core
{
    class LanguageLocale
    {
        private static Dictionary<string, string> values;
        internal static bool welcomeAlertEnabled;
        internal static string welcomeAlert;

        private static List<string> swearwords;

        internal static void Init()
        {
            values = IniReader.ReadFile(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/locale.ini"));
            InitWelcomeMessage();
        }

        internal static void InitSwearWord()
        {
            swearwords = new List<string>();
            DataTable dTable;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT word FROM room_swearword_filter");
                dTable = dbClient.getTable();
            }

            string swearWord;
            foreach (DataRow dRow in dTable.Rows)
            {
                swearWord = (string)dRow[0];
                swearwords.Add(swearWord);
            }

        }

        private static void InitWelcomeMessage()
        {
            Dictionary<string, string> configFile = IniReader.ReadFile(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"System/welcome_config.ini"));
            welcomeAlertEnabled = configFile["welcome.alert.enabled"] == "true";

            if (welcomeAlertEnabled)
            {
                welcomeAlert = File.ReadAllText(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"System/welcome_message.ini"));
            }
        }

        private static string ReplaceEx(string original,
                    string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        internal static string FilterSwearwords(string original)
        {
            
            foreach (string word in swearwords)
            {
                original = ReplaceEx(original, word, "subba");
            }
            return original;
        }

        internal static string GetValue(string value)
        {
            if (values.ContainsKey(value))
                return values[value];
            else
                throw new MissingLocaleException("Missing language locale for [" + value + "]");
        }
    }

    class MissingLocaleException : Exception
    {
        public MissingLocaleException(string message)
            : base(message)
        {

        }
    }
}
