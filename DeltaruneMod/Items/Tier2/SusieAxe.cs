using DeltaruneMod.Util;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier2
{
    public class SusieAxe : ItemBase<SusieAxe>
    {
        public override string ItemName => "Monster's Axe";

        public override string ItemLangTokenName => "SUSIE_AXE";

        public override string ItemPickupDesc => "Shoot a Rude Buster on Primary or Secondary skill and activate UltimateHeal.";

        public override string ItemFullDescription => "On <style=cIsUtility>Primary or Seconary skill</style> activation, fire a Rude Buster and activate UtilmateHeal." +
            "\nRude Buster: Shoot a projectile that deals <style=cIsDamage>600%</style> base damage <style=cStack>(+200% per stack)</style>." +
            "\nRude Buster reloads every <style=cIsUtility>5</style> seconds, stacking <style=cIsUtility>1</style> stack per item." +
            "\nUltimateHeal: Heal <style=cIsHealing>5% hp</style> on use.";

        public override string ItemLore => "\"Where did you find this?\" ... \"This is the axe that ended the roaring!\"" +
            "\n\"It's owner was Susie the Hero... though she perferred many other names...\"" +
            "\n\"The other names?? Well... some include Violent Ax Susie, Susiezilla, AS- maybe I shouldn't tell you this...";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("susie_axe.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("susie_axe_icon 1.png");

        public Sprite BuffIcon = MainAssets.LoadAsset<Sprite>("rude_buster_effect_icon.png");

        public static GameObject projectilePrefab;

        public BuffDef SusieAxeBuff;

        public static GameObject SusieAxeEffectPrefab;

        public static NetworkSoundEventDef SusieAxeSound;

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            CreateEffect();
            CreateProjectile();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += SusieAxeEffect;
        }

        
        public void SusieAxeEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender) return;

            var itemCount = GetCount(sender);
            var existing = sender.GetComponent<PrimarySkillSusieAxeBehavior>();
            if (!existing && sender.inventory && itemCount > 0)
            {
                existing = sender.gameObject.AddComponent<PrimarySkillSusieAxeBehavior>();
                existing.body = sender;
                existing.stack = itemCount;
                existing.SusieAxeBuff = SusieAxeBuff;
            }
            else if (existing && itemCount <= 0) existing.enabled = false;
            else if (existing && itemCount > 0 && !existing.enabled) existing.enabled = true;
            if (existing) existing.stack = itemCount;
        }

        public void CreateBuff()
        {
            SusieAxeBuff = ScriptableObject.CreateInstance<BuffDef>();
            SusieAxeBuff.name = "SusieAxeBuff";
            SusieAxeBuff.iconSprite = BuffIcon;
            SusieAxeBuff.canStack = true;
            SusieAxeBuff.isDebuff = false;

            ContentAddition.AddBuffDef(SusieAxeBuff);
        }

        public void CreateEffect()
        {
            SusieAxeEffectPrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile"), "SusieAxeEffect", true);

            SusieAxeSound = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            SusieAxeSound.eventName = "Play_rude_buster";
            SusieAxeSound.name = "rude_buster_sfx";
            R2API.ContentAddition.AddNetworkSoundEventDef(SusieAxeSound);

            var effectComponent = SusieAxeEffectPrefab.GetComponent<EffectComponent>() ?? SusieAxeEffectPrefab.AddComponent<EffectComponent>();
            effectComponent.soundName = "Play_rude_buster";

            if (!SusieAxeEffectPrefab.GetComponent<NetworkIdentity>())
                SusieAxeEffectPrefab.AddComponent<NetworkIdentity>();

            if (SusieAxeEffectPrefab) { PrefabAPI.RegisterNetworkPrefab(SusieAxeEffectPrefab); }
            ContentAddition.AddEffect(SusieAxeEffectPrefab);
        }
        // Spawns shuriken at map spawn
        public void CreateProjectile()
        {
            projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile").InstantiateClone("SusieAxeProjectile", true);
            if (!projectilePrefab.GetComponent<NetworkIdentity>()) projectilePrefab.AddComponent<NetworkIdentity>();

            var ghost = MainAssets.LoadAsset<GameObject>("rude_buster.prefab").InstantiateClone("rude_buster", true);
            ghost.AddComponent<ProjectileGhostController>();
            ghost.AddComponent<NetworkIdentity>();
            ghost.transform.localScale = new Vector3(150f, 150f, 150f);

            var projCont = projectilePrefab.GetComponent<ProjectileController>();
            if (projCont.ghostPrefab != null) UnityEngine.Object.Destroy(projCont.ghostPrefab);
            projCont.startSound = "";
            projCont.shouldPlaySounds = false;
            projCont.ghostPrefab = ghost;

            var projSimp = projectilePrefab.GetComponent<ProjectileSimple>();
            projSimp.desiredForwardSpeed *= 0.5f;
            projSimp.GetComponent<Rigidbody>().useGravity = false;

            UnityEngine.Object.Destroy(projectilePrefab.GetComponent<ProjectileSteerTowardTarget>());
            UnityEngine.Object.Destroy(projectilePrefab.GetComponent<ProjectileTargetComponent>());

            PrefabAPI.RegisterNetworkPrefab(projectilePrefab);
            ContentAddition.AddProjectile(projectilePrefab);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.34926F, 0.25313F, -0.41051F),
                    localAngles = new Vector3(14.01768F, 50.88708F, 82.11794F),
                    localScale = new Vector3(23.74224F, 23.74224F, 23.74224F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.09873F, -0.13067F, 0.02605F),
                    localAngles = new Vector3(23.6536F, 93.07247F, 24.55241F),
                    localScale = new Vector3(25.51046F, 16.67866F, 25.51046F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "UpperArmL",
                    localPos = new Vector3(0.40818F, 0.30742F, 0.55069F),
                    localAngles = new Vector3(2.01947F, 281.0584F, 67.86629F),
                    localScale = new Vector3(166.2022F, 166.2022F, 166.2022F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.28472F, 0.15643F, -0.12816F),
                    localAngles = new Vector3(354.5077F, 347.7494F, 281.5692F),
                    localScale = new Vector3(50.83732F, 50.83732F, 50.83732F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.12374F, 0.37286F, -0.03677F),
                    localAngles = new Vector3(5.54981F, 275.2047F, 216.0641F),
                    localScale = new Vector3(18.16601F, 18.16601F, 18.16601F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.18235F, -0.04304F, -0.42368F),
                    localAngles = new Vector3(340.2831F, 185.451F, 340.5156F),
                    localScale = new Vector3(24.97854F, 24.97854F, 24.97854F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-1.09289F, 0.13743F, -0.11748F),
                    localAngles = new Vector3(3.25975F, 162.9752F, 33.30826F),
                    localScale = new Vector3(30.4944F, 30.4944F, 30.4944F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.28829F, 0.38631F, 0.02157F),
                    localAngles = new Vector3(6.18454F, 3.13923F, 148.0287F),
                    localScale = new Vector3(13.47214F, 13.47214F, 13.47214F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-2.4045F, -1.67265F, 2.1141F),
                    localAngles = new Vector3(354.3773F, 254.687F, 358.6025F),
                    localScale = new Vector3(121.9903F, 121.9903F, 121.9903F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.34605F, 0.39082F, -0.10218F),
                    localAngles = new Vector3(5.15182F, 25.06269F, 146.8649F),
                    localScale = new Vector3(14.66418F, 14.66418F, 14.66418F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.09482F, 0.14564F, 0.01243F),
                    localAngles = new Vector3(331.879F, 77.49539F, 210.0147F),
                    localScale = new Vector3(15.98449F, 15.98449F, 15.98449F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "BottomRail",
                    localPos = new Vector3(-0.01703F, 0.23118F, -0.02844F),
                    localAngles = new Vector3(1.06541F, 180.4571F, 13.00509F),
                    localScale = new Vector3(15.71294F, 15.71294F, 15.71294F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CannonEnd",
                    localPos = new Vector3(0.13748F, -0.11991F, 0.07944F),
                    localAngles = new Vector3(4.53002F, 272.7654F, 4.84901F),
                    localScale = new Vector3(21.07476F, 24.49854F, 17.5623F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.04536F, -0.42105F, 0.09325F),
                    localAngles = new Vector3(320.1729F, 10.61689F, 101.4086F),
                    localScale = new Vector3(21.83015F, 18.71156F, 18.71156F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.2744F, -0.30065F, 0.07598F),
                    localAngles = new Vector3(328.4284F, 358.3063F, 35.0899F),
                    localScale = new Vector3(15.67788F, 13.93588F, 13.93588F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01619F, -0.62815F, 0.30422F),
                    localAngles = new Vector3(4.056F, 154.1342F, 354.7828F),
                    localScale = new Vector3(8F, 8F, 8F)

                }
            });
            return rules;
        }


        [RequireComponent(typeof(TeamFilter))]
        public class PrimarySkillSusieAxeBehavior : CharacterBody.ItemBehavior
        {
            public const int numShurikensPerStack = 1;
            public const int numShurikensBase = 1;
            public float damageCoefficientBase =  6f;
            public float damageCoefficientPerStack = 2f;
            public const float force = 4f;
            public float reloadTime = 5f;
            public float reloadTimer;
            public float healPercent = 0.05f;

            public SkillLocator skillLocator;
            public GameObject projectilePrefab;
            public GameObject susieProjPrefab;
            public BuffDef SusieAxeBuff;
            public InputBankTest inputBank;

            private void Awake()
            {
                enabled = false;
            }
            private void Start()
            {
                projectilePrefab = SusieAxe.projectilePrefab;
            }

            private void OnEnable()
            {
                if (body)
                {
                    body.onSkillActivatedServer += new Action<GenericSkill>(OnSkillActivated);
                    skillLocator = body.GetComponent<SkillLocator>();
                    inputBank = body.GetComponent<InputBankTest>();
                }
            }
            private void OnDisable()
            {
                if (body)
                {
                    body.onSkillActivatedServer -= new Action<GenericSkill>(OnSkillActivated);
                    if (NetworkServer.active)
                    {
                        int num = 10000;
                        while (body.HasBuff(SusieAxeBuff) && num > 0)
                        {
                            num--;
                            body.RemoveBuff(SusieAxeBuff);
                        }
                    }
                }
                inputBank = null;
                skillLocator = null;
            }
            private void OnSkillActivated(GenericSkill skill)
            {
                if (!NetworkServer.active) return;

                SkillLocator skillLocator = this.skillLocator;
                if (((skillLocator != null ? skillLocator.primary : null) == skill  || (skillLocator != null ? skillLocator.secondary : null) == skill)  && body.GetBuffCount(SusieAxeBuff) > 0)
                {
                    FireSusieAxe();
                    //RoR2.Util.PlaySound("Play_rude_buster", gameObject);
                    //RpcPlaySusieSound();
                    EffectManager.SpawnEffect(SusieAxeEffectPrefab, new EffectData { origin = transform.position, scale = 1f }, true);
                    body.RemoveBuff(SusieAxeBuff);

                    float healCalc = body.maxHealth * healPercent;
                    //Debug.Log(body.maxHealth + " | Heal: " + healCalc);
                    if (body.healthComponent.health + healCalc > body.maxHealth) body.healthComponent.health = body.maxHealth;
                    else body.healthComponent.health += healCalc;
                }
            }
            private void FixedUpdate()
            {
                if (!NetworkServer.active) return;

                int numOfShurikens = numShurikensBase + stack;
                if (body.GetBuffCount(SusieAxeBuff) < numOfShurikens)
                {
                    float reloadNum = reloadTime;
                    reloadTimer += Time.fixedDeltaTime;
                    while (reloadTimer > reloadNum && body.GetBuffCount(SusieAxeBuff) < numOfShurikens)
                    {
                        body.AddBuff(SusieAxeBuff);
                        reloadTimer -= reloadNum;
                    }
                }
            }
            private void FireSusieAxe()
            {
                Ray aimRay = GetAimRay();
                float dmgCalc = body.damage * (damageCoefficientBase + (damageCoefficientPerStack * (stack - 1)));
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    projectilePrefab = projectilePrefab,
                    position = aimRay.origin,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                    owner = gameObject,
                    damage = dmgCalc,
                    force = force,
                    crit = RoR2.Util.CheckRoll(body.crit, body.master),
                    damageColorIndex = DamageColorIndex.Item,
                    comboNumber = 3,
                }); 
            }
            private Ray GetAimRay()
            {
                return new Ray(body.inputBank.aimOrigin, body.inputBank.aimDirection);
            }
        }
    }

}
