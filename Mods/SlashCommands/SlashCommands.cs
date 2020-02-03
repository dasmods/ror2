using System;
using BepInEx;
using UnityEngine;
using RoR2;

namespace SlashCommands
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.slashcommands", "SlashCommands", "1.0")]
    public class SlashCommands : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.Chat.UserChatMessage.OnProcessed += (onProcessed, userChatMessage) =>
            {
                onProcessed(userChatMessage);

                if (CommandRunner.IsCommandStr(userChatMessage.text))
                {
                    try
                    {
                        CommandRunner.Call(userChatMessage.text);
                    }
                    catch (Exception exception)
                    {
                        Chat.AddMessage($"Error running command: {exception.Message}");
                        Debug.LogError(exception);
                    }
                }
            };
        }
    }
}
