using R2API;
using RoR2;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.CharacterAI;
using UnityEngine.AddressableAssets;
using EntityStates;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class DroneHealingBoost : ItemBase<DroneHealingBoost>
    {
        public override string ItemName => "DroneHealingBoost";

        public override string ItemLangTokenName => "DRONE_HEAL_BOOST";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("guide_book.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("guide_book_icon.png");

        public override bool isChapter1 => true;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject HealEffectPrefab;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            //On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {

            // No worky, adds a bunch
            if (GetCount(self) > 0)
            {
                self.AddItemBehavior<DroneHealingBehavior>(GetCount(self));
                Debug.Log("Gave " + self + " " + ItemName + ".");
            } 

            orig(self);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || sender.isPlayerControlled) return;

            if (!sender.inventory) return;

            #region Healing Behavior/Timer Setup
            var itemCount = GetCount(sender);
            var healingBehavior = sender.gameObject.GetComponent<DroneHealingBehavior>();
            if (itemCount > 0)
            {
                // If the ally somehow got this and is a void enemy, remove EVERYTHING
                if (sender.bodyFlags.HasFlag(CharacterBody.BodyFlags.Void))
                {
                    sender.inventory.RemoveItem(ItemDef);
                    if (healingBehavior) healingBehavior.enabled = false;
                    
                }
                // Add healing behavior is doesnt have and not void
                else if (!healingBehavior)
                {
                    healingBehavior = sender.gameObject.AddComponent<DroneHealingBehavior>();
                    healingBehavior.body = sender;
                    healingBehavior.enabled = true;
                    Debug.Log("Gave " + sender + " " + ItemName + ".");
                }
            }
            if (itemCount <= 0 && healingBehavior) healingBehavior.enabled = false;
            #endregion
        }

        private static void CreateEffect()
        {
            HealEffectPrefab = MainAssets.LoadAsset<GameObject>("guide_book_heal.prefab").InstantiateClone("guidebook_heal", true);
            Util.Helpers.CreateNetworkedEffectPrefab(HealEffectPrefab, true);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateEffect();
            Hooks();
        }

        public class DroneHealingBehavior : CharacterBody.ItemBehavior
        {
            private float healTimer = 0f;
            private const float healInterval = 10f;
            private const float healFraction = 0.025f;

            private void Awake()
            {
                base.enabled = false;
            }

            private void OnEnable()
            {
            }
            // Healing timer
            private void FixedUpdate()
            {
                healTimer -= Time.fixedDeltaTime;
                if (healTimer <= 0f)
                {
                    healTimer = healInterval;
                    FindAndHeal(body);
                }
            }
            private void OnDisable()
            {             
            }
            private void FindAndHeal(CharacterBody drone)
            {
                TeamIndex team = drone.teamComponent.teamIndex;

                Ray aimRay = drone.GetComponent<InputBankTest>().GetAimRay();
                TeamMask none = TeamMask.none;
                none.AddTeam(drone.master.teamIndex);

                // Search for target to heal
                BullseyeSearch search = new BullseyeSearch
                {
                    viewer = drone,
                    filterByDistinctEntity = true,
                    filterByLoS = false,
                    minDistanceFilter = 0f,
                    maxDistanceFilter = 100f,
                    maxAngleFilter = 360f,
                    searchDirection = aimRay.direction,
                    searchOrigin = aimRay.origin,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    queryTriggerInteraction = 0,
                    teamMaskFilter = none,

                };
                search.RefreshCandidates();
                search.FilterOutGameObject(drone.gameObject);
                
                // For each found ally, heal
                foreach (HurtBox target in search.GetResults())
                {
                    CharacterBody targetBody = target.healthComponent?.body;
                    if (targetBody && targetBody != drone)
                    {
                        //targetBody.gameObject.AddComponent<DroneHealingEffectController>();
                        try
                        {
                            // Spawn healing effect that sometimes dies
                            EffectData effectData = new EffectData
                            {
                                origin = targetBody.transform.position,
                                scale = 1f,
                            };
                            EffectManager.SpawnEffect(HealEffectPrefab, effectData, true);
                        }
                        catch
                        {
                            Debug.Log("NRE, don't spawn effect.");
                        }
                        
                        // Heal target
                        float healAmount = targetBody.healthComponent.fullCombinedHealth * healFraction;
                        targetBody.healthComponent.Heal(healAmount, default, true);
                        Debug.Log("Healed " + targetBody + " for " + healAmount + ".");
                    }
                }
            }
        }
    }
}
