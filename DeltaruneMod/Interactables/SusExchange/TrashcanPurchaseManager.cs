using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DeltaruneMod.Interactables.SusExchange
{
    public class TrashcanPurchaseManager : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        public float ScalingModifier;
        public bool UseDefaultScaling;

        [SyncVar]
        public int BaseCostDetermination;

        public int uses;

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

            AkSoundEngine.PostEvent(3865094552, gameObject);

            purchaseInteraction.onPurchase.AddListener(TrashcanPurchaseAttempt);

            uses = TrashcanPickerManager.maxUses;

            allItems = Util.Helpers.GetItems(99);
            allTier1 = Util.Helpers.GetItems(0);
            allTier2 = Util.Helpers.GetItems(1);
            allTier3 = Util.Helpers.GetItems(2);
            for (ItemIndex i = 0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(i);
                if (itemDef != null)
                {
                    //if (itemDef.name == "Pearl") { allTakeableItems.Add(itemDef); pearl = itemDef; }
                    //else if (itemDef.name == "ShinyPearl") { allTakeableItems.Add(itemDef); shinyPearl = itemDef; }
                    if (itemDef.name == "ITEM_KROMER") kromer = itemDef;
                    else if (itemDef.name == "ITEM_PIPIS") pipis = itemDef;
                    else if (itemDef.name == "ITEM_MR_PIPIS") mrPipis = itemDef;
                    else if (itemDef.name == "ITEM_COMM_RING") commRing = itemDef;
                }
            }
        }

        [Server]
        public void TrashcanPurchaseAttempt(Interactor interactor)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            if (!interactor) { return; }
            var body = interactor.GetComponent<CharacterBody>();
            var pickerManager = GetComponent<TrashcanPickerManager>();
            //uses = pickerManager.uses;
            
            if (body && body.master)
            {
                /*
                var inventory = body.inventory;
                foreach (KeyValuePair<ItemIndex, ItemIndex> pairedItems in ShrineOfRepairDictionary.RepairItemsDictionary)
                {
                    int numberOfItems = inventory.GetItemCount(pairedItems.Key);
                    if (numberOfItems > 0)
                    {
                        inventory.RemoveItem(pairedItems.Key, numberOfItems);
                        inventory.GiveItem(pairedItems.Value, numberOfItems);
                        CharacterMasterNotificationQueue.SendTransformNotification(body.master, pairedItems.Key, pairedItems.Value, CharacterMasterNotificationQueue.TransformationType.Default);
                    }
                }
                
                Debug.Log("WHAT DO!!!\nBDWDBNAWUJ\nDWBADAWBJD");
                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData()
                {
                    origin = gameObject.transform.position,
                    rotation = Quaternion.identity,
                    scale = 1f,
                    color = (Color32)Color.red
                }, true);

                uses--;
                if (uses <= 0)
                {
                    var billboard = gameObject.transform.Find("Symbol").gameObject;
                    billboard.SetActive(false);
                }

                Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage
                {
                    subjectAsCharacterBody = interactor.GetComponent<CharacterBody>(),
                    baseToken = $"INTERACTABLE_{Trashcan.gloablLangToken}_INTERACT"
                });
                */
                if (NetworkServer.active && uses <= 0)
                {
                    purchaseInteraction.SetAvailable(false);
                }
            }
        }
    }
}
