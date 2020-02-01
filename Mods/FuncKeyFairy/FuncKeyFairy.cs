using System;
using System.Collections.Generic;
using BepInEx;
using RoR2;
using UnityEngine;

namespace FuncKeyFairy
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.funckeyfairy", "FuncKeyFairy", "1.0")]
    public class FuncKeyFairy : BaseUnityPlugin
    {
        private static void dropPickup(PickupIndex pickupIndex, GameObject obj)
        {
            Transform transform = obj.transform;
            Vector3 position = transform.position;
            Vector3 velocity = transform.forward * 20f;
            PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
        }

        private static void dropRandPickup(List<PickupIndex> dropList, GameObject obj)
        {
            PickupIndex pickupIndex = Util.pickRand<PickupIndex>(dropList);
            dropPickup(pickupIndex, obj);
        }

        public void Update()
        {
            GameObject player = PlayerCharacterMasterController.instances[0].master.GetBodyObject();
            if (Input.GetKeyDown(KeyCode.F2))
            {
                dropRandPickup(Run.instance.availableTier1DropList, player);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                dropRandPickup(Run.instance.availableTier2DropList, player);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                dropRandPickup(Run.instance.availableTier3DropList, player);
            }
        }
    }
}
