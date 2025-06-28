using BepInEx;
using DeltaruneMod.Items;
using DeltaruneMod.Util;
using DeltaruneMod.Interactables;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
        public const string PluginVersion = "1.3.0";

        public static DeltarunePlugin Instance;
        public static CharacterMaster characterMaster;
        public static CharacterBody characterBody;

        public static AssetBundle MainAssets;

        public List<ItemBase> Items = new List<ItemBase>();

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
            try { Log.Debug("Trashcan empty... loading!"); }
            catch { }
            
            try { Trashcan.Init(); }
            catch { Log.Debug("Trashcan FAILED!! KYS"); }

            Log.Debug("Trashcan full!");
            #endregion

            Events.Init();
            
            Log.Debug(PluginName + " loaded successfully!");
        }
        private void Update()
        {
        }

        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            itemList.Add(item);
            return enabled;
        }
    }
}
