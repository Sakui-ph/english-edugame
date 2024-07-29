using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS {
    public class CommandDatabase
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        public bool HasCommand(string commandName) => database.ContainsKey(commandName.ToLower());

        public void AddCommand(string commandName, Delegate command)
        {
            
            if (HasCommand(commandName))
                Debug.LogError($"CommandDatabase already contains command with name {commandName}");
            else
                database.Add(commandName, command);
        }

        public Delegate GetCommand(string commandName)
        {
            if (!HasCommand(commandName))
            {
                Debug.LogError($"CommandDatabase does not contain command with name {commandName}");
                return null;
            }
    
            return database[commandName];
        }
    }
}
