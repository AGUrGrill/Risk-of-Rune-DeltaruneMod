using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Spamton
{
    public class MalfunctionCore : ItemBase<MalfunctionCore>
    {
        public override string ItemName => "Malfunctioning Core";

        public override string ItemLangTokenName => "MALFUNCTION_CORE";

        public override string ItemPickupDesc => "On kill, enemies drop an orb, giving 2 temporary shield.";

        public override string ItemFullDescription => "On kill, enemies will drop an orb that gives <style=cIsUtility>2</style> temporary shield <style=cStack>(+1 per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("yoru_orb_plus.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ok.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public readonly int additionalShield = 1;

        public readonly int baseShield = 2;

        public static readonly int shieldCap = 1997;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);

            var body = damageReport.attackerBody;
            var enemyBody = damageReport.victimBody;
            if (!body || !body.isPlayerControlled) return;
            var itemCount = GetCount(body);

            if (GetCount(body) > 0)
            {
                CoreOrb orb = new CoreOrb();
                orb.target = body.mainHurtBox;
                orb.shieldValue = baseShield + additionalShield * (itemCount-1);
                OrbManager.instance.AddOrb(orb);
            }
        }

        public void CreateOrb()
        {
            OrbAPI.AddOrb<CoreOrb>();
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateOrb();
            Hooks();
        }

        public class CoreOrb : Orb
        {
            public float shieldValue;

            public bool scaleOrb = true;

            public float overrideDuration = 0.6f;

            public override void Begin()
            {
                if (target)
                {
                    duration = overrideDuration;
                    float scale = scaleOrb ? Mathf.Min(shieldValue / target.healthComponent.fullShield, 1f) : 1f;
                    EffectData effectData = new EffectData
                    {
                        scale = scale,
                        origin = origin,
                        genericFloat = duration
                    };
                    effectData.SetHurtBoxReference(target);
                    EffectManager.SpawnEffect(OrbStorageUtility.Get("Prefabs/Effects/OrbEffects/SquidOrbEffect"), effectData, transmit: true);
                }
            }

            public override void OnArrival()
            {
                if (target)
                {
                    var healthComp = target.healthComponent;
                    if (healthComp)
                    {
                        if (healthComp.shield + shieldValue <= shieldCap)
                            healthComp.shield += shieldValue;
                    }
                }
            }
        }
    }
}