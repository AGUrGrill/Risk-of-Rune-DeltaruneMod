using BepInEx.Configuration;
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
        public ItemDef kromer, pearl, shinyPearl, pipis, mrPipis;
        

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
                    Debug.Log(itemDef.name);
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
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "Pss... Wanna become a <style=cDeath>[Big Shot]</style>" });
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

            if (!body.inventory) return;

            // Add all inventory items
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

            // Collects all takeable items into special list
            for (int i = 0; i < allTakeableItems.Count; i++)
            {
                if (allInventoryItems.Contains(allTakeableItems[i]))
                    allTakeableInvItems.Add(allTakeableItems[i]);
            }
            if (allTakeableInvItems.Count <= 0) return;

            // Picks random item
            ItemDef randomItemFromInventory;
            try 
            {
                randomItemFromInventory = allTakeableInvItems[Random.Range(0, allTakeableInvItems.Count)];
                if (allTakeableInvItems.Contains(shinyPearl)) randomItemFromInventory = allTakeableInvItems[allTakeableInvItems.IndexOf(shinyPearl)];
                else if (allTakeableInvItems.Contains(pearl)) randomItemFromInventory = allTakeableInvItems[allTakeableInvItems.IndexOf(pearl)];
                Debug.Log(randomItemFromInventory); 
            }
            catch { return; }

            PickupIndex pickupIndex = new PickupIndex(randomItemFromInventory.itemIndex);
            PickupDef pickupDef = pickupIndex.pickupDef;

            // Send in item to take
            ScrapperController.CreateItemTakenOrb(body.corePosition, base.gameObject, pickupDef.itemIndex);
            body.inventory.RemoveItem(randomItemFromInventory);

            AkSoundEngine.PostEvent(2011881192, this.gameObject);


            // Roll for result
            if (randomItemFromInventory == pearl)
            {
                pickupIndex = new PickupIndex(pipis.itemIndex);
            }
            else if (randomItemFromInventory == shinyPearl)
            {
                pickupIndex = new PickupIndex(mrPipis.itemIndex);
            }
            else
            {
                bool giveItem = RoR2.Util.CheckRoll(50, body.master);
                if (giveItem)
                {
                    if (randomItemFromInventory.tier == ItemTier.Tier1) //body.inventory.GiveItem(randomTier2);
                        pickupIndex = new PickupIndex(randomTier2.itemIndex);
                    else if (randomItemFromInventory.tier == ItemTier.Tier2) //body.inventory.GiveItem(randomTier3);
                        pickupIndex = new PickupIndex(randomTier3.itemIndex);
                    Debug.Log("Got item");
                }
                else
                {
                    pickupIndex = new PickupIndex(kromer.itemIndex);
                    Debug.Log("Gain 1 Kromer!!!!!!!!!!!!");
                }
            }
            
            // Give item
            PickupDropletController.CreatePickupDroplet(pickupIndex, dropletOrigin.position, dropletOrigin.forward * 20f);


        }
    }

}
