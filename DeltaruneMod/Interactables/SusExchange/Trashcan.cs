using BepInEx.Configuration;
using R2API;
using Rewired.UI;
using RoR2;
using RoR2.Hologram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables.SusExchange
{
    public class Trashcan : InteractableBase<Trashcan>
    {
        public override string InteractableName => "Suspicious Exchange";

        public override string InteractableContext => "WANNA CHANCE TO BECOME A <style=cDeath>[[Big Shot]]</style>? ";//"Pss... Wanna become a <style=cDeath>[Big Shot]</style>?";

        public override string InteractableLangToken => "SPAMTON_TRASH";

        public override GameObject InteractableModel => MainAssets.LoadAsset<GameObject>("spamton_trash.prefab");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

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
            InteractableBodyModelPrefab = InteractableModel.InstantiateClone("trachcan");
            InteractableBodyModelPrefab.AddComponent<NetworkIdentity>();
            InteractableBodyModelPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            PurchaseInteraction purchaseInteraction = InteractableBodyModelPrefab.AddComponent<PurchaseInteraction>();
            purchaseInteraction.displayNameToken = $"INTERACTABLE_{InteractableLangToken}_NAME";
            purchaseInteraction.contextToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";
            purchaseInteraction.available = true;
            purchaseInteraction.setUnavailableOnTeleporterActivated = false;
            purchaseInteraction.isShrine = true;
            purchaseInteraction.isGoldShrine = false;

            TrashcanBehavior mgr = InteractableBodyModelPrefab.AddComponent<TrashcanBehavior>();
            mgr.purchaseInteraction = purchaseInteraction;

            var pingInfoProvider = InteractableBodyModelPrefab.AddComponent<PingInfoProvider>();
            pingInfoProvider.pingIconOverride = MainAssets.LoadAsset<Sprite>("spamton_ping.png");

            var highlightController = InteractableBodyModelPrefab.GetComponent<Highlight>();
            highlightController.targetRenderer = InteractableBodyModelPrefab.GetComponentsInChildren<MeshRenderer>().Where(x => x.gameObject.name.Contains("polySurface51")).Single();
            highlightController.strength = 1;
            highlightController.highlightColor = Highlight.HighlightColor.interactive;

            Transform pivot = new GameObject("HologramPivot").transform;
            pivot.SetParent(InteractableBodyModelPrefab.transform);
            pivot.localPosition = new Vector3(0f, 0.9f, 1);

            var projector = InteractableBodyModelPrefab.AddComponent<HologramProjector>();
            projector.hologramPivot = pivot;
            projector.displayDistance = 10f;
            projector.disableHologramRotation = false;

            var textController = InteractableBodyModelPrefab.AddComponent<TextController>();
            textController.center = pivot;

            InteractableBodyModelPrefab.GetComponent<Highlight>().targetRenderer = InteractableBodyModelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            GameObject something = new GameObject();
            GameObject trigger = UnityEngine.Object.Instantiate(something, InteractableBodyModelPrefab.transform);
            trigger.AddComponent<BoxCollider>().isTrigger = true;
            trigger.AddComponent<EntityLocator>().entity = InteractableBodyModelPrefab;
            InteractableBodyModelPrefab.RegisterNetworkPrefab();
        }

        public void CreateInteractableSpawnCard()
        {
            InteractableSpawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
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
            InteractableSpawnCard.maxSpawnsPerStage = 1;
            InteractableSpawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            DirectorCard directorCard = new DirectorCard
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
    public class TextController : NetworkBehaviour
    {
        [SyncVar] public string text;
        private string oldText;
        private TextMeshPro textMesh;
        public Transform center;
        private void Start()
        {
            GameObject textObject = new GameObject("HologramText");
            textObject.transform.SetParent(center);
            textObject.transform.localPosition = Vector3.zero; 

            oldText = text;
            textMesh = textObject.AddComponent<TMPro.TextMeshPro>();
            textMesh.text = Util.UniversalVariables.maxSusExUses + " USES";
            textMesh.fontSize = 5f;
            textMesh.color = Color.red;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
            textObject.AddComponent<Billboard>();

            NetworkServer.Spawn(textObject);
        }

        [ClientRpc]
        private void UpdateText()
        {
            textMesh.text = text;
            Debug.Log("TEXT CHANGED ON MESH!\n" + oldText + " -> " + text);
            Debug.Log("IF U DONT SEE IT I KMS :)");
        }

        [Command]
        public void SetText(string newText)
        {
            oldText = text;
            text = newText;
            Debug.Log("TEXT CHANGED!\n" + oldText + " -> " + text);
            UpdateText();
        }
    }
}
