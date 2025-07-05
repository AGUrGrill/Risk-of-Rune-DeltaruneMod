using BepInEx.Configuration;
using DeltaruneMod.Items;
using DeltaruneMod.Items.Tier2;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables
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
        

        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                purchaseInteraction.SetAvailable(true);
            }

            AkSoundEngine.PostEvent(3865094552, this.gameObject);

            for (ItemIndex i = (ItemIndex)0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(i);
                if (itemDef != null)
                {
                    allItems.Add(itemDef);
                    if (itemDef.tier == ItemTier.Tier1) { allTier1.Add(itemDef); allTakeableItems.Add(itemDef); }
                    else if (itemDef.tier == ItemTier.Tier2) { allTier2.Add(itemDef); allTakeableItems.Add(itemDef); }
                    else if (itemDef.tier == ItemTier.Tier3) allTier3.Add(itemDef);

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

        [Server]
        public void OnPurchase(Interactor interactor)
        {
            if (!NetworkServer.active) return;

            EffectManager.SpawnEffect(shrineUseEffect, new EffectData()
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.blue
            }, true);

            ApplySpamtonShop(interactor);
        }

        public void ApplySpamtonShop(Interactor interactor)
        {
            CharacterBody body = interactor.GetComponent<CharacterBody>();
            Transform dropletOrigin = body.transform;
            List<ItemDef> allInventoryItems = new List<ItemDef>();
            List<ItemDef> allTakeableInvItems = new List<ItemDef>();
            ItemDef randomTier2 = allTier2[Random.Range(0, allTier2.Count)];
            ItemDef randomTier3 = allTier3[Random.Range(0, allTier3.Count)];
            ItemDef itemTaken = null; 
            ItemDef itemGiven = null;

            if (!body.inventory) return;

            // int commRingItemCount = body.inventory.GetItemCount(commRing);

            #region Get inventory items
            for (ItemIndex i = (ItemIndex)0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                int itemCount = body.inventory.GetItemCount(i);
                ItemDef itemDef = ItemCatalog.GetItemDef(i);

                if (itemCount > 0 && itemDef != null)
                {
                    allInventoryItems.Add(itemDef);
                    Debug.Log("Inventory Item: " + itemDef);
                }
            }
            #endregion

            #region Get all takeable items from inventory
            // Collects all takeable items into special list
            for (int i = 0; i < allTakeableItems.Count; i++)
            {
                if (allInventoryItems.Contains(allTakeableItems[i]))
                    allTakeableInvItems.Add(allTakeableItems[i]);
            }
            if (allTakeableInvItems.Count <= 0) return;
            #endregion

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
                int roll_chance = 50;

                //if (commRingItemCount > 0) roll_chance = 60;

                bool giveItem = RoR2.Util.CheckRoll(roll_chance, body.master);
                if (giveItem)
                {
                    if (itemFromInventory.tier == ItemTier.Tier1) itemGiven = randomTier2;
                    else if (itemFromInventory.tier == ItemTier.Tier2) itemGiven = randomTier3;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: THAT'S A REAL BIGSHOT MOVE KID!!! YOU'RE LIKE ME..." });

                }
                else
                {
                    itemGiven = kromer;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: DELICIOUS KROMER" });
                }
            }
            #endregion

            #region Complete interaction
            //try
            //{
                PickupIndex take = new PickupIndex(itemTaken.itemIndex);
                PickupIndex give = new PickupIndex(itemGiven.itemIndex);
                PickupDef pickupDef = take.pickupDef;

                AkSoundEngine.PostEvent(2011881192, this.gameObject);
                ScrapperController.CreateItemTakenOrb(body.corePosition, base.gameObject, pickupDef.itemIndex);
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
