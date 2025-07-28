using BepInEx;
using BepInEx.Configuration;
using DeltaruneMod.Interactables;
using DeltaruneMod.Items;
using DeltaruneMod.Items.VoidTier3;
using DeltaruneMod.Neo;
using DeltaruneMod.Util;
using R2API;
using R2API.Networking;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace DeltaruneMod
{
    // BIG thanks to Aetherium mod github page and Risk of Rain modding discord for providing me with the knowledge
    // to actually learn how to use all of this stuff!!

    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInDependency(DamageAPI.PluginGUID)]
    [BepInDependency(DifficultyAPI.PluginGUID)]
    [BepInDependency(DotAPI.PluginGUID)]
    [BepInDependency(DirectorAPI.PluginGUID)]
    [BepInDependency(OrbAPI.PluginGUID)]
    [BepInDependency(SoundAPI.PluginGUID)]
    [BepInDependency(NetworkingAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class DeltarunePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AGU";
        public const string PluginName = "DeltaruneMod";
        public const string PluginVersion = "1.8.0";

        public static DeltarunePlugin Instance;
        public static CharacterMaster characterMaster;
        public static CharacterBody characterBody;

        public static AssetBundle MainAssets;

        public List<ItemBase> Items = new List<ItemBase>();
        public List<InteractableBase> Interactables = new List<InteractableBase>();

        public static HashSet<ItemDef> BlacklistedFromPrinter = new HashSet<ItemDef>();

        public static bool useChapter1 = true;
        public static bool useChapter2 = true;
        public static bool useChapter3 = true;
        public static bool useChapter4 = true;

        public void Awake()
        {
            useChapter1 = Config.Bind("Chapter Settings", "Use Chapter 1 Features", true, "Enable or Disable Chapter 1").Value;
            useChapter2 = Config.Bind("Chapter Settings", "Use Chapter 2 Features", true, "Enable or Disable Chapter 2").Value;
            useChapter3 = Config.Bind("Chapter Settings", "Use Chapter 3 Features", true, "Enable or Disable Chapter 3").Value;
            useChapter4 = Config.Bind("Chapter Settings", "Use Chapter 4 Features", true, "Enable or Disable Chapter 4").Value;

            Instance = this;

            Log.Init(Logger);

            #region Model Initialization
            Debug.Log("Starting Model Intialization for " + PluginName);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DeltaruneMod.AssetBundle.deltarune_mod"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }
            Debug.Log("Model Intialization for " + PluginName + " successful!");
            #endregion

            #region Item Initialization
            Debug.Log("Starting Item Intialization for " + PluginName);
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));
            foreach (var itemType in ItemTypes)
            {
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init();
                    Debug.Log("Item: " + item.ItemName + " Initialized!");
                }
            }
            Debug.Log("Item Intialization for " + PluginName + " successful!");
            #endregion

            #region Interactable Initialization
            Log.Debug("Trashcan empty... loading!");
            var InteractableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(InteractableBase)));
            foreach (var interactableType in InteractableTypes)
            {
                InteractableBase interactable = (InteractableBase)System.Activator.CreateInstance(interactableType);
                if (ValidateInteractable(interactable, Interactables))
                {
                    interactable.Init();
                    Debug.Log("Interactable: " + interactable.InteractableName + " Initialized!");
                }
            }
            Log.Debug("Trashcan full!");
            #endregion

            //Neo neoElite = new Neo();

            StartCoroutine(LoadSoundBankWhenReady());

            Log.Debug(PluginName + " loaded successfully!");
        }

        private IEnumerator LoadSoundBankWhenReady()
        {
            while (!AkSoundEngine.IsInitialized())
            {
                Debug.Log("Waiting for sound engine");
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            SoundBank.Init();
        }

        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = Config.Bind(item.ConfigCategory, "Enable Item?", true, "Should this item appear in runs?").Value;
            bool itemAlreadyHasBlacklist = item.ItemTags.Contains(RoR2.ItemTag.AIBlacklist);
            var aiBlacklist = Config.Bind(item.ConfigCategory, "Blacklist Item from AI Use?", itemAlreadyHasBlacklist, "Should the AI not be able to obtain this item?").Value;
            var printerBlacklist = Config.Bind(item.ConfigCategory, "Blacklist Item from Printers?", false, "Should the printers be able to print this item?").Value;
            if (!useChapter1 && item.isChapter1)
            {
                enabled = false;
            }
            if (!useChapter2 && item.isChapter2)
            {
                enabled = false;
            }
            if (!useChapter3 && item.isChapter3)
            {
                enabled = false;
            }
            if (!useChapter4 && item.isChapter4)
            {
                enabled = false;
            }
            if (enabled)
            {
                itemList.Add(item);
                if (printerBlacklist)
                {
                    item.PrinterBlacklisted = true;
                }
                if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                }
            }
            return enabled;
        }

        public bool ValidateInteractable(InteractableBase interactable, List<InteractableBase> interactableList)
        {
            var enabled = Config.Bind(interactable.ConfigCategory, "Enable Interactable?", true, "Should this interactable appear in runs?").Value;
            if (!useChapter1 && interactable.isChapter1)
            {
                enabled = false;
            }
            if (!useChapter2 && interactable.isChapter2)
            {
                enabled = false;
            }
            if (!useChapter3 && interactable.isChapter3)
            {
                enabled = false;
            }
            if (!useChapter4 && interactable.isChapter4)
            {
                enabled = false;
            }
            if (enabled)
            {
                interactableList.Add(interactable);
            }
            return enabled;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                //var testing_item = GasterMask.instance.ItemDef.itemIndex;

                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                foreach (ItemBase item in Items)
                {
                    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(item.ItemDef.itemIndex), transform.position, transform.forward * 20f);
                }
            }
        }
    }
}
