using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static R2API.DeployableAPI;

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

        public static GameObject orbProjectile;

        public DeployableSlot BeadOrbs;

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
                return 4;
            }
            BeadOrbs = DeployableAPI.RegisterDeployableSlot(limit);

            // Create Projectile
            orbProjectile = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarSunProjectile").InstantiateClone("BeadProjectile", false);

            var oldProjCont = orbProjectile.GetComponent<LunarSunProjectileController>();
            if (oldProjCont)
            {
                Debug.Log("Deleting old controller!");
                UnityEngine.Object.DestroyImmediate(orbProjectile.GetComponent<LunarSunProjectileController>());
            }
            orbProjectile.AddComponent<BeadProjectileController>();

            var projectileDeployToOwner = orbProjectile.GetComponent<ProjectileDeployToOwner>();
            projectileDeployToOwner.deployableSlot = BeadOrbs;

            
            var oldOrbiter = orbProjectile.GetComponent<ProjectileOwnerOrbiter>();
            var newOrbiter = orbProjectile.AddComponent<BeadProjectileOwnerOrbiter>();
            newOrbiter.initialRunTime = oldOrbiter.initialRunTime;
            newOrbiter.initialDegreesFromOwnerForward = oldOrbiter.initialDegreesFromOwnerForward;
            newOrbiter.initialRadialDirection = oldOrbiter.initialRadialDirection;
            newOrbiter.offset = oldOrbiter.offset;
            newOrbiter.radius = oldOrbiter.radius;
            newOrbiter.degreesPerSecond = oldOrbiter.degreesPerSecond;
            // idk maybe this helps
            if (oldOrbiter)
            {
                Debug.Log("Deleting old orbiter!");
                UnityEngine.Object.DestroyImmediate(orbProjectile.GetComponent<ProjectileOwnerOrbiter>());
            }
            
            var projSimple = orbProjectile.GetComponent<ProjectileSimple>();
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
            private const float secondsPerProjectile = 3f;

            private const int baseMaxProjectiles = 4;

            private const int maxProjectilesPerStack = 2;

            private const float baseOrbitRadius = 2f;

            private const float baseDamageCoefficient = 1f;

            private float projectileTimer;

            public GameObject projectilePrefab;

            public DeployableSlot orbDeployable;

            public event Action<BeadBehavior> onDisabled;

            public static int GetMaxProjectiles(Inventory inventory)
            {
                return baseMaxProjectiles + (maxProjectilesPerStack * inventory.GetItemCount(SusBeads.instance.ItemDef));
            }

            public void InitializeOrbiter(BeadProjectileOwnerOrbiter orbiter, BeadProjectileController controller)
            {
                float radius = body.radius + baseOrbitRadius;
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(180f, 180f), Vector3.up); // front to back
                Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 0f), Vector3.forward);
                Vector3 planeNormal = quaternion * quaternion2 * Vector3.forward;
                float initialDegreesFromOwnerForward = UnityEngine.Random.Range(80f, -80f); // left to right
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
                        rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                        damageTypeOverride = DamageType.BypassArmor
                    };
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
        }

        [DisallowMultipleComponent]
        [RequireComponent(typeof(BeadProjectileOwnerOrbiter))]
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
                    BeadProjectileOwnerOrbiter playerOrbiter = GetComponent<BeadProjectileOwnerOrbiter>();
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

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ProjectileController))]
    public class BeadProjectileOwnerOrbiter : NetworkBehaviour
    {
        [SerializeField]
        [SyncVar]
        public Vector3 offset;

        [SyncVar]
        [SerializeField]
        public float initialDegreesFromOwnerForward;

        [SerializeField]
        [SyncVar]
        public float degreesPerSecond;

        [SerializeField]
        [SyncVar]
        public float radius;

        [SyncVar]
        [SerializeField]
        private Vector3 planeNormal = Vector3.up;

        private Transform ownerTransform;

        private Rigidbody rigidBody;

        private bool resetOnAcquireOwner = true;

        [SyncVar]
        public Vector3 initialRadialDirection;

        [SyncVar]
        public float initialRunTime;

        public Vector3 Networkoffset
        {
            get
            {
                return offset;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref offset, 1u);
            }
        }

        public float NetworkinitialDegreesFromOwnerForward
        {
            get
            {
                return initialDegreesFromOwnerForward;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref initialDegreesFromOwnerForward, 2u);
            }
        }

        public float NetworkdegreesPerSecond
        {
            get
            {
                return degreesPerSecond;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref degreesPerSecond, 4u);
            }
        }

        public float Networkradius
        {
            get
            {
                return radius;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref radius, 8u);
            }
        }

        public Vector3 NetworkplaneNormal
        {
            get
            {
                return planeNormal;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref planeNormal, 16u);
            }
        }

        public Vector3 NetworkinitialRadialDirection
        {
            get
            {
                return initialRadialDirection;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref initialRadialDirection, 32u);
            }
        }

        public float NetworkinitialRunTime
        {
            get
            {
                return initialRunTime;
            }
            [param: In]
            set
            {
                SetSyncVar(value, ref initialRunTime, 64u);
            }
        }

        public void Initialize(Vector3 planeNormal, float radius, float degreesPerSecond, float initialDegreesFromOwnerForward)
        {
            NetworkplaneNormal = planeNormal;
            Networkradius = radius;
            NetworkdegreesPerSecond = degreesPerSecond;
            NetworkinitialDegreesFromOwnerForward = initialDegreesFromOwnerForward;
            ResetState();
        }

        private void OnEnable()
        {
            rigidBody = GetComponent<Rigidbody>();
            ProjectileController component = GetComponent<ProjectileController>();
            if ((bool)component.owner)
            {
                AcquireOwner(component);
            }
            else
            {
                component.onInitialized += AcquireOwner;
            }
        }

        public void FixedUpdate()
        {
            UpdatePosition(doSnap: false);
        }

        private void ResetState()
        {
            NetworkinitialRunTime = Time.fixedTime;
            planeNormal.Normalize();
            if ((bool)ownerTransform)
            {
                // foward -> up for upward arc
                NetworkinitialRadialDirection = Quaternion.AngleAxis(initialDegreesFromOwnerForward, planeNormal) * ownerTransform.up;
                resetOnAcquireOwner = false;
            }
            UpdatePosition(doSnap: true);
        }

        private void UpdatePosition(bool doSnap)
        {
            if ((bool)ownerTransform)
            {
                float angle = (Time.fixedTime - initialRunTime) * degreesPerSecond;
                Vector3 position = ownerTransform.position + offset + Quaternion.AngleAxis(angle, planeNormal) * initialRadialDirection * radius;
                if (!rigidBody || doSnap)
                {
                    base.transform.position = position;
                }
                else if ((bool)rigidBody)
                {
                    rigidBody.MovePosition(position);
                }
            }
        }

        public void SetInitialDegreesFromOwnerForward(float degrees)
        {
            NetworkinitialDegreesFromOwnerForward = degrees;
            if ((bool)ownerTransform)
            {
                // foward -> up for upward arc
                NetworkinitialRadialDirection = Quaternion.AngleAxis(initialDegreesFromOwnerForward, planeNormal) * ownerTransform.up;
            }
        }

        public float GetInitialRunTime()
        {
            return initialRunTime;
        }

        public void SetInitialRunTime(float _time)
        {
            NetworkinitialRunTime = Mathf.Max(_time, 0f);
        }

        private void AcquireOwner(ProjectileController controller)
        {
            ownerTransform = controller.owner.transform;
            controller.onInitialized -= AcquireOwner;
            if (resetOnAcquireOwner)
            {
                resetOnAcquireOwner = false;
                ResetState();
            }
        }

        private void UNetVersion()
        {
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(offset);
                writer.Write(initialDegreesFromOwnerForward);
                writer.Write(degreesPerSecond);
                writer.Write(radius);
                writer.Write(planeNormal);
                writer.Write(initialRadialDirection);
                writer.Write(initialRunTime);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & 1) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(offset);
            }
            if ((base.syncVarDirtyBits & 2) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialDegreesFromOwnerForward);
            }
            if ((base.syncVarDirtyBits & 4) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(degreesPerSecond);
            }
            if ((base.syncVarDirtyBits & 8) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(radius);
            }
            if ((base.syncVarDirtyBits & 0x10) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(planeNormal);
            }
            if ((base.syncVarDirtyBits & 0x20) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialRadialDirection);
            }
            if ((base.syncVarDirtyBits & 0x40) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialRunTime);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (initialState)
            {
                offset = reader.ReadVector3();
                initialDegreesFromOwnerForward = reader.ReadSingle();
                degreesPerSecond = reader.ReadSingle();
                radius = reader.ReadSingle();
                planeNormal = reader.ReadVector3();
                initialRadialDirection = reader.ReadVector3();
                initialRunTime = reader.ReadSingle();
                return;
            }
            int num = (int)reader.ReadPackedUInt32();
            if ((num & 1) != 0)
            {
                offset = reader.ReadVector3();
            }
            if ((num & 2) != 0)
            {
                initialDegreesFromOwnerForward = reader.ReadSingle();
            }
            if ((num & 4) != 0)
            {
                degreesPerSecond = reader.ReadSingle();
            }
            if ((num & 8) != 0)
            {
                radius = reader.ReadSingle();
            }
            if ((num & 0x10) != 0)
            {
                planeNormal = reader.ReadVector3();
            }
            if ((num & 0x20) != 0)
            {
                initialRadialDirection = reader.ReadVector3();
            }
            if ((num & 0x40) != 0)
            {
                initialRunTime = reader.ReadSingle();
            }
        }

        public override void PreStartClient()
        {
        }
    }
}
