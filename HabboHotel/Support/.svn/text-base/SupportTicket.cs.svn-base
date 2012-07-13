using System;

using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Support
{
    enum TicketStatus
    {
        OPEN = 0,
        PICKED = 1,
        RESOLVED = 2,
        ABUSIVE = 3,
        INVALID = 4,
        DELETED = 5
    }

    class SupportTicket
    {
        private UInt32 Id;
        internal Int32 Score;
        internal Int32 Type;

        internal TicketStatus Status;

        internal UInt32 SenderId;
        internal UInt32 ReportedId;
        internal UInt32 ModeratorId;

        internal String Message;

        internal UInt32 RoomId;
        internal String RoomName;

        internal Double Timestamp;

        private string SenderName;
        private string ReportedName;
        private string ModName;

        internal int TabId
        {
            get
            {
                if (Status == TicketStatus.OPEN)
                {
                    return 1;
                }

                if (Status == TicketStatus.PICKED)
                {
                    return 2;
                }

                return 0;
            }
        }

        internal UInt32 TicketId
        {
            get
            {
                return Id;
            }
        }

        internal SupportTicket(UInt32 Id, int Score, int Type, UInt32 SenderId, UInt32 ReportedId, String Message, UInt32 RoomId, String RoomName, Double Timestamp)
        {
            this.Id = Id;
            this.Score = Score;
            this.Type = Type;
            this.Status = TicketStatus.OPEN;
            this.SenderId = SenderId;
            this.ReportedId = ReportedId;
            this.ModeratorId = 0;
            this.Message = Message;
            this.RoomId = RoomId;
            this.RoomName = RoomName;
            this.Timestamp = Timestamp;

            this.SenderName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(SenderId);
            this.ReportedName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(ReportedId);
            this.ModName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(ModeratorId);
        }

        internal SupportTicket(UInt32 Id, int Score, int Type, UInt32 SenderId, UInt32 ReportedId, String Message, UInt32 RoomId, String RoomName, Double Timestamp, object senderName, object reportedName, object modName)
        {
            this.Id = Id;
            this.Score = Score;
            this.Type = Type;
            this.Status = TicketStatus.OPEN;
            this.SenderId = SenderId;
            this.ReportedId = ReportedId;
            this.ModeratorId = 0;
            this.Message = Message;
            this.RoomId = RoomId;
            this.RoomName = RoomName;
            this.Timestamp = Timestamp;

            if (senderName == DBNull.Value)
                this.SenderName = string.Empty;
            else
                this.SenderName = (string)senderName;

            if (reportedName == DBNull.Value)
                this.ReportedName = string.Empty;
            else
                this.ReportedName = (string)reportedName;

            if (modName == DBNull.Value)
                this.ModName = string.Empty;
            else
                this.ModName = (string)modName;
        }

        internal void Pick(UInt32 pModeratorId, Boolean UpdateInDb)
        {
            this.Status = TicketStatus.PICKED;
            this.ModeratorId = pModeratorId;

            if (UpdateInDb)
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE moderation_tickets SET status = 'picked', moderator_id = " + pModeratorId + " WHERE id = " + Id + "");
                }
            }
        }

        internal void Close(TicketStatus NewStatus, Boolean UpdateInDb)
        {
            this.Status = NewStatus;

            if (UpdateInDb)
            {
                String dbType = "";

                switch (NewStatus)
                {
                    case TicketStatus.ABUSIVE:

                        dbType = "abusive";
                        break;

                    case TicketStatus.INVALID:

                        dbType = "invalid";
                        break;

                    case TicketStatus.RESOLVED:
                    default:

                        dbType = "resolved";
                        break;
                }

                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE moderation_tickets SET status = '" + dbType + "' WHERE id = " + Id + "");
                }
            }
        }

        internal void Release(Boolean UpdateInDb)
        {
            this.Status = TicketStatus.OPEN;

            if (UpdateInDb)
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE moderation_tickets SET status = 'open' WHERE id = " + Id + "");
                }
            }
        }

        internal void Delete(Boolean UpdateInDb)
        {
            this.Status = TicketStatus.DELETED;

            if (UpdateInDb)
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE moderation_tickets SET status = 'deleted' WHERE id = " + Id + "");
                }
            }
        }

        internal ServerMessage Serialize()
        {
            ServerMessage message = new ServerMessage(530);
            message.AppendUInt(Id);
            message.AppendInt32(TabId);
            message.AppendInt32(11); // ??
            message.AppendInt32(Type);
            message.AppendInt32(11); // ??
            message.AppendInt32(Score);
            message.AppendUInt(SenderId);
            message.AppendStringWithBreak(SenderName);
            message.AppendUInt(ReportedId);
            message.AppendStringWithBreak(ReportedName);
            message.AppendUInt(ModeratorId);
            message.AppendStringWithBreak(ModName);
            message.AppendStringWithBreak(this.Message);
            message.AppendUInt(RoomId);
            message.AppendStringWithBreak(RoomName);
            return message;
        }
    }
}
