using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class LancerCard : ItemBase<LancerCard>
    {
        public override string ItemName => "Jack of Spades";
        public override string ItemLangTokenName => "LANCER_CARD";
        public override string ItemPickupDesc => "Free unlock on stage start.";
        public override string ItemFullDescription => "Gain 1 free unlock per stage (+1 per 2 collected afterwards)";
        public override string ItemLore => "You do hear a faint hohoho...\nThis is just a card right..?";
        public override ItemTier Tier => ItemTier.Tier1;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("lancer_card.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("lancer_card_icon.png");
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };
        private bool canUseEffect = false;

        public override void Init()
        {
            CreateLang();
            CreateItem();
            Hooks();
        }

        
        public override void Hooks()
        {
            CharacterBody.onBodyStartGlobal += LancerCardEffect;
            On.RoR2.CharacterMaster.OnServerStageBegin += CharacterMaster_OnServerStageBegin;
        }

        private void CharacterMaster_OnServerStageBegin(On.RoR2.CharacterMaster.orig_OnServerStageBegin orig, CharacterMaster self, Stage stage)
        {
            try
            {
                if (stage.sceneDef.cachedName != "bazaar")
                {
                    canUseEffect = true;
                }
                else canUseEffect = false;
                //Debug.Log("Can use lancer effect: " + canUseEffect);
            }
            catch { Debug.Log("Issue checking stage for " + PluginName); } 
        }

        public void LancerCardEffect(CharacterBody sender)
        {
            if (!NetworkServer.active && !sender.isPlayerControlled || !canUseEffect) return;

            var itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    if (i == 0 || (i % 2 == 0))
                        sender.AddBuff(DLC2Content.Buffs.FreeUnlocks.buffIndex);
                    Debug.Log($"Added lancer unlock effect to {sender.name}");
                }  
            }
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.07338F, -0.01371F, 0.06854F),
                    localAngles = new Vector3(340.6402F, 339.2884F, 271.4926F),
                    localScale = new Vector3(20F, 20F, 20F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "BowBase",
                    localPos = new Vector3(-0.26355F, 0.06285F, 0.00232F),
                    localAngles = new Vector3(65.41045F, 113.2841F, 275.2764F),
                    localScale = new Vector3(20F, 20F, 20F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-1.05724F, 2.01082F, 1.97824F),
                    localAngles = new Vector3(1.59236F, 4.04869F, 310.2545F),
                    localScale = new Vector3(180F, 180F, 180F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.16299F, -0.0141F, -0.01082F),
                    localAngles = new Vector3(352.1657F, 9.08538F, 89.4313F),
                    localScale = new Vector3(20F, 20F, 20F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmR",
                    localPos = new Vector3(0.03357F, 0.18668F, 0.08785F),
                    localAngles = new Vector3(1.5598F, 99.57597F, 89.17175F),
                    localScale = new Vector3(9F, 9F, 9F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.08579F, 0.03093F, 0.06917F),
                    localAngles = new Vector3(4.72732F, 162.5068F, 101.3186F),
                    localScale = new Vector3(15F, 15F, 15F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(0.19925F, 0.49019F, 0.65711F),
                    localAngles = new Vector3(4.30325F, 116.4788F, 310.6998F),
                    localScale = new Vector3(30F, 30F, 30F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.16061F, 0.11169F, 0.19605F),
                    localAngles = new Vector3(357.3508F, 232.417F, 288.577F),
                    localScale = new Vector3(15F, 15F, 15F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    /* IN SPIKE
                    childName = "SpineChest1",
                    localPos = new Vector3(-1.15837F, 0.56798F, -0.5748F),
                    localAngles = new Vector3(7.83539F, 152.3057F, 307.5611F),
                    localScale = new Vector3(150F, 150F, 150F)
                    */
                    childName = "Head",
                    localPos = new Vector3(-0.4082F, 4.57236F, 1.54619F),
                    localAngles = new Vector3(54.89391F, 264.5713F, 274.137F),
                    localScale = new Vector3(150F, 150F, 150F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfL",
                    localPos = new Vector3(-0.00526F, -0.01909F, -0.07054F),
                    localAngles = new Vector3(1.77072F, 265.6638F, 81.00603F),
                    localScale = new Vector3(15F, 15F, 15F)


                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.07663F, -0.023F, -0.1486F),
                    localAngles = new Vector3(359.1839F, 67.41568F, 269.528F),
                    localScale = new Vector3(15F, 15F, 15F)
                }
            });
            rules.Add("mdlHeretic", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.02105F, -0.87071F, 0.01385F),
                    localAngles = new Vector3(355.2848F, 47.55381F, 355.0908F),
                    localScale = new Vector3(0.20392F, 0.20392F, 0.20392F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.10607F, 0.13683F, -0.00698F),
                    localAngles = new Vector3(358.747F, 175.4618F, 259.2015F),
                    localScale = new Vector3(12F, 12F, 12F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LargeExhaust2R",
                    localPos = new Vector3(0.11295F, -0.05305F, 0.18708F),
                    localAngles = new Vector3(20.83908F, 342.4109F, 351.6339F),
                    localScale = new Vector3(18F, 18F, 18F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.07766F, -0.45351F, 0.2643F),
                    localAngles = new Vector3(0.28656F, 193.4864F, 174.0694F),
                    localScale = new Vector3(21F, 21F, 21F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfR",
                    localPos = new Vector3(0.06344F, 0.11426F, 0.02464F),
                    localAngles = new Vector3(6.67547F, 181.9402F, 90.91825F),
                    localScale = new Vector3(15F, 15F, 15F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.28028F, 0.9131F, -0.71561F),
                    localAngles = new Vector3(20.01395F, 232.2571F, 48.91186F),
                    localScale = new Vector3(24F, 24F, 24F)
                }
            });
            return rules;
        }

        /* Lunar Item Conept
        public static void LancerCardDebuffEffect()
        {
            foreach (CharacterBody body in CharacterBody.readOnlyInstancesList)
            {
                if (body.isPlayerControlled)
                {
                    characterBody = body;
                    Debug.Log("Found player: " + body.name);
                }
            }

            if (characterBody == null || !NetworkServer.active || characterBody.inventory == null) return;

            var itemCount = characterBody.inventory.GetItemCount(lancerCardItemDef);
            if (characterBody.inventory && itemCount > 0)
            {
                characterMaster = characterBody.master;
                var moners = characterMaster.money;
                Debug.Log("$: " + moners);
                if (moners > 0) characterMaster.GiveMoney((uint)(-moners));
            }
        }
        */
    }
}
