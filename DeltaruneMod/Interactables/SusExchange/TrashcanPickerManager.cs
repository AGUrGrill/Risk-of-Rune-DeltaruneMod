using DeltaruneMod.Interactables.SusExchange.TradingItems;
using DeltaruneMod.Items.Spamton;
using DeltaruneMod.Items.Yellow;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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

        public static int uses;

        public List<ItemDef> allItems = new List<ItemDef>();
        public List<ItemDef> allTier1 = new List<ItemDef>();
        public List<ItemDef> allTier2 = new List<ItemDef>();
        public List<ItemDef> allTier3 = new List<ItemDef>();
        public List<ItemDef> allTakeableItems = new List<ItemDef>();
        public List<ItemDef> allDisplayItems = new List<ItemDef>();
        public ItemDef pearl, shinyPearl;

        enum ShopItemCosts
        {
            Bulb=3,
            Ring=6,
            Core=6,
            Heart=10,
            Pipis=2,
            MrPipis=3
        }

        public void Start()
        {
            AkSoundEngine.PostEvent(3865094552, gameObject);

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
                    if (itemDef.name == "Pearl") pearl = itemDef;
                    else if (itemDef.name == "ShinyPearl") shinyPearl = itemDef;
                }
            }
            // Add trade items to display
            allDisplayItems.Add(RandomTradingItem.instance.ItemDef);
            allDisplayItems.Add(LightBulbTradingItem.instance.ItemDef);
            allDisplayItems.Add(MalfunctiongCoreTradingItem.instance.ItemDef);
            allDisplayItems.Add(BrokenHeartTradingItem.instance.ItemDef);

            
        }

        public List<ItemDef> ListTakeableInventoryItems(List<ItemDef> allInventoryItems)
        {
            List<ItemDef> returnList = new List<ItemDef>();
            for (int i = 0; i < allTakeableItems.Count; i++)
            {
                if (allInventoryItems.Contains(allTakeableItems[i]))
                    returnList.Add(allTakeableItems[i]);
            }
            return returnList;
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
                //var isItem = pickupDef.itemTier == ItemTier.Tier1 || pickupDef.itemTier == ItemTier.Tier2;
                ItemTier tier = ItemCatalog.GetItemDef(choosenItem).tier;
                List<ItemDef> allInventoryItems = Util.Helpers.GetAllItemsFromInventory(body.inventory);
                List<ItemDef> allTakeableInvItems = new List<ItemDef>();
                var numOfTakeableItems = 0;
                ItemDef randomTier2 = allTier2[UnityEngine.Random.Range(0, allTier2.Count)];
                ItemDef randomTier3 = allTier3[UnityEngine.Random.Range(0, allTier3.Count)];
                ItemDef itemGiven = null;
                #endregion

                Debug.Log("Finding takeable items");
                #region Get all takeable items from inventory
                // Collects all takeable items into special list
                allTakeableInvItems = ListTakeableInventoryItems(allInventoryItems);
                numOfTakeableItems = allTakeableInvItems.Count;
                #endregion

                Debug.Log("Deciding item to give");
                #region Choose given item based on options
                // Others
                if (choosenItem == pearl.itemIndex)
                {
                    itemGiven = Pipis.instance.ItemDef;
                    for (int i = 0; i < (int)ShopItemCosts.Pipis; i++)
                    {
                        body.inventory.RemoveItem(choosenItem);
                    }
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOUR FIRST STEP TO BECOMING A [[Big shot]]. [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == shinyPearl.itemIndex)
                {
                    itemGiven = MrPipis.instance.ItemDef;
                    for (int i = 0; i < (int)ShopItemCosts.MrPipis; i++)
                    {
                        body.inventory.RemoveItem(choosenItem);
                    }
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU WON WON WON MY [[Hyperlink blocked]]. [" + (uses - 1) + "] tries left." });
                }
                // Kromer Items
                else if (choosenItem == CommRingTradingItem.instance.ItemDef.itemIndex) 
                {
                    choosenItem = Kromer.instance.ItemDef.itemIndex;
                    for (int i = 0; i < (int)ShopItemCosts.Ring; i++)
                    {
                        body.inventory.RemoveItem(Kromer.instance.ItemDef);
                    }
                    itemGiven = CommRing.instance.ItemDef;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: YOU ARE THE FIRST TO OWN MY <style=cIsUtility>[[Commemorative Ring]]</style>. [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == LightBulbTradingItem.instance.ItemDef.itemIndex)
                {
                    choosenItem = Kromer.instance.ItemDef.itemIndex;
                    for (int i = 0; i < (int)ShopItemCosts.Bulb; i++)
                    {
                        body.inventory.RemoveItem(Kromer.instance.ItemDef);
                    }
                    itemGiven = LightBulb.instance.ItemDef;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: HEY WATCH WHERE YOU'RE [[Looking]]!!! [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == MalfunctiongCoreTradingItem.instance.ItemDef.itemIndex)
                {
                    choosenItem = Kromer.instance.ItemDef.itemIndex;
                    for (int i = 0; i < (int)ShopItemCosts.Core; i++)
                    {
                        body.inventory.RemoveItem(Kromer.instance.ItemDef);
                    }
                    itemGiven = MalfunctiongCore.instance.ItemDef;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: MY SIGNATURE [[Orb]] FOR SA-[[Error]]!! [" + (uses - 1) + "] tries left." });
                }
                else if (choosenItem == BrokenHeartTradingItem.instance.ItemDef.itemIndex)
                {
                    choosenItem = Kromer.instance.ItemDef.itemIndex;
                    for (int i = 0; i < (int)ShopItemCosts.Heart; i++)
                    {
                        body.inventory.RemoveItem(Kromer.instance.ItemDef);
                    }
                    itemGiven = BrokenHeart.instance.ItemDef;
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: MY POOR OL' HEART CAN'T TAKE THIS LOSS!! [[Help me...]] [" + (uses - 1) + "] tries left." });
                }
                // Random Item
                else if (choosenItem == RandomTradingItem.instance.ItemDef.itemIndex)
                {
                    var commRingCount = body.inventory.GetItemCount(CommRing.instance.ItemDef);
                    var roll_chance = 40 + (commRingCount * 10);

                    // Choose Random Item
                    var getRandomItem = allTakeableInvItems[UnityEngine.Random.Range(0, numOfTakeableItems)];
                    choosenItem = getRandomItem.itemIndex;
                    body.inventory.RemoveItem(choosenItem);

                    bool giveItem = RoR2.Util.CheckRoll(roll_chance, body.master);
                    if (giveItem)
                    {
                        if (getRandomItem.tier == ItemTier.Tier1) itemGiven = randomTier2;
                        else if (getRandomItem.tier == ItemTier.Tier2) itemGiven = randomTier3;
                        else itemGiven = Kromer.instance.ItemDef; // Just in case weird bugs happen
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: THAT'S A REAL <style=cDeath>[[Big Shot]]</style> MOVE KID!!! YOU'RE JUST LIKE [Me]... [" + (uses - 1) + "] tries left." });
                    }
                    else
                    {
                        itemGiven = Kromer.instance.ItemDef;
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "[TRASH DWELLER]: DELICIOUS KROMER. [" + (uses - 1) + "] tries left." });
                    }
                }
                #endregion

                #region Give item and show stuff
                Debug.Log("Taken " + choosenItem + " | Given " + itemGiven);
                string pickupColorHex, pickupName;
                Transform dropletOrigin = body.transform;
                PickupIndex give = new PickupIndex(itemGiven.itemIndex);
                PickupDropletController.CreatePickupDroplet(give, dropletOrigin.position, dropletOrigin.forward * 20f);
                //body.inventory.GiveItem(itemGiven.itemIndex);
                CharacterMasterNotificationQueue.SendTransformNotification(body.master, choosenItem, itemGiven.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);

                //pickupColorHex = ColorCatalog.GetColorHexString(ItemTierCatalog.GetItemTierDef(tier).colorIndex);
                //pickupName = Language.GetString(ItemCatalog.GetItemDef(choosenItem).nameToken);
                AkSoundEngine.PostEvent(2011881192, gameObject);
                #endregion

                // Count down uses
                uses--;
                if (uses <= 0)
                {
                    RpcHandleDeactivateClient();
                    pickerController.SetAvailable(false);
                }

                Debug.Log("Finished Interaction.");

                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData()
                {
                    origin = gameObject.transform.position,
                    rotation = Quaternion.identity,
                    scale = 1f,
                    color = (Color32)Color.yellow
                }, true);
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

            List<ItemDef> allInventoryItems = Util.Helpers.GetAllItemsFromInventory(charBody.inventory);
            List<ItemDef> allTakeableInvItems = ListTakeableInventoryItems(allInventoryItems);

            // Add items to screen
            if (charBody && charBody.master)
            {
                // Normal Method
                /*
                // Random Item
                if (allTakeableItems.Count > 0)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(RandomTradingItem.instance.ItemDef.itemIndex)
                    });
                }

                // Neo Items
                var Kromer.instance.ItemDefCount = charBody.inventory.GetItemCount(Kromer.instance.ItemDef);
                if (Kromer.instance.ItemDefCount >= 3)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(LightBulbTradingItem.instance.ItemDef.itemIndex)
                    });
                }
                if (Kromer.instance.ItemDefCount >= 6)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(MalfunctionCoreTradingItem.instance.ItemDef.itemIndex)
                    });
                }
                if (Kromer.instance.ItemDefCount >= 10)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(BrokenHeartTradingItem.instance.ItemDef.itemIndex)
                    });
                }

                //Others
                var pearlCount = charBody.inventory.GetItemCount(pearl);
                var shinyPearlCount = charBody.inventory.GetItemCount(shinyPearl);
                if (pearlCount > 0)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(pearl.itemIndex)
                    });
                }
                if (shinyPearlCount > 0)
                {
                    options.Add(new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex(shinyPearl.itemIndex)
                    });
                }
                */

                // Unlock Method
                var ranItemAvaliable = false;
                var commRingAvaliable = false;
                var lightBulbAvaliable = false;
                var malfunctionCoreAvaliable = false;
                var brokenHeartAvaliable = false;
                var pearlAvaliable = false;
                var shinyPearlAvaliable = false;

                if (allTakeableInvItems.Count > 0) ranItemAvaliable = true;
                // Random Item
                options.Add(new PickupPickerController.Option
                {
                    available = ranItemAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(RandomTradingItem.instance.ItemDef.itemIndex)
                });

                // Kromer Items
                var kromerCount = charBody.inventory.GetItemCount(Kromer.instance.ItemDef);
                if (kromerCount >= (int)ShopItemCosts.Bulb) lightBulbAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = lightBulbAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(LightBulbTradingItem.instance.ItemDef.itemIndex)
                });
                if (kromerCount >= (int)ShopItemCosts.Ring) commRingAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = commRingAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(CommRingTradingItem.instance.ItemDef.itemIndex)
                });
                if (kromerCount >= (int)ShopItemCosts.Core) malfunctionCoreAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = malfunctionCoreAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(MalfunctiongCoreTradingItem.instance.ItemDef.itemIndex)
                });
                if (kromerCount >= (int)ShopItemCosts.Heart) brokenHeartAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = brokenHeartAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(BrokenHeartTradingItem.instance.ItemDef.itemIndex),
                });

                //Others
                var pearlCount = charBody.inventory.GetItemCount(pearl);
                var shinyPearlCount = charBody.inventory.GetItemCount(shinyPearl);
                var commRingCount = charBody.inventory.GetItemCount(CommRing.instance.ItemDef);
                if (pearlCount >= (int)ShopItemCosts.Pipis) pearlAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = pearlAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(pearl.itemIndex)
                });
                if (shinyPearlCount >= (int)ShopItemCosts.MrPipis) shinyPearlAvaliable = true;
                options.Add(new PickupPickerController.Option
                {
                    available = shinyPearlAvaliable,
                    pickupIndex = PickupCatalog.FindPickupIndex(shinyPearl.itemIndex)
                });

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
