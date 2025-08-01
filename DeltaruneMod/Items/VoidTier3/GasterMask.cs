using R2API;
using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static DeltaruneMod.Util.Components;
using static RoR2.UI.HGHeaderNavigationController;

namespace DeltaruneMod.Items.VoidTier3
{
    public class GasterMask : ItemBase<GasterMask>
    {
        public override string ItemName => "Mystery Man's Mask";

        public override string ItemLangTokenName => "GASTER_MASK";

        public override string ItemPickupDesc => "Enemies that fall below 10% hp become corrupted allies.";

        public override string ItemFullDescription => "Enemies gain <style=cIsDamage>corruption</style> when hit, when below <style=cIsDamage>10%</style> hp" +
            "\nenemies become corrupted allies for <style=cIsUtility>7.5</style> seconds <style=cStack>(+5 seconds per stack)</style>.";

        public override string ItemLore => "ENTRY NUMBER SEVENTEEN" +
            "\nDARK DARKER YET DARKER" +
            "\nTHE DARKNESS KEEPS GROWING" +
            "\nTHE SHADOWS CUTTING DEEPER" +
            "\nPHOTON READINGS NEGATIVE" +
            "\nTHIS NEXT EXPERIMENT SEEMS" +
            "\nVERY" +
            "\n<style=cMono>VERY</style>" +
            "\nINTERESTING" +
            "\n..." +
            "\nWHAT DO YOU TWO THINK?";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("jim_carry.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("gaster_mask_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject CorruptedEffect;

        public static GameObject CorruptedEffectHolder;

        public static BuffDef CorruptedBuff;

        private static Dictionary<CharacterBody, float> lastTimeCloneSpawned = [];

        private static int CorruptConversionTime;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00484F, 0.15576F, 0.17125F),
                    localAngles = new Vector3(1.25397F, 11.50918F, 6.27662F),
                    localScale = new Vector3(20.17389F, 16.41514F, 14.26258F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.06212F, 0.18936F, 0.06251F),
                    localAngles = new Vector3(343.725F, 48.58814F, 87.27997F),
                    localScale = new Vector3(20.71162F, 11.90766F, 20.163F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.04526F, 0.43044F, 0.00021F),
                    localAngles = new Vector3(310.7578F, 272.6447F, 200.3735F),
                    localScale = new Vector3(8.16914F, 10.08745F, 22.35655F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.03019F, 2.26872F, -1.59739F),
                    localAngles = new Vector3(352.7149F, 184.7088F, 5.8332F),
                    localScale = new Vector3(156.4683F, 154.412F, 137.6366F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.02221F, -0.14321F, 0.14116F),
                    localAngles = new Vector3(23.69127F, 0.49073F, 4.00108F),
                    localScale = new Vector3(18.76241F, 18.76241F, 23.30275F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00277F, -0.00501F, 0.06621F),
                    localAngles = new Vector3(44.7427F, 2.09911F, 11.68187F),
                    localScale = new Vector3(13.09389F, 12.52534F, 17.95528F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00327F, 0.00105F, 0.13535F),
                    localAngles = new Vector3(17.26723F, 1.85906F, 3.79916F),
                    localScale = new Vector3(16.98133F, 16.98133F, 16.98133F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(0.01572F, -0.71368F, 0.45949F),
                    localAngles = new Vector3(35.03239F, 355.333F, 5.21368F),
                    localScale = new Vector3(10.81663F, 11.5587F, 15.57455F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.00682F, 0.01759F, 0.12759F),
                    localAngles = new Vector3(21.5564F, 357.711F, 8.21154F),
                    localScale = new Vector3(16.32249F, 16.32249F, 16.32249F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmR",
                    localPos = new Vector3(0.64784F, 4.53546F, -0.38265F),
                    localAngles = new Vector3(328.7607F, 126.7533F, 196.0015F),
                    localScale = new Vector3(105.2913F, 105.2913F, 105.2913F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.00626F, -0.05815F, 0.06957F),
                    localAngles = new Vector3(45.36594F, 6.86109F, 4.36906F),
                    localScale = new Vector3(20.47853F, 22.18686F, 16.38765F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfR",
                    localPos = new Vector3(0.03037F, 0.4968F, -0.00454F),
                    localAngles = new Vector3(322.3176F, 103.4467F, 189.6833F),
                    localScale = new Vector3(5.93988F, 5.93988F, 5.93988F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.15373F, 0.24446F, 0.03026F),
                    localAngles = new Vector3(340.4142F, 73.74163F, 188.0902F),
                    localScale = new Vector3(11.62083F, 14.08597F, 13.00007F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01803F, 0.07721F, -0.01165F),
                    localAngles = new Vector3(301.0483F, 110.7157F, 352.321F),
                    localScale = new Vector3(27.02637F, 26.15091F, 23.47674F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00883F, 0.00561F, 0.08645F),
                    localAngles = new Vector3(31.61168F, 16.3422F, 8.38587F),
                    localScale = new Vector3(16.20963F, 17.43578F, 17.43578F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02389F, 0.08106F, 0.09232F),
                    localAngles = new Vector3(39.72403F, 14.81765F, 12.42787F),
                    localScale = new Vector3(35.0727F, 35.0727F, 35.0727F)
                }
            });
            /*
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
            */
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
                // Convert item if happiest mask is present (item index 77)
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
            orig(self, damageInfo, victim);

            if (!NetworkServer.active) return;

            var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            var victimBody = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(attackerBody);
            var thresholdHP = 0.1f;

            if (!attackerBody.inventory || itemCount <= 0) return;

            // Add visual buff
            if (!victimBody.HasBuff(CorruptedBuff)) victimBody.AddBuff(CorruptedBuff);

