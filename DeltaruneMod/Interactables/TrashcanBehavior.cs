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

        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                purchaseInteraction.SetAvailable(true);
            }

            for (ItemIndex i = (ItemIndex)0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(i);
                if (itemDef != null)
                {
                    allItems.Add(itemDef);
                    if (itemDef.tier == ItemTier.Tier1) allTier1.Add(itemDef);
                    else if (itemDef.tier == ItemTier.Tier2) allTier2.Add(itemDef);
                    else if (itemDef.tier == ItemTier.Tier3) allTier3.Add(itemDef);
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
                color = Color.white
            }, true);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "<style=cEvent><color=#307FFF>Pss... Wanna become a <style=cDeath>[Big Shot]</style></color></style>" });
            ApplySpamtonShop(interactor);
        }

        public void ApplySpamtonShop(Interactor interactor)
        {
            CharacterBody body = interactor.GetComponent<CharacterBody>();
            List<ItemDef> allInventoryItems = new List<ItemDef>();
            bool hasItems = (body.inventory.GetTotalItemCountOfTier(ItemTier.Tier1) > 0) && (body.inventory.GetTotalItemCountOfTier(ItemTier.Tier2) > 0);

            ItemDef randomTier2 = allTier2[Random.Range(0, allTier2.Count)];
            ItemDef randomTier3 = allTier3[Random.Range(0, allTier3.Count)];
            for (ItemIndex i = (ItemIndex)0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                int itemCount = body.inventory.GetItemCount(i);
                ItemDef itemDef = ItemCatalog.GetItemDef(i);
                //Debug.Log("Item: " + itemDef);
                if (itemCount > 0 && itemDef != null && itemDef.tier != ItemTier.Tier3)
                {
                    allInventoryItems.Add(itemDef);
                    Debug.Log("Inventory Item: " + itemDef);
                }
            }
            ItemDef randomItemFromInventory;
            try { randomItemFromInventory = allInventoryItems[Random.Range(0, allInventoryItems.Count)]; Debug.Log(randomItemFromInventory); }
            catch { return; }
            if (!body.inventory || randomItemFromInventory.tier == ItemTier.Tier3) return;
            bool giveItem = RoR2.Util.CheckRoll(50, body.master);
            if (giveItem)
            {
                if (randomItemFromInventory.tier == ItemTier.Tier1) body.inventory.GiveItem(randomTier2);
                else if (randomItemFromInventory.tier == ItemTier.Tier2) body.inventory.GiveItem(randomTier3);
                Debug.Log("Got item");
            }
            else
            {
                Debug.Log("Gain 1 Kromer!!!!!!!!!!!!");
            }
            Debug.Log("Removing Item");
            body.inventory.RemoveItem(randomItemFromInventory);

        }
    }

}
