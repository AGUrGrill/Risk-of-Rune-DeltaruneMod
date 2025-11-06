using DeltaruneMod.Items;
using DeltaruneMod.Neo;
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
            "\nSpawn up to 2 maximum projectiles, deals <style=cIsDamage>199.7% base damage</style> <style=cStack>(+199.7% per stack)</style>.";

        public override string ItemLore => "Distorted laughter emenates from the dark and empty room." +
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

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
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
                    sender.inventory.RemoveItem(ItemDef);
                    sender.inventory.RemoveItem(MalfunctiongCore.instance.ItemDef);
                    sender.inventory.RemoveItem(LightBulb.instance.ItemDef);
                    sender.inventory.GiveItem(FinalForm.instance.ItemDef);
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

            // Create Projectile
            orbProjectile = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarSunProjectile"), "BeadProjectile", false);

            // Add Ghost
            var ghost = ShardPrefab.InstantiateClone("ShardPrefabGhost", false);
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
            projectileDeployToOwner.deployableSlot = BeadOrbs;

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