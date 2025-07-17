using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier3
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

        public GameObject ProjectileModel = MainAssets.LoadAsset<GameObject>("hammer_justice.prefab");

        public static GameObject ProjectilePrefab;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            var itemCount = GetCount(sender);
            var behaviorExists = sender.GetComponent<HammerOfJusticeBehavior>();

            if (sender.inventory && itemCount > 0 && !behaviorExists)
            {
                behaviorExists = sender.gameObject.AddComponent<HammerOfJusticeBehavior>();
            }
            else if (sender.inventory && itemCount <= 0 && behaviorExists)
            {
                behaviorExists.enabled = false;
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (!NetworkServer.active) return;

            var player = damageInfo.attacker.GetComponent<CharacterBody>();
            var enemy = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(player);
            var behavior = player.GetComponent<HammerOfJusticeBehavior>();

            if (player.inventory && itemCount > 0 && behavior)
            {
                behavior.FireProjectile();
                Debug.Log("Preparing to fire");
            }
        }

        

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateProjectile();
            Hooks();
        }

        public void CreateProjectile()
        {
            var seekDistance = 80;
            byte combo = 3;
            ProjectilePrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/FMJ"), "shell_projectile", true);

            var projMissileController = ProjectilePrefab.AddComponent<MissileController>();
            projMissileController.maxSeekDistance = seekDistance;

            var projController = ProjectilePrefab.GetComponent<ProjectileController>();
            projController.ghostPrefab = ProjectileModel;
            projController.combo = combo;

            Util.Helpers.CreateNetworkedProjectilePrefab(ProjectilePrefab);
        }

        public void CreateSFX()
        {

        }

        public class HammerOfJusticeBehavior : CharacterBody.ItemBehavior
        {
            private int maxTargets = 3;
            private int currBounces = 0;
            SkillLocator skillLocator;
            InputBankTest inputBank;

            private void OnEnable()
            {
                StartCoroutine(DelayedInit());
            }

            private IEnumerator DelayedInit()
            {
                yield return new WaitForSeconds(0.1f); 
                if (body)
                {
                    skillLocator = body.skillLocator;
                    inputBank = body.inputBank; 
                }
            }
            private void OnDisable()
            {
                Destroy(this);
            }
            public void FireProjectile()
            {
                Debug.Log("starting");
                if (!NetworkServer.active || inputBank == null) return;
                Debug.Log("passsed");
                var target = ProjectilePrefab.GetComponent<MissileController>();
                Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection); //null
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    projectilePrefab = ProjectilePrefab,
                    position = aimRay.origin,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                    owner = gameObject,
                    damage = 10f,
                    force = 1f,
                    crit = RoR2.Util.CheckRoll(body.crit, body.master),
                    damageColorIndex = DamageColorIndex.Default,
                    target = target.targetComponent.target.gameObject,
                });
                Debug.Log(target.targetComponent.target);
            }
        }
    } 
}
