using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static DeltaruneMod.Items.Spamton.MalfunctiongCore;
using static R2API.DeployableAPI;

namespace DeltaruneMod.Neo
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

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

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
            return null;
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
                args.critAdd += critPercent*100 + ((GetCount(sender) - 1) + additionalCritPercent*100);
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
                    self.damageValue *= 1 + (LightningDamageMultiplier * GetCount(body));
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
                    self.damageValue *= 1 + (LightningDamageMultiplier * GetCount(body));
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
                    self.damageValue *= 1 + (LightningDamageMultiplier * GetCount(body));
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
                    self.damageValue *= 1 + (LightningDamageMultiplier * GetCount(body));
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
            FinalFormOrbs = DeployableAPI.RegisterDeployableSlot(limit);

            // Create Projectile
            orbProjectile = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarSunProjectile"), "BeadProjectile", false);

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
                base.enabled = false;
            }

            private void OnEnable()
            {
            }

            private void OnDisable()
            {
                this.onDisabled?.Invoke(this);
                this.onDisabled = null;
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
                        damage = (body.damage * baseDamageCoefficient) * stack,
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