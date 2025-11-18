using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.UI.LogBook;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static DeltaruneMod.Items.Spamton.MalfunctiongCore;
using static R2API.DeployableAPI;

namespace DeltaruneMod.Items.Spamton
{
    public class FinalForm : ItemBase<FinalForm>
    {
        public override string ItemName => "N.E.O.";

        public override string ItemLangTokenName => "FINAL_FORM";

        public override string ItemPickupDesc => "Gain the perfected effects of all three NEO items...";

        public override string ItemFullDescription => "Perfected Heart: Every <style=cIsUtility>" + OrbCooldown + "</style> seconds, spawn an armor piercing projectile that orbits the player in stasis." +
            "\nSpawn up to " + MaxOrbs + " maximum projectiles, deals <style=cIsDamage>199.7% base damage</style> <style=cStack>(+199.7% per stack)</style>." +
            "\n" +
            "\nPerfected Core: Gain a permenant <style=cIsUtility>" + critPercent*100 + "%</style> crit chance. <style=cStack>(+" + additionalCritPercent*100 + "% per stack)</style>" +
            "\n" +
            "\nPerfected Bulb: All forms of lightning damage are increased by <style=cIsUtility>" + LightningDamageMultiplier*100 + "%</style> <style=cStack>(+" + LightningDamageMultiplier*100 + "% per stack)</style>.";

        public override string ItemLore => "Upon getting the final piece you have a vision." +
            "\nIt's unlike anything you've ever felt before, you feel a wave of emotion, things " +
            "that were broken mend, things mended become perfect, YOU become perfect." +
            "\nA voice calls to you, \"[Heaven], are you WATCHING? IT'S TIME TO MAKE A VERY [SPECIAL] DEAL...\"" +
            "\nYou make the deal..." +
            "\nIt has a name, it's the final form, your final form, the HEART SHAPED OBJECT beats in your chest, " +
            "before violently extruding from your body." +
            "\nYou pickup your reward, you made a deal and now its a part of you, and you, a part of it." +
            "\nIt's name is <style=cMono><style=cDeath>N.</style><style=cShrine>E.</style><style=cArtifact>O.</style></style>";

