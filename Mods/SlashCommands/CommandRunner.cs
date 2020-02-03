using System;
using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace SlashCommands
{
    public class CommandRunner
    {
        public static bool IsCommandStr(string rawString)
        {
            return rawString.StartsWith("/");
        }

        public static void Call(string rawString)
        {
            Validate(rawString);

            (string, string[]) parsed = Parse(rawString);
            string cmdName = parsed.Item1;
            string[] args = parsed.Item2;

            ICommand cmd = Commands.GetCommand(cmdName);

            Log(cmdName, args);

            cmd.Run(args);
        }

        private static void Validate(string rawString)
        {
            if (!IsCommandStr(rawString))
            {
                throw new ArgumentException($"Invalid slash command: {rawString}");
            }
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

        private static void Log(string cmdName, string[] args)
        {
            string argsStr = string.Join(",", args);
            Debug.LogWarning($"Running cmd {cmdName} with args: {argsStr}");
        }
    }
}
