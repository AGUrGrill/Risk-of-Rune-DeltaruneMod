using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.VoidTier3
{
    public class HammerJustice : ItemBase<HammerJustice>
    {
        public override string ItemName => "Hammer of Justice";

        public override string ItemLangTokenName => "HAMMER_JUSTICE";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("hammer_justice.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("hammer_justice_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => true;

        public static GameObject ProjectilePrefab;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public void CreateProjectile()
        {
            #region Setup Projectile Stats
            var procCoefficent = 1f;

            var lookRange = 75;
            var lookCone = 20;

            var rotationSpd = 180;
            #endregion

            #region Projectile Setup/Modification
            ProjectilePrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FMJ"), "ShellProjectile", true);

            var model = MainAssets.LoadAsset<GameObject>("hammer_justice.prefab");
            model.AddComponent<NetworkIdentity>();
            model.AddComponent<ProjectileGhostController>();
            model.transform.localScale = new Vector3(100f, 100f, 100f);

            var controller = ProjectilePrefab.GetComponent<ProjectileController>();
            controller.procCoefficient = procCoefficent;
            controller.ghostPrefab = model;

            ProjectilePrefab.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;

            var damage = ProjectilePrefab.GetComponent<ProjectileDamage>();
            damage.damageType = DamageType.CrippleOnHit;
            damage.damage = 0;

            var intervalController = ProjectilePrefab.GetComponent<ProjectileIntervalOverlapAttack>();
            UnityEngine.Object.Destroy(intervalController);

            //var impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/VagrantCannonExplosion");

            //var overlapAttack = ProjectilePrefab.GetComponent<ProjectileOverlapAttack>();
            //overlapAttack.impactEffect = impactEffect;

            //var applyTorqueOnStart = ProjectilePrefab.AddComponent<ApplyTorqueOnStart>();
            //applyTorqueOnStart.localTorque = new Vector3(0, 1500, 0);

            var projectileTarget = ProjectilePrefab.AddComponent<ProjectileTargetComponent>();

            var projectileDirectionalTargetFinder = ProjectilePrefab.AddComponent<ProjectileDirectionalTargetFinder>();
            projectileDirectionalTargetFinder.lookRange = lookRange;
            projectileDirectionalTargetFinder.lookCone = lookCone;
            projectileDirectionalTargetFinder.targetSearchInterval = 0.1f;
            projectileDirectionalTargetFinder.onlySearchIfNoTarget = true;
            projectileDirectionalTargetFinder.allowTargetLoss = false;
            projectileDirectionalTargetFinder.testLoS = false;
            projectileDirectionalTargetFinder.ignoreAir = false;
            projectileDirectionalTargetFinder.flierAltitudeTolerance = float.PositiveInfinity;
            projectileDirectionalTargetFinder.targetComponent = projectileTarget;

            var projectileHoming = ProjectilePrefab.AddComponent<ProjectileSteerTowardTarget>();
            projectileHoming.targetComponent = projectileTarget;
            projectileHoming.rotationSpeed = rotationSpd;
            projectileHoming.yAxisOnly = false;

            var projectileSimple = ProjectilePrefab.GetComponent<ProjectileSimple>();
            projectileSimple.enableVelocityOverLifetime = true;
            projectileSimple.updateAfterFiring = true;
            projectileSimple.velocityOverLifetime = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(2, 70) });

            Util.Helpers.CreateNetworkedProjectilePrefab(ProjectilePrefab);
            #endregion
        }

        public override void Hooks()
        {
            //On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.OnSkillActivated += CharacterBody_OnSkillActivated;
        }

        private void CharacterBody_OnSkillActivated(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
        {
            orig(self, skill);

            if (!NetworkServer.active || !self.skillLocator) return;

            var skillLocator = self.skillLocator;
            if (skillLocator.primary == skill && GetCount(self) > 0) 
            {
                FireShellProjectile(self);
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            var player = damageInfo.attacker.GetComponent<CharacterBody>();
            var enemy = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(player);
        }

        public void FireShellProjectile(CharacterBody player)
        {
            //Check every part for no obj reference
            #region Projectile Stats
            var baseDmg = 4f;
            var dmgMult = 1.5f;
            var dmgCalc = player.damage * (baseDmg * (dmgMult * GetCount(player)));

            var force = 2f;
            var maxDistance = 300f;
            var speedOverride = 50f;
            byte maxCombo = 3;
            #endregion

            #region Projectile
            var inputBank = player.inputBank;
            Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);

            ProjectileManager.instance.FireProjectile(new FireProjectileInfo
            {
                projectilePrefab = ProjectilePrefab,
                position = aimRay.origin,
                rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = player.gameObject,
                damage = dmgCalc,
                force = force,
                crit = RoR2.Util.CheckRoll(player.crit, player.master),
                damageColorIndex = DamageColorIndex.Default,
                speedOverride = speedOverride,
                maxDistance = maxDistance,
                comboNumber = maxCombo,
            });
            #endregion
        }

        public void CreateSFX()
        {

        }

        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //CreateProjectile();
            //Hooks();
        }

        public class ShellController : MonoBehaviour
        {
            readonly int maxBounces = 3;
            public int currBounces = 0;
            private void Awake()
            {
                base.enabled = false;
            }
            private void OnEnable()
            {

            }
            private void FixedUpdate()
            {

            }
            private void OnDisable()
            {

            }
        }
    }  
}