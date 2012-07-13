using System;
using System.Text;

namespace Butterfly.Util
{
    internal static class OldEncoding
    {
        internal static string encodeVL64(int i)
        {
            byte[] wf = new byte[6]; int pos = 0; int startPos = pos; int bytes = 1; int negativeMask = i >= 0 ? 0 : 4; i = Math.Abs(i); wf[pos++] = (byte)(64 + (i & 3));

            for (i >>= 2; i != 0; i >>= 6)
            {
                bytes++;
                wf[pos++] = (byte)(64 + (i & 0x3f));
            }

            wf[startPos] = (byte)(wf[startPos] | bytes << 3 | negativeMask);

            System.Text.ASCIIEncoding encoder = new ASCIIEncoding();
            string tmp = encoder.GetString(wf);
            return tmp.Replace("\0", "");
        }

        internal static int decodeVL64(string data)
        {
            return decodeVL64(data.ToCharArray());
        }

        internal static int decodeVL64(char[] raw)
        {
            try
            {
                int pos = 0;
                int v = 0;
                bool negative = (raw[pos] & 4) == 4;
                int totalBytes = raw[pos] >> 3 & 7;
                v = raw[pos] & 3;
                pos++;
                int shiftAmount = 2;
                for (int b = 1; b < totalBytes; b++)
                {
                    v |= (raw[pos] & 0x3f) << shiftAmount;
                    shiftAmount = 2 + 6 * b;
                    pos++;
                }

                if (negative)
                    v *= -1;

                return v;
            }
            catch
            {
                return 0;
            }
        }
    }
}
