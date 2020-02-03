using BepInEx;
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
                if (CommandRunner.IsCommandStr(userChatMessage.text))
                {
                    CommandRunner.Call(userChatMessage.text);
                }

                onProcessed(userChatMessage);
            };
        }
    }
}
