using System.Data;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.Messages
{
    internal partial class GameClientMessageHandler
    {
        //Dw`d_GFansites{{2}}Keep tabs on our Fansites.{{2}}M{{2}}{{1}}
        internal void GetGroupdetails()
        {
            if (!PiciEnvironment.groupsEnabled)
                return;

            int groupID = Request.PopWiredInt32();

            DataRow dRow;
            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT name,description,roomid FROM groups_details WHERE id = " + groupID + "");
                dRow = dbClient.getRow();
            }

            if (dRow != null)
            {
                Response.Init(311); // Dw

                Response.Append(groupID);
                Response.Append((string)dRow["name"]);
                Response.Append((string)dRow["description"]);

                int roomID = (int)dRow["roomid"];

                if (roomID > 0)
                {
                    Response.Append(roomID);
                }
                else
                {
                    Response.Append(-1);
                }

                Response.Append("Test");

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
