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

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("mis_heart_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject orbProjectile;

        public static GameObject ShardPrefab = MainAssets.LoadAsset<GameObject>("spam_projectile.prefab");

        public DeployableSlot BeadOrbs;

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
                var coreCount = sender.inventory.GetItemCount(MalfunctionCore.instance.ItemDef);
                var bulbCount = sender.inventory.GetItemCount(LightBulb.instance.ItemDef);
                if (heartCount > 0 && coreCount > 0 && bulbCount > 0)
                {
                    sender.inventory.RemoveItem(ItemDef);
                    sender.inventory.RemoveItem(MalfunctionCore.instance.ItemDef);
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
                return 2;
            }
            BeadOrbs = RegisterDeployableSlot(limit);

            // Create Projectile
            orbProjectile = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarSunProjectile").InstantiateClone("BeadProjectile", false);

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
            projSimple.desiredForwardSpeed = fwrdSpd * 3;

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
            private const float secondsPerProjectile = 2f;

            private const int baseMaxProjectiles = 2;

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

            public void FInitializeOrbiter(BeadProjectileOwnerOrbiter orbiter, BeadProjectileController controller)
            {
                //OG does not work with upward arc
                float radius = body.radius + baseOrbitRadius;
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(180f, 180f), Vector3.up); // front to back
                Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 180f), Vector3.forward);
                Vector3 planeNormal = quaternion * quaternion2 * Vector3.forward;
                float initialDegreesFromOwnerUp = UnityEngine.Random.Range(75f, -75f); // left to right
                orbiter.Initialize(planeNormal, radius, 0, initialDegreesFromOwnerUp);
                onDisabled += DestroyOrbiter;
                void DestroyOrbiter(BeadBehavior beadBehavior)
                {
                    if (controller)
                    {
                        controller.Detonate();
                    }
                }
            }
            public void InitializeOrbiter(ProjectileOwnerOrbiter orbiter, BeadProjectileController controller)
            {
                float radius = body.radius + baseOrbitRadius;
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(180f, 180f), Vector3.up); // 0, 360 Sphere | 180, 180 Halo Circle Thing | 90, 90 Spot Behind Characters
                Quaternion quaternion2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 180f), Vector3.forward); // horizontal plane, left to right
                Vector3 planeNormal = quaternion * quaternion2 * Vector3.up;
                float initialDegreesFromOwnerForward = UnityEngine.Random.Range(30f, -210f); // vertical plane, left to right
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

            private void FAcquireOwner(ProjectileController controller)
            {
                controller.onInitialized -= AcquireOwner;
                CharacterBody player = controller.owner.GetComponent<CharacterBody>();
                if (player)
                {
                    BeadProjectileOwnerOrbiter playerOrbiter = GetComponent<BeadProjectileOwnerOrbiter>();
                    //player.GetComponent<BeadBehavior>().InitializeOrbiter(playerOrbiter, this);
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
                    transform.position = position;
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
                NetworkinitialRadialDirection = Quaternion.AngleAxis(initialDegreesFromOwnerForward, planeNormal) * ownerTransform.forward;
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
            if ((syncVarDirtyBits & 1) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(offset);
            }
            if ((syncVarDirtyBits & 2) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialDegreesFromOwnerForward);
            }
            if ((syncVarDirtyBits & 4) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(degreesPerSecond);
            }
            if ((syncVarDirtyBits & 8) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(radius);
            }
            if ((syncVarDirtyBits & 0x10) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(planeNormal);
            }
            if ((syncVarDirtyBits & 0x20) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialRadialDirection);
            }
            if ((syncVarDirtyBits & 0x40) != 0)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(initialRunTime);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
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