using System;
using Butterfly.Core;

namespace Butterfly.HabboHotel.Misc
{
    static class AntiMutant
    {
        internal static bool ValidateLook(string Look, string Gender)
        {
            bool HasHead = false;

            if (Look.Length < 1)
            {
                return false;
            }

            try
            {
                string[] Sets = Look.Split('.');

                if (Sets.Length < 4)
                {
                    return false;
                }

                foreach (string Set in Sets)
                {
                    string[] Parts = Set.Split('-');

                    if (Parts.Length < 3)
                    {
                        return false;
                    }

                    string Name = Parts[0];
                    int Type = int.Parse(Parts[1]);
                    int Color = int.Parse(Parts[1]);

                    if (Type <= 0 || Color < 0)
                    {
                        return false;
                    }

                    if (Name.Length != 2)
                    {
                        return false;
                    }

                    if (Name == "hd")
                    {
                        HasHead = true;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.HandleException(e, "AntiMutant.ValidateLook");
                return false;
            }

            if (!HasHead || (Gender != "M" && Gender != "F"))
            {
                return false;
            }

            return true;
        }
    }
}
