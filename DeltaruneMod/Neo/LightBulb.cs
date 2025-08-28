using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Neo
{
    public class LightBulb : ItemBase<LightBulb>
    {
        public override string ItemName => "Fractured Light";

        public override string ItemLangTokenName => "LIGHT_BULB";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("ok.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ok.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static float damageMultiplier = 0.25f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.Orbs.LightningOrb.OnArrival += LightningOrb_OnArrival; // uke tesla BFG arti loader 
            On.RoR2.Orbs.SimpleLightningStrikeOrb.OnArrival += SimpleLightningStrikeOrb_OnArrival; ; // charged perforator
            On.RoR2.Orbs.LightningStrikeOrb.OnArrival += LightningStrikeOrb_OnArrival; // royal capacitor
            On.RoR2.Orbs.VoidLightningOrb.OnArrival += VoidLightningOrb_OnArrival; // polylute
        }

        private void VoidLightningOrb_OnArrival(On.RoR2.Orbs.VoidLightningOrb.orig_OnArrival orig, VoidLightningOrb self)
        {
            var attacker = self.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                if (body && body.isPlayerControlled)
                {
                    self.damageValue *= 1 + (damageMultiplier * GetCount(body));
                }
            }
            orig(self);
        }

        private void LightningStrikeOrb_OnArrival(On.RoR2.Orbs.LightningStrikeOrb.orig_OnArrival orig, LightningStrikeOrb self)
        {
            var attacker = self.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                if (body && body.isPlayerControlled)
                {
                    self.damageValue *= 1 + (damageMultiplier * GetCount(body));
                }
            }
            orig(self);
        }

        private void SimpleLightningStrikeOrb_OnArrival(On.RoR2.Orbs.SimpleLightningStrikeOrb.orig_OnArrival orig, SimpleLightningStrikeOrb self)
        {
            var attacker = self.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                if (body && body.isPlayerControlled)
                {
                    self.damageValue *= 1 + (damageMultiplier * GetCount(body));
                }
            }
            orig(self);
        }

        private void LightningOrb_OnArrival(On.RoR2.Orbs.LightningOrb.orig_OnArrival orig, LightningOrb self)
        {
            var attacker = self.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                if (body && body.isPlayerControlled)
                {
                    self.damageValue *= 1 + (damageMultiplier * GetCount(body));
                }
            }
            orig(self);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
