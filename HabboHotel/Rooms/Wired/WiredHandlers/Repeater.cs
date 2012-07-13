namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers
{
    public class Repeater : IWiredHandler
    {
        private int cyclesRequired;
        private int cycleCount;
        private WiredHandler handler;

        public Repeater(WiredHandler handler, int cyclesRequired)
        {
            this.cyclesRequired = cyclesRequired;
        }

        public bool IsHandleable()
        {
            return (cycleCount > cyclesRequired);
        }

        public void HandleCycle()
        { }

        public void HandleCycle(RoomUser user)
        { }

        public void OnCycle()
        {
            cycleCount++;
        }
    }
}
