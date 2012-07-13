using System.Collections.Generic;
using System.IO;
using System.Text;
using Butterfly.HabboHotel.Misc;
using Butterfly.HabboHotel.GameClients;
using Butterfly.Core;
using Butterfly.HabboHotel.Catalogs;
using System;

namespace Butterfly.Messages
{
    class ChatCommandRegister
    {
        private static Dictionary<int, string> commandRegister;
        private static Dictionary<string, ChatCommand> commandRegisterInvokeable;

        internal static void Init()
        {
            commandRegister = new Dictionary<int, string>();
            commandRegisterInvokeable = new Dictionary<string, ChatCommand>();

            InitCommandRegister();
            InitInvokeableRegister();
        }

        private static void InitCommandRegister()
        {
            commandRegister = IniReader.ReadFileWithInt(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/commands_register.ini"));
        }

        private static void InitInvokeableRegister()
        {
            Dictionary<string, string> commandDatabase = IniReader.ReadFile(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/commands.ini"));

            foreach (KeyValuePair<int, string> pair in commandRegister)
            {
                int commandID = pair.Key;
                string commandStringedID = pair.Value;

                int commandMinRank = int.Parse(commandDatabase[commandStringedID + ".minrank"]);
                string commandDescription = commandDatabase[commandStringedID + ".description"];
                string[] commandInput = commandDatabase[commandStringedID + ".input"].ToLower().Split(',');
                string commandPrefix = commandDatabase[commandStringedID + ".prefix"];
                string[] clubsAllowed = commandDatabase[commandStringedID + ".clubs"].ToLower().Split(',');

                foreach (string command in commandInput)
                {
                    commandRegisterInvokeable.Add(command, new ChatCommand(commandID, command, commandMinRank, commandDescription, commandPrefix, clubsAllowed));
                }
            }

            //string supersecret = "ditunvdjgnpwuiyrvb";
            //ChatCommand acommand = new ChatCommand(400, supersecret, 0, string.Empty, string.Empty, new string[0]);
            //commandRegisterInvokeable.Add(supersecret, acommand);
        }

        internal static string GenerateCommandList(GameClient client)
        {
            StringBuilder notif = new StringBuilder();

            foreach (ChatCommand command in commandRegisterInvokeable.Values)
            {
                if (command.commandID == 400)
                    continue;
                if (command.UserGotAuthorization(client))
                    notif.Append(command.input + " " + command.prefix + " - " + command.description + "\r");
            }

            return notif.ToString();
        }

        internal static bool IsChatCommand(string command)
        {
            return commandRegisterInvokeable.ContainsKey(command);
        }

        internal static ChatCommand GetCommand(string command)
        {
            return commandRegisterInvokeable[command];
        }

        #region Invoking
        internal static void InvokeCommand(ChatCommandHandler commandHandler, int commandID)
        {
            switch (commandID)
            {
                case 1:
                    {
                        commandHandler.pickall();
                        break;
                    }

                case 2:
                    {
                        commandHandler.setspeed();
                        break;
                    }

                case 3:
                    {
                        commandHandler.unload();
                        break;
                    }

                case 4:
                    {
                        commandHandler.disablediagonal();
                        break;
                    }

                case 5:
                    {
                        commandHandler.setmax();
                        break;
                    }

                case 6:
                    {
                        commandHandler.overridee();
                        break;
                    }
                    
                case 7:
                    {
                        commandHandler.teleport();
                        break;
                    }

                case 8:
                    {
                        ChatCommandHandler.catarefresh();
                        break;
                    }

                case 10:
                    {
                        commandHandler.roomalert();
                        break;
                    }

                case 11:
                    {
                        commandHandler.coords();
                        break;
                    }

                case 12:
                    {
                        commandHandler.coins();
                        break;
                    }

                case 13:
                    {
                        commandHandler.pixels();
                        break;
                    }

                case 14:
                    {
                        commandHandler.handitem();
                        break;
                    }

                case 15:
                    {
                        commandHandler.hotelalert();
                        break;
                    }

                case 16:
                    {
                        commandHandler.freeze();
                        break;
                    }

                case 17:
                    {
                        commandHandler.buyx();
                        break;
                    }

                case 18:
                    {
                        commandHandler.enable();
                        break;
                    }

                case 19:
                    {
                        commandHandler.roommute();
                        break;
                    }

                case 20:
                    {
                        commandHandler.masscredits();
                        break;
                    }

                case 21:
                    {
                        commandHandler.globalcredits();
                        break;
                    }

                case 22:
                    {
                        commandHandler.openroom();
                        break;
                    }

                case 23:
                    {
                        commandHandler.roombadge();
                        break;
                    }

                case 24:
                    {
                        commandHandler.massbadge();
                        break;
                    }

                case 25:
                    {
                        commandHandler.language();
                        break;
                    }

                case 26:
                    {
                        commandHandler.userinfo();
                        break;
                    }

                case 27:
                    {
                        commandHandler.linkAlert();
                        break;
                    }

                case 28:
                    {
                        commandHandler.shutdown();
                        break;
                    }

                case 29:
                    {
                        commandHandler.dumpmaps();
                        break;
                    }

                case 30:
                    {
                        commandHandler.giveBadge();
                        break;
                    }

                case 31:
                    {
                        commandHandler.invisible();
                        break;
                    }

                case 32:
                    {
                        commandHandler.ban();
                        break;
                    }

                case 33:
                    {
                        commandHandler.disconnect();
                        break;
                    }

                case 34:
                    {
                        commandHandler.superban();
                        break;
                    }

                case 35:
                    {
                        commandHandler.langban();
                        break;
                    }

                case 36:
                    {
                        commandHandler.roomkick();
                        break;
                    }

                case 37:
                    {
                        commandHandler.mute();
                        break;
                    }

                case 38:
                    {
                        commandHandler.unmute();
                        break;
                    }

                case 39:
                    {
                        commandHandler.alert();
                        break;
                    }

                case 40:
                    {
                        commandHandler.kick();
                        break;
                    }

                case 41:
                    {
                        commandHandler.commands();
                        break;
                    }

                case 42:
                    {
                        commandHandler.faq();
                        break;
                    }

                case 43:
                    {
                        commandHandler.info();
                        break;
                    }

                case 44:
                    {
                        commandHandler.enablestatus();
                        break;
                    }

                case 45:
                    {
                        commandHandler.disablefriends();
                        break;
                    }

                case 46:
                    {
                        commandHandler.enablefriends();
                        break;
                    }

                case 47:
                    {
                        commandHandler.disabletrade();
                        break;
                    }

                case 48:
                    {
                        commandHandler.enabletrade();
                        break;
                    }

                case 49:
                    {
                        commandHandler.mordi();
                        break;
                    }

                case 50:
                    {
                        commandHandler.wheresmypet();
                        break;
                    }

                case 51:
                    {
                        commandHandler.powerlevels();
                        break;
                    }

                case 52:
                    {
                        commandHandler.forcerot();
                        break;
                    }

                case 53:
                    {
                        commandHandler.seteffect();
                        break;
                    }

                case 54:
                    {
                        commandHandler.empty();
                        break;
                    }

                case 55:
                    {
                        commandHandler.whosonline();
                        break;
                    }

                case 56:
                    {
                        commandHandler.stalk();
                        break;
                    }

                case 57:
                    {
                        commandHandler.registerIRC();
                        break;
                    }
                case 58:
                    {
                        commandHandler.unbanUser();
                        break;
                    }
                case 59:
                    {
                        commandHandler.giveCrystals();
                        break;
                    }
                case 60:
                    {
                        commandHandler.warp();
                        break;
                    }
                case 61:
                    {
                        commandHandler.deleteMission();
                        break;
                    }
                case 62:
                    {
                        break;
                    }
                case 63:
                    {
                        commandHandler.come();
                        break;
                    }
                case 64:
                    {
                        commandHandler.moonwalk();
                        break;
                    }
                case 65:
                    {
                        commandHandler.push();
                        break;
                    }
                case 66:
                    {
                        commandHandler.pull();
                        break;
                    }
                case 67:
                    {
                        commandHandler.copylook();
                        break;
                    }
                case 68:
                    {
                        commandHandler.Fly();
                        break;
                    }
                case 400:
                    {
                        //commandHandler.dario();
                        break;
                    }
            }
        }
        #endregion
    }
}
