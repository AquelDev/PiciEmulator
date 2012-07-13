using System.Data;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.Messages
{
    internal partial class GameClientMessageHandler
    {
        //Dw`d_GFansites{{2}}Keep tabs on our Fansites.{{2}}M{{2}}{{1}}
        internal void GetGroupdetails()
        {
            if (!ButterflyEnvironment.groupsEnabled)
                return;

            int groupID = Request.PopWiredInt32();

            DataRow dRow;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT name,description,roomid FROM groups_details WHERE id = " + groupID + "");
                dRow = dbClient.getRow();
            }

            if (dRow != null)
            {
                Response.Init(311); // Dw

                Response.AppendInt32(groupID);
                Response.AppendStringWithBreak((string)dRow["name"]);
                Response.AppendStringWithBreak((string)dRow["description"]);

                int roomID = (int)dRow["roomid"];

                if (roomID > 0)
                {
                    Response.AppendInt32(roomID);
                }
                else
                {
                    Response.AppendInt32(-1);
                }

                Response.AppendStringWithBreak("Test");

                SendResponse();
            }
        }

        //internal void RegisterGroups()
        //{
        //    RequestHandlers.Add(231, GetGroupdetails);
        //}

        //internal void UnRegisterGroups()
        //{
        //    RequestHandlers.Remove(231);
        //}

        //RequestHandlers.Add(94, new RequestHandler(Wave));
    }
}
