using System.Reflection;
using BepInEx;
using EntityStates.Engi.Mine;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace HealBois
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.healbois", "HealBois", "1.0")]
    public class HealBois : BaseUnityPlugin
    {
        private static bool IsMineStuck(WaitForStick waitForStick)
        {
            ProjectileStickOnImpact projectileStickOnImpact = (ProjectileStickOnImpact)typeof(WaitForStick)
                .GetProperty("projectileStickOnImpact", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(waitForStick);

            return projectileStickOnImpact && projectileStickOnImpact.stuck;
        }

        public void Awake()
        {
            On.EntityStates.Engi.Mine.WaitForStick.FixedUpdate += (fixedUpdate, waitForStick) =>
            {
                if (NetworkServer.active && IsMineStuck(waitForStick))
                {
                    waitForStick.outer.SetNextState(new Heal());
                }
            };
        }
    }
}
