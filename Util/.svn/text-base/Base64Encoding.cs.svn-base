using System;

namespace Butterfly.Util
{
    internal class Base64Encoding
    {
        private const byte NEGATIVE = 64;
        private const byte POSITIVE = 65;

        internal static byte[] EncodeInt32(int i, int length)
        {
            byte[] data = new byte[length];
            for (int j = 1; j <= length; j++)
            {
                data[j - 1] = (byte)(64 + ((i >> (length - j) * 6) & 63));
            }

            return data;
        }

        internal static byte[] Encodeuint(uint i, int numBytes)
        {
            return EncodeInt32((int)i, numBytes);
        }

        internal static int DecodeInt32(byte[] data)
        {
            int total = 0, j = 0, k = data.Length - 1;
            while (k >= 0)
                total += (data[k--] - 64) * (int)Math.Pow(64, j++);

            return total;
        }

        internal static uint DecodeUInt32(byte[] bzData)
        {
            return (uint)DecodeInt32(bzData);
        }
    }
}
