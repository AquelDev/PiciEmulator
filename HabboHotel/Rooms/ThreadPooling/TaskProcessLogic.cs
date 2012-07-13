
namespace Pici.HabboHotel.Rooms.ThreadPooling
{
    class TaskProcessLogic
    {
        internal static void processMessage(IProcessable information)
        {
            information.ProcessLogic();
        }
    }
}
