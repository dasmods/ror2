using System;
using BepInEx;
using RoR2;
using UnityEngine;

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
                if (userChatMessage.text.StartsWith("/"))
                {
                    Debug.Log($"received slash command: {userChatMessage.text}");
                    CommandRunner.ParseAndRun(userChatMessage.text);
                }
                onProcessed(userChatMessage);
            };
        }
    }
}
