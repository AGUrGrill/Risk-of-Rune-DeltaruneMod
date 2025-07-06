using DeltaruneMod.Items.Lunar;
using DeltaruneMod.Items.Tier2;
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

            //AkSoundEngine.PostEvent(3865094552, gameObject);

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
                color = Color.gray
            }, true);

            ApplyGravestone(interactor);
        }

        public void ApplyGravestone(Interactor interactor)
        {
            var body = interactor.GetComponent<CharacterBody>();
            var commRing = CommRing.instance.ItemDef;
            var thornRing = ThornRing.instance.ItemDef;

            if (body.inventory && body.inventory.GetItemCount(commRing) > 0)
            {
                body.inventory.RemoveItem(commRing);
                body.inventory.GiveItem(thornRing);
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = "You will never wake from this nightmare..." });
            }
        }
    }
}
