using System;

namespace Pici.HabboHotel.Support
{
    [Serializable()]
    public class ModerationBanException : Exception
    {
        internal ModerationBanException(string Reason) : base(Reason) { }
    }
}
