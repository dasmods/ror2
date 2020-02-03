using System;
using System.Collections.Generic;
using RoR2;

namespace SlashCommands
{
    public class Commands
    {
        // command names must not have any spaces
        private static Dictionary<string, Func<ICommand>> COMMANDS = new Dictionary<string, Func<ICommand>>
        {
            ["echo"] = () => new Echo(),
            ["hittele"] = () => new ActivateTele()
        };

        public static ICommand GetCommand(string cmdName)
        {
            if (!COMMANDS.ContainsKey(cmdName))
            {
                string msg = $"Unsupported cmdName: {cmdName}";
                Chat.AddMessage(msg);
                throw new ArgumentException(msg);
            }

            Func<ICommand> createCmd = COMMANDS[cmdName];
            return createCmd();
        }
    }

    public interface ICommand
    {
        void Run(string[] args);
    }

    public class Echo : ICommand
    {
        public void Run(string[] args)
        {
            Chat.AddMessage(string.Join(" ", args));
        }
    }

    public class ActivateTele : ICommand
    {
        // from private enum RoR2.TeleporterInteraction.ActivationState
        private static uint IDLE_ACTIVATION_STATE = 0;
        private static uint IDLE_TO_CHARGING_ACTIVATION_STATE = 1;

        public void Run(string[] args)
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