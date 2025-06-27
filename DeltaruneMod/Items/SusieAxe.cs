using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public static class SusieAxe
    {
        /*
        public static GameObject SusieAxePrefab;
        public static GameObject SusieAxeProjectilePrefab;
        private static Sprite SusieAxeIcon;
        private static Sprite SusieAxeEffectIcon;
        public static ItemDef susieAxeItemDef;
        public static BuffDef SusieAxeBuff;
        private static AssetBundle bundle;
        private static GameObject axeController;


        public static void Init()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DeltaruneMod.susie_axe")) 
            {
                bundle = AssetBundle.LoadFromStream(stream);

                SusieAxePrefab = bundle.LoadAsset<GameObject>("susie_axe.prefab");
                SusieAxeProjectilePrefab = bundle.LoadAsset<GameObject>("susie_axe.prefab").InstantiateClone("susie_axe_proj.prefab", true);
                SusieAxeIcon = bundle.LoadAsset<Sprite>("susie_axe_effect_icon.png");
                SusieAxeEffectIcon = bundle.LoadAsset<Sprite>("susie_axe_effect_icon.png");
            }
            //shurikenPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile");
 
            SusieAxeItem();
            CreateSusieAxe();
            CreateBuff();
            AddLanguageTokens();
        }

        public static void SusieAxeItem()
        {
            susieAxeItemDef = ScriptableObject.CreateInstance<ItemDef>();
            susieAxeItemDef.name = "SusieAxe";
            susieAxeItemDef.tier = ItemTier.Tier1;
            susieAxeItemDef.deprecatedTier = ItemTier.Tier1;
            susieAxeItemDef.pickupModelPrefab = SusieAxePrefab;
            susieAxeItemDef.pickupIconSprite = SusieAxeIcon;
            susieAxeItemDef.nameToken = "SUSIEAXE_NAME";
            susieAxeItemDef.pickupToken = "SUSIEAXE_PICKUP";
            susieAxeItemDef.descriptionToken = "SUSIEAXE_DESC";
            susieAxeItemDef.loreToken = "SUSIEAXE_LORE";
            susieAxeItemDef.tags = new[]
            {
                ItemTag.Damage
            };
            susieAxeItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();

            var itemDisplay = SusieAxePrefab.AddComponent<ItemDisplay>();
            ItemDisplayRuleDict rules = GetItemDisplayRules();
            var susieAxe = new CustomItem(susieAxeItemDef, rules);
            ItemAPI.Add(susieAxe);
        }

        private static void AddLanguageTokens()
        {
            LanguageAPI.Add("SUSIEAXE_NAME", "Rude Buster");
            LanguageAPI.Add("SUSIEAXE_PICKUP", "Shoot a rude buster on primary skill.");
            LanguageAPI.Add("SUSIEAXE_DESC",
                "Every 30 seconds, fire a powerful rude buster.\nDMG: Base dmg * 10 + 3 (per item).");
            LanguageAPI.Add("SUSIEAXE_LORE", "ASS");
        }

        public static void SusieAxeEffect(CharacterBody body)
        {
            if (!NetworkServer.active || !body) return;

            var itemCount = body.inventory.GetItemCount(susieAxeItemDef);
            var existing = body.GetComponent<PrimarySkillSusieAxeBehavior>();
            if (!existing && body.inventory && itemCount > 0)
            {
                existing = body.gameObject.AddComponent<PrimarySkillSusieAxeBehavior>();
                existing.body = body;
                existing.enabled = true;
                existing.stack = itemCount;
                existing.totalReloadTime = 5f;
                existing.damageCoefficientBase = 9f;
                existing.damageCoefficientPerStack = 3f;
            }
            else if (existing && itemCount <= 0) existing.enabled = false;
            else if (existing && itemCount > 0 && !existing.enabled) existing.enabled = true;
            if (existing) existing.stack = itemCount;
        }

        private static void CreateBuff()
        {
            SusieAxeBuff = ScriptableObject.CreateInstance<BuffDef>();
            SusieAxeBuff.name = "SusieAxeBuff";
            SusieAxeBuff.iconSprite = SusieAxeEffectIcon;
            SusieAxeBuff.canStack = true;
            SusieAxeBuff.isDebuff = false;

            ContentAddition.AddBuffDef(SusieAxeBuff);
        }

        private static GameObject CreateGhostPrefab(string ghostNameToSet)
        {
            GameObject ghostPrefab = SusieAxeProjectilePrefab.InstantiateClone(ghostNameToSet, true);
            ghostPrefab.AddComponent<NetworkIdentity>();
            ghostPrefab.AddComponent<ProjectileGhostController>();
            ghostPrefab.AddComponent<VFXAttributes>().DoNotPool = true;

            return ghostPrefab;
        }
        private static void CreateSusieAxe()
        {
            GameObject axeProjectile = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile").InstantiateClone("AxeProjectile", true);

            axeController = axeProjectile.GetComponent<ProjectileController>().ghostPrefab = CreateGhostPrefab("axePrefab");
        }

        [RequireComponent(typeof(TeamFilter))]
        public class PrimarySkillSusieAxeBehavior : CharacterBody.ItemBehavior
        {
            public GameObject projectilePrefabInstance;
            private bool setPrefab = false;

            private void Awake()
            {
                enabled = false;
            }
            private void Start()
            {
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
                SkillLocator skillLocator = this.skillLocator;
                if (skillLocator.primary && body.GetBuffCount(SusieAxeBuff) > 0)
                {
                    if (NetworkServer.active)
                    {
                        body.RemoveBuff(SusieAxeBuff);
                    }
                    FireSusieAxe();
                }
            }
            private void FixedUpdate()
            {
                if (!NetworkServer.active) return;

                var hasItem = stack > 0;
                if (projectilePrefabInstance != hasItem)
                {
                    if (hasItem)
                    {
                        projectilePrefabInstance = Instantiate(axeController);
                        projectilePrefabInstance.GetComponent<TeamFilter>().teamIndex = body.teamComponent.teamIndex;
                        projectilePrefabInstance.GetComponent<BuffWard>().Networkradius = 25f + body.radius;
                        projectilePrefabInstance.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(body.gameObject);
                    }
                    else
                    {
                        Destroy(projectilePrefabInstance);
                        projectilePrefabInstance = null;
                    }
                }

                int num = stack + 1;
                if (body.GetBuffCount(SusieAxeBuff) < num)
                {
                    float num2 = totalReloadTime / num;
                    reloadTimer += Time.fixedDeltaTime;
                    while (reloadTimer > num2 && body.GetBuffCount(SusieAxeBuff) < num)
                    {
                        body.AddBuff(SusieAxeBuff);
                        reloadTimer -= num2;
                    }
                }
            }
            private void FireSusieAxe()
            {
                Ray aimRay = GetAimRay();
                ProjectileManager.instance.FireProjectileWithoutDamageType(projectilePrefabInstance, aimRay.origin, RoR2.Util.QuaternionSafeLookRotation(aimRay.direction) * GetRandomRollPitch(), gameObject, body.damage * (damageCoefficientBase + damageCoefficientPerStack * stack), 0f, RoR2.Util.CheckRoll(body.crit, body.master), DamageColorIndex.Item, null, -1f);
            }
            private Ray GetAimRay()
            {
                if (inputBank)
                {
                    return new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                }
                return new Ray(transform.position, transform.forward);
            }
            protected Quaternion GetRandomRollPitch()
            {
                Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward);
                Quaternion quaternion2 = Quaternion.AngleAxis(0f + UnityEngine.Random.Range(0f, 1f), Vector3.left);
                return quaternion * quaternion2;
            }
            private const float minSpreadDegrees = 0f;
            private const float rangeSpreadDegrees = 0.2f;
            private const int numShurikensPerStack = 1;
            private const int numShurikensBase = 1;
            public const string projectilePrefabPath = "Prefabs/Projectiles/ShurikenProjectile";
            public float totalReloadTime = 5f;
            public float damageCoefficientBase = 10f;
            public float damageCoefficientPerStack = 3f;
            private const float force = 0f;
            private SkillLocator skillLocator;
            private float reloadTimer;
            public GameObject projectilePrefab;
            private InputBankTest inputBank;
        }

        private static ItemDisplayRuleDict GetItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
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
                    followerPrefab = SusieAxePrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.01619F, -0.62815F, 0.30422F),
                    localAngles = new Vector3(4.056F, 154.1342F, 354.7828F),
                    localScale = new Vector3(8F, 8F, 8F)

                }
            });
            return rules;
        }
        */
    }

}
