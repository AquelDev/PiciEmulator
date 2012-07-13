using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Pici.Core.Helpers
{
    public class Encryptor
    {
        private static string salt1 = "ana";
        private static string salt2 = "genesis";
        private static string salt3 = "JKASD";

        /// <summary>
        /// Hashes a string to provide a hashed pattern
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A hash of the string</returns>
        public static string hashUsernameAndPassword(string username, string hashedPassword)
        {
            return GetSHA1(salt1 + username + salt2 + hashedPassword + salt3);
        }

        /// <summary>
        /// Gets the hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string hashPassword(string password)
        {
            return GetSHA1(salt1 + salt2 + password + salt3);
        }

        /// <summary>
        /// Gets the SHA1 value
        /// </summary>
        /// <param name="text">The text of the item</param>
        /// <returns>The hashed information</returns>
        private static string GetSHA1(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}
