using DeltaruneMod.Elites;
using DeltaruneMod.Items.Lunar;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DeltaruneMod.NeoMithrix
{
    public class NeoMithrixController
    {
        // BrotherGlassBody(Clone), BrotherBody(Clone), BrotherHurtBody(Clone), ITBrotherBody(Clone)
        static bool playerHadThornRing = false;

        public NeoMithrixController()
        {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
        }

        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body)
        {
            orig(self,body);

            // Check if throrn ring is present
            if (body.isPlayerControlled && body.inventory.GetItemCount(ThornRing.instance.ItemDef) > 0)
            {
                playerHadThornRing = true;
                Debug.Log("Player had thorn ring.");
            }

            if (!playerHadThornRing) return;

            // Give mithrix funny item 
            if (body.name == "BrotherBody(Clone)" && body.inventory.GetItemCountPermanent(NeoMithrixItem.instance.ItemDef) <= 0)
            {
                self.inventory.GiveItemPermanent(NeoMithrixItem.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLeftWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixRightWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLimb.instance.ItemDef);
                self.inventory.SetEquipmentIndex(NeoElite.instance.EliteEquip.equipmentIndex, true);
                Debug.Log("Giving basic mithrix item.");
            }

            else if (body.name == "BrotherGlassBody(Clone)" && body.inventory.GetItemCountPermanent(NeoMithrixItem.instance.ItemDef) <= 0)
            {
                self.inventory.GiveItemPermanent(NeoMithrixItem.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLeftWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixRightWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLimb.instance.ItemDef);
                self.inventory.SetEquipmentIndex(NeoElite.instance.EliteEquip.equipmentIndex, true);
                Debug.Log("Giving glass mithrix item.");
            }

            else if (body.name == "BrotherHurtBody(Clone)" && body.inventory.GetItemCountPermanent(NeoMithrixItem.instance.ItemDef) <= 0)
            {
                self.inventory.GiveItemPermanent(NeoMithrixItem.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLeftWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixRightWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLimb.instance.ItemDef);
                self.inventory.SetEquipmentIndex(NeoElite.instance.EliteEquip.equipmentIndex, true);
                Debug.Log("Giving hurt mithrix item.");
            }

            else if (body.name == "ITBrotherBody(Clone)" && body.inventory.GetItemCountPermanent(NeoMithrixItem.instance.ItemDef) <= 0)
            {
                self.inventory.GiveItemPermanent(NeoMithrixItem.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLeftWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixRightWing.instance.ItemDef);
                self.inventory.GiveItemPermanent(NeoMithrixLimb.instance.ItemDef);
                self.inventory.SetEquipmentIndex(NeoElite.instance.EliteEquip.equipmentIndex, true);
                Debug.Log("Giving IT mithrix item.");
            }
        }
    }
}
