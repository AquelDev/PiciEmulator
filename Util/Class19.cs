namespace Pici.Util
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Class19
    {
        private static bool bool_0 = true;
        private static bool bool_1 = true;
        private static bool bool_10 = false;
        private static bool bool_11 = false;
        private static bool bool_12 = false;
        private static bool bool_13 = false;
        private static bool bool_2 = true;
        private static bool bool_3 = true;
        private static bool bool_4 = true;
        private static bool bool_5 = true;
        private static bool bool_6 = false;
        private static bool bool_7 = false;
        private static bool bool_8 = false;
        private static bool bool_9 = false;
        private static int int_0 = -1;
        private static int int_1 = 15;
        private static int int_2 = 0x4b;
        private static int int_3 = 15;
        private static int int_4 = 50;
        public const string string_0 = "a";
        public const string string_1 = "b";
        public const string string_10 = "k";
        public const string string_11 = "l";
        public const string string_12 = "m";
        public const string string_13 = "n";
        public const string string_14 = "o";
        public const string string_15 = "p";
        public const string string_16 = "q";
        public const string string_17 = "r";
        public const string string_18 = "s";
        public const string string_19 = "t";
        public const string string_2 = "c";
        public const string string_20 = "u";
        public const string string_21 = "v";
        public const string string_22 = "w";
        public const string string_23 = "x";
        public const string string_24 = "y";
        public const string string_25 = "z";
        private static string string_26 = "";
        private static string string_27 = "";
        private static string string_28 = "disabled";
        private static string string_29 = "D93196DD0B540C93941AFB3FD1D7E7C3";
        public const string string_3 = "d";
        private static string string_30 = "";
        private static string string_31 = "";
        private static string string_32 = "";
        public const string string_4 = "e";
        public const string string_5 = "f";
        public const string string_6 = "g";
        public const string string_7 = "h";
        public const string string_8 = "i";
        public const string string_9 = "j";

        public static string smethod_0(string string_33)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(string_33);
            bytes = provider.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in bytes)
            {
                builder.Append(num2.ToString("x2").ToLower());
            }
            return builder.ToString().ToUpper();
        }

        public static string smethod_1(string string_33)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(string_33);
            byte[] buffer2 = new SHA1Managed().ComputeHash(bytes);
            string str = string.Empty;
            foreach (byte num2 in buffer2)
            {
                str = str + num2.ToString("X2");
            }
            return str;
        }

        private static string smethod_2(string string_33)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(string_33);
            return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", "").ToLower();
        }

        public static bool Boolean_0
        {
            get
            {
                return bool_13;
            }
            set
            {
                bool_13 = value;
            }
        }

        public static bool Boolean_1
        {
            get
            {
                return bool_4;
            }
            set
            {
                bool_4 = value;
            }
        }

        public static bool Boolean_10
        {
            get
            {
                return bool_8;
            }
            set
            {
                bool_8 = value;
            }
        }

        public static bool Boolean_11
        {
            get
            {
                return bool_9;
            }
            set
            {
                bool_9 = value;
            }
        }

        public static bool Boolean_12
        {
            get
            {
                return bool_10;
            }
            set
            {
                bool_10 = value;
            }
        }

        public static bool Boolean_13
        {
            get
            {
                return bool_11;
            }
            set
            {
                bool_11 = value;
            }
        }

        public static bool Boolean_2
        {
            get
            {
                return bool_5;
            }
            set
            {
                bool_5 = value;
            }
        }

        public static bool Boolean_3
        {
            get
            {
                return bool_3;
            }
            set
            {
                bool_3 = value;
            }
        }

        public static bool Boolean_4
        {
            get
            {
                return bool_0;
            }
            set
            {
                bool_0 = value;
            }
        }

        public static bool Boolean_5
        {
            get
            {
                return bool_1;
            }
            set
            {
                bool_1 = value;
            }
        }

        public static bool Boolean_6
        {
            get
            {
                return bool_2;
            }
            set
            {
                bool_2 = value;
            }
        }

        public static bool Boolean_7
        {
            get
            {
                return bool_6;
            }
            set
            {
                if (bool_6 || Boolean_7)
                {
                    bool_6 = false;
                }
                else
                {
                    bool_6 = value;
                }
            }
        }

        public static bool Boolean_8
        {
            get
            {
                return bool_7;
            }
            set
            {
                bool_7 = value;
            }
        }

        public static bool Boolean_9
        {
            get
            {
                return bool_12;
            }
            set
            {
                bool_12 = value;
            }
        }

        public static int Int32_0
        {
            get
            {
                return int_1;
            }
            set
            {
                int_1 = value;
            }
        }

        public static int Int32_1
        {
            get
            {
                return int_2;
            }
            set
            {
                int_2 = value;
            }
        }

        public static int Int32_2
        {
            get
            {
                return int_3;
            }
            set
            {
                int_3 = value;
            }
        }

        public static int Int32_3
        {
            get
            {
                return int_4;
            }
            set
            {
                int_4 = value;
            }
        }

        public static int Int32_4
        {
            get
            {
                return int_0;
            }
            set
            {
                int_0 = value;
            }
        }

        public static string String_1
        {
            get
            {
                return string_32;
            }
            set
            {
                string_32 = value;
            }
        }

        public static string String_2
        {
            get
            {
                return string_28;
            }
            set
            {
                string_28 = value;
            }
        }

        public static string String_3
        {
            get
            {
                return string_30;
            }
            set
            {
                string_30 = value;
            }
        }

        public static string String_4
        {
            get
            {
                return string_26;
            }
            set
            {
                string_26 = value.Replace(@"\n", "\n");
            }
        }

        public static string String_5
        {
            get
            {
                return string_29;
            }
            set
            {
                string_29 = smethod_0(string_29 + value);
            }
        }

        public static string String_6
        {
            get
            {
                return string_27;
            }
            set
            {
                string_27 = value;
            }
        }
    }
}

