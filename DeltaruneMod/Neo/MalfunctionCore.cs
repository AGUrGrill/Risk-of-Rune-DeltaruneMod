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

namespace DeltaruneMod.Neo
{
    public class MalfunctionCore : ItemBase<MalfunctionCore>
    {
        public override string ItemName => "Malfunctioning Core";

        public override string ItemLangTokenName => "MALFUNCTION_CORE";

        public override string ItemPickupDesc => "On kill, spawn an orb that gives 10 barrier.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("ok.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ok.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public readonly int additionalOvershield = 5;

        public readonly int baseOvershield = 10;

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
            if (!body || !body.isPlayerControlled) return;
            var itemCount = GetCount(body);

            if (GetCount(body) > 0)
            {
                CoreOrb orb = new CoreOrb();
                orb.target = body.mainHurtBox;
                orb.barrierValue = baseOvershield + (additionalOvershield * itemCount);
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
            public float barrierValue;

            public bool scaleOrb = true;

            public float overrideDuration = 0.6f;

            public override void Begin()
            {
                if (target)
                {
                    base.duration = overrideDuration;
                    float scale = (scaleOrb ? Mathf.Min(barrierValue / target.healthComponent.fullBarrier, 1f) : 1f);
                    EffectData effectData = new EffectData
                    {
                        scale = scale,
                        origin = origin,
                        genericFloat = base.duration
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
                        healthComp.AddBarrier(barrierValue);
                    }
                }
            }
        }
    }
}