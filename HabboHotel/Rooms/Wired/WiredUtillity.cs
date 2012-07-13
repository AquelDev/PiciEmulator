using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pici.HabboHotel.Items;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Rooms.Wired
{
    class WiredUtillity
    {
        internal static bool TypeIsWired(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.triggertimer:
                case InteractionType.triggerroomenter:
                case InteractionType.triggergameend:
                case InteractionType.triggergamestart:
                case InteractionType.triggerrepeater:
                case InteractionType.triggeronusersay:
                case InteractionType.triggerscoreachieved:
                case InteractionType.triggerstatechanged:
                case InteractionType.triggerwalkonfurni:
                case InteractionType.triggerwalkofffurni:
                case InteractionType.actiongivescore:
                case InteractionType.actionposreset:
                case InteractionType.actionmoverotate:
                case InteractionType.actionresettimer:
                case InteractionType.actionshowmessage:
                case InteractionType.actionteleportto:
                case InteractionType.actiontogglestate:
                case InteractionType.conditionfurnishaveusers:
                case InteractionType.conditionstatepos:
                case InteractionType.conditiontimelessthan:
                case InteractionType.conditiontimemorethan:
                case InteractionType.conditiontriggeronfurni:
                    return true;
                default:
                    return false;
            }

        }

        internal static void SaveTrigger(IQueryAdapter dbClient, int itemID, int triggetItemID)
        {
            dbClient.setQuery("INSERT INTO trigger_in_place (original_trigger,triggers_item) VALUES (@my_id,@trigger_item)");
            dbClient.addParameter("my_id", itemID);
            dbClient.addParameter("trigger_item", triggetItemID);
            dbClient.runQuery();
        }

        internal static void SaveTriggerItem(IQueryAdapter dbClient, int triggerID, string triggerInput, string triggerData2, string triggerData, bool allUsertriggerable)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = " + triggerID);
            dbClient.setQuery("INSERT INTO trigger_item (trigger_id,trigger_input,trigger_data,trigger_data_2,all_user_triggerable) VALUES (@id,@triggerinput,@trigger_data,@trigger_data_2,@triggerable)");

            dbClient.addParameter("id", triggerID);
            dbClient.addParameter("triggerinput", triggerInput);
            dbClient.addParameter("trigger_data", triggerData);
            dbClient.addParameter("trigger_data_2", triggerData2);
            dbClient.addParameter("triggerable", allUsertriggerable ? 1 : 0);

            dbClient.runQuery();
        }
    }
}
