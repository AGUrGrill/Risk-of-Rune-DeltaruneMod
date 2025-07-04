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
        public override string ItemName => "Rude Buster";

        public override string ItemLangTokenName => "SUSIE_AXE";

        public override string ItemPickupDesc => "Shoot a Red Buster on Primary or Secondary skill and heal.";

        public override string ItemFullDescription => "On <style=cIsUtility>Primary or Seconary skill</style> activation, fire a Red Buster and activate UtilmateHeal." +
            "\nRed Buster: Shoot a projectile that deals <style=cIsDamage>600%</style> base damage <style=cStack>(+200% per stack)</style>." +
            "\nRed Buster reloads every <style=cIsUtility>5</style> seconds, stacking <style=cIsUtility>1</style> stack per item." +
            "\nUltimateHeal: Heal <style=cIsHealing>10% hp</style> on use.";

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

        public void CreateProjectile()
        {
            projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile").InstantiateClone("SusieAxeProjectile", true);
            if (!projectilePrefab.GetComponent<NetworkIdentity>()) projectilePrefab.AddComponent<NetworkIdentity>();

            var ghost = MainAssets.LoadAsset<GameObject>("rude_buster.prefab").InstantiateClone("rude_buster", true);
            ghost.AddComponent<ProjectileGhostController>();
            ghost.AddComponent<NetworkIdentity>();
            ghost.transform.localScale = new Vector3(150f, 150f, 150f);

            var projCont = projectilePrefab.GetComponent<ProjectileController>();
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
                    localPos = new Vector3(-0.04222F, -0.44262F, 0.35685F),
                    localAngles = new Vector3(11.85053F, 148.0504F, 351.0004F),
                    localScale = new Vector3(7F, 7F, 7F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.61995F, 0.23677F, 0.12246F),
                    localAngles = new Vector3(50.48947F, 179.8686F, 88.65546F),
                    localScale = new Vector3(6F, 6F, 6F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.75843F, 0.44873F, -5.97378F),
                    localAngles = new Vector3(48.64816F, 311.6693F, 319.3044F),
                    localScale = new Vector3(60F, 60F, 60F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.07696F, -0.82386F, 0.0362F),
                    localAngles = new Vector3(350.3063F, 149.1217F, 7.03048F),
                    localScale = new Vector3(8F, 8F, 8F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02684F, -0.25777F, 0.1529F),
                    localAngles = new Vector3(0.99506F, 146.1651F, 359.0777F),
                    localScale = new Vector3(3F, 3F, 3F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.05516F, -0.36669F, 0.19114F),
                    localAngles = new Vector3(0.78083F, 149.236F, 1.34581F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-0.07465F, -1.96628F, 0.74331F),
                    localAngles = new Vector3(3.95929F, 149.5434F, 357.3385F),
                    localScale = new Vector3(10F, 10F, 10F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.03695F, -0.38561F, 0.19133F),
                    localAngles = new Vector3(358.3326F, 145.6386F, 1.02962F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.31579F, 6.3261F, -2.34545F),
                    localAngles = new Vector3(39.29791F, 216.9755F, 205.7235F),
                    localScale = new Vector3(50F, 50F, 50F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.04942F, -0.43712F, 0.1296F),
                    localAngles = new Vector3(358.6895F, 151.388F, 1.96694F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02233F, -0.43605F, 0.13625F),
                    localAngles = new Vector3(359.8591F, 149.4673F, 358.1331F),
                    localScale = new Vector3(4.6F, 4.6F, 4.6F)

                }
            });
            rules.Add("mdlHeretic", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.02105F, -0.87071F, 0.01385F),
                    localAngles = new Vector3(355.2848F, 47.55381F, 355.0908F),
                    localScale = new Vector3(0.20392F, 0.20392F, 0.20392F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.04595F, -0.25266F, 0.13105F),
                    localAngles = new Vector3(3.30229F, 153.9347F, 3.33027F),
                    localScale = new Vector3(3F, 3F, 3F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.0316F, -0.32915F, 0.44771F),
                    localAngles = new Vector3(26.42275F, 153.9711F, 348.9298F),
                    localScale = new Vector3(6F, 5F, 5F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.81985F, 0.20914F, -0.05524F),
                    localAngles = new Vector3(55.56215F, 357.1792F, 266.0527F),
                    localScale = new Vector3(7F, 6F, 6F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.04614F, -0.32434F, 0.20333F),
                    localAngles = new Vector3(4.86292F, 154.7291F, 0.52935F),
                    localScale = new Vector3(4.5F, 4F, 4F)

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
            public float healPercent = 0.10f;
            public float healPercentPerStack = 0.05f;

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

                    float healCalc = body.maxHealth * (healPercent + (healPercentPerStack * (stack-1)));
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
