using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables
{
    public class Trashcan : InteractableBase<Trashcan>
    {
        public override string InteractableName => "Spamton Trash";

        public override string InteractableContext => "<color=#307FFF>Pss... Wanna become a <style=cDeath>[Big Shot]</style></color>";

        public override string InteractableLangToken => "SPAMTON_TRASH";

        public override GameObject InteractableModel => MainAssets.LoadAsset<GameObject>("spamton_trash.prefab");

        public static GameObject InteractableBodyModelPrefab;

        public static InteractableSpawnCard InteractableSpawnCard;

        public override void Init()
        {
            CreateInteractable();
            CreateInteractableSpawnCard();
            CreateLang();
        }

        public void CreateInteractable()
        {
            InteractableBodyModelPrefab = PrefabAPI.InstantiateClone(InteractableModel, "trachcan");
            InteractableBodyModelPrefab.AddComponent<NetworkIdentity>();
            InteractableBodyModelPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            PurchaseInteraction purchaseInteraction = InteractableBodyModelPrefab.AddComponent<PurchaseInteraction>();
            purchaseInteraction.displayNameToken = $"INTERACTABLE_{InteractableLangToken}_NAME";
            purchaseInteraction.contextToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";
            purchaseInteraction.available = true;
            purchaseInteraction.setUnavailableOnTeleporterActivated = true;
            purchaseInteraction.isShrine = true;
            purchaseInteraction.isGoldShrine = false;

            TrashcanBehavior mgr = InteractableBodyModelPrefab.AddComponent<TrashcanBehavior>();
            mgr.purchaseInteraction = purchaseInteraction;

            InteractableBodyModelPrefab.GetComponent<Highlight>().targetRenderer = InteractableBodyModelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            GameObject something = new GameObject();
            GameObject trigger = GameObject.Instantiate(something, InteractableBodyModelPrefab.transform);
            trigger.AddComponent<BoxCollider>().isTrigger = true;
            trigger.AddComponent<EntityLocator>().entity = InteractableBodyModelPrefab;

            PrefabAPI.RegisterNetworkPrefab(InteractableBodyModelPrefab);
        }

        public void CreateInteractableSpawnCard()
        {
            InteractableSpawnCard = ScriptableObject.CreateInstance<RoR2.InteractableSpawnCard>();
            InteractableSpawnCard.name = PluginName.ToUpper()+"_isSpamtonTrash";
            InteractableSpawnCard.prefab = InteractableBodyModelPrefab;
            InteractableSpawnCard.sendOverNetwork = true;
            InteractableSpawnCard.hullSize = HullClassification.Golem;
            InteractableSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            InteractableSpawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            InteractableSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoShrineSpawn | RoR2.Navigation.NodeFlags.NoChestSpawn;
            InteractableSpawnCard.directorCreditCost = 25;
            InteractableSpawnCard.occupyPosition = true;
            InteractableSpawnCard.orientToFloor = false;
            InteractableSpawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            RoR2.DirectorCard directorCard = new RoR2.DirectorCard
            {
                selectionWeight = 5, // 230 = Normal Chest
                spawnCard = InteractableSpawnCard,
            };

            DirectorAPI.DirectorCardHolder directorCardHolder = new DirectorAPI.DirectorCardHolder
            {
                Card = directorCard,
                InteractableCategory = DirectorAPI.InteractableCategory.Shrines,
            };

            DirectorAPI.Helpers.AddNewInteractable(directorCardHolder);
        }
    }
}
