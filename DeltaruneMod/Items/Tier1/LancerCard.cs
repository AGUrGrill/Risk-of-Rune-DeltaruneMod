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

namespace DeltaruneMod.Items.Tier1
{
    public class LancerCard : ItemBase<LancerCard>
    {
        public override string ItemName => "Jack of Spades";
        public override string ItemLangTokenName => "LANCER_CARD";
        public override string ItemPickupDesc => "Free unlock on stage start.";
        public override string ItemFullDescription => "Gain <style=cIsUtility>1</style> free unlock per stage <style=cStack>(+1 per 2 collected afterwards)</style>";
        public override string ItemLore => "You hear a faint hohoho...\nThis is just a card right..?";
        public override ItemTier Tier => ItemTier.Tier1;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("lancer_card.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("lancer_card_icon.png");
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.OnStageBeginEffect };

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

            orig(self, stage);
        }

        public void LancerCardEffect(CharacterBody sender)
        {
            if (!NetworkServer.active && !sender.isPlayerControlled || !canUseEffect) return;

            var itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    if (i == 0 || i % 2 == 0)
                        sender.AddBuff(DLC2Content.Buffs.FreeUnlocks.buffIndex);
                    Debug.Log($"Added lancer unlock effect to {sender.name}");
                }
                AkSoundEngine.PostEvent(2353227689, sender.gameObject);
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
                    localPos = new Vector3(0.05621F, -0.01305F, 0.08701F),
                    localAngles = new Vector3(336.7992F, 346.0364F, 319.8058F),
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
                    localPos = new Vector3(-0.26323F, 0.06472F, -0.00032F),
                    localAngles = new Vector3(40.349F, 147.5673F, 337.1469F),
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
                    localPos = new Vector3(-1.19104F, 1.84019F, 1.95193F),
                    localAngles = new Vector3(359.9579F, 12.5627F, 352.9454F),
                    localScale = new Vector3(180F, 180F, 180F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.25831F, -0.01319F, 0.00067F),
                    localAngles = new Vector3(352.5149F, 166.6494F, 141.9863F),
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
                    localPos = new Vector3(0.031F, 0.1832F, 0.08832F),
                    localAngles = new Vector3(353.5615F, 90.10542F, 144.1951F),
                    localScale = new Vector3(12F, 12F, 12F)
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
                    localAngles = new Vector3(2.88064F, 153.149F, 143.3174F),
                    localScale = new Vector3(20F, 20F, 20F)

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
                    localAngles = new Vector3(341.0073F, 109.2714F, 2.21412F),
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
                    localPos = new Vector3(-0.1606F, 0.11101F, 0.1941F),
                    localAngles = new Vector3(5.31841F, 245.6385F, 338.9428F),
                    localScale = new Vector3(24F, 24F, 24F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(-0.23574F, -0.03458F, 0.09329F),
                    localAngles = new Vector3(38.25168F, 191.5127F, 279.8038F),
                    localScale = new Vector3(200F, 200F, 200F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfL",
                    localPos = new Vector3(-0.00602F, -0.02117F, -0.07068F),
                    localAngles = new Vector3(355.5136F, 266.9259F, 124.83F),
                    localScale = new Vector3(15F, 15F, 15F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.05233F, 0.07269F, 0.14273F),
                    localAngles = new Vector3(6.88681F, 285.7394F, 323.3983F),
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
                    childName = "GunBarrel",
                    localPos = new Vector3(-0.02802F, 0.17708F, -0.13867F),
                    localAngles = new Vector3(285.2631F, 200.9528F, 236.2997F),
                    localScale = new Vector3(12F, 12F, 12F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.00512F, 0.04804F, -0.20226F),
                    localAngles = new Vector3(342.1225F, 91.86257F, 310.8853F),
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
                    localPos = new Vector3(-0.08207F, -0.47537F, 0.27714F),
                    localAngles = new Vector3(14.88056F, 194.516F, 218.5694F),
                    localScale = new Vector3(28F, 28F, 28F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfR",
                    localPos = new Vector3(0.049F, 0.27209F, 0.01171F),
                    localAngles = new Vector3(349.9406F, 174.9632F, 139.0277F),
                    localScale = new Vector3(15F, 15F, 15F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.15619F, 0.47124F, -0.87054F),
                    localAngles = new Vector3(14.75215F, 229.616F, 57.08662F),
                    localScale = new Vector3(30F, 30F, 30F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.00671F, 0.00172F, -0.00149F),
                    localAngles = new Vector3(10.40342F, 336.8046F, 152.8039F),
                    localScale = new Vector3(1F, 1F, 1F)

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
