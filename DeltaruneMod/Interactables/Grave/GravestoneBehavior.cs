using DeltaruneMod.Items.Lunar;
using DeltaruneMod.Items.Spamton;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace DeltaruneMod.Interactables.Grave
{
    public class GravestoneBehavior : NetworkBehaviour
    {
        public PurchaseInteraction purchaseInteraction;
        private GameObject shrineUseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ShrineUseEffect.prefab").WaitForCompletion();

        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                purchaseInteraction.SetAvailable(true);
            }
            
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            purchaseInteraction.onPurchase.AddListener(OnPurchase);
        }
        // On stage spawn, destory if not Rallypoint Delta
        private void Stage_onStageStartGlobal(Stage obj)
        {
            if (!NetworkServer.active) return;
            gameObject.transform.position = new Vector3(-174.39f, 14.6f, 195.54f);
            gameObject.transform.Rotate(new Vector3(0f, -45f));
            //Debug.Log("Location: " + gameObject.transform.position);
            Debug.Log("Gravestone spawned on " + obj.sceneDef.cachedName + " or " + obj.sceneDef.nameToken);
            if (obj.sceneDef.nameToken != "MAP_FROZENWALL_TITLE")
            {
                Debug.Log("Gravestone destroyed.");
                NetworkServer.Destroy(gameObject);
            }
        }
        // Unsubscribe when destoryed
        void OnDestroy()
        {
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
        }

        [Server]
        public void OnPurchase(Interactor interactor)
        {
            if (!NetworkServer.active) return;
            var player = interactor.GetComponent<CharacterBody>();
            var commRingCount = player.inventory.GetItemCount(CommRing.instance.ItemDef);
            if (commRingCount <= 0) return;

            EffectManager.SpawnEffect(shrineUseEffect, new EffectData()
            {
                origin = gameObject.transform.position,
                rotation = Quaternion.identity,
                scale = 3f,
                color = Color.gray
            }, true);

            ApplyGravestone(interactor);
        }

        public void ApplyGravestone(Interactor interactor)
        {
            var body = interactor.GetComponent<CharacterBody>();
            var commRing = CommRing.instance.ItemDef;
            var thornRing = ThornRing.instance.ItemDef;

            if (body.inventory.GetItemCount(commRing) > 0)
            {
                body.inventory.RemoveItem(commRing);
                Transform dropletOrigin = body.transform;
                PickupIndex take = new PickupIndex(commRing.itemIndex);
                PickupIndex give = new PickupIndex(thornRing.itemIndex);
                PickupDef pickupDef = take.pickupDef;
                ScrapperController.CreateItemTakenOrb(body.corePosition, gameObject, pickupDef.itemIndex);
                PickupDropletController.CreatePickupDroplet(give, dropletOrigin.position + new Vector3(0, 0.5f, 0), dropletOrigin.forward * 20f);
                //body.inventory.GiveItem(thornRing);
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "You will never wake from this nightmare..." });
                purchaseInteraction.available = false;
            }
        }
    }
}
