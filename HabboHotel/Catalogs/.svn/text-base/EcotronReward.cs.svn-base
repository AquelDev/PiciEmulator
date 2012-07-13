
using Butterfly.HabboHotel.Items;

namespace Butterfly.HabboHotel.Catalogs
{
    class EcotronReward
    {
        //internal uint Id;
        internal uint DisplayId;
        internal uint BaseId;
        internal uint RewardLevel;

        internal EcotronReward(uint DisplayId, uint BaseId, uint RewardLevel)
        {
            //this.Id = Id;
            this.DisplayId = DisplayId;
            this.BaseId = BaseId;
            this.RewardLevel = RewardLevel;
        }

        internal Item GetBaseItem()
        {
            return ButterflyEnvironment.GetGame().GetItemManager().GetItem(this.BaseId);
        }
    }
}
