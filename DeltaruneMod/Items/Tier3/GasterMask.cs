using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.CharacterAI;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier3
{
    public class GasterMask : ItemBase<GasterMask>
    {
        public override string ItemName => "Mystery Man's Mask";

        public override string ItemLangTokenName => "GASTER_MASK";

        public override string ItemPickupDesc => "Enemies that stay at low hp become corrupted allies.";

        public override string ItemFullDescription => "Enemies gain <style=cIsDamage>corruption</style> when hit, when below <style=cIsDamage>10%</style> hp" +
            "\nenemies become corrupted allies for <style=cIsUtility>7.5</style> seconds <style=cStack>(+5 seconds per stack)</style>.";

        public override string ItemLore => "\"ARE YOU THERE? ARE WE CONNECTED?\"" +
            "\nThe voice rings through your head, pounding, probing..." +
            "\n\"GO FORWARD, MAKE ALLIES, DON'T <style=cMono>GIVE UP</style>.";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("roaring_blade.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("gaster_mask_icon.png");

        public static GameObject CorruptedEffect;

        public static BuffDef GhasterCorrupt;

        private static Dictionary<CharacterBody, float> lastTimeCloneSpawned = [];

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
                    localPos = new Vector3(0.03424F, 0.24531F, -0.28029F),
                    localAngles = new Vector3(29.02806F, 9.87036F, 0.15779F),
                    localScale = new Vector3(43.11241F, 48.88401F, 48.88401F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.15683F, 0.03304F, -0.05687F),
                    localAngles = new Vector3(8.67311F, 301.7072F, 343.5933F),
                    localScale = new Vector3(32.31651F, 20.56886F, 31.46058F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.03685F, 0.04701F, -0.19537F),
                    localAngles = new Vector3(351.7767F, 8.65373F, 346.0138F),
                    localScale = new Vector3(46.44885F, 26.87783F, 2.25375F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.35631F, 0.30782F, -1.88334F),
                    localAngles = new Vector3(4.96775F, 356.1217F, 343.4172F),
                    localScale = new Vector3(342.7814F, 342.7814F, 342.7814F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.61056F, 0.2345F, 0.19811F),
                    localAngles = new Vector3(351.2091F, 161.5561F, 217.4899F),
                    localScale = new Vector3(50.83732F, 50.83732F, 50.83732F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ClavicleR",
                    localPos = new Vector3(0.19937F, -0.11671F, -0.00412F),
                    localAngles = new Vector3(351.81F, 103.9537F, 14.96851F),
                    localScale = new Vector3(26.79127F, 25.62798F, 28.75669F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.64632F, 0.31625F, 0.05835F),
                    localAngles = new Vector3(350.5506F, 180.2126F, 212.7281F),
                    localScale = new Vector3(63.08693F, 63.08693F, 63.08693F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-1.09289F, -0.17844F, 0.06287F),
                    localAngles = new Vector3(6.32783F, 163.8389F, 43.61153F),
                    localScale = new Vector3(64.85601F, 64.85601F, 64.85601F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(0.05452F, 0.96759F, 0.03388F),
                    localAngles = new Vector3(342.5309F, 38.24583F, 185.2787F),
                    localScale = new Vector3(43.79625F, 43.79625F, 43.79625F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.40245F, -0.11621F, -0.95978F),
                    localAngles = new Vector3(349.0351F, 87.08124F, 49.89027F),
                    localScale = new Vector3(545.4311F, 545.4311F, 545.4311F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.13571F, 0.20722F, -0.26261F),
                    localAngles = new Vector3(6.9968F, 350.0926F, 341.5277F),
                    localScale = new Vector3(44.86234F, 37.9991F, 37.9991F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ToeR",
                    localPos = new Vector3(-0.05914F, 0.23115F, -0.02773F),
                    localAngles = new Vector3(359.2351F, 160.0996F, 254.6561F),
                    localScale = new Vector3(15.71294F, 15.71294F, 15.71294F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.00696F, -0.22984F, -0.10106F),
                    localAngles = new Vector3(19.71435F, 168.1724F, 315.2494F),
                    localScale = new Vector3(35.84454F, 41.6678F, 29.87045F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.21611F, -0.41288F, 0.0538F),
                    localAngles = new Vector3(296.2543F, 222.2426F, 355.6819F),
                    localScale = new Vector3(18.60245F, 27.97708F, 27.97708F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(0.31906F, -0.45957F, -0.1822F),
                    localAngles = new Vector3(336.2322F, 23.51318F, 346.6137F),
                    localScale = new Vector3(47.03091F, 41.80524F, 41.80524F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01625F, 0.17917F, 0.0463F),
                    localAngles = new Vector3(354.49F, 266.0255F, 41.55928F),
                    localScale = new Vector3(55.72091F, 55.72091F, 55.72091F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.00839F, 0.00898F, 0.00293F),
                    localAngles = new Vector3(334.2737F, 179.8516F, 182.9772F),
                    localScale = new Vector3(0.91639F, 0.91639F, 0.91639F)
                }
            });
            return rules;
        }
        
        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            try
            {
                var itemCount = GetCount(sender);
                var happiestItemCount = sender.inventory.GetItemCount((ItemIndex)77);

                if (itemCount > 0 && happiestItemCount > 0)
                {
                    sender.inventory.RemoveItem((ItemIndex)77);
                    sender.inventory.GiveItem(ItemDef);
                }
            } catch { }
            
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            Debug.Log("GASTER 0");
            orig(self, damageInfo, victim);

            Debug.Log("GASTER 1");
            if (!NetworkServer.active) return;
            Debug.Log("GASTER 2");
            var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            var victimBody = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(attackerBody);
            var thresholdHP = 0.1f;
            Debug.Log(victimBody.name);
            if (!attackerBody.inventory || itemCount <= 0) return;
            Debug.Log("GASTER 3");
            if (victimBody.healthComponent.health <= victimBody.maxHealth * thresholdHP)
            {
                Debug.Log("GASTER 4");
                float time = Time.time; // Make sure clone dosent spawn twice
                if (lastTimeCloneSpawned.TryGetValue(attackerBody, out float lastTime))
                {
                    if (time - lastTime < 0.05f) return;
                }
                Debug.Log("GASTER 5");
                lastTimeCloneSpawned[attackerBody] = time;
                Debug.Log(victimBody.name);
                EffectManager.SpawnEffect(CorruptedEffect, new EffectData
                {
                    origin = victimBody.corePosition,
                    scale = 1f,
                }, true);
                ConvertEnemy(victimBody, attackerBody);
                Debug.Log("GASTER 6");
            }
                
        }

        private void CreateBuff()
        {
            GhasterCorrupt = new BuffDef();
            GhasterCorrupt.name = "ghaster_corruption";
            GhasterCorrupt.canStack = false;
            GhasterCorrupt.isDebuff = true;
            GhasterCorrupt.isHidden = true;
            GhasterCorrupt.iconSprite = ItemIcon;

            ContentAddition.AddBuffDef(GhasterCorrupt);
        }

        private void ConvertEnemy(CharacterBody target, CharacterBody owner)
        {
            if (target.isPlayerControlled || target.bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)) return;

            var targetCopy = target;
            target.healthComponent.Suicide();

            targetCopy.rigidbody.velocity = Vector3.zero;
            targetCopy.master.teamIndex = TeamIndex.Void;
            targetCopy.teamComponent.teamIndex = TeamIndex.Void;

            targetCopy.inventory.SetEquipmentIndex(DLC1Content.Elites.Void.eliteEquipmentDef.equipmentIndex);

            BaseAI ai = targetCopy.master.GetComponent<BaseAI>();
            if (ai)
            {
                ai.enemyAttention = 0f;
                ai.ForceAcquireNearestEnemyIfNoCurrentEnemy();
            }

            var conversionTime = 2 + (GetCount(owner) * 5);
            RoR2.Util.TryToCreateGhost(targetCopy, owner, conversionTime);
        }

        public void CreateEffect()
        {
            CorruptedEffect = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("wingshit.prefab"), "gaster_corrupt", true);
            CorruptedEffect.transform.localScale = new Vector3(100f, 100f, 100f);
            Util.Helpers.CreateNetworkedEffectPrefab(CorruptedEffect);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            CreateEffect();
            Hooks();
        }
    }
}
