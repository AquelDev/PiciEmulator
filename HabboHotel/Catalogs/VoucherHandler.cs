using System;
using System.Data;
using Pici;
using Pici.HabboHotel.GameClients;
using Pici.Messages;
using Pici.Storage.Database.Session_Details.Interfaces;
using Pici.Storage.Managers.Database;

namespace Pici.HabboHotel.Catalogs
{
    class VoucherHandler
    {
        private static Boolean IsValidCode(string Code)
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT null FROM credit_vouchers WHERE code = @code");
                dbClient.addParameter("code", Code);

                if (dbClient.getRow() != null)
                {
                    return true;
                }
            }

            return false;
        }

        internal static int GetVoucherValue(string Code)
        {
            DataRow Data;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT value FROM credit_vouchers WHERE code = @code");
                dbClient.addParameter("code", Code);

                Data = dbClient.getRow();
            }

            if (Data != null)
            {
                return (int)Data[0];
            }

            return 0;
        }

        private static void TryDeleteVoucher(string Code)
        {
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("DELETE FROM credit_vouchers WHERE code = @code");
                dbClient.addParameter("code", Code);
                dbClient.runQuery();
            }
        }

        internal static void TryRedeemVoucher(GameClient Session, string Code)
        {
            if (!IsValidCode(Code))
            {
                ServerMessage Error = new ServerMessage(213);
                Error.AppendRawInt32(1);
                Session.SendMessage(Error);
                return;
            }

            int Value = GetVoucherValue(Code);

            TryDeleteVoucher(Code);

            Session.GetHabbo().Credits += Value;
            Session.GetHabbo().UpdateCreditsBalance();

            Session.SendMessage(new ServerMessage(212));
        }
    }
}
