using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier3
{
    public class RoaringBlade : ItemBase<RoaringBlade>
    {
        public override string ItemName => "Roaring Blade";

        public override string ItemLangTokenName => "ROARING_BLADE";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("roaring_blade.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("roaring_blade_icon.png");

        public static GameObject SwoonModel = MainAssets.LoadAsset<GameObject>("swoon.prefab");

        public static BuffDef SwoonBuff;

        public static GameObject SwoonEffectPrefabL;
        public static GameObject SwoonEffectPrefabR;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            CreateSwoonEffect();
            Hooks();

        }

        public void CreateBuff()
        {
            SwoonBuff = ScriptableObject.CreateInstance<BuffDef>();
            SwoonBuff.name = "SwoonDebuff";
            SwoonBuff.iconSprite = ItemIcon;
            SwoonBuff.canStack = true;
            SwoonBuff.isDebuff = true;

            ContentAddition.AddBuffDef(SwoonBuff);
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += SwoonEffect;
        }

        private void SwoonEffect(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (!NetworkServer.active) return;

            var attacker = damageInfo.attacker;
            CharacterBody sender = attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            var existing = victimBody.GetComponent<SwoonDamageTracker>();

            #region Add Damage Tracker
            int itemCount = GetCount(sender);
            if (!existing && sender.inventory && itemCount > 0)
            {
                existing = victimBody.gameObject.AddComponent<SwoonDamageTracker>();
                existing.body = victimBody;
                existing.stack = itemCount;
            }
            else if (existing && itemCount <= 0) existing.enabled = false;
            else if (existing && itemCount > 0 && !existing.enabled) existing.enabled = true;
            if (existing) existing.stack = itemCount;
            #endregion

            // Add Buff
            if (existing && sender.inventory && itemCount > 0)
            {
                if (RoR2.Util.CheckRoll(50, sender.master))
                {
                    victimBody.AddBuff(SwoonBuff);
                }
            }

            // Buff Stack 1: set prev health
            if (existing && victimBody.GetBuffCount(SwoonBuff) <= 1)
            {
                existing.prevHealth = victimBody.healthComponent.health;
            }
 
            // Buff Stack 3: set curr health, do swoon
            if (existing && victimBody.GetBuffCount(SwoonBuff) >= 3)
            {
                existing.currHealth = victimBody.healthComponent.health;
                existing.DoSwoonDamage();
                for (int i = 0; i <= victimBody.GetBuffCount(SwoonBuff); i++)
                {
                    victimBody.RemoveBuff(SwoonBuff);
                }
            }

            orig(self, damageInfo, victim);
        }

        private static void CreateSwoonEffect()
        {
            SwoonEffectPrefabL = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("swoon_left.prefab"), "swoon_left", true);
            SwoonEffectPrefabR = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("swoon_right.prefab"), "swoon_right", true);

            if (!SwoonEffectPrefabL.GetComponent<EffectComponent>())
                SwoonEffectPrefabL.AddComponent<EffectComponent>();

            if (!SwoonEffectPrefabR.GetComponent<EffectComponent>())
                SwoonEffectPrefabR.AddComponent<EffectComponent>();

            if (!SwoonEffectPrefabL.GetComponent<NetworkIdentity>())
                SwoonEffectPrefabL.AddComponent<NetworkIdentity>();

            if (!SwoonEffectPrefabR.GetComponent<NetworkIdentity>())
                SwoonEffectPrefabR.AddComponent<NetworkIdentity>();

            if (SwoonEffectPrefabL) { PrefabAPI.RegisterNetworkPrefab(SwoonEffectPrefabL); }
            ContentAddition.AddEffect(SwoonEffectPrefabL);

            if (SwoonEffectPrefabR) { PrefabAPI.RegisterNetworkPrefab(SwoonEffectPrefabR); }
            ContentAddition.AddEffect(SwoonEffectPrefabR);
        }

        public class SwoonDamageTracker : CharacterBody.ItemBehavior
        {
            public float currHealth;
            public float prevHealth;
            float totalDamageTaken;

            private void OnDisable()
            {
                currHealth = 0;
                prevHealth = 0;
            }
            public void DoSwoonDamage()
            {
                totalDamageTaken = (prevHealth - currHealth) * (stack + 1);
                body.healthComponent.health -= totalDamageTaken;
                PlaySwoonAnimation();
                Debug.Log("Swooned " + body.name + " for " +totalDamageTaken + "!");
                Debug.Log("Prev HP " + prevHealth + " | Curr HP " + currHealth);
            }
            public void PlaySwoonAnimation()
            {
                Transform bodyTransform = body.transform;
                Debug.Log(body.GetComponentInChildren<Transform>());

                GameObject itemEffect = Instantiate(SwoonModel, bodyTransform);
                itemEffect.transform.localScale = new Vector3(120f, 120f, 120f);

                Animator anim = itemEffect.GetComponent<Animator>();

                Transform leftAttach = itemEffect.transform.Find("Roaring_Blade1");
                Transform rightAttach = itemEffect.transform.Find("Roaring_Blade");

                var leftEffect = Instantiate(SwoonEffectPrefabL, leftAttach);
                leftEffect.transform.localPosition = Vector3.zero;
                leftEffect.transform.localScale = new Vector3(20f, 20f, 20f); ;

                var rightEffect = Instantiate(SwoonEffectPrefabR, rightAttach);
                rightEffect.transform.localPosition = Vector3.zero;
                rightEffect.transform.localScale = new Vector3(20f, 20f, 20f);

                anim.speed = 18f;
                Destroy(itemEffect, 0.35f);
            }
        }
    }
}
