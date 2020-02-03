using System;
using System.Collections.Generic;
using System.Reflection;
using RoR2;
using UnityEngine;

namespace SlashCommands
{
    public class CommandRunner
    {
        private static Dictionary<string, ICommand> COMMANDS_BY_NAME = new Dictionary<string, ICommand>
        {
            ["echo"] = new Echo(),
            ["hittele"] = new ActivateTele()
        };

        public static void ParseAndRun(string rawString)
        {
            // parse
            var parsed = Parse(rawString);
            string cmdName = parsed.Item1;
            string[] args = parsed.Item2;

            // validate
            if (!COMMANDS_BY_NAME.ContainsKey(cmdName))
            {
                OnInvalidCmd(cmdName, args);
            }

            // run
            ICommand cmd = COMMANDS_BY_NAME[cmdName];
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

    public interface ICommand
    {
        void Run(params string[] args);
    }

    public class Echo : ICommand
    {
        public void Run(params string[] args)
        {
            string argsStr = string.Join(" ", args);
            Chat.AddMessage($"echo: {argsStr}");
        }
    }

    public class ActivateTele : ICommand
    {
        // from private enum RoR2.TeleporterInteraction.ActivationState
        private static uint IDLE_ACTIVATION_STATE = 0;
        private static uint IDLE_TO_CHARGING_ACTIVATION_STATE = 1;

        public void Run(params string[] args)
        {
            TeleporterInteraction teleporterInteraction = TeleporterInteraction.instance;
               
            if (teleporterInteraction.NetworkactivationStateInternal != IDLE_ACTIVATION_STATE)
            {
                throw new InvalidOperationException("teleporter must be idle to force tele");
            }

            teleporterInteraction.NetworkactivationStateInternal = IDLE_TO_CHARGING_ACTIVATION_STATE;
        }
    }
}
