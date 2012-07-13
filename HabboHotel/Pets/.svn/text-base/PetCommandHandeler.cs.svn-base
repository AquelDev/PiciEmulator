using System.Collections.Generic;
using Butterfly.Core;

namespace Butterfly.HabboHotel.Pets
{
    class PetCommandHandeler
    {
        private static Dictionary<int, string> commandRegister;
        private static Dictionary<string, PetCommand> petCommands;

        internal static void Init()
        {
            commandRegister = IniReader.ReadFileWithInt(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/commands_register.pets.ini"));
            petCommands = new Dictionary<string, PetCommand>();

            InitCommands();
        }

        private static void InitCommands()
        {
            Dictionary<string, string> commandDatabase = IniReader.ReadFile(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath,@"System/commands.pets.ini"));

            foreach (KeyValuePair<int, string> pair in commandRegister)
            {
                int commandID = pair.Key;
                string commandStringedID = pair.Value;
                string[] commandInput = commandDatabase[commandStringedID + ".input"].Split(',');

                foreach (string command in commandInput)
                {
                    petCommands.Add(command, new PetCommand(commandID, command));
                }
            }
        }

        internal static int TryInvoke(string input)
        {
            PetCommand command;
            if (petCommands.TryGetValue(input, out command))
                return command.commandID;
            else
                return 0;
        }
    }
}
