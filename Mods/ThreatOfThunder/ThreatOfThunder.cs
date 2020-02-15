using System;
using BepInEx;
using UnityEngine;

namespace ThreatOfThunder
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.threatofthunder", "ThreatOfThunder", "1.0")]
    public class ThreatOfThunder : BaseUnityPlugin
    {
        public void Awake()
        {
            Debug.LogError("ThreatOfThunder loaded!");
        }
    }
}
