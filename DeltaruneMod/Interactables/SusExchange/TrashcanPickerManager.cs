using DeltaruneMod.Items.Spamton;
using DeltaruneMod.Items.Yellow;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.BlastAttack;
using static RoR2.Networking.HostDescription;

namespace DeltaruneMod.Interactables.SusExchange
{
    public class TrashcanPickerManager : NetworkBehaviour
    {
        public PickupPickerController pickerController;

        public Transform iconTransform;

        [SyncVar]
        public float coefficient;

        private Interactor interactor;

        public readonly static int maxUses = 10;

        public int uses;

        public List<ItemDef> allItems = new List<ItemDef>();
        public List<ItemDef> allTier1 = new List<ItemDef>();
        public List<ItemDef> allTier2 = new List<ItemDef>();
        public List<ItemDef> allTier3 = new List<ItemDef>();
        public List<ItemDef> allTakeableItems = new List<ItemDef>();
        public ItemDef kromer, pearl, shinyPearl, pipis, mrPipis, commRing;

        public void Start()
        {
            uses = maxUses;
            allItems = Util.Helpers.GetItems(99);
            allTier1 = Util.Helpers.GetItems(0);
            allTier2 = Util.Helpers.GetItems(1);
            allTier3 = Util.Helpers.GetItems(2);
            allTakeableItems.AddRange(allTier1);
            allTakeableItems.AddRange(allTier2);
            for (ItemIndex i = 0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(i);
                if (itemDef != null)
                {
                    if (itemDef.name == "Pearl") { allTakeableItems.Add(itemDef); pearl = itemDef; }
                    else if (itemDef.name == "ShinyPearl") { allTakeableItems.Add(itemDef); shinyPearl = itemDef; }
                    else if (itemDef.name == "ITEM_KROMER") kromer = itemDef;
                    else if (itemDef.name == "ITEM_PIPIS") pipis = itemDef;
                    else if (itemDef.name == "ITEM_MR_PIPIS") mrPipis = itemDef;
                    else if (itemDef.name == "ITEM_COMM_RING") commRing = itemDef;
                }
            }
        }

        public void HandleSelection(int selection)
        {
            if (!NetworkServer.active) return;

            GetComponent<NetworkUIPromptController>().ClearParticipant();

            if (interactor)
            {
                #region Making vars
                PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(selection));
                CharacterBody body = interactor.GetComponent<CharacterBody>();
                var choosenItem = pickupDef.itemIndex;
                var isItem = pickupDef.itemTier == ItemTier.Tier1 || pickupDef.itemTier == ItemTier.Tier2;
                ItemTier tier = ItemCatalog.GetItemDef(choosenItem).tier;
                List<ItemDef> allInventoryItems = Util.Helpers.GetAllItemsFromInventory(body.inventory);
                List<ItemDef> allTakeableInvItems = new List<ItemDef>();
                ItemDef randomTier2 = allTier2[UnityEngine.Random.Range(0, allTier2.Count)];
                ItemDef randomTier3 = allTier3[UnityEngine.Random.Range(0, allTier3.Count)];
                ItemDef itemGiven = null;
                #endregion

                Debug.Log("Finding takeable items");
                #region Get all takeable items from inventory
                // Collects all takeable items into special list
                for (int i = 0; i < allTakeableItems.Count; i++)
                {
                    if (allInventoryItems.Contains(allTakeableItems[i]))
                        allTakeableInvItems.Add(allTakeableItems[i]);
                }
                /*
                if (allTakeableInvItems.Count <= 0)
                {
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: NO [[Usable]] ITEMS." });
                    return;
                }
                */
                #endregion

                Debug.Log("Deciding item to give");
                #region Choose given item based on options
                if (choosenItem == pearl.itemIndex)
                {
                    itemGiven = pipis;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOUR FIRST STEP TO BECOMING A [[Big shot]]. [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == shinyPearl.itemIndex)
                {
                    itemGiven = mrPipis;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU WON WON WON MY [[Hyperlink blocked]]. [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == kromer.itemIndex)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        body.inventory.RemoveItem(kromer);
                    }
                    itemGiven = commRing;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU ARE THE FIRST TO OWN MY <style=cIsUtility>[Commemorative Ring]</style>!!! [" + (uses - 1) + "] tries left." });
                }
                else
                {
                    int roll_chance = 40;
                    //if (commRingItemCount > 0) roll_chance = 60;

                    bool giveItem = RoR2.Util.CheckRoll(roll_chance, body.master);
                    if (giveItem)
                    {
                        if (tier == ItemTier.Tier1) itemGiven = randomTier2;
                        else if (tier == ItemTier.Tier2) itemGiven = randomTier3;
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: THAT'S A REAL <style=cDeath>[[Big Shot]]</style> MOVE KID!!! YOU'RE JUST LIKE [Me]... [" + (uses - 1) + "] tries left." });
                    }
                    else
                    {
                        itemGiven = kromer;
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: DELICIOUS KROMER. [" + (uses - 1) + "] tries left." });
                    }
                }
                #endregion

                Debug.Log("Taken " + choosenItem + " | Given " + itemGiven);
                string pickupColorHex, pickupName, pickupAmountString;
                if (isItem && itemGiven)
                {
                    body.inventory.RemoveItem(choosenItem);
                    body.inventory.GiveItem(itemGiven.itemIndex);
                    CharacterMasterNotificationQueue.SendTransformNotification(body.master, choosenItem, itemGiven.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);

                    pickupColorHex = ColorCatalog.GetColorHexString(ItemTierCatalog.GetItemTierDef(tier).colorIndex);
                    pickupName = Language.GetString(ItemCatalog.GetItemDef(choosenItem).nameToken);
                }
                else
                {
                    return;
                }

                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData()
                {
                    origin = gameObject.transform.position,
                    rotation = Quaternion.identity,
                    scale = 1f,
                    color = (Color32)Color.yellow
                }, true);

                /*
                Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage
                {
                    subjectAsCharacterBody = body,
                    baseToken = $"INTERACTABLE_{Trashcan.gloablLangToken}_INTERACT_PICKER",
                    paramTokens = new string[] { "<color=#" + pickupColorHex + ">" + pickupName + "</color>"}
                });
                */

                uses--;
                if (uses <= 0)
                {
                    RpcHandleDeactivateClient();
                    pickerController.SetAvailable(false);
                }
            }

        }

        public void HandleInteraction(Interactor interactor)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            this.interactor = interactor;

            List<PickupPickerController.Option> options = new List<PickupPickerController.Option>();

            var charBody = interactor.GetComponent<CharacterBody>();

            if (charBody && charBody.master)
            {
                foreach (var item in allTakeableItems)
                {
                    var itemCount = charBody.inventory.GetItemCount(item);
                    if (itemCount > 0)
                    {
                        options.Add(new PickupPickerController.Option
                        {
                            available = false,
                            pickupIndex = PickupCatalog.FindPickupIndex(item.itemIndex)
                        });
                        Debug.Log("Added " + item);
                    }
                }
                pickerController.SetOptionsServer(options.ToArray());
            }
        }

        [ClientRpc]
        public void RpcHandleDeactivateClient()
        {
            if (iconTransform) iconTransform.gameObject.SetActive(false);
            // lets brute force it because fuck it, what could possibly go wrong
            // we cant use SetAvailable() because it's not allowed on clients
            // and I guess PickupPickerController doesn't sync it for some reason
            // unlike PurchaseInteraction
            if (pickerController) pickerController.available = false;
        }
    }
}
