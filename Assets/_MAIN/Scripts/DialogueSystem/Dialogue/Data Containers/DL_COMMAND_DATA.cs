using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace DIALOGUE
{
    public class DL_COMMAND_DATA
    {
        private const char COMMAND_SPLITTER_ID = ',';
        private const char ARGUMENT_CONTAINER_ID = '(';
        private const string WAIT_COMMAND_ID = "[wait]";
        public List<Command> commands;
        public struct Command
        {
            public string name;
            public string[] args;
            public bool waitForCompletion;
        }

        public DL_COMMAND_DATA(string rawCommands)
        {
            commands = RipCommands(rawCommands);
        }

        public List<Command> RipCommands(string rawCommands)
        {
            string[] data = rawCommands.Split(COMMAND_SPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
            List<Command> result = new List<Command>();
    
            foreach (string commandString in data)
            {
                Command command = new Command();
                string[] parts = commandString.Split(ARGUMENT_CONTAINER_ID);
                command.name = parts[0].Trim();

                if (command.name.ToLower().StartsWith(WAIT_COMMAND_ID))
                {
                    command.waitForCompletion = true;
                    command.name = command.name.Substring(WAIT_COMMAND_ID.Length);
                }
                else
                    command.waitForCompletion = false;

                command.args = GetArgs(parts[1].Trim().TrimEnd(')'));
                result.Add(command);
            }
            return result;
        }

        private string[] GetArgs(string args)
        {
            List<string> argList = new List<string>();
            StringBuilder currentArg = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (!inQuotes && args[i] == ' ')
                {
                    argList.Add(currentArg.ToString());
                    currentArg.Clear();
                    continue;
                }

                currentArg.Append(args[i]);
            }

            if (currentArg.Length > 0)
                argList.Add(currentArg.ToString());

            return argList.ToArray();
        }
    }

}
