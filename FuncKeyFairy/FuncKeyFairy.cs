using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace Dasmods
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.funckeyfairy", "FuncKeyFairy", "1.0")]
    public class FuncKeyFairy : BaseUnityPlugin
    {
        private GameObject player;
        private static void dropPickup(PickupIndex pickupIndex, GameObject obj)
        {
            Transform transform = obj.transform;
            Vector3 position = transform.position;
            Vector3 velocity = transform.forward * 20f;
            PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
        }

        private static void dropRandPickup(System.Collections.Generic.List<PickupIndex> dropList, GameObject obj)
        {
            PickupIndex pickupIndex = Util.pickRand<PickupIndex>(dropList);
            dropPickup(pickupIndex, obj);
        }

        public void Awake()
        {
            this.player = PlayerCharacterMasterController.instances[0].master.GetBodyObject();
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                dropRandPickup(Run.instance.availableTier1DropList, this.player);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                dropRandPickup(Run.instance.availableTier2DropList, this.player);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                dropRandPickup(Run.instance.availableTier3DropList, this.player);
            }
        }
    }
}
