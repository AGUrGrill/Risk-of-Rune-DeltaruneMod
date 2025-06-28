using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using BepInEx.Configuration;
using R2API;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables
{
    public static class Trashcan
    {
        private static GameObject beebleStatue = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/mdlBeetle.fbx").WaitForCompletion(), "BeebleMemorialStatue");
        private static Material beebleStatueMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/MonstersOnShrineUse/matMonstersOnShrineUse.mat").WaitForCompletion();

        public static void Init()
        {
            StartCreation();
        }

        public static void StartCreation()
        {
            // Statue
            beebleStatue.name = "BeebleMemorialStatue";
            beebleStatue.AddComponent<NetworkIdentity>();
            beebleStatue.transform.localScale = new Vector3(3f, 3f, 3f);
            beebleStatue.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial = beebleStatueMat;
            beebleStatue.transform.GetChild(1).gameObject.AddComponent<BoxCollider>();

            // Interactable
            TrashcanBehavior mgr = beebleStatue.AddComponent<TrashcanBehavior>();
            PurchaseInteraction interaction = beebleStatue.AddComponent<PurchaseInteraction>();
            interaction.contextToken = "<color=#307FFF>Pss... Wanna become a <style=cDeath>[Big Shot]</style></color>";
            interaction.NetworkdisplayNameToken = "<color=#307FFF>Pss... Wanna become a <style=cDeath>[Big Shot]</style></color>";
            mgr.purchaseInteraction = interaction;
            beebleStatue.GetComponent<Highlight>().targetRenderer = beebleStatue.GetComponentInChildren<SkinnedMeshRenderer>();
            GameObject something = new GameObject();
            GameObject trigger = GameObject.Instantiate(something, beebleStatue.transform);
            trigger.AddComponent<BoxCollider>().isTrigger = true;
            trigger.AddComponent<EntityLocator>().entity = beebleStatue;

            InteractableSpawnCard interactableSpawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            interactableSpawnCard.name = "iscBeebleMemorialStatue";
            interactableSpawnCard.prefab = beebleStatue;
            interactableSpawnCard.sendOverNetwork = true;
            interactableSpawnCard.hullSize = HullClassification.Golem;
            interactableSpawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            interactableSpawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            interactableSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoShrineSpawn;
            interactableSpawnCard.directorCreditCost = 0;
            interactableSpawnCard.occupyPosition = true;
            interactableSpawnCard.orientToFloor = false;
            interactableSpawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            DirectorCard directorCard = new DirectorCard
            {
                selectionWeight = 500, // 230 = Normal Chest
                spawnCard = interactableSpawnCard,
            };

            DirectorAPI.DirectorCardHolder directorCardHolder = new DirectorAPI.DirectorCardHolder
            {
                Card = directorCard,
                InteractableCategory = DirectorAPI.InteractableCategory.Shrines
            };

            DirectorAPI.Helpers.AddNewInteractable(directorCardHolder);
        }
    }
}
