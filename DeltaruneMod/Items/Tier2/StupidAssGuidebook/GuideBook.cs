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

        public override string ItemPickupDesc => "Teaches all drones a healing prayer.";

        public override string ItemFullDescription => "All drones can now additionally cast a heal prayer," +
			"\nhealing for <style=cIsHealing>2.5%</style> hp every <style=cIsUtility>10</style> seconds <style=cStack>(+1% hp per stack)</style>.";

        public override string ItemLore => "You tried to read the handcrafted manual, but it was so dense it made your head spin..." +
            "\nPerhaps just the chapter on healing will be good for now...";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("guide_book.prefab");

		public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("guide_book_icon.png");

        public override bool isChapter1 => true;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.06842F, -0.06525F, 0.00302F),
                    localAngles = new Vector3(344.6374F, 10.5189F, 172.5793F),
                    localScale = new Vector3(11.24511F, 12.75054F, 12.75054F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "BowHinge2L",
                    localPos = new Vector3(-0.01511F, -0.12038F, -0.04515F),
                    localAngles = new Vector3(40.99971F, 359.4588F, 0.25029F),
                    localScale = new Vector3(20.1074F, 12.97089F, 15.01608F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MuzzlePistol",
                    localPos = new Vector3(0.00462F, 0.07294F, -0.24116F),
                    localAngles = new Vector3(23.59849F, 87.6305F, 336.2294F),
                    localScale = new Vector3(2.71367F, 4.69018F, 2.90618F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.07447F, 0.64331F, -1.79574F),
                    localAngles = new Vector3(75.89338F, 94.53836F, 185.0458F),
                    localScale = new Vector3(145.8154F, 161.3661F, 145.8154F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MuzzleLeft",
                    localPos = new Vector3(0.21551F, 0.07161F, -0.24095F),
                    localAngles = new Vector3(344.467F, 179.7876F, 178.1696F),
                    localScale = new Vector3(10.38355F, 10.38355F, 10.38355F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmL",
                    localPos = new Vector3(0.00965F, 0.19812F, -0.0943F),
                    localAngles = new Vector3(341.6551F, 268.496F, 179.2602F),
                    localScale = new Vector3(8.24085F, 7.88303F, 8.8454F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.08936F, 0.00467F, 0.06222F),
                    localAngles = new Vector3(345.8772F, 20.92191F, 177.8589F),
                    localScale = new Vector3(10.62848F, 10.62848F, 10.62848F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(0.41321F, -0.38063F, 0.19475F),
                    localAngles = new Vector3(13.43373F, 343.3212F, 21.37775F),
                    localScale = new Vector3(14.47604F, 15.15385F, 15.43011F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechBase",
                    localPos = new Vector3(0.20024F, 0.40954F, 0.35092F),
                    localAngles = new Vector3(14.93578F, 90.2113F, 14.04156F),
                    localScale = new Vector3(5.11029F, 4.13625F, 4.33906F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MouthMuzzle",
                    localPos = new Vector3(-1.32711F, 2.22417F, 2.74239F),
                    localAngles = new Vector3(359.0917F, 42.2724F, 63.16629F),
                    localScale = new Vector3(75.89112F, 75.89112F, 75.89112F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.00749F, 0.16526F, 0.03505F),
                    localAngles = new Vector3(71.62508F, 193.732F, 183.1327F),
                    localScale = new Vector3(14.82975F, 11.77351F, 12.56107F)
                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootL",
                    localPos = new Vector3(0.01065F, 0.03102F, 0.10055F),
                    localAngles = new Vector3(350.7331F, 80.90894F, 225.1298F),
                    localScale = new Vector3(3.5252F, 3.48996F, 3.53609F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.13241F, 0.03476F, -0.08426F),
                    localAngles = new Vector3(48.72685F, 157.9988F, 295.8664F),
                    localScale = new Vector3(14.89942F, 17.31997F, 12.4162F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(-0.07427F, -0.05237F, -0.39707F),
                    localAngles = new Vector3(332.633F, 92.34814F, 354.5216F),
                    localScale = new Vector3(7.42348F, 5.85202F, 6.0095F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ClavL",
                    localPos = new Vector3(-0.10036F, 0.79036F, -0.31671F),
                    localAngles = new Vector3(63.96176F, 129.177F, 16.81915F),
                    localScale = new Vector3(10.35707F, 10.35707F, 10.35707F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.30605F, -0.02151F, -0.1316F),
                    localAngles = new Vector3(293.5251F, 187.9981F, 294.4022F),
                    localScale = new Vector3(4.5489F, 5.00249F, 5.00249F)
                }
            });
            /*
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
            */
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

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("guide_book.prefab").InstantiateClone("SageManualPickup", true);
            pickupModel.transform.localScale = new Vector3(2f, 2f, 2f);

            ItemDef.pickupModelPrefab = pickupModel;
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
                if ((bodyFlags & CharacterBody.BodyFlags.Void) > CharacterBody.BodyFlags.None) return;

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
