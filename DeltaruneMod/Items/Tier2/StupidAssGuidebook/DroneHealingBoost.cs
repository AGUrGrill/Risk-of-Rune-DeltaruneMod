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
        }
        
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || sender.isPlayerControlled) return;

            var itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                var healingBehavior = sender.gameObject.GetComponent<DroneHealingBehavior>();
                if (!healingBehavior)
                {
                    healingBehavior = sender.gameObject.AddComponent<DroneHealingBehavior>();
                    healingBehavior.body = sender;
                    Debug.Log("Gave " + sender + " " + ItemName + ".");
                }
            }
        }

        private static void CreateEffect()
        {
            HealEffectPrefab = MainAssets.LoadAsset<GameObject>("guide_book_heal.prefab").InstantiateClone("guidebook_heal", true);
            //HealEffectPrefab.transform.localRotation = Quaternion.LookRotation(Vector3.up);
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

            public void FixedUpdate()
            {
                healTimer -= Time.fixedDeltaTime;
                if (healTimer <= 0f)
                {
                    FindAndHeal(body);
                    healTimer = healInterval;
                }
            }

            public void FindAndHeal(CharacterBody drone)
            {
                TeamIndex team = drone.teamComponent.teamIndex;

                Ray aimRay = drone.GetComponent<InputBankTest>().GetAimRay();
                TeamMask none = TeamMask.none;
                none.AddTeam(drone.master.teamIndex);
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
                
                foreach (HurtBox target in search.GetResults())
                {
                    CharacterBody targetBody = target.healthComponent?.body;
                    if (targetBody && targetBody != drone)
                    {
                        //targetBody.gameObject.AddComponent<DroneHealingEffectController>();

                        try
                        {
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
                        

                        float healAmount = targetBody.healthComponent.fullCombinedHealth * healFraction;
                        targetBody.healthComponent.Heal(healAmount, default, true);
                        Debug.Log("Healed " + targetBody + " for " + healAmount + ".");
                    }
                }
            }
        }
    
        public class DroneHealingEffectController : NetworkBehaviour
        {
            float effectLength = 1.5f;
            void Start()
            {
                try
                {
                    CharacterBody body = GetComponentInParent<CharacterBody>();
                    HurtBox hurtBox = body ? body.mainHurtBox : null;

                    if (hurtBox)
                    {
                        // Spawn effect and tie to ally
                        EffectData effectData = new EffectData
                        {
                            origin = transform.position,
                            scale = 1f,
                            //rotation = Quaternion.LookRotation(Vector3.up),
                        };
                        effectData.SetHurtBoxReference(hurtBox);
                        HealEffectPrefab.AddComponent<DestroyOnTimer>().duration = effectLength;
                        EffectManager.SpawnEffect(HealEffectPrefab, effectData, true);
                    }

                    Destroy(this, effectLength);
                }
                catch
                {
                    Debug.Log("Error applying drone heal effect.");
                }
            }
        }
    }
}
