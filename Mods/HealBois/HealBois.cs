using System;
using BepInEx;
using RoR2;

namespace HealBois
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.healbois", "HealBois", "1.0")]
    public class HealBois : BaseUnityPlugin
    {
        void Awake() {
            Chat.AddMessage("HealBois!");
        }
    }
}
