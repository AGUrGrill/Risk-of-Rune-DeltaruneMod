using BepInEx;
using DeltaruneMod.Elites;
using DeltaruneMod.Interactables;
using DeltaruneMod.Items;
using DeltaruneMod.Util;
using R2API;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class MainClass : BaseUnityPlugin
{
    public static PluginInfo PInfo { get; private set; }
    public void Awake()
    {
        PInfo = Info;
    }
}

namespace DeltaruneMod
{
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class DeltarunePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AGU";
        public const string PluginName = "DeltaruneMod";
        public const string PluginVersion = "1.7.7";

        public static DeltarunePlugin Instance;
        public static CharacterMaster characterMaster;
        public static CharacterBody characterBody;

        public static AssetBundle MainAssets;

        public List<ItemBase> Items = new List<ItemBase>();
        public List<InteractableBase> Interactables = new List<InteractableBase>();

        public static HashSet<ItemDef> BlacklistedFromPrinter = new HashSet<ItemDef>();

        public void Awake()
        {
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

            Events.Init();

            Neo neoElite = new Neo();

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
            itemList.Add(item);
            return true;
        }

        public bool ValidateInteractable(InteractableBase interactable, List<InteractableBase> interactableList)
        {
            interactableList.Add(interactable);
            return true;
        }
        
        private void Update()
        {
        }
    }
}
