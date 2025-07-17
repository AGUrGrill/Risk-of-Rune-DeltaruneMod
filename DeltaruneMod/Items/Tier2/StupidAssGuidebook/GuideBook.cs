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
    internal class GuideBook : ItemBase<GuideBook>
    {
        public override string ItemName => "Sage's Manual";

        public override string ItemLangTokenName => "GUIDE_BOOK";

        public override string ItemPickupDesc => "Teaches all drones to heal.";

        public override string ItemFullDescription => "All drones can now additionally cast Heal," +
			"\nhealing for <style=cIsHealing>2.5%</style> hp every <style=cIsUtility>10</style> seconds <style=cStack>(+1% hp per stack)</style>.";

        public override string ItemLore => "You tried to read the handcrafted manual, but it was so dense it made your head spin..." +
            "\nPerhaps just the chapter on healing will be good for now...";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("guie_book.prefab");

		public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("guide_book_icon.png");

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

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            var itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                var existing = sender.GetComponent<GuideBookBehavior>();
                if (!existing && sender.inventory && itemCount > 0)
                {
                    existing = sender.gameObject.AddComponent<GuideBookBehavior>();
                    existing.body = sender;
                    existing.stack = itemCount;
                    //Debug.Log("Gave " + sender + " " + ItemName + ".");
                }
                else if (existing && itemCount <= 0) existing.enabled = false;
                else if (existing && itemCount > 0 && !existing.enabled) existing.enabled = true;
                if (existing) existing.stack = itemCount;
            }
        }

        //Maybe for the ralsei book if you’re playing as ralsei you heal allies instead of them healing you
        public override void Init()
        {
			CreateItem();
			CreateLang();
			Hooks();
        }

    }

	public class GuideBookBehavior : CharacterBody.ItemBehavior
	{
		private int previousStack;

		private void OnEnable()
		{
			ulong num = Run.instance.seed ^ (ulong)((long)Run.instance.stageClearCount);
			UpdateAllMinions(stack);
			MasterSummon.onServerMasterSummonGlobal += OnServerMasterSummonGlobal;
		}

		private void OnDisable()
		{
			MasterSummon.onServerMasterSummonGlobal -= OnServerMasterSummonGlobal;
			UpdateAllMinions(0);
		}

		private void FixedUpdate()
		{
			if (previousStack != stack)
			{
				UpdateAllMinions(stack);
			}
		}

		private void OnServerMasterSummonGlobal(MasterSummon.MasterSummonReport summonReport)
		{
			if (body && body.master && body.master == summonReport.leaderMasterInstance)
			{
				CharacterMaster summonMasterInstance = summonReport.summonMasterInstance;
				if (summonMasterInstance)
				{
					CharacterBody body = summonMasterInstance.GetBody();
					if (body)
					{
						UpdateMinionInventory(summonMasterInstance.inventory, body.bodyFlags, stack);
					}
				}
			}
		}

		private void UpdateAllMinions(int newStack)
		{
			if (!this.body) return;
			CharacterBody body = this.body;
			if ((body != null) ? body.master : null)
			{
				MinionOwnership.MinionGroup minionGroup = MinionOwnership.MinionGroup.FindGroup(body.master.netId);
				if (minionGroup != null)
				{
					foreach (MinionOwnership minionOwnership in minionGroup.members)
					{
						if (minionOwnership)
						{
							CharacterMaster component = minionOwnership.GetComponent<CharacterMaster>();
							if (component && component.inventory)
							{
								CharacterBody body2 = component.GetBody();
								if (body2)
								{
									UpdateMinionInventory(component.inventory, body2.bodyFlags, newStack);
								}
							}
						}
					}
					previousStack = newStack;
				}
			}
		}

		private void UpdateMinionInventory(Inventory inventory, CharacterBody.BodyFlags bodyFlags, int newStack)
		{
			if ((inventory && newStack > 0 && (bodyFlags & CharacterBody.BodyFlags.Mechanical) > CharacterBody.BodyFlags.None) || (bodyFlags & CharacterBody.BodyFlags.Devotion) > CharacterBody.BodyFlags.None)
			{
				int itemCount = inventory.GetItemCount(DroneHealingBoost.instance.ItemDef);
				if (itemCount < stack)
				{
					inventory.GiveItem(DroneHealingBoost.instance.ItemDef, stack - itemCount);
				}
				else if (itemCount > stack)
				{
					inventory.RemoveItem(DroneHealingBoost.instance.ItemDef, itemCount - stack);
				}
			}
			else
			{
				inventory.ResetItem(DroneHealingBoost.instance.ItemDef);
			}
		}
	}
}
