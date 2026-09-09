using DeltaruneMod.Util;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier2
{
    public class Umbrella : ItemBase<Umbrella>
    {
        public override string ItemName => "Weather Report";

        public override string ItemLangTokenName => "UMBRELLA";

        public override string ItemPickupDesc => "10% chance to launch a cloudy debuff or a burning sun projectile.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("tension_bow.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tension_bow_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;
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
            projectileDirectionalTargetFinder.allowTargetLoss = true;
            projectileDirectionalTargetFinder.testLoS = true;
            projectileDirectionalTargetFinder.ignoreAir = true;
            //projectileDirectionalTargetFinder.flierAltitudeTolerance = float.PositiveInfinity;
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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.BulletAttack.ProcessHitList += BulletAttack_ProcessHitList;
        }

        // Code derived from ThinkInvis Pinball
        private GameObject BulletAttack_ProcessHitList(On.RoR2.BulletAttack.orig_ProcessHitList orig, BulletAttack self, List<BulletAttack.BulletHit> hits, ref Vector3 endPosition, List<GameObject> ignoreList)
        {
            var retv = orig(self, hits, ref endPosition, ignoreList);

            if (!self.owner) return retv;

            var body = self.owner.GetComponent<CharacterBody>();
            var count = GetCount(body);

            if (count > 0)
            {
                var baseBounces = 2;
                var stackBounces = 1;
                var bounceChance = 0.1f;
                var bounceDamageFrac = 0.1f;
                var maxBounces = baseBounces + (count - 1) * stackBounces;

                var bounceEnd = endPosition
                    - self.aimVector * 0.25f; //back ray off slightly for world clearance

                GameObject lastBounceTarget = null;

                var origDamage = self.damage;
                for (var i = 1; i <= maxBounces; i++)
                {
                    if (!RoR2.Util.CheckRoll(bounceChance, body.master?.luck ?? 0, body.master)) return retv;
                    var enemies = Helpers.GatherEnemies(TeamComponent.GetObjectTeam(self.owner), TeamIndex.Neutral)
                        .Select(x => Helpers.GetRootWithLocators(x.gameObject))
                        .Select(obj => {
                            if (obj == lastBounceTarget) return (null, default);
                            var hc = obj.GetComponent<HealthComponent>();
                            if (!hc || !hc.alive) return (null, default);
                            var dvec = (obj.transform.position - bounceEnd);
                            var ddist = dvec.magnitude;
                            if (ddist > self.maxDistance) return (null, default);
                            var ray = new Ray(bounceEnd, dvec.normalized);
                            var worldcastDidHit = Physics.Raycast(ray, ddist, LayerIndex.world.mask, QueryTriggerInteraction.Ignore);
                            if (worldcastDidHit)
                                return (null, default);
                            var rayDidHit = Physics.Raycast(ray, out var rayHitInfo, ddist, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Ignore);
                            if (!rayDidHit)
                                return (null, default);
                            return (obj, rayHitInfo);
                        }).Where(kvp => kvp.obj != null);

                    if (enemies.Count() > 0)
                    {
                        var (obj, rayHitInfo) = enemies.ToArray()[0];
                        var aimVec = (obj.transform.position - bounceEnd).normalized;
                        var nhi = BulletAttack.bulletHitPool.Request();
                        self.damage = origDamage * bounceDamageFrac;
                        self.InitBulletHitFromRaycastHit(ref nhi, new(bounceEnd, aimVec), ref rayHitInfo);
                        self.ProcessHit(ref nhi);

                        if (self.tracerEffectPrefab)
                        {
                            EffectData effectData = new()
                            {
                                origin = obj.transform.position,
                                start = bounceEnd
                            };
                            EffectManager.SpawnEffect(self.tracerEffectPrefab, effectData, true);
                        }

                        EffectData pinbEffData = new()
                        {
                            origin = bounceEnd,
                            start = bounceEnd
                        };

                        bounceEnd = obj.transform.position;
                        lastBounceTarget = obj;
                    }
                    else break;
                }

                self.damage = origDamage;
            }   

			return retv;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            var player = damageInfo.attacker.GetComponent<CharacterBody>();
            var enemy = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(player);

            if (!NetworkServer.active || !player.skillLocator) return;

            var skillLocator = player.skillLocator;
            if (GetCount(player) > 0)
            {
                FireUmbrellaProjectile(enemy);
            }
        }

        public void FireUmbrellaProjectile(CharacterBody owner)
        {
            //Check every part for no obj reference
            #region Projectile Stats
            var baseDmg = 4f;
            var dmgMult = 1.5f;
            var dmgCalc = owner.damage * (baseDmg * (dmgMult * GetCount(owner)));

            var force = 2f;
            var maxDistance = 300f;
            var speedOverride = 50f;
            byte maxCombo = 3;
            #endregion

            #region Projectile
            var inputBank = owner.inputBank;
            Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var target = Helpers.GatherEnemies(TeamComponent.GetObjectTeam(owner.gameObject), TeamIndex.Neutral)[0];//GetNearestTarget(owner, 300);
            if (target != null)
            {
                Debug.Log("target is " + target.name);
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    projectilePrefab = ProjectilePrefab,
                    position = aimRay.origin,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                    owner = owner.gameObject,
                    damage = dmgCalc,
                    force = force,
                    crit = RoR2.Util.CheckRoll(owner.crit, owner.master),
                    damageColorIndex = DamageColorIndex.Default,
                    speedOverride = speedOverride,
                    maxDistance = maxDistance,
                    comboNumber = maxCombo,
                    target = target.gameObject
                });
            } 
            #endregion
        }

        public CharacterBody GetNearestTarget(CharacterBody owner, float maxSearchRadius)
        {
            BullseyeSearch search = new BullseyeSearch();

            search.viewer = owner;
            search.searchOrigin = owner.corePosition;
            search.searchDirection = owner.inputBank ? owner.inputBank.aimDirection : Vector3.forward;
            search.maxDistanceFilter = maxSearchRadius;

            TeamMask friendlyMask = new TeamMask();
            friendlyMask.AddTeam(owner.teamComponent.teamIndex);
            search.teamMaskFilter = friendlyMask;

            search.sortMode = BullseyeSearch.SortMode.Distance;

            search.RefreshCandidates();

            HurtBox[] closestHurtBoxs = search.GetResults().ToArray();
            if (closestHurtBoxs.Length > 0)
            {
                if (closestHurtBoxs[1] && closestHurtBoxs[1].healthComponent)
                {
                    return closestHurtBoxs[1].healthComponent.body;
                }
            }

            return null; 
        }

        public void CreateSFX()
        {

        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateProjectile();
            Hooks();
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
