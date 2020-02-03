using System;
using System.Collections.Generic;
using RoR2;

namespace SlashCommands
{
    public class Commands
    {
        // command names must not have any spaces
        public static Dictionary<string, ICommand> COMMANDS = new Dictionary<string, ICommand>
        {
            ["echo"] = new Echo(),
            ["hittele"] = new ActivateTele()
        };
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