using BepInEx.Configuration;
using DeltaruneMod.Util;
using LeTai.Asset.TranslucentImage;
using R2API;
using Rewired.UI;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Hologram;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements.UIR;
using static DeltaruneMod.DeltarunePlugin;
using static Rewired.UI.ControlMapper.ControlMapper;

namespace DeltaruneMod.Interactables.SusExchange
{
    public class Trashcan : InteractableBase<Trashcan>
    {
        public override string InteractableName => "Suspicious Exchange";

        public override string InteractableContext => "WANNA CHANCE TO BECOME A <style=cDeath>[[Big Shot]]</style>? ";

        public readonly static string gloablLangToken = "SPAMTON_TRASH";
        public override string InteractableLangToken => gloablLangToken;

        public override GameObject InteractableModel => MainAssets.LoadAsset<GameObject>("spamton_shop.prefab");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static GameObject InteractableBodyModelPrefab;

        public static InteractableSpawnCard InteractableSpawnCard;

        public static Sprite common_bg = MainAssets.LoadAsset<Sprite>("spamton_trash_bg_common");
        public static Sprite uncommon_bg = MainAssets.LoadAsset<Sprite>("spamton_trash_bg_uncommon");
        public static Sprite rare_bg = MainAssets.LoadAsset<Sprite>("spamton_trash_bg_rare");

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

            #region Purchase Interaction
            var purchaseInteraction = InteractableBodyModelPrefab.AddComponent<PurchaseInteraction>();
            purchaseInteraction.displayNameToken = $"INTERACTABLE_{InteractableLangToken}_NAME";
            purchaseInteraction.contextToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";
            purchaseInteraction.available = true;
            purchaseInteraction.setUnavailableOnTeleporterActivated = false;
            purchaseInteraction.isShrine = true;
            purchaseInteraction.isGoldShrine = false;

            //var purchaseManager = InteractableBodyModelPrefab.AddComponent<TrashcanPurchaseManager>();
            //purchaseManager.purchaseInteraction = purchaseInteraction;
            #endregion

            #region Interactable Settings
            var pingInfoProvider = InteractableBodyModelPrefab.AddComponent<PingInfoProvider>();
            pingInfoProvider.pingIconOverride = MainAssets.LoadAsset<Sprite>("spamton_ping.png");

            var highlightController = InteractableBodyModelPrefab.GetComponent<Highlight>();
            highlightController.targetRenderer = InteractableBodyModelPrefab.GetComponentsInChildren<MeshRenderer>().Where(x => x.gameObject.name.Contains("polySurface51")).Single();
            highlightController.strength = 1;
            highlightController.highlightColor = Highlight.HighlightColor.interactive;

            Transform pivot = new GameObject("HologramPivot").transform;
            pivot.SetParent(InteractableBodyModelPrefab.transform);
            pivot.localPosition = new Vector3(0f, 0.9f, 1f);

            var projector = InteractableBodyModelPrefab.AddComponent<HologramProjector>();
            projector.hologramPivot = pivot;
            projector.displayDistance = 10f;
            projector.disableHologramRotation = false;
            
            var hologramText = new GameObject("HologramText");
            hologramText.transform.SetParent(pivot);
            hologramText.transform.localPosition = Vector3.zero;

            var textMesh = hologramText.AddComponent<TMPro.TextMeshPro>();
            textMesh.text = "";
            textMesh.fontSize = 6f;
            textMesh.color = Color.red;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
            hologramText.AddComponent<Billboard>();

            var textController = InteractableBodyModelPrefab.AddComponent<Util.Components.TextController>();
            textController.textMesh = textMesh;
            #endregion

            #region Picker UI
            var pickerUIPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Scrapper/ScrapperPickerPanel.prefab").WaitForCompletion(), "TrashcanPickerPanel");

