using System;
using System.Collections.Generic;


namespace Butterfly.HabboHotel.RoomBots
{
    class RoomBot
    {
        internal uint BotId;
        internal UInt32 RoomId;

        internal AIType AiType;
        internal string WalkingMode;

        internal string Name;
        internal string Motto;
        internal string Look;

        internal int X;
        internal int Y;
        internal int Z;
        internal int Rot;

        internal int minX;
        internal int maxX;
        internal int minY;
        internal int maxY;

        internal List<RandomSpeech> RandomSpeech;
        internal List<BotResponse> Responses;

        internal bool IsPet
        {
            get
            {
                return (this.AiType == AIType.Pet);
            }
        }

        internal RoomBot(uint BotId, UInt32 RoomId, AIType AiType, string WalkingMode, string Name, string Motto, string Look,
            int X, int Y, int Z, int Rot, int minX, int minY, int maxX, int maxY, ref List<RandomSpeech> Speeches, ref List<BotResponse> Responses)
        {
            this.BotId = BotId;
            this.RoomId = RoomId;
            this.AiType = AiType;
            this.WalkingMode = WalkingMode;
            this.Name = Name;
            this.Motto = Motto;
            this.Look = Look;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Rot = Rot;
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;

            LoadRandomSpeech(Speeches);
            LoadResponses(Responses);
        }

        internal void LoadRandomSpeech(List<RandomSpeech> Speeches)
        {
            this.RandomSpeech = new List<RandomSpeech>();

            foreach (RandomSpeech Speech in Speeches)
            {
                if (Speech.BotID == BotId)
                    RandomSpeech.Add(Speech);
            }
        }

        internal void LoadResponses(List<BotResponse> Respons)
        {
            this.Responses = new List<BotResponse>();

            foreach (BotResponse Resp in Respons)
            {
                if (Resp.BotId == BotId)
                    Responses.Add(Resp);
            }
        }

        internal BotResponse GetResponse(string Message)
        {
            foreach (BotResponse Response in Responses)
            {
                if (Response.KeywordMatched(Message))
                {
                    return Response;
                }
            }

            return null;
        }

        internal RandomSpeech GetRandomSpeech()
        {
            return RandomSpeech[ButterflyEnvironment.GetRandomNumber(0, (RandomSpeech.Count - 1))];
        }

        internal BotAI GenerateBotAI(int VirtualId)
        {
            switch (AiType)
            {
                default:
                case AIType.Generic:
                    return new GenericBot(VirtualId);
                case AIType.Guide:
                    return new GuideBot();
                case AIType.Pet:
                    return new PetBot(VirtualId);
            }
        }
    }

    internal enum AIType
    {
        Pet,
        Guide,
        Generic
    }
}
