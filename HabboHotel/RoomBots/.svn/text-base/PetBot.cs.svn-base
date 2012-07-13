using System;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Core;
using Butterfly.HabboHotel.Pets;
using System.Drawing;

namespace Butterfly.HabboHotel.RoomBots
{
    class PetBot : BotAI
    {
        private int SpeechTimer;
        private int ActionTimer;
        private int EnergyTimer;

        internal PetBot(Int32 VirtualId)
        {
            this.SpeechTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
            this.ActionTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 30 + VirtualId);
            this.EnergyTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
        }

        private void RemovePetStatus()
        {
            RoomUser Pet = GetRoomUser();

            // Remove Status
            Pet.Statusses.Remove("sit");
            Pet.Statusses.Remove("lay");
            Pet.Statusses.Remove("snf");
            Pet.Statusses.Remove("eat");
            Pet.Statusses.Remove("ded");
            Pet.Statusses.Remove("jmp");
        }

        internal override void OnSelfEnterRoom()
        {
            Point nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
            //int randomX = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeX);
            //int randomY = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeY);
            if (GetRoomUser() != null)
                GetRoomUser().MoveTo(nextCoord.X, nextCoord.Y);
        }

        internal override void OnSelfLeaveRoom(bool Kicked) { }

        internal override void OnUserEnterRoom(Rooms.RoomUser User)
        {
            if (User.GetClient() != null && User.GetClient().GetHabbo() != null)
            {
                if (User.GetClient().GetHabbo().Username.ToLower() == "tim")
                    GetRoomUser().Chat(null, "TIM IN DA HOUSE!!!", true);


            }
                
        }

        internal override void OnUserLeaveRoom(GameClients.GameClient Client) { }

        #region Commands
        internal override void OnUserSay(Rooms.RoomUser User, string Message)
        {
            RoomUser Pet = GetRoomUser();
            if (Pet.PetData.DBState != Pets.DatabaseUpdateState.NeedsInsert)
                Pet.PetData.DBState = Pets.DatabaseUpdateState.NeedsUpdate;
            

            if (Message.ToLower().Equals(Pet.PetData.Name.ToLower()))
            {
                Pet.SetRot(Rotation.Calculate(Pet.X, Pet.Y, User.X, User.Y), false);
                return;
            }

            if (Message.ToLower().StartsWith(Pet.PetData.Name.ToLower() + " ") && User.GetClient().GetHabbo().Username.ToLower() == GetRoomUser().PetData.OwnerName.ToLower())
            {
                string Command = Message.Substring(Pet.PetData.Name.ToLower().Length + 1);

                int r = ButterflyEnvironment.GetRandomNumber(1, 8); // Made Random

                if (Pet.PetData.Energy > 10 && r < 6 || Pet.PetData.Level > 15)
                {
                    RemovePetStatus(); // Remove Status

                    switch (PetCommandHandeler.TryInvoke(Command))
                    {
                        // TODO - Level you can use the commands at...

                        #region free
                        case 1:
                            RemovePetStatus();

                            //int randomX = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeX);
                            //int randomY = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeY);
                            Point nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
                            Pet.MoveTo(nextCoord.X, nextCoord.Y);

                            Pet.PetData.AddExpirience(10); // Give XP

                            break;
                        #endregion

                        #region here
                        case 2:

                            RemovePetStatus();

                            int NewX = User.X;
                            int NewY = User.Y;
                            
                            ActionTimer = 30; // Reset ActionTimer

                            #region Rotation
                            if (User.RotBody == 4)
                            {
                                NewY = User.Y + 1;
                            }
                            else if (User.RotBody == 0)
                            {
                                NewY = User.Y - 1;
                            }
                            else if (User.RotBody == 6)
                            {
                                NewX = User.X - 1;
                            }
                            else if (User.RotBody == 2)
                            {
                                NewX = User.X + 1;
                            }
                            else if (User.RotBody == 3)
                            {
                                NewX = User.X + 1;
                                NewY = User.Y + 1;
                            }
                            else if (User.RotBody == 1)
                            {
                                NewX = User.X + 1;
                                NewY = User.Y - 1;
                            }
                            else if (User.RotBody == 7)
                            {
                                NewX = User.X - 1;
                                NewY = User.Y - 1;
                            }
                            else if (User.RotBody == 5)
                            {
                                NewX = User.X - 1;
                                NewY = User.Y + 1;
                            }
                            #endregion

                            Pet.PetData.AddExpirience(10); // Give XP

                            Pet.MoveTo(NewX, NewY);
                            break;
                        #endregion

                        #region sit
                        case 3:
                            // Remove Status
                            RemovePetStatus();

                            Pet.PetData.AddExpirience(10); // Give XP

                            // Add Status
                            Pet.Statusses.Add("sit", TextHandling.GetString(Pet.Z));
                            ActionTimer = 25;
                            EnergyTimer = 10;
                            break;
                        #endregion

                        #region lay
                        case 4:
                            // Remove Status
                            RemovePetStatus();

                            // Add Status
                            Pet.Statusses.Add("lay", TextHandling.GetString(Pet.Z));

                            Pet.PetData.AddExpirience(10); // Give XP

                            ActionTimer = 30;
                            EnergyTimer = 5;
                            break;
                        #endregion

                        #region dead
                        case 5:
                            // Remove Status
                            RemovePetStatus();

                            // Add Status 
                            Pet.Statusses.Add("ded", TextHandling.GetString(Pet.Z));

                            Pet.PetData.AddExpirience(10); // Give XP

                            // Don't move to speak for a set amount of time.
                            SpeechTimer = 45;
                            ActionTimer = 30;

                            break;
                        #endregion

                        #region sleep
                        case 6:
                            // Remove Status
                            RemovePetStatus();

                            Pet.Chat(null, "ZzzZZZzzzzZzz", false);
                            Pet.Statusses.Add("lay", TextHandling.GetString(Pet.Z));

                            Pet.PetData.AddExpirience(10); // Give XP

                            // Don't move to speak for a set amount of time.
                            EnergyTimer = 5;
                            SpeechTimer = 30;
                            ActionTimer = 45;
                            break;
                        #endregion

                        #region jump
                        case 7:
                            // Remove Status
                            RemovePetStatus();

                            // Add Status 
                            Pet.Statusses.Add("jmp", TextHandling.GetString(Pet.Z));

                            Pet.PetData.AddExpirience(10); // Give XP

                            // Don't move to speak for a set amount of time.
                            EnergyTimer = 5;
                            SpeechTimer = 10;
                            ActionTimer = 5;
                            break;
                        #endregion

                        default:
                            string[] Speech = PetLocale.GetValue("pet.unknowncommand");

                            Random RandomSpeech = new Random();
                            Pet.Chat(null, Speech[RandomSpeech.Next(0, Speech.Length - 1)], false);
                            break;
                    }
                    Pet.PetData.PetEnergy(false); // Remove Energy
                    Pet.PetData.PetEnergy(false); // Remove Energy

                }
                else
                {

                    RemovePetStatus(); // Remove Status

                    if (Pet.PetData.Energy < 10)
                    {
                        string[] Speech = PetLocale.GetValue("pet.tired");

                        Random RandomSpeech = new Random();
                        Pet.Chat(null, Speech[RandomSpeech.Next(0, Speech.Length - 1)], false);

                        Pet.Statusses.Add("lay", TextHandling.GetString(Pet.Z));

                        SpeechTimer = 50;
                        ActionTimer = 45;
                        EnergyTimer = 5;

                    }
                    else
                    {

                        string[] Speech = PetLocale.GetValue("pet.lazy");

                        Random RandomSpeech = new Random();
                        Pet.Chat(null, Speech[RandomSpeech.Next(0, Speech.Length - 1)], false);

                        Pet.PetData.PetEnergy(false); // Remove Energy
                    }
                }
            }
            //Pet = null;
        }
        #endregion

