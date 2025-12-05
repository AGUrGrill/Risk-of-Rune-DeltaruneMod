using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Spamton
{
    public class LightBulb : ItemBase<LightBulb>
    {
        public override string ItemName => "Fractured Light";

        public override string ItemLangTokenName => "LIGHT_BULB";

        public override string ItemPickupDesc => "Increase all lightning damage by 25%.";

        public override string ItemFullDescription => "All forms of lightning damage are increased by <style=cIsUtility>25%</style> <style=cStack>(+25% per stack)</style>.";

        public override string ItemLore => "A dim light shines from the dark..." +
            "\nIt flickers faintly. The light that showed the way is gone." +
            "\nYou feel the darkness swell around you on all sides until suddenly the bulb is on again." +
            "\nThe void retreats as you grab the bulb and run back to where you came from." +
            "\n\"Does its light repel the dark... or is the dark scared of something greater, more ominous from within?\"";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("light_bulb.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("light_bulb_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static float damageMultiplier = 0.25f;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };
        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.15149F, 0.44434F, -0.11223F),
                    localAngles = new Vector3(15.5804F, 308.87F, 36.10917F),
                    localScale = new Vector3(4.97053F, 4.97053F, 4.97053F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.18911F, 0.20662F, -0.05561F),
                    localAngles = new Vector3(28.58928F, 22.22084F, 271.2374F),
                    localScale = new Vector3(5.50717F, 5.01193F, 5.50717F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-1.93378F, 2.60226F, 1.85581F),
                    localAngles = new Vector3(13.14056F, 265.5432F, 359.9856F),
                    localScale = new Vector3(41.06096F, 41.06096F, 41.06096F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CannonHeadL",
                    localPos = new Vector3(0.17584F, 0.23346F, 0.18431F),
                    localAngles = new Vector3(351.3961F, 300.2342F, 273.4574F),
                    localScale = new Vector3(7.02379F, 6.73428F, 6.73428F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(-0.04949F, -0.14778F, 0.12593F),
                    localAngles = new Vector3(68.03916F, 230.585F, 238.6301F),
                    localScale = new Vector3(5.39778F, 5.39778F, 5.39778F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.00234F, 0.05735F, 0.09209F),
                    localAngles = new Vector3(297.0692F, 265.8345F, 263.1639F),
                    localScale = new Vector3(4.31714F, 4.31714F, 4.31714F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "PlatformBase",
                    localPos = new Vector3(-0.5871F, 1.447F, -0.59961F),
                    localAngles = new Vector3(20.805F, 216.0389F, 354.5287F),
                    localScale = new Vector3(7.52245F, 7.52245F, 7.52245F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechUpperArmL",
                    localPos = new Vector3(0.11877F, 0.35057F, -0.00006F),
                    localAngles = new Vector3(57.87173F, 342.4818F, 255.1724F),
                    localScale = new Vector3(5.50575F, 5.50575F, 5.50575F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(1.83877F, 1.022F, 3.80196F),
                    localAngles = new Vector3(52.74911F, 273.6116F, 226.3601F),
                    localScale = new Vector3(47.67143F, 45.41201F, 47.67143F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.20673F, 0.17749F, -0.09678F),
                    localAngles = new Vector3(12.17691F, 327.2426F, 51.37902F),
                    localScale = new Vector3(3.38769F, 3.38769F, 3.38769F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.16504F, 0.15646F, -0.209F),
                    localAngles = new Vector3(333.8095F, 20.49624F, 284.8171F),
                    localScale = new Vector3(3.62923F, 3.62923F, 3.62923F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootL",
                    localPos = new Vector3(0.0831F, 0.16522F, 0.00469F),
                    localAngles = new Vector3(322.6733F, 349.8335F, 260.642F),
                    localScale = new Vector3(4.38116F, 4.30193F, 3.80032F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Cleaver",
                    localPos = new Vector3(-0.02F, 0.50133F, -0.02706F),
                    localAngles = new Vector3(315.6261F, 81.94097F, 277.6905F),
                    localScale = new Vector3(5.13661F, 4.40281F, 4.40281F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "UpperArmR",
                    localPos = new Vector3(-0.03095F, 0.08804F, 0.06031F),
                    localAngles = new Vector3(307.6126F, 114.2467F, 35.58443F),
                    localScale = new Vector3(5.30269F, 4.7135F, 4.7135F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.13591F, 0.66485F, -0.03529F),
                    localAngles = new Vector3(353.4294F, 125.2011F, 323.0759F),
                    localScale = new Vector3(5.38082F, 5.38082F, 5.38082F)
                }
            });
            return rules;
        }

        public override void Hooks()
        {
            // Concept adapted from Startstorm 2
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
                    self.damageValue *= 1 + damageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + damageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + damageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + damageMultiplier * GetCount(body);
                }
            }
            orig(self);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("light_bulb.prefab").InstantiateClone("LightBulbPickup", false);
            pickupModel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            ItemDef.pickupModelPrefab = pickupModel;
        }
    }
}
