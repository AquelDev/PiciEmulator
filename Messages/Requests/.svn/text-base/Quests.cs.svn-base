
using System;
namespace Butterfly.Messages
{
    partial class GameClientMessageHandler
    {
        public void OpenQuests()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetList(Session, Request);
        }

        public void StartQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().ActivateQuest(Session, Request);
        }

        public void StopQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().CancelQuest(Session, Request);
        }

        public void GetCurrentQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetCurrentQuest(Session, Request);
        }

        //public void RegisterQuests()
        //{
        //    RequestHandlers.Add(3101, new RequestHandler(OpenQuests));
        //    RequestHandlers.Add(3102, new RequestHandler(StartQuest));
        //    RequestHandlers.Add(3106, new RequestHandler(StopQuest));
        //    RequestHandlers.Add(3107, new RequestHandler(GetCurrentQuest));
        //}
    }
}
