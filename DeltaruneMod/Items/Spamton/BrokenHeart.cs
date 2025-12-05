using DeltaruneMod.Items;
using DeltaruneMod.Util;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static R2API.DeployableAPI;

namespace DeltaruneMod.Items.Spamton
{
    public class BrokenHeart : ItemBase<BrokenHeart>
    {
        public override string ItemName => "Misshapen Heart";

        public override string ItemLangTokenName => "BROKE_HEART";

        public override string ItemPickupDesc => "Spawn an orbiting, armor piercing projectile every 2 seconds.";

        public override string ItemFullDescription => "Every <style=cIsUtility>2</style> seconds, spawn an armor piercing projectile that orbits the player in stasis." +
            "\nSpawn up to 3 maximum projectiles, deals <style=cIsDamage>199.7% base damage</style> <style=cStack>(+199.7% per stack)</style>.";

        public override string ItemLore => "Distorted laughter emanates from the dark and empty room." +
            "\nThe laughter is mixed with another emotion... you can feel it, its an overwhelming sadness reverberating within." +
            "\nYou walk closer, feeling its pain as its emotions take control of you." +
            "\n\n\"Is this you?\", you call out, but no one awnsers." +
            "\nYou pick up the ominous heart, you can feel it, the rush, the excitement, the laughter, the pain..." +
            "\nAll these emotions swell inside you, you feel the <style=cMono>DETERMINATION</style>, " +
            "\nthe <style=cMono>DETERMINATION</style> to become a <style=cDeath>[[Big Shot]]</style>.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("mis_heart.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("mis_heart_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject orbProjectile;

        public static GameObject ShardPrefab = MainAssets.LoadAsset<GameObject>("spam_projectile.prefab");

        public DeployableSlot BeadOrbs;

        public const int MaxOrbs = 3;

        public const float OrbCooldown = 2f;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

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
                    localPos = new Vector3(0.18475F, 0.43531F, 0.06669F),
                    localAngles = new Vector3(12.71502F, 118.5311F, 27.65873F),
                    localScale = new Vector3(3.43066F, 3.43066F, 3.43066F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.07347F, 0.26925F, -0.11147F),
                    localAngles = new Vector3(344.4833F, 157.2875F, 24.74886F),
                    localScale = new Vector3(6.42886F, 4.20317F, 6.42886F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-1.91492F, 0.54265F, 0.99989F),
                    localAngles = new Vector3(346.6342F, 290.7388F, 291.0602F),
                    localScale = new Vector3(64.77013F, 64.77013F, 64.77013F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.04275F, 0.13148F, 0.07674F),
                    localAngles = new Vector3(319.3107F, 338.2517F, 340.6354F),
                    localScale = new Vector3(8.52303F, 8.52303F, 8.52303F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.10766F, 0.11525F, -0.03917F),
                    localAngles = new Vector3(335.8464F, 270.3703F, 4.12816F),
                    localScale = new Vector3(7.05254F, 7.05254F, 7.05254F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.13464F, 0.14899F, -0.00139F),
                    localAngles = new Vector3(339.0311F, 93.592F, 334.2282F),
                    localScale = new Vector3(7.12055F, 7.12055F, 7.12055F)


                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.91288F, 1.32861F, -0.03517F),
                    localAngles = new Vector3(347.8391F, 274.0605F, 346.7606F),
                    localScale = new Vector3(8.68742F, 8.68742F, 8.68742F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.13462F, 0.17267F, 0.02228F),
                    localAngles = new Vector3(318.7485F, 78.92309F, 15.94955F),
                    localScale = new Vector3(3.60433F, 5.22948F, 5.22948F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-1.79498F, 1.44784F, 0.20349F),
                    localAngles = new Vector3(355.5392F, 290.8363F, 240.7348F),
                    localScale = new Vector3(47.10388F, 47.10388F, 47.10388F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.10906F, 0.06376F, 0.07416F),
                    localAngles = new Vector3(336.4185F, 90.84381F, 355.8852F),
                    localScale = new Vector3(3.35465F, 3.35465F, 3.35465F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.10538F, 0.04744F, 0.02644F),
                    localAngles = new Vector3(343.5705F, 93.7125F, 16.36206F),
                    localScale = new Vector3(2.16919F, 2.16919F, 2.16919F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.19268F, 0.02681F, -0.04928F),
                    localAngles = new Vector3(354.3749F, 132.6399F, 300.215F),
                    localScale = new Vector3(5.93916F, 6.90403F, 4.9493F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.14696F, -0.077F, 0.17095F),
                    localAngles = new Vector3(11.1291F, 331.6624F, 74.4416F),
                    localScale = new Vector3(6.34747F, 5.4407F, 5.4407F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.13128F, 0.06277F, -0.04087F),
                    localAngles = new Vector3(346.1569F, 100.6667F, 336.058F),
                    localScale = new Vector3(5.50237F, 4.89099F, 4.89099F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.18457F, 0.12766F, -0.0558F),
                    localAngles = new Vector3(355.5451F, 115.7917F, 341.5839F),
                    localScale = new Vector3(5.0728F, 5.0728F, 5.0728F)

                }
            });
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            #region Change to Final Form if applicable
            try
            {
                var heartCount = GetCount(sender);
                var coreCount = sender.inventory.GetItemCount(MalfunctiongCore.instance.ItemDef);
                var bulbCount = sender.inventory.GetItemCount(LightBulb.instance.ItemDef);
                if (heartCount > 0 && coreCount > 0 && bulbCount > 0)
                {
                    sender.inventory.RemoveItemPermanent(ItemDef);
                    sender.inventory.RemoveItemPermanent(MalfunctiongCore.instance.ItemDef);
                    sender.inventory.RemoveItemPermanent(LightBulb.instance.ItemDef);
                    // Pickup for logbook entry
                    if (DeltaruneMod.DeltarunePlugin.antiFunMode.Value)
                        sender.inventory.GiveItemPermanent(FinalForm.instance.ItemDef);
                    else
                        PickupDropletController.CreatePickupDroplet(new PickupIndex(FinalForm.instance.ItemDef.itemIndex), sender.transform.position, sender.transform.forward * 1f);
                    //sender.inventory.GiveItem(FinalForm.instance.ItemDef);
                }
            }
            catch { }
            #endregion
        }

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            #region Add Bead Behavior
            var itemCount = GetCount(self);
            var player = self.GetBody();
            if (!player) return;

            var beadBehavior = player.GetComponent<BeadBehavior>();
            if (itemCount > 0)
            {
                if (!beadBehavior)
                {
                    beadBehavior = player.gameObject.AddComponent<BeadBehavior>();
                    beadBehavior.body = player;
                    beadBehavior.stack = itemCount;
                    beadBehavior.enabled = true;
                    beadBehavior.projectilePrefab = orbProjectile;
                    beadBehavior.orbDeployable = BeadOrbs;
                }
                else if (beadBehavior)
                {
                    beadBehavior.stack = itemCount;
                }
            }
            else if (itemCount <= 0)
            {
                if (beadBehavior)
                {
                    beadBehavior.enabled = false;
                }
            }
            #endregion
        }

        public void CreatePrefab()
        {
            // Create Deployable
            int limit(CharacterMaster self, int deployableCountMultiplier)
            {
                return MaxOrbs;
            }
            BeadOrbs = RegisterDeployableSlot(limit);

            // Add Ghost
            var ghost = ShardPrefab.InstantiateClone("ShardPrefabGhost", false);
            ghost.AddComponent<ProjectileGhostController>();
            ghost.AddComponent<NetworkIdentity>();
            ghost.transform.localScale = new Vector3(10f, 10f, 10f);

            // Projectile Stuff
            orbProjectile = Helpers.ModifyVanillaPrefab("RoR2/DLC1/LunarSun/LunarSunProjectile.prefab", "BrokenHeartProjectile", false,
                (lunarSunProjectile) => {
                    lunarSunProjectile.GetComponent<ProjectileController>().ghostPrefab = ghost;
                    lunarSunProjectile.GetComponent<ProjectileController>().startSound = "";
                    // Change Proj Simple
                    var fwrdSpd = lunarSunProjectile.GetComponent<ProjectileSimple>().desiredForwardSpeed;
                    lunarSunProjectile.GetComponent<ProjectileSimple>().desiredForwardSpeed = fwrdSpd * 5;
                    lunarSunProjectile.GetComponent<ProjectileSimple>().oscillate = false;
                    // Change Deployable Type
                    lunarSunProjectile.GetComponent<ProjectileDeployToOwner>().deployableSlot = BeadOrbs;
                    // Replace Controller
                    UnityEngine.Object.DestroyImmediate(lunarSunProjectile.GetComponent<LunarSunProjectileController>());
                    lunarSunProjectile.AddComponent<BeadProjectileController>();

                    return lunarSunProjectile;
                });

            Util.Helpers.CreateNetworkedProjectilePrefab(orbProjectile);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreatePrefab();
            Hooks();
        }

        public class BeadBehavior : CharacterBody.ItemBehavior
        {
            private const float secondsPerProjectile = OrbCooldown;

            private const int baseMaxProjectiles = MaxOrbs;

            private const float baseOrbitRadius = 2f;

            private const float baseDamageCoefficient = 1.997f;

            private float projectileTimer;

            public GameObject projectilePrefab;

            public DeployableSlot orbDeployable;

            public event Action<BeadBehavior> onDisabled;

            public static int GetMaxProjectiles(Inventory inventory)
            {
                return baseMaxProjectiles;
            }
            public void InitializeOrbiter(ProjectileOwnerOrbiter orbiter, BeadProjectileController controller)
            {
                float radius = body.radius + baseOrbitRadius;
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(180f, 180f), Vector3.up); // 0, 360 Sphere | 180, 180 Halo Circle Thing | 90, 90 Spot Behind Characters
                Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 180f), Vector3.forward); // horizontal plane, left to right
                Vector3 planeNormal = quaternion * quaternion2 * Vector3.up;
                float initialDegreesFromOwnerForward = UnityEngine.Random.Range(30f, -210f); // vertical plane, left to right
                orbiter.Initialize(planeNormal, radius, 180, initialDegreesFromOwnerForward);
                onDisabled += DestroyOrbiter;
                void DestroyOrbiter(BeadBehavior beadBehavior)
                {
                    if (controller)
                    {
                        controller.Detonate();
                    }
                }
            }

            private void Awake()
            {
                enabled = false;
            }

            private void OnEnable()
            {
            }

            private void OnDisable()
            {
                onDisabled?.Invoke(this);
                onDisabled = null;
            }

            private void FixedUpdate()
            {
                projectileTimer += Time.fixedDeltaTime;
                if (!body.master.IsDeployableLimited(orbDeployable) && projectileTimer > secondsPerProjectile)
                {
                    projectileTimer = 0f;
                    Ray aimRay = new Ray(body.inputBank.aimOrigin, body.inputBank.aimDirection);

                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = projectilePrefab,
                        crit = body.RollCrit(),
                        damage = body.damage * baseDamageCoefficient * stack,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        owner = body.gameObject,
                        position = body.transform.position,
                        rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction) * Quaternion.Euler(90f, 0f, 0f),
                        damageTypeOverride = DamageType.BypassArmor
                    };
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
        }

        [DisallowMultipleComponent]
        [RequireComponent(typeof(ProjectileOwnerOrbiter))]
        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileImpactExplosion))]
        public class BeadProjectileController : MonoBehaviour
        {
            private ProjectileImpactExplosion explosion;

            public void OnEnable()
            {
                explosion = GetComponent<ProjectileImpactExplosion>();
                if (NetworkServer.active)
                {
                    var projectileController = GetComponent<ProjectileController>();
                    if (projectileController.owner)
                    {
                        AcquireOwner(projectileController);
                    }
                    else
                    {
                        projectileController.onInitialized += AcquireOwner;
                    }
                }
            }
            private void AcquireOwner(ProjectileController controller)
            {
                controller.onInitialized -= AcquireOwner;
                CharacterBody player = controller.owner.GetComponent<CharacterBody>();
                if (player)
                {
                    ProjectileOwnerOrbiter playerOrbiter = GetComponent<ProjectileOwnerOrbiter>();
                    player.GetComponent<BeadBehavior>().InitializeOrbiter(playerOrbiter, this);
                }
            }

            public void Detonate()
            {
                if (explosion)
                {
                    explosion.Detonate();
                }
            }
        }
    }
}