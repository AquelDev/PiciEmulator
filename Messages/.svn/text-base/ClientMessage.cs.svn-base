using System;

using System.Text;

using Butterfly.Util;
using Butterfly.Core;

namespace Butterfly.Messages
{
    public class ClientMessage
    {
        private uint MessageId;
        private byte[] Body;
        private int Pointer;

        internal uint Id
        {
            get
            {
                return MessageId;
            }
        }

        //internal int Length
        //{
        //    get
        //    {
        //        return Body.Length;
        //    }
        //}
        private static readonly string expl0it = Convert.ToChar(1).ToString();

        private void CheckForExploits(string packetdata)
        {
            if (packetdata.Contains(expl0it))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("PACKET EXPLOIT IN PACKET " + MessageId);
                builder.AppendLine("Packet content : " + ToString());
                Logging.LogCriticalException(builder.ToString());
            }
        }

        internal int RemainingLength
        {
            get
            {
                return Body.Length - Pointer;
            }
        }

        internal string Header
        {
            get
            {
                return Encoding.Default.GetString(Base64Encoding.Encodeuint(MessageId, 2));
            }
        }

        internal ClientMessage(uint _MessageId, byte[] _Body)
        {
            if (_Body == null)
            {
                _Body = new byte[0];
            }

            MessageId = _MessageId;
            Body = _Body;

            Pointer = 0;

            //Console.WriteLine("[" + ToString() + "]");
        }

        public override string ToString()
        {
            string text = Header + ButterflyEnvironment.GetDefaultEncoding().GetString(Body);
            //CheckForExploits(text);
            return text;
        }

        //internal void ResetPointer()
        //{
        //    Pointer = 0;
        //}

        internal void AdvancePointer(int i)
        {
            Pointer += i;
        }

        //internal string GetBody()
        //{
        //    return Encoding.Default.GetString(Body);
        //}

        internal byte[] ReadBytes(int Bytes)
        {
            if (Bytes > RemainingLength)
                Bytes = RemainingLength;

            byte[] data = new byte[Bytes];

            for (int i = 0; i < Bytes; i++)
                data[i] = Body[Pointer++];

            return data;
        }

        internal byte[] PlainReadBytes(int Bytes)
        {
            if (Bytes > RemainingLength)
                Bytes = RemainingLength;

            byte[] data = new byte[Bytes];

            for (int x = 0, y = Pointer; x < Bytes; x++, y++)
            {
                data[x] = Body[y];
            }

            return data;
        }

        internal byte[] ReadFixedValue()
        {
            int len = Base64Encoding.DecodeInt32(ReadBytes(2));
            return ReadBytes(len);
        }

        //internal Boolean PopBase64Boolean()
        //{
        //    if (RemainingLength > 0 && Body[Pointer++] == Base64Encoding.POSITIVE)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //internal Int32 PopInt32()
        //{
        //    return Base64Encoding.DecodeInt32(ReadBytes(2));
        //}

        //internal UInt32 PopUInt32()
        //{
        //    return (UInt32)PopInt32();
        //}

        internal string PopFixedString()
        {
            string data = PopFixedString(ButterflyEnvironment.GetDefaultEncoding());
            CheckForExploits(data);
            return data;
        }

        internal string PopFixedString(Encoding encoding)
        {
            string data = encoding.GetString(ReadFixedValue()).Replace(Convert.ToChar(1), ' ');
            CheckForExploits(data);
            return data;
        }

        internal Int32 PopFixedInt32()
        {
            Int32 i = 0;

            string s = PopFixedString(Encoding.ASCII);

            Int32.TryParse(s, out i);

            return i;
        }

        //internal UInt32 PopFixedUInt32()
        //{
        //    return (uint)PopFixedInt32();
        //}

        internal Boolean PopWiredBoolean()
        {
            if (this.RemainingLength > 0 && Body[Pointer++] == WireEncoding.POSITIVE)
            {
                return true;
            }

            return false;
        }

        internal Int32 PopWiredInt32()
        {
            if (RemainingLength < 1)
            {
                return 0;
            }

            byte[] Data = PlainReadBytes(WireEncoding.MAX_INTEGER_BYTE_AMOUNT);

            Int32 TotalBytes = 0;
            Int32 i = WireEncoding.DecodeInt32(Data, out TotalBytes);

            Pointer += TotalBytes;

            return i;
        }

        internal uint PopWiredUInt()
        {
            return (uint)PopWiredInt32();
        }
    }
}