            // Convert is hp is low enough
            if (victimBody.healthComponent.health <= victimBody.maxHealth * thresholdHP)
            {
                float time = Time.time; // Make sure clone dosent spawn twice
                if (lastTimeCloneSpawned.TryGetValue(attackerBody, out float lastTime))
                {
                    if (time - lastTime < 0.05f) return;
                }
                lastTimeCloneSpawned[attackerBody] = time;

                // Get this guy outta here!!
                if (victimBody.name != "VoidInfestorBody(Clone)" && victimBody.name != "VoidInfestorBody"
                    && victimBody.master.name != "VoidInfestorMaster(Clone)" && victimBody.master.name != "VoidInfestorMaster")
                    ConvertEnemy(victimBody, attackerBody);
            }       
        }
        
        private void ConvertEnemy(CharacterBody target, CharacterBody owner)
        {
            if (!NetworkServer.active) return;

            if (target.isPlayerControlled || target.bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)) return;
            if (target.GetComponent<ThingyMaBobber>()) return;

            target.gameObject.AddComponent<ThingyMaBobber>();

            // Destroy old target and set models to not release on death
            var targetCopy = target;
            var modelLocator = target.modelLocator;
            if (modelLocator) modelLocator.dontReleaseModelOnDeath = true;
            modelLocator = targetCopy.modelLocator;
            if (modelLocator) modelLocator.dontReleaseModelOnDeath = true;
            target.healthComponent.Suicide();
            UnityEngine.Object.Destroy(target);

            // Setup target copy
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

            // Create ally
            CorruptConversionTime = 2 + GetCount(owner) * 5;
            var voidAlly = RoR2.Util.TryToCreateGhost(targetCopy, owner, CorruptConversionTime);
            
            // Stop corpse from spawning
            modelLocator = voidAlly.modelLocator;
            if (modelLocator) modelLocator.dontReleaseModelOnDeath = true;

            // Add controller and set follow target
            var wingDingy = voidAlly.gameObject.AddComponent<WingDingyAnimController>();
            wingDingy.followTarget = voidAlly.gameObject;
            wingDingy.CorruptedEffect = CorruptedEffect;
            wingDingy.CorruptedEffectHolder = CorruptedEffectHolder;
            wingDingy.CorruptedConversionTime = CorruptConversionTime;
            wingDingy.CmdFIRE();

            // Destroy old target copy
            if (targetCopy) targetCopy.healthComponent.Suicide();
            //UnityEngine.Object.Destroy(targetCopy);
            
        }

        public void CreateEffect()
        {
            CorruptedEffect = MainAssets.LoadAsset<GameObject>("wingshit.prefab").InstantiateClone("gaster_corrupt_effect", true);
            CorruptedEffect.transform.localScale = new Vector3(1f, 1f, 1f);
            Util.Helpers.CreateNetworkedEffectPrefab(CorruptedEffect, true);

            var corrHolder = new GameObject("temp_holder");
            corrHolder.transform.localPosition = Vector3.zero;
            corrHolder.transform.localScale = new Vector3(1f, 1f, 1f);
            corrHolder.AddComponent<FollowTarget>();
            //GameObject.DontDestroyOnLoad(CorruptedEffectHolder);
            CorruptedEffectHolder = corrHolder.InstantiateClone("CorruptedEffectHolder", false);
            Util.Helpers.CreateNetworkedObjectPrefab(CorruptedEffectHolder);
            //CorruptedEffectHolder.RegisterNetworkPrefab();
        }

        public void CreateBuff()
        {
            CorruptedBuff = ScriptableObject.CreateInstance<BuffDef>();
            CorruptedBuff.name = "CorruptedDebuff";
            CorruptedBuff.iconSprite = MainAssets.LoadAsset<Sprite>("corrupt_effect_icon.png");
            CorruptedBuff.canStack = false;
            CorruptedBuff.isDebuff = true;

            ContentAddition.AddBuffDef(CorruptedBuff);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateEffect();
            CreateBuff();
            Hooks();

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("jim_carry.prefab").InstantiateClone("GasterMaskPickup", true);
            pickupModel.transform.localScale = new Vector3(650f, 650f, 650f); 

            ItemDef.pickupModelPrefab = pickupModel;
        }

        // Just makes sure the target isnt converted twice by tagging it (hopefully)
        public class ThingyMaBobber : MonoBehaviour
        {
            void Start()
            { 
                Debug.Log("bleh :P");
            }
        }
    }

    // All the anim stuff
    public class WingDingyAnimController : NetworkBehaviour
    {
        public GameObject followTarget;
        public GameObject CorruptedEffectHolder;
        public GameObject CorruptedEffect;
        public int CorruptedConversionTime;

        // Do all the visual stuff that wont work on server cause f me ig
        [Command]
        public void CmdFIRE()
        {
            if (!NetworkServer.active) return;

            if (!followTarget || !CorruptedEffectHolder || !CorruptedEffect || CorruptedConversionTime <= 0) return;

            var holder = Instantiate(CorruptedEffectHolder);

            // Oh why does this not spawn for client 2 ????
            NetworkServer.Spawn(holder);

            // Get the follow for the effect
            var follow = holder.GetComponent<FollowTarget>();
            follow.target = followTarget.transform;
            follow.enabled = true;

            // Setup and spawn effect
            EffectData effectData = new EffectData
            {
                scale = 1f
            };
            effectData.SetNetworkedObjectReference(holder);

            EffectManager.SpawnEffect(CorruptedEffect, effectData, true);

            // Destroy the remains, BURN IT DOWN!
            Destroy(holder, CorruptedConversionTime);
            Destroy(this, CorruptedConversionTime);
        }
    }
}
