using System;

namespace Butterfly.Util
{
    internal static class WireEncoding
    {
        internal const byte NEGATIVE = 72;
        internal const byte POSITIVE = 73;
        internal const int MAX_INTEGER_BYTE_AMOUNT = 6;

        internal static byte[] EncodeInt32(int i)
        {
            int pos = 1, numBytes = 1, negativeMask = i >= 0 ? 0 : 4;

            if (i < 0)
                i *= -1;

            byte[] wf = new byte[6] { (byte)(64 + (i & 3)), 0, 0, 0, 0, 0 };

            for (i >>= 2; i > 0; i >>= 6, numBytes++)
            {
                wf[pos++] = (byte)(64 + (i & 63));
            }
            wf[0] = (byte)(wf[0] | numBytes << 3 | negativeMask);

            byte[] bzData = new byte[numBytes];
            for (int x = 0; x < numBytes; x++)
            {
                bzData[x] = wf[x];
            }

            return bzData;
        }

        internal static int DecodeInt32(byte[] bzData, out int totalBytes)
        {
            int pos = 1, output = bzData[0] & 3, shiftAmount = 2;
            totalBytes = bzData[0] >> 3 & 7;

            while (pos < totalBytes)
            {
                output |= (bzData[pos] & 63) << shiftAmount;
                shiftAmount = 2 + 6 * pos++;
            }

            if ((bzData[0] & 4) == 4)
                output *= -1;

            return output;
        }

        internal static uint DecodeUInt32(byte[] bzData, out int totalBytes)
        {
            int pos = 1, output = bzData[0] & 3, shiftAmount = 2;
            totalBytes = bzData[0] >> 3 & 7;

            while (pos < totalBytes)
            {
                output |= (bzData[pos] & 63) << shiftAmount;
                shiftAmount = 2 + 6 * pos++;
            }

            return (uint)output;
        }
    }
}
