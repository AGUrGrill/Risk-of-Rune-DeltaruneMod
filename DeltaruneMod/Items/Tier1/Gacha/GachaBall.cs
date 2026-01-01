using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class GachaBall : ItemBase<GachaBall>
    {
        public override string ItemName => "Grand Prize";

        public override string ItemLangTokenName => "GACHA_BALL";

        public override string ItemPickupDesc => "Gain a temporary gacha pull on stage start.";

        public override string ItemFullDescription => "Gain a random item from the gacha item pool on stage start <style=cStack>(+1 item per stack)</style>.";

        public override string ItemLore => "Congradulations! You are our <style=cKeywordName>397th WINNER</style>!\n" +
            "For being our <style=cKeywordName>397th WINNER</style>, you will recieve a <style=cKeywordName>CONSOLATION PRIZE</style>.\n" +
            "\n\n\n*Consolation prize subject to an 80% tax.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("gacha_ball.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("gacha_ball_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public static List<ItemDef> gachaItems = new List<ItemDef>();

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
localPos = new Vector3(-0.09767F, 0.18165F, 0.23138F),
localAngles = new Vector3(25.59187F, 108.3924F, 357.5829F),
localScale = new Vector3(3.43066F, 3.43066F, 3.43066F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(-0.09767F, 0.18165F, 0.23138F),
localAngles = new Vector3(25.59187F, 108.3924F, 357.5829F),
localScale = new Vector3(3.43066F, 3.43066F, 3.43066F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MainWheelL",
localPos = new Vector3(-0.80123F, 0.01261F, -0.05036F),
localAngles = new Vector3(22.03093F, 193.1643F, 293.1384F),
localScale = new Vector3(41.25451F, 41.25451F, 41.25451F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(0.14757F, 0.47531F, -0.17837F),
localAngles = new Vector3(63.61968F, 206.9357F, 211.2521F),
localScale = new Vector3(2.87533F, 2.87533F, 2.87533F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootL",
localPos = new Vector3(0.00697F, 0.082F, -0.0516F),
localAngles = new Vector3(358.5168F, 247.8026F, 98.09467F),
localScale = new Vector3(2.16937F, 2.16937F, 2.16937F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfR",
localPos = new Vector3(-0.00903F, -0.02159F, -0.01258F),
localAngles = new Vector3(332.2478F, 140.918F, 214.9979F),
localScale = new Vector3(2.87252F, 2.87252F, 2.87252F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootBackL",
localPos = new Vector3(-0.04999F, 1.32125F, -0.01616F),
localAngles = new Vector3(13.91979F, 83.61308F, 188.4137F),
localScale = new Vector3(4.1695F, 4.1695F, 4.1695F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechLowerArmL",
localPos = new Vector3(0.11835F, 0.54309F, 0.00279F),
localAngles = new Vector3(31.08598F, 331.5507F, 199.0387F),
localScale = new Vector3(2.21822F, 3.05491F, 2.6062F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
localPos = new Vector3(-0.18318F, 3.87908F, 1.49318F),
localAngles = new Vector3(11.95531F, 265.0479F, 75.21944F),
localScale = new Vector3(47.10388F, 47.10388F, 47.10388F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Neck",
localPos = new Vector3(-0.23606F, -0.01474F, 0.04785F),
localAngles = new Vector3(20.43124F, 28.62147F, 52.04087F),
localScale = new Vector3(3.35465F, 3.35465F, 3.35465F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfL",
localPos = new Vector3(-0.07775F, 0.31852F, 0.01962F),
localAngles = new Vector3(4.41864F, 230.4836F, 309.8528F),
localScale = new Vector3(1.23245F, 1.23245F, 1.23245F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LargeExhaust2L",
localPos = new Vector3(-0.02756F, -0.00821F, 0.02134F),
localAngles = new Vector3(351.5643F, 26.98711F, 125.3889F),
localScale = new Vector3(4.40777F, 4.14464F, 3.6732F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Wheel",
localPos = new Vector3(-0.74618F, -0.00219F, 0.01216F),
localAngles = new Vector3(11.1291F, 331.6624F, 74.44159F),
localScale = new Vector3(5.0823F, 5.04516F, 4.35625F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
localPos = new Vector3(0.0395F, -0.00441F, -0.10403F),
localAngles = new Vector3(3.30137F, 74.72095F, 184.1599F),
localScale = new Vector3(2.50045F, 2.22258F, 2.22258F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(-0.06796F, 0.33392F, -0.4559F),
localAngles = new Vector3(355.5451F, 115.7917F, 341.5839F),
localScale = new Vector3(2.83273F, 2.83273F, 2.83273F)
                }
            });
            return rules;
        }


        public override void Hooks()
        {
            //On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
        }
        /*
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);

            #region Add Timer
            var timer = self.GetComponent<GachaBallTimer>();
            if (GetCount(self) > 0 && !timer)
            {
                timer = self.gameObject.AddComponent<GachaBallTimer>();
                timer.player = self;
                timer.stackCount = GetCount(self);
                timer.enabled = true;
            }
            else if (GetCount(self) <= 0 && timer)
            {
                timer.enabled = false;
            }
            else if (timer && timer.stackCount != GetCount(self))
            {
                timer.stackCount = GetCount(self);
            }
            #endregion
        }
        */


        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);

            if (gachaItems.Count <= 0) GetGachaItems();

            // Global hook to always delete all gacha items on stage start
            try
            {
                foreach (var item in gachaItems)
                {
                    var itemCount = self.inventory.GetItemCountEffective(item);
                    for (int i = 0; i < itemCount; i++)
                    {
                        self.inventory.RemoveItemPermanent(item);
                        Debug.Log("Removed " + item.name + " from " + self.name);
                    }
                }
            }
            catch
            {
                Debug.Log("Error removing gacha items.");
            }

            // On stage start give random items
            if (GetCount(self) > 0)
            {
                for (int i = 0; i < GetCount(self); i++)
                {
                    var ranGachaItem = gachaItems[UnityEngine.Random.Range(0, gachaItems.Count)];
                    self.inventory.GiveItemPermanent(ranGachaItem);
                    Debug.Log("Gave " + self.name + " " + ranGachaItem.name);
                }
            }
        }

        private void GetGachaItems()
        {
            gachaItems.Add(TVDinner.instance.ItemDef);
            gachaItems.Add(ExecBuffet.instance.ItemDef);
            gachaItems.Add(GoldenIdol.instance.ItemDef);
            gachaItems.Add(BlueRibbon.instance.ItemDef);
            gachaItems.Add(GingerGuard.instance.ItemDef);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
        private class GachaBallTimer : MonoBehaviour
        {
            // Temp items last 80sec, 90sec with substandard dup

            readonly float timerInterval = 90f;
            float timer = 0f;

            public int stackCount = 0;
            public CharacterBody player;

            private void Awake()
            {
                base.enabled = false;
            }
            private void OnEnable()
            {
                if (!player)
                {
                    Debug.Log("Player not found! Destroying...");
                    Destroy(this);
                }

                timer = timerInterval;
            }
            // Timer
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    GAMBLE();
                    timer = timerInterval;
                }
            }
            // Add buff to increase speed
            private void GAMBLE()
            {
                for (int i = 0; i < stackCount; i++)
                {
                    var ranGachaItem = gachaItems[UnityEngine.Random.Range(0, gachaItems.Count)];
                    player.inventory.GiveItemTemp(ranGachaItem.itemIndex);
                    Debug.Log("Gave " + player.name + " " + ranGachaItem.name);
                }
            }
        }

    }
}