            var imagePanel = pickerUIPrefab.transform.Find("MainPanel/Juice/BG");
            if (imagePanel != null)
            {
                var img = imagePanel.GetComponent<Image>();
                var backgroundRoll = UnityEngine.Random.Range(0, 100);
                img.sprite = common_bg;
                if (backgroundRoll >= 66 && backgroundRoll < 93) img.sprite = uncommon_bg;
                else if (backgroundRoll >= 93) img.sprite = rare_bg;
                var tempColor = img.color;
                tempColor.a = 0.0f;
                img.color = tempColor;
            }
            var label = pickerUIPrefab.transform.Find("MainPanel/Juice/Label");
            if (label != null)
            {
                var text = label.GetComponent<LanguageTextMeshController>();
                if (text != null)
                {
                    text.token = "SHOP FOR BIG [[Big] DEALS. NOW!!!!";
                }
            }
;
            /*
            var scrapperInfo = pickerUIPrefab.GetComponent<ScrapperInfoPanelHelper>();
            var repairInfo = pickerUIPrefab.AddComponent<TrashcanInfoPanelHelper>();
            var cont = repairInfo.inspectPanelController = scrapperInfo.inspectPanelController;
            repairInfo.correspondingScrapImage = scrapperInfo.correspondingScrapImage;
            UnityEngine.Object.DestroyImmediate(scrapperInfo);
            

            var panel = pickerUIPrefab.GetComponent<PickupPickerPanel>();
            panel.pickupSelected.AddPersistentListener(repairInfo.ShowInfo);
            panel.pickupBaseContentReady.AddPersistentListener(repairInfo.AddQuantityToPickerButton);

            repairInfo.panel = panel;
            */
            #endregion

            #region Picker Controller
            var uiPromptController = InteractableBodyModelPrefab.AddComponent<NetworkUIPromptController>();

            var pickupManager = InteractableBodyModelPrefab.AddComponent<TrashcanPickerManager>();

            var pickerController = InteractableBodyModelPrefab.AddComponent<PickupPickerController>();
            pickerController.panelPrefab = pickerUIPrefab;
            pickerController.onPickupSelected = new PickupPickerController.PickupIndexUnityEvent();
            pickerController.onPickupSelected.AddPersistentListener(pickupManager.HandleSelection);
            pickerController.onServerInteractionBegin = new GenericInteraction.InteractorUnityEvent();
            pickerController.onServerInteractionBegin.AddPersistentListener(pickupManager.HandleInteraction);
            pickerController.cutoffDistance = 10f;
            pickerController.contextString = $"INTERACTABLE_{InteractableLangToken}_CONTEXT";

            pickupManager.pickerController = pickerController;

            /*
            var inspectDef = ScriptableObject.CreateInstance<InspectDef>();
            (inspectDef as ScriptableObject).name = "idTrashcan";
            inspectDef.Info = new RoR2.UI.InspectInfo
            {
                Visual = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texShrineIconOutlined.png").WaitForCompletion(),
                TitleToken = $"INTERACTABLE_{InteractableLangToken}_CONTEXT",
                DescriptionToken = $"INTERACTABLE_{InteractableLangToken}_DESCRIPTION_PICKER",
                FlavorToken = $"INTERACTABLE_{InteractableLangToken}_LORE",
                TitleColor = UnityEngine.Color.white,
                isConsumedItem = false
            };
            InteractableBodyModelPrefab.AddComponent<GenericInspectInfoProvider>().InspectInfo = inspectDef;
            */
            #endregion

            #region Instantiation
            InteractableBodyModelPrefab.GetComponent<Highlight>().targetRenderer = InteractableBodyModelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            GameObject something = new GameObject();
            GameObject trigger = UnityEngine.Object.Instantiate(something, InteractableBodyModelPrefab.transform);
            trigger.AddComponent<BoxCollider>().isTrigger = true;
            trigger.AddComponent<EntityLocator>().entity = InteractableBodyModelPrefab;
            InteractableBodyModelPrefab.RegisterNetworkPrefab();
            #endregion
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
            InteractableSpawnCard.directorCreditCost = 5;
            InteractableSpawnCard.occupyPosition = true;
            InteractableSpawnCard.orientToFloor = false;
            InteractableSpawnCard.maxSpawnsPerStage = 1;
            InteractableSpawnCard.skipSpawnWhenSacrificeArtifactEnabled = false;

            DirectorCard directorCard = new DirectorCard
            {
                selectionWeight = 50, // 230 = Normal Chest
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
