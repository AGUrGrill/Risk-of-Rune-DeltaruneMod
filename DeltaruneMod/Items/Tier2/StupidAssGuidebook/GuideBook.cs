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
        public override string ItemName => "Guiding Manual";

        public override string ItemLangTokenName => "GUIDE_BOOK";

        public override string ItemPickupDesc => "All allied drones learn how to heal.";

        public override string ItemFullDescription => "All allied drones learn to heal," +
			"\nhealing for <style=cIsHealing>2.5%</style> hp every <style=cIsUtility>10</style> seconds <style=cStack>(+1% hp per stack)</style>.";

        public override string ItemLore => "You tried to read the manual, but it was so dense it made your head spin..." +
            "\nPerhaps just the chapter on healing will be good for now...";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("guide_book.prefab");
		public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("guide_book_icon.png");

		public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
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
