using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace SlashCommands
{
    public class CommandRunner
    {
        public static void ParseAndRun(string rawString)
        {
            // parse
            var parsed = Parse(rawString);
            string cmdName = parsed.Item1;
            string[] args = parsed.Item2;

            // validate
            if (!Commands.COMMANDS.ContainsKey(cmdName))
            {
                OnInvalidCmd(cmdName, args);
            }

            // run
            ICommand cmd = Commands.COMMANDS[cmdName];
            Debug.LogWarning($"Running cmd {cmdName} with args: {args}");

            try
            {
                cmd.Run(args);
            }
            catch (Exception exception)
            {
                Chat.AddMessage($"Error running cmd {cmdName}: {exception.Message}");
            }
        }

        private static void OnInvalidCmd(string cmdName, string[] args)
        {
            string argsStr = string.Join(",", args);
            Chat.AddMessage($"Invalid cmd: {cmdName}");
            throw new ArgumentException($"Unsupported cmdName: {cmdName} (args: {argsStr})");
        }

        private static (string cmdName, string[] args) Parse(string rawString)
        {
            string[] tokens = rawString.Remove(0, 1).Split(' ');
            string cmdName = tokens[0];
            List<string> args = new List<string>();

            for (int ndx = 1; ndx < tokens.Length; ndx++)
            {
                args.Add(tokens[ndx]);
            }

            return (cmdName, args.ToArray());
        }
    }
}
