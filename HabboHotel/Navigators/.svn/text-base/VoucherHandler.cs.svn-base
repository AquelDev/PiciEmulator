//using System;
//using System.Data;

//using Uber.HabboHotel.GameClients;
//using Uber.Messages;
//using Uber.Storage;

//namespace Uber.HabboHotel.Navigators
//{
//    class VoucherHandler
//    {
//        internal VoucherHandler() { }

//        internal Boolean IsValidCode(string Code)
//        {
//            using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
//            {
//                dbClient.addParameter("voucher", Code);
//                if (dbClient.getRow("SELECT null FROM credit_vouchers WHERE code = @voucher LIMIT 1") != null)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        internal int GetVoucherValue(string Code)
//        {
//            using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
//            {
//                dbClient.addParameter("voucher", Code);
//                DataRow Data = dbClient.getRow("SELECT value FROM credit_vouchers WHERE code = @voucher LIMIT 1;");

//                if (Data != null)
//                {
//                    return (int)Data[0];
//                }

//                return 0;
//            }
//        }

//        internal void TryRedeemVoucher(GameClient Session, string Code)
//        {
//            if (!IsValidCode(Code))
//            {
//                Session.SendMessage(new ServerMessage(213));
//            }

//            int Value = GetVoucherValue(Code);

//            if (Value >= 0)
//            {
//                Session.GetHabbo().Credits += Value;
//                Session.GetHabbo().UpdateCreditsBalance(true);
//            }

//            Session.SendMessage(new ServerMessage(212));
//        }
//    }
//}
