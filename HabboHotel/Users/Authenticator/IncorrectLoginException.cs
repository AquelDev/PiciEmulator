using System;

namespace Pici.HabboHotel.Users.Authenticator
{
    [Serializable()]
    public class IncorrectLoginException : Exception
    {
        internal IncorrectLoginException(string Reason) : base(Reason) { }
    }
}
