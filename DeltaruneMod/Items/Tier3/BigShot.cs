using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier3
{
    public class BigShot : ItemBase<BigShot>
    {
        public override string ItemName => "Dealmaker";
        public override string ItemLangTokenName => "BIGSHOT";
        public override string ItemPickupDesc => "Gain stacks of <style=cDeath>[Big Shot]</style> on gold gain.";
        public override string ItemFullDescription => "<style=cShrine>+30%</style> gold gain.\nOn gold gain <style=cStack>($50 per stage)</style>, gain <style=cStack>1</style> stack of <style=cDeath>[Big Shot]</style>.\n" +
            "On <style=cIsUtility>10</style> stacks, shoot a projectile on primary skill dealing <style=cIsDamage>777%</style> dmg <style=cStack>(+222% per stack)</style>.";
        public override string ItemLore => "As the days became more dull, and bussiness started to dry, a call came in." +
            "\nIt's your chance... a once in a lifetime chance... to become a <style=cDeath>[Big Shot]</style>.";
        public override ItemTier Tier => ItemTier.Tier3;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("big_shot.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("big_shot_icon.png");
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public Sprite BuffIcon = MainAssets.LoadAsset<Sprite>("big_shot_effect_icon.png");

        public BuffDef BigShotBuff;

        public uint TotalStages = 0;

        public uint GoldIncreasePerStage = 25;

        public static GameObject projectilePrefab;

        public static GameObject BigShotEffectPrefab;

        public static NetworkSoundEventDef BigShotSound;

        public override void Init()
        {
            CreateLang();
            CreateItem();
            CreateBuff();
            CreateEffect();
            CreateProjectile();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += BigShotEffect;
            On.RoR2.CharacterMaster.GiveMoney += BigShotMoneyEffect;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            if (!NetworkServer.active) return;

            if (obj.sceneDef.cachedName != "bazaar")
            {
                TotalStages += 1;
                Debug.Log("Total Stages: " + TotalStages);
            }
        }

        public void BigShotMoneyEffect(On.RoR2.CharacterMaster.orig_GiveMoney orig, CharacterMaster self, uint amount)
        {
            if (!NetworkServer.active || !self.GetBody()) return;

            var sender = self.GetBody();
            var itemCount = GetCount(sender);
            var existing = sender.GetComponent<BigShotBehavior>();

            if (sender.inventory && itemCount > 0)
            {
                Debug.Log($"Amount | " + amount);
                uint bonus = (uint)Mathf.CeilToInt(amount * 0.3f);
                amount += bonus;
                Debug.Log($"Adjusted Dealmaker Amount | " + amount);

                if (existing)
                {
                    existing.TotalGoldGained += amount;
                }
            }
            orig(self, amount);
        }

        public void BigShotEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender) return;

            var itemCount = GetCount(sender);
            var existing = sender.GetComponent<BigShotBehavior>();
            if (sender.inventory && itemCount > 0 && !existing)
            {
                existing = sender.gameObject.AddComponent<BigShotBehavior>();
                existing.body = sender;
                existing.stack = itemCount;
                existing.BigShotBuff = BigShotBuff;
            }
            else if (existing && itemCount <= 0) existing.enabled = false;
            else if (existing && itemCount > 0 && !existing.enabled) existing.enabled = true;
            if (existing)
            {
                existing.stack = itemCount;
                existing.GoldThreshold = 50 + TotalStages * GoldIncreasePerStage;
                //Debug.Log("New Bigshot Threshold: " + existing.GoldThreshold);
            }
            
        }

        public void CreateBuff()
        {
            BigShotBuff = ScriptableObject.CreateInstance<BuffDef>();
            BigShotBuff.name = "BigShotBuff";
            BigShotBuff.iconSprite = BuffIcon;
            BigShotBuff.canStack = true;
            BigShotBuff.isDebuff = false;

            ContentAddition.AddBuffDef(BigShotBuff);
        }

        public void CreateEffect()
        {
            BigShotEffectPrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile"), "BigShotEffect", true);

            BigShotSound = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            BigShotSound.eventName = "Play_BIGSHOT";
            BigShotSound.name = "bigshot_sfx";
            R2API.ContentAddition.AddNetworkSoundEventDef(BigShotSound);

            var effectComponent = BigShotEffectPrefab.GetComponent<EffectComponent>() ?? BigShotEffectPrefab.AddComponent<EffectComponent>();
            effectComponent.soundName = "Play_BIGSHOT";

            if (!BigShotEffectPrefab.GetComponent<NetworkIdentity>())
                BigShotEffectPrefab.AddComponent<NetworkIdentity>();

            if (BigShotEffectPrefab) { PrefabAPI.RegisterNetworkPrefab(BigShotEffectPrefab); }
            ContentAddition.AddEffect(BigShotEffectPrefab);
        }

        public void CreateProjectile()
        {
            projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ShurikenProjectile").InstantiateClone("BigShotProjectile", true);
            if (!projectilePrefab.GetComponent<NetworkIdentity>()) projectilePrefab.AddComponent<NetworkIdentity>();

            var ghost = MainAssets.LoadAsset<GameObject>("big_shot_projectile.prefab").InstantiateClone("big_shot", true);
            ghost.AddComponent<ProjectileGhostController>();
            ghost.AddComponent<NetworkIdentity>();
            ghost.transform.localScale = new Vector3(180f, 180f, 180f);
            ghost.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            var projCont = projectilePrefab.GetComponent<ProjectileController>();
            projCont.startSound = "";
            projCont.shouldPlaySounds = false;
            projCont.ghostPrefab = ghost;

            var projSimp = projectilePrefab.GetComponent<ProjectileSimple>();
            projSimp.desiredForwardSpeed *= 1f;
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
                    localPos = new Vector3(-0.0181F, 0.18318F, 0.11438F),
                    localAngles = new Vector3(359.9379F, 89.13351F, 344.7787F),
                    localScale = new Vector3(14F, 14F, 14F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.05111F, 0.20041F, 0.05749F),
                    localAngles = new Vector3(71.13284F, 0.85515F, 269.7938F),
                    localScale = new Vector3(10.65627F, 10.65627F, 10.65627F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.4929F, 2.87609F, -0.55534F),
                    localAngles = new Vector3(3.62907F, 268.6543F, 301.4389F),
                    localScale = new Vector3(71.05827F, 71.05827F, 71.05827F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.01001F, -0.04382F, 0.06658F),
                    localAngles = new Vector3(359.9916F, 87.82423F, 5.2221F),
                    localScale = new Vector3(12.66013F, 12.66013F, 12.66013F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00812F, 0.04364F, 0.07607F),
                    localAngles = new Vector3(1.42508F, 90.78919F, 359.603F),
                    localScale = new Vector3(6.82893F, 6.82893F, 6.82893F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00841F, 0.10428F, 0.1014F),
                    localAngles = new Vector3(358.1436F, 86.20536F, 0.08487F),
                    localScale = new Vector3(8.35088F, 8.35088F, 8.35088F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-0.01F, -1.06369F, 0.55248F),
                    localAngles = new Vector3(1.072F, 89.76335F, 9.88333F),
                    localScale = new Vector3(13.6413F, 13.6413F, 13.6413F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00822F, 0.06086F, 0.09935F),
                    localAngles = new Vector3(357.9385F, 87.69575F, 1.17614F),
                    localScale = new Vector3(8.36439F, 8.36439F, 8.36439F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.09198F, 2.71101F, 0.6164F),
                    localAngles = new Vector3(0.90842F, 268.449F, 232.6822F),
                    localScale = new Vector3(103.8343F, 103.8343F, 103.8343F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00992F, 0.02677F, 0.06682F),
                    localAngles = new Vector3(359.7724F, 91.02103F, 7.08265F),
                    localScale = new Vector3(10.78019F, 10.78019F, 10.78019F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.0085F, 0.09478F, 0.10477F),
                    localAngles = new Vector3(2.16612F, 88.15824F, 337.8445F),
                    localScale = new Vector3(8.60173F, 8.60173F, 8.60173F)

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
                    childName = "MuzzlePistol",
                    localPos = new Vector3(-0.02261F, 0.08256F, -0.26332F),
                    localAngles = new Vector3(313.8425F, 83.98895F, 185.9064F),
                    localScale = new Vector3(4.2464F, 3.18434F, 3.18434F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.05952F, 0.11334F, 0.0741F),
                    localAngles = new Vector3(15.16286F, 99.01782F, 299.4797F),
                    localScale = new Vector3(8.81553F, 7.34627F, 7.34627F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.28516F, 0.06555F, -0.01708F),
                    localAngles = new Vector3(1.14554F, 359.4018F, 259.1054F),
                    localScale = new Vector3(18.30414F, 15.68926F, 15.68926F)


                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01224F, 0.12337F, 0.0967F),
                    localAngles = new Vector3(3.5382F, 92.96323F, 345.6394F),
                    localScale = new Vector3(8.24892F, 7.33238F, 7.33238F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.03355F, 0.13485F, 0.09933F),
                    localAngles = new Vector3(7.05586F, 94.36584F, 356.7482F),
                    localScale = new Vector3(15.23684F, 15.23684F, 15.23684F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00062F, 0.00413F, 0.00324F),
                    localAngles = new Vector3(0.92012F, 90.02956F, 357.8276F),
                    localScale = new Vector3(0.57343F, 0.57343F, 0.57343F)

                }
            });
            return rules;
        }

        [RequireComponent(typeof(TeamFilter))]
        public class BigShotBehavior : CharacterBody.ItemBehavior
        {
            #region Variables
            public BuffDef BigShotBuff;
            public float reloadTimer;
            public SkillLocator skillLocator;
            public InputBankTest inputBank;

            public int BigShotThreshold = 10;
            public float DmgMult = 77.7f;
            public float StackDmgMult = 22.2f;
            public uint MaxBigShotStacks = 30;
            public float TotalDamageCalc;

            public uint TotalGoldGained = 0;
            public uint GoldThreshold = 50;

            private bool TimeForABigShot;

            public GameObject projectilePrefab;
            #endregion
            private void Awake()
            {
                enabled = false;
            }
            private void Start()
            {
                projectilePrefab = BigShot.projectilePrefab;
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
                        while (body.HasBuff(BigShotBuff) && num > 0)
                        {
                            num--;
                            body.RemoveBuff(BigShotBuff);
                        }
                    }
                }
                inputBank = null;
                skillLocator = null;
            }
            private void FixedUpdate()
            {
                if (!NetworkServer.active) return;

                #region Gold/Buff/Dmg Calcs
                TotalDamageCalc = body.damage * (DmgMult + StackDmgMult * (stack - 1));
                if (body.GetBuffCount(BigShotBuff) >= MaxBigShotStacks+1) body.RemoveBuff(BigShotBuff); // Remove buff on higher than alloted stack
                if (TotalGoldGained >= GoldThreshold * MaxBigShotStacks) TotalGoldGained = GoldThreshold * MaxBigShotStacks; // Set max internal gold if higher than possible stacks
                if (TotalGoldGained >= GoldThreshold)
                {
                    body.AddBuff(BigShotBuff);
                    TotalGoldGained -= GoldThreshold;
                } // Add buff and remove gold
                if (body.GetBuffCount(BigShotBuff) >= BigShotThreshold && !TimeForABigShot) TimeForABigShot = true; // If buff count is equal to threshold, allow big shot
                #endregion
            }
            private void OnSkillActivated(GenericSkill skill)
            {
                if (!NetworkServer.active) return;

                SkillLocator skillLocator = this.skillLocator;
                if ((skillLocator != null ? skillLocator.primary : null) == skill && TimeForABigShot)
                {
                    ShootBigShot();
                    //RoR2.Util.PlaySound("Play_BIGSHOT", gameObject);
                    EffectManager.SpawnEffect(BigShotEffectPrefab, new EffectData { origin = transform.position, scale = 1f }, true);
                    for (int i = 0; i < BigShotThreshold; i++)
                    {
                        body.RemoveBuff(BigShotBuff);
                    }
                    TimeForABigShot = false;
                }
            }
            private void ShootBigShot()
            {
                if (NetworkServer.active)
                {
                    Ray aimRay = GetAimRay();
                    ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                    {
                        projectilePrefab = projectilePrefab,
                        position = aimRay.origin,
                        rotation = RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = gameObject,
                        damage = TotalDamageCalc,
                        force = 4f,
                        crit = RoR2.Util.CheckRoll(body.crit, body.master),
                        damageColorIndex = DamageColorIndex.Default,
                    });
                }
            }
            /*
            private GameObject FindTarget(float searchRadius)
            {
                if (!body || !body.teamComponent) return null;

                TeamIndex teamIndex = body.teamComponent.teamIndex;

                BullseyeSearch search = new BullseyeSearch
                {
                    teamMaskFilter = TeamMask.allButNeutral,
                    filterByLoS = true,
                    maxDistanceFilter = searchRadius,
                    searchOrigin = body.corePosition,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    viewer = body
                };

                search.teamMaskFilter.RemoveTeam(teamIndex);
                search.RefreshCandidates();

                HurtBox targetHurtBox = search.GetResults().FirstOrDefault();
                GameObject targetGameObject = targetHurtBox ? targetHurtBox.gameObject : null;
                return targetGameObject;
            }
            */
            private Ray GetAimRay()
            {
                return new Ray(body.inputBank.aimOrigin, body.inputBank.aimDirection);
            }
        }
    }
}