        internal override void OnUserShout(Rooms.RoomUser User, string Message) { }

        internal override void OnTimerTick()
        {
            #region Speech
            if (SpeechTimer <= 0)
            {
                RoomUser Pet = GetRoomUser();
                if (Pet.PetData.DBState != Pets.DatabaseUpdateState.NeedsInsert)
                    Pet.PetData.DBState = Pets.DatabaseUpdateState.NeedsUpdate;

                if (Pet != null)
                {
                    Random RandomSpeech = new Random();
                    RemovePetStatus();

                    string[] Speech = PetLocale.GetValue("speech.pet" + Pet.PetData.Type);

                    string rSpeech = Speech[RandomSpeech.Next(0, Speech.Length - 1)];

                    if (rSpeech.Length != 3)
                        Pet.Chat(null, rSpeech, false);
                    else
                        Pet.Statusses.Add(rSpeech, TextHandling.GetString(Pet.Z));
                }
                SpeechTimer = ButterflyEnvironment.GetRandomNumber(20, 120);
            }
            else
            {
                SpeechTimer--;
            }
            #endregion

            if (ActionTimer <= 0)
            {
                try
                {
                    // Remove Status
                    RemovePetStatus();

                    Point nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
                    //int randomX = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeX);
                    //int randomY = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeY);
                    GetRoomUser().MoveTo(nextCoord.X, nextCoord.Y);

                    ActionTimer = ButterflyEnvironment.GetRandomNumber(15, 40 + GetRoomUser().PetData.VirtualId);
                }
                catch (Exception e)
                {
                    Logging.HandleException(e, "PetBot.OnTimerTick");
                }
            }
            else
            {
                ActionTimer--;
            }

            if (EnergyTimer <= 0)
            {
                RemovePetStatus(); // Remove Status

                RoomUser Pet = GetRoomUser();

                Pet.PetData.PetEnergy(true); // Add Energy

                EnergyTimer = ButterflyEnvironment.GetRandomNumber(30, 120); // 2 Min Max
            }
            else
            {
                EnergyTimer--;
            }
        }
    }
}
