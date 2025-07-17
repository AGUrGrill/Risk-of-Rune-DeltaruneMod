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

        public override string ItemPickupDesc => "Stack swoon on hit, total damage dealt prior reapplies after 3 stacks.";

        public override string ItemFullDescription => "<style=cIsDamage>50%</style> chance to apply Swoon. Upon reaching " +
            "<style=cIsUtility>3</style> stacks, reapply <style=cIsDamage>200%</style> of all damage dealt back to enemy <style=cStack>(+100% per stack)</style>.";

        public override string ItemLore => "You pick up the strange, cold blade..." +
            "\nShivers crawl down your spine as you lift it into the air." +
            "\nA strange feeling emerges from it, you can feel it speaking... crying... hurting..." +
            "\nYou respond to its call, its <style=cMono><style=cDeath>anger</style></style>, you move forward, gripping the blade tightly, changed ever so slightly.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("roaring_blade.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("roaring_blade_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public static BuffDef SwoonBuff;

        public static Sprite SwoonEffectIcon = MainAssets.LoadAsset<Sprite>("swoon_effect_icon.png");

        public static GameObject SwoonModelPrefab;
        public static GameObject SwoonEffectPrefabL;
        public static GameObject SwoonEffectPrefabR;
        public static NetworkSoundEventDef SwoonSFX;

        private int MaxSwoonStacks = 3;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.03424F, 0.24531F, -0.28029F),
                    localAngles = new Vector3(29.02806F, 9.87036F, 0.15779F),
                    localScale = new Vector3(43.11241F, 48.88401F, 48.88401F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.15683F, 0.03304F, -0.05687F),
                    localAngles = new Vector3(8.67311F, 301.7072F, 343.5933F),
                    localScale = new Vector3(32.31651F, 20.56886F, 31.46058F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.35631F, 0.30782F, -1.88334F),
                    localAngles = new Vector3(4.96775F, 356.1217F, 343.4172F),
                    localScale = new Vector3(342.7814F, 342.7814F, 342.7814F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.61056F, 0.2345F, 0.19811F),
                    localAngles = new Vector3(351.2091F, 161.5561F, 217.4899F),
                    localScale = new Vector3(50.83732F, 50.83732F, 50.83732F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ClavicleR",
                    localPos = new Vector3(0.19937F, -0.11671F, -0.00412F),
                    localAngles = new Vector3(351.81F, 103.9537F, 14.96851F),
                    localScale = new Vector3(26.79127F, 25.62798F, 28.75669F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.64632F, 0.31625F, 0.05835F),
                    localAngles = new Vector3(350.5506F, 180.2126F, 212.7281F),
                    localScale = new Vector3(63.08693F, 63.08693F, 63.08693F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-1.09289F, -0.17844F, 0.06287F),
                    localAngles = new Vector3(6.32783F, 163.8389F, 43.61153F),
                    localScale = new Vector3(64.85601F, 64.85601F, 64.85601F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(0.05452F, 0.96759F, 0.03388F),
                    localAngles = new Vector3(342.5309F, 38.24583F, 185.2787F),
                    localScale = new Vector3(43.79625F, 43.79625F, 43.79625F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.40245F, -0.11621F, -0.95978F),
                    localAngles = new Vector3(349.0351F, 87.08124F, 49.89027F),
                    localScale = new Vector3(545.4311F, 545.4311F, 545.4311F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.13571F, 0.20722F, -0.26261F),
                    localAngles = new Vector3(6.9968F, 350.0926F, 341.5277F),
                    localScale = new Vector3(44.86234F, 37.9991F, 37.9991F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.03685F, 0.04701F, -0.19537F),
                    localAngles = new Vector3(351.7767F, 8.65373F, 346.0138F),
                    localScale = new Vector3(46.44885F, 26.87783F, 2.25375F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ToeR",
                    localPos = new Vector3(-0.05914F, 0.23115F, -0.02773F),
                    localAngles = new Vector3(359.2351F, 160.0996F, 254.6561F),
                    localScale = new Vector3(15.71294F, 15.71294F, 15.71294F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.00696F, -0.22984F, -0.10106F),
                    localAngles = new Vector3(19.71435F, 168.1724F, 315.2494F),
                    localScale = new Vector3(35.84454F, 41.6678F, 29.87045F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.21611F, -0.41288F, 0.0538F),
                    localAngles = new Vector3(296.2543F, 222.2426F, 355.6819F),
                    localScale = new Vector3(18.60245F, 27.97708F, 27.97708F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(0.31906F, -0.45957F, -0.1822F),
                    localAngles = new Vector3(336.2322F, 23.51318F, 346.6137F),
                    localScale = new Vector3(47.03091F, 41.80524F, 41.80524F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01625F, 0.17917F, 0.0463F),
                    localAngles = new Vector3(354.49F, 266.0255F, 41.55928F),
                    localScale = new Vector3(55.72091F, 55.72091F, 55.72091F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.00839F, 0.00898F, 0.00293F),
                    localAngles = new Vector3(334.2737F, 179.8516F, 182.9772F),
                    localScale = new Vector3(0.91639F, 0.91639F, 0.91639F)
                }
            });
            return rules;
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            CreateSFX();
            CreateSwoonEffect();
            Hooks();

        }

        public void CreateBuff()
        {
            SwoonBuff = ScriptableObject.CreateInstance<BuffDef>();
            SwoonBuff.name = "SwoonDebuff";
            SwoonBuff.iconSprite = SwoonEffectIcon;
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
            orig(self, damageInfo, victim);

            if (!NetworkServer.active) return;

            // Try catch for occasional NRE
            try 
            {
                var attacker = damageInfo.attacker;
                var sender = attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                var existing = victimBody.GetComponent<SwoonDamageTracker>();

                if (!sender.isPlayerControlled || victimBody.isPlayerControlled) return;

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

                #region Buff Application & Effect
                // Add Buff
                if (existing && sender.inventory && itemCount > 0
                    && existing.canSwoon && victimBody.GetBuffCount(SwoonBuff) <= MaxSwoonStacks)
                {
                    if (RoR2.Util.CheckRoll(50, sender.master))
                    {
                        victimBody.AddBuff(SwoonBuff);
                    }
                }

                // Buff Stack 1: set prev health on first time
                if (existing && victimBody.GetBuffCount(SwoonBuff) <= 1)
                {
                    existing.prevHealth = victimBody.healthComponent.health;
                }

                // Buff Stack 3: set curr health, do swoon
                if (existing && victimBody.GetBuffCount(SwoonBuff) >= MaxSwoonStacks)
                {
                    existing.currHealth = victimBody.healthComponent.health;
                    existing.DoSwoonDamage();
                    for (int i = 0; i <= MaxSwoonStacks; i++)
                    {
                        victimBody.RemoveBuff(SwoonBuff);
                    }
                }
                #endregion
            }
            catch { return; } 
        }

        private static void CreateSwoonEffect()
        {
            SwoonEffectPrefabL = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("swoon_left.prefab"), "swoon_left", true);
            SwoonEffectPrefabR = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("swoon_right.prefab"), "swoon_right", true);
            
            SwoonModelPrefab = PrefabAPI.InstantiateClone(MainAssets.LoadAsset<GameObject>("swoon.prefab"), "swoon_main", true);
            SwoonModelPrefab.transform.localScale = new Vector3(120f, 120f, 120f);

            var effectController = SwoonModelPrefab.GetComponent<SwoonEffectController>();
            if (!effectController) effectController = SwoonModelPrefab.AddComponent<SwoonEffectController>();

            SwoonEffectPrefabL.AddComponent<NetworkIdentity>();
            SwoonEffectPrefabR.AddComponent<NetworkIdentity>();

            Util.Helpers.CreateNetworkedEffectPrefab(SwoonModelPrefab);
        }

        public void CreateSFX()
        {
            SwoonSFX = Util.Helpers.CreateNetworkSoundEventDef("Play_snd_knight_cut");
        }

        public class SwoonDamageTracker : CharacterBody.ItemBehavior
        {
            public float currHealth;
            public float prevHealth;
            public float totalDamageTaken;
            public bool canSwoon;
            private float swoonTimer = 0f;
            private float swoonTimerInterval = 1f;

            private void Start()
            {
                canSwoon = true;
            }
            private void FixedUpdate()
            {
                if (!canSwoon)
                {
                    swoonTimer -= Time.fixedDeltaTime;
                    if (swoonTimer <= 0f)
                    {
                        canSwoon = true;
                        swoonTimer = swoonTimerInterval;
                    }
                }  
            }
            public void DoSwoonDamage()
            {
                if (!NetworkServer.active) return;

                totalDamageTaken = (prevHealth - currHealth) * (stack + 1);
                body.healthComponent.health -= totalDamageTaken;

                // Hopefully fix potential of multiple peoples swoon causing the roaring (the health bar to go crazy)
                if (body.healthComponent.health - totalDamageTaken > body.maxHealth)
                {
                    ResetSwoonStats();
                    return;
                }

                EffectManager.SpawnEffect(SwoonModelPrefab, new EffectData { origin = body.transform.position, scale = 1f }, true);
                EffectManager.SimpleSoundEffect(SwoonSFX.index, body.corePosition, true);

                Debug.Log("Swooned " + body.name + " for " + totalDamageTaken + "!");
                Debug.Log("Prev HP " + prevHealth + " | Old Curr HP " + currHealth);

                canSwoon = false;
            }
            public void ResetSwoonStats()
            {
                prevHealth = 0;
                currHealth = 0;
            }
        }

        public class SwoonEffectController : MonoBehaviour
        {

            void Start()
            {
                Animator anim = GetComponent<Animator>();
                if (anim) anim.Play("swoon_animation", 0, 0f);
                anim.speed = 18f;

                Transform leftAttach = transform.Find("Roaring_Blade1");
                Transform rightAttach = transform.Find("Roaring_Blade");

                GameObject left = Instantiate(SwoonEffectPrefabL, leftAttach);
                left.transform.localPosition = Vector3.zero;
                left.transform.localScale = new Vector3(10f, 10f, 10f);

                GameObject right = Instantiate(SwoonEffectPrefabR, rightAttach);
                right.transform.localPosition = Vector3.zero;
                right.transform.localScale = new Vector3(10f, 10f, 10f);

                Destroy(gameObject, 0.35f);
            }
        }
    }
}
