using System;
using System.Collections.Generic;
using System.Text;

using Butterfly.Util;
using System.IO;
using Butterfly.Core;

namespace Butterfly.Messages
{
    internal class ServerMessage
    {
        private UInt32 MessageId;

        internal UInt32 Id
        {
            get
            {
                return MessageId;
            }
        }

        private List<byte> Body;

        internal string Header
        {
            get
            {
                return ButterflyEnvironment.GetDefaultEncoding().GetString(Base64Encoding.Encodeuint(MessageId, 2));
            }
        }

        private static readonly char expl0it = Convert.ToChar(1);

        internal int Length
        {
            get
            {
                return (int)Body.Count;
            }
        }

        internal ServerMessage() { }

        internal ServerMessage(uint _MessageId)
        {
            Init(_MessageId);
        }

        public override string ToString()
        {
            return Header + ButterflyEnvironment.GetDefaultEncoding().GetString(Body.ToArray());
        }

        //internal string ToBodyString()
        //{
        //    return ButterflyEnvironment.GetDefaultEncoding().GetString(Body.ToArray());
        //}

        //internal void Clear()
        //{
        //    Body.Clear();
        //}

        internal void Init(UInt32 _MessageId)
        {
            MessageId = _MessageId;
            Body = new List<byte>();
        }

        internal void AppendByte(byte b)
        {
            Body.Add(b);
        }

        internal void AppendBytes(byte[] Data)
        {
            if (Data == null || Data.Length == 0)
                return;

            Body.AddRange(Data);
        }

        internal void AppendString(string s, Encoding encoding)
        {
            if (s == null || s.Length == 0)
            {
                return;
            }
            s = s.Replace(expl0it, ' ');
            AppendBytes(encoding.GetBytes(s));
        }

        internal void AppendString(string s)
        {
            AppendString(s, ButterflyEnvironment.GetDefaultEncoding());
        }

        internal void AppendStringWithBreak(string s)
        {
            AppendStringWithBreak(s, 2);
        }

        internal void AppendStringWithBreak(string s, string u)
        {
            AppendStringWithBreak(s, 2);
        }

        internal void AppendStringWithBreak(string s, byte BreakChar)
        {
            AppendString(s);
            AppendByte(BreakChar);
        }

        internal void AppendInt32(Int32 i)
        {
            AppendBytes(WireEncoding.EncodeInt32(i));
        }

        internal void AppendRawInt32(Int32 i)
        {
            AppendString(i.ToString(), Encoding.ASCII);
        }

        internal void AppendUInt(uint i)
        {
            AppendInt32((int)i);
        }

        internal void AppendRawUInt(uint i)
        {
            AppendRawInt32((int)i);
        }

        internal void AppendBoolean(Boolean Bool)
        {
            Body.Add(Bool ? WireEncoding.POSITIVE : WireEncoding.NEGATIVE);
        }

        internal byte[] GetBytes()
        {
            byte[] Data = new byte[Length + 3];
            byte[] Header = Base64Encoding.Encodeuint(MessageId, 2);
            byte[] realData = Body.ToArray();
            Data[0] = Header[0];
            Data[1] = Header[1];

            for (int i = 0; i < Length; i++)
            {
                Data[i + 2] = realData[i];
            }

            Data[Data.Length - 1] = 1;

            return Data;
        }
    }
}
