using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Neo
{
    public class SusBeads : ItemBase<SusBeads>
    {
        public override string ItemName => "Bead Chain";

        public override string ItemLangTokenName => "SUS_BEAD";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("ok.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ok.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
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
                    beadBehavior.enabled = true;
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

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }

        public class BeadBehavior : CharacterBody.ItemBehavior
        {
            private const float secondsPerProjectile = 1f;

            private const string projectilePath = "Prefabs/Projectiles/LunarSunProjectile";

            private const int baseMaxProjectiles = 4;

            private const int maxProjectilesPerStack = 1;

            private const float baseOrbitDegreesPerSecond = 180f;

            private const float orbitDegreesPerSecondFalloff = 0.9f;

            private const float baseOrbitRadius = 2f;

            private const float orbitRadiusPerStack = 0.25f;

            private const float maxInclinationDegrees = 180f;

            private const float baseDamageCoefficient = 3.6f;

            private float projectileTimer;

            private GameObject projectilePrefab;

            public event Action<BeadBehavior> onDisabled;

            private DeployableSlot BeadOrbs = (DeployableSlot)8;

            public static int GetMaxProjectiles(Inventory inventory)
            {
                return baseMaxProjectiles + (maxProjectilesPerStack * inventory.GetItemCount(SusBeads.instance.ItemDef));
            }

            public void InitializeOrbiter(ProjectileOwnerOrbiter orbiter, BeadProjectileController controller)
            {
                float radius = body.radius + baseOrbitRadius + UnityEngine.Random.Range(orbitRadiusPerStack, orbitRadiusPerStack * stack);
                float orbitRadius = radius / baseOrbitRadius;
                orbitRadius *= orbitRadius;
                float degreesPerSecond = baseOrbitDegreesPerSecond * Mathf.Pow(orbitDegreesPerSecondFalloff, orbitRadius);
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 180f), Vector3.forward);
                Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, maxInclinationDegrees), Vector3.forward);
                Vector3 planeNormal = quaternion * quaternion2 * Vector3.up;
                float initialDegreesFromOwnerForward = UnityEngine.Random.Range(0f, 360f);
                orbiter.Initialize(planeNormal, radius, 0, initialDegreesFromOwnerForward);
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
                /*
                var prefab = LegacyResourcesAPI.Load<GameObject>(projectilePath).InstantiateClone("BeadProjectile", false);
                var oldProjCont = prefab.GetComponent<LunarSunProjectileController>();
                if (oldProjCont)
                {
                    Debug.Log("Deleting old controller!");
                    Destroy(prefab.GetComponent<LunarSunProjectileController>());
                }
                prefab.AddComponent<BeadProjectileController>();
                //var deploy = projectilePrefab.GetComponent<ProjectileDeployToOwner>();
                //deploy.deployableSlot = BeadOrbs;
                projectilePrefab = prefab;
                Destroy(prefab);
                Util.Helpers.GetAllComponentNames(projectilePrefab);
                */
                projectilePrefab = LegacyResourcesAPI.Load<GameObject>(projectilePath);
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
                //!body.master.IsDeployableLimited(DeployableSlot.LunarSunBomb) &&
                if (projectileTimer > secondsPerProjectile)
                {
                    projectileTimer = 0f;
                    Ray aimRay = new Ray(body.inputBank.aimOrigin, body.inputBank.aimDirection);

                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = projectilePrefab,
                        crit = body.RollCrit(),
                        damage = body.damage * baseDamageCoefficient,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        owner = body.gameObject,
                        position = body.transform.position,
                        rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction)
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