        public override ItemTier Tier => ItemTier.Boss;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("final_form.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("final_form_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject orbProjectile;

        public static GameObject ShardPrefab = MainAssets.LoadAsset<GameObject>("spam_projectile.prefab");

        public DeployableSlot FinalFormOrbs;

        public const int MaxOrbs = 16;

        public const float OrbCooldown = 1f;

        public static float LightningDamageMultiplier = 0.33f;

        public static float critPercent = 0.25f;

        public static float additionalCritPercent = 0.1f;

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
                    localPos = new Vector3(-0.29043F, -0.02646F, 0.15993F),
                    localAngles = new Vector3(354.9149F, 167.6319F, 3.93012F),
                    localScale = new Vector3(5.52715F, 5.66094F, 5.66094F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.50492F, -0.10487F, 0.01353F),
                    localAngles = new Vector3(5.92784F, 140.589F, 5.70529F),
                    localScale = new Vector3(8.0404F, 8.1219F, 7.18788F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Neck",
                    localPos = new Vector3(-2.33612F, 0.66576F, 3.35203F),
                    localAngles = new Vector3(357.8803F, 186.4762F, 357.1776F),
                    localScale = new Vector3(58.55358F, 58.55358F, 58.55358F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CannonHeadR",
                    localPos = new Vector3(0.2886F, 0.01301F, -0.11433F),
                    localAngles = new Vector3(55.82204F, 329.8341F, 339.3175F),
                    localScale = new Vector3(4.7932F, 4.7932F, 4.7932F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.17492F, -0.21243F, 0.33896F),
                    localAngles = new Vector3(354.6992F, 192.206F, 341.9989F),
                    localScale = new Vector3(6.17744F, 6.17744F, 6.17744F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.22129F, -0.02885F, 0.11114F),
                    localAngles = new Vector3(17.24668F, 174.8952F, 350.5795F),
                    localScale = new Vector3(5.30776F, 5.30776F, 3.60387F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-1.06139F, 0.2354F, 0.48228F),
                    localAngles = new Vector3(352.137F, 164.5131F, 356.5106F),
                    localScale = new Vector3(17.00706F, 17.00706F, 17.00706F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.25857F, -0.06008F, 0.17796F),
                    localAngles = new Vector3(3.82091F, 175.997F, 350.7232F),
                    localScale = new Vector3(5.74637F, 6.19757F, 6.19757F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "UpperArmL",
                    localPos = new Vector3(-2.68845F, 3.69412F, -2.61108F),
                    localAngles = new Vector3(350.7655F, 0.58101F, 168.6521F),
                    localScale = new Vector3(59.79227F, 59.79227F, 59.79227F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.12893F, 0.20509F, 0.25282F),
                    localAngles = new Vector3(357.6487F, 212.0068F, 355.0177F),
                    localScale = new Vector3(5.0544F, 5.0544F, 5.41385F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(-0.32372F, 0.14683F, 0.07836F),
                    localAngles = new Vector3(352.7583F, 152.0193F, 356.4997F),
                    localScale = new Vector3(5.43432F, 5.43432F, 5.43432F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "UpperArmL",
                    localPos = new Vector3(0.04481F, 0.37664F, 0.25784F),
                    localAngles = new Vector3(8.14335F, 142.9294F, 168.6543F),
                    localScale = new Vector3(4.52354F, 5.25842F, 4.89493F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.1089F, 0.26473F, 0.23636F),
                    localAngles = new Vector3(71.0227F, 335.4342F, 54.50938F),
                    localScale = new Vector3(7.97052F, 7.86754F, 6.83188F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(-0.08516F, 0.12197F, -0.34407F),
                    localAngles = new Vector3(327.7921F, 356.1507F, 49.77155F),
                    localScale = new Vector3(3.16199F, 3.25608F, 3.29963F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.59992F, -0.26726F, 0.14843F),
                    localAngles = new Vector3(4.056F, 154.1342F, 354.7828F),
                    localScale = new Vector3(9.8243F, 9.8243F, 9.8243F)

                }
            });
            return rules;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;

            // Concept adapted from Startstorm 2
            On.RoR2.Orbs.LightningOrb.OnArrival += LightningOrb_OnArrival; // uke tesla BFG arti loader 
            On.RoR2.Orbs.SimpleLightningStrikeOrb.OnArrival += SimpleLightningStrikeOrb_OnArrival; ; // charged perforator
            On.RoR2.Orbs.LightningStrikeOrb.OnArrival += LightningStrikeOrb_OnArrival; // royal capacitor
            On.RoR2.Orbs.VoidLightningOrb.OnArrival += VoidLightningOrb_OnArrival; // polylute

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (GetCount(sender) > 0)
            {
                args.critAdd += critPercent * 100 + (GetCount(sender) - 1) * (additionalCritPercent * 100);
            }
        }

        private void VoidLightningOrb_OnArrival(On.RoR2.Orbs.VoidLightningOrb.orig_OnArrival orig, VoidLightningOrb self)
        {
            var attacker = self.attacker;
            if (attacker)
            {
                var body = attacker.GetComponent<CharacterBody>();
                if (body && body.isPlayerControlled)
                {
                    self.damageValue *= 1 + LightningDamageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + LightningDamageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + LightningDamageMultiplier * GetCount(body);
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
                    self.damageValue *= 1 + LightningDamageMultiplier * GetCount(body);
                }
            }
            orig(self);
        }

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            #region Add Final Form Orb Behavior
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
                    beadBehavior.orbDeployable = FinalFormOrbs;
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
            FinalFormOrbs = RegisterDeployableSlot(limit);

            // Create Projectile
            orbProjectile = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarSunProjectile").InstantiateClone("BeadProjectile", false);

            // Add Ghost
            var ghost = ShardPrefab.InstantiateClone("ShardPrefabGhostNeo", false);
            ghost.AddComponent<ProjectileGhostController>();
            ghost.AddComponent<NetworkIdentity>();
            ghost.transform.localScale = new Vector3(10f, 10f, 10f);

            var projCont = orbProjectile.GetComponent<ProjectileController>();
            if (projCont.ghostPrefab) UnityEngine.Object.Destroy(projCont.ghostPrefab);
            projCont.shouldPlaySounds = false;
            projCont.startSound = "";
            projCont.ghostPrefab = ghost;

            // Replace Special Controller
            var lunarSunProjCont = orbProjectile.GetComponent<LunarSunProjectileController>();
            if (lunarSunProjCont)
            {
                Debug.Log("Deleting old controller!");
                UnityEngine.Object.DestroyImmediate(orbProjectile.GetComponent<LunarSunProjectileController>());
            }
           var beadProjCont = orbProjectile.AddComponent<BeadProjectileController>();

            // Change Deployable Type
            var projectileDeployToOwner = orbProjectile.GetComponent<ProjectileDeployToOwner>();
            projectileDeployToOwner.deployableSlot = FinalFormOrbs;

            var projSimple = orbProjectile.GetComponent<ProjectileSimple>();
            var fwrdSpd = projSimple.desiredForwardSpeed;
            projSimple.desiredForwardSpeed = fwrdSpd * 5;
            projSimple.oscillate = false;

            Util.Helpers.GetAllComponentNames(orbProjectile);
            Util.Helpers.CreateNetworkedProjectilePrefab(orbProjectile);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreatePrefab();
            Hooks();

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("final_form.prefab").InstantiateClone("FinalFormPickup", false);
            pickupModel.transform.localScale = new Vector3(2f, 2f, 2f);
            ItemDef.pickupModelPrefab = pickupModel;
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
