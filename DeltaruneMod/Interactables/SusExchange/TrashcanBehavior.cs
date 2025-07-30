using BepInEx.Configuration;
using DeltaruneMod.Items;
using DeltaruneMod.Items.Tier2;
using R2API;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static DeltaruneMod.Util.Components;
using static DeltaruneMod.Util.Helpers;

namespace DeltaruneMod.Interactables.SusExchange
{
    public class TrashcanBehavior : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject shrineUseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ShrineUseEffect.prefab").WaitForCompletion();
        public List<ItemDef> allItems = new List<ItemDef>();
        public List<ItemDef> allTier1 = new List<ItemDef>();
        public List<ItemDef> allTier2 = new List<ItemDef>();
        public List<ItemDef> allTier3 = new List<ItemDef>();
        public List<ItemDef> allTakeableItems = new List<ItemDef>();
        public ItemDef kromer, pearl, shinyPearl, pipis, mrPipis, commRing;

        private int timesUsed = 10;

        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                purchaseInteraction.SetAvailable(true);
                GetComponent<Util.Components.TextController>()?.SetText(timesUsed + " USED");
            }
            
            //Util.Helpers.GetAllComponentNames(gameObject);

            AkSoundEngine.PostEvent(3865094552, gameObject);

            allItems = Util.Helpers.GetItems(99);
            allTier1 = Util.Helpers.GetItems(0);
            allTier2 = Util.Helpers.GetItems(1);
            allTier3 = Util.Helpers.GetItems(2);
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
            purchaseInteraction.onPurchase.AddListener(OnPurchase);
        }
        public void FixedUpdate()
        {
            if (!NetworkServer.active) return;

            GetComponent<Util.Components.TextController>()?.SetText(timesUsed + " USED");
        }

        [Server]
        public void OnPurchase(Interactor interactor)
        {
            if (!NetworkServer.active) return;

            if (timesUsed <= 0)
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: SHOP IS [Closed for the season] TRY AGAIN [Next time]." });
                //textController.SetText("CLOSED");
                return;
            }
            
            EffectManager.SpawnEffect(shrineUseEffect, new EffectData()
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.blue
            }, true);

            ApplySpamtonShop(interactor);
            timesUsed --;
        }

        public void ApplySpamtonShop(Interactor interactor)
        {
            Debug.Log("Starting shop purchase");
            CharacterBody body = interactor.GetComponent<CharacterBody>();
            Transform dropletOrigin = body.transform;
            List<ItemDef> allInventoryItems = Util.Helpers.GetAllItemsFromInventory(body.inventory);
            List<ItemDef> allTakeableInvItems = new List<ItemDef>();
            ItemDef randomTier2 = allTier2[Random.Range(0, allTier2.Count)];
            ItemDef randomTier3 = allTier3[Random.Range(0, allTier3.Count)];
            ItemDef itemTaken = null; 
            ItemDef itemGiven = null;

            if (!body.inventory) return;

            // int commRingItemCount = body.inventory.GetItemCount(commRing);

            Debug.Log("Finding takeable items");
            #region Get all takeable items from inventory
            // Collects all takeable items into special list
            allTakeableItems.AddRange(allTier1);
            allTakeableItems.AddRange(allTier2);
            for (int i = 0; i < allTakeableItems.Count; i++)
            {
                if (allInventoryItems.Contains(allTakeableItems[i]))
                    allTakeableInvItems.Add(allTakeableItems[i]);
            }
            if (allTakeableInvItems.Count <= 0)
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: NO [[Usable]] ITEMS." });
                timesUsed++;
                return;
            }
            
            #endregion

            Debug.Log("Picking item");
            #region Pick item to take
            ItemDef itemFromInventory;
            try 
            {
                itemFromInventory = allTakeableInvItems[Random.Range(0, allTakeableInvItems.Count)];
                if (allTakeableInvItems.Contains(shinyPearl)) itemFromInventory = allTakeableInvItems[allTakeableInvItems.IndexOf(shinyPearl)];
                else if (allTakeableInvItems.Contains(pearl)) itemFromInventory = allTakeableInvItems[allTakeableInvItems.IndexOf(pearl)];
                else if (allTakeableInvItems.Contains(kromer) && body.inventory.GetItemCount(kromer) >= 10) itemFromInventory = allTakeableInvItems[allTakeableInvItems.IndexOf(kromer)];
                itemTaken = itemFromInventory;
            }
            catch { return; }
            #endregion

            Debug.Log("Starting  transaction");
            #region Start transaction
            if (itemFromInventory == pearl)
            {
                body.inventory.RemoveItem(itemFromInventory);
                itemGiven = pipis;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOUR FIRST STEP TO BECOMING A [[Big shot]]." });
            }
            else if (itemFromInventory == shinyPearl)
            {
                body.inventory.RemoveItem(itemFromInventory);
                itemGiven = mrPipis;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU WON WON WON MY [[Hyperlink blocked]]." });
            }
            else if (itemFromInventory == kromer)
            {
                for (int i = 0; i < 10; i++)
                {
                    body.inventory.RemoveItem(kromer);
                }
                itemGiven = commRing;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU ARE THE FIRST TO OWN MY <style=cIsUtility>[Commemorative Ring]</style>!!!" });
            }
            else
            {
                body.inventory.RemoveItem(itemFromInventory);
                int roll_chance = 40;

                //if (commRingItemCount > 0) roll_chance = 60;

                bool giveItem = RoR2.Util.CheckRoll(roll_chance, body.master);
                if (giveItem)
                {
                    if (itemFromInventory.tier == ItemTier.Tier1) itemGiven = randomTier2;
                    else if (itemFromInventory.tier == ItemTier.Tier2) itemGiven = randomTier3;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: THAT'S A REAL <style=cDeath>[[Big Shot]]</style> MOVE KID!!! YOU'RE JUST LIKE [Me]..." });

                }
                else
                {
                    itemGiven = kromer;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: DELICIOUS KROMER" });
                }
            }
            #endregion

            Debug.Log("Taking " + itemTaken + ", Giving " + itemGiven);
            #region Complete interaction
            //try
            //{
            PickupIndex take = new PickupIndex(itemTaken.itemIndex);
            PickupIndex give = new PickupIndex(itemGiven.itemIndex);
            PickupDef pickupDef = take.pickupDef;

            AkSoundEngine.PostEvent(2011881192, gameObject);
            ScrapperController.CreateItemTakenOrb(body.corePosition, gameObject, pickupDef.itemIndex);
            PickupDropletController.CreatePickupDroplet(give, dropletOrigin.position, dropletOrigin.forward * 20f);
            //}
            //catch
            //{
                //Debug.Log("Error taking and giving item.");
            //}
            #endregion
        }
    }

}
