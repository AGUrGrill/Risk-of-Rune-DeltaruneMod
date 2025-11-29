using BepInEx;
using BepInEx.Configuration;
using DeltaruneMod.Elite;
using DeltaruneMod.Elites;
using DeltaruneMod.Interactables;
using DeltaruneMod.Interactables.SusExchange.TradingItems;
using DeltaruneMod.Items;
using DeltaruneMod.Items.Lunar;
using DeltaruneMod.Items.Spamton;
using DeltaruneMod.Items.VoidTier3;
using DeltaruneMod.Items.Yellow;
using DeltaruneMod.NeoMithrix;
using DeltaruneMod.Util;
using R2API;
using R2API.Networking;
using R2API.Utils;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.Util.Components;

namespace DeltaruneMod
{
    // BIG thanks to Aetherium mod github page and Risk of Rain modding discord for providing me with the knowledge
    // to actually learn how to use all of this stuff!!

    [BepInDependency(EliteAPI.PluginGUID)]
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
    [BepInDependency(DeployableAPI.PluginGUID)]
    [BepInDependency(NetworkingAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    [BepInDependency("com.rune580.riskofoptions")]
    public class DeltarunePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AGU";
        public const string PluginName = "DeltaruneMod";
        public const string PluginVersion = "2.0.7";

        public static DeltarunePlugin Instance;
        public static CharacterMaster characterMaster;
        public static CharacterBody characterBody;

        public static AssetBundle MainAssets;

        public List<ItemBase> Items = new List<ItemBase>();
        public List<InteractableBase> Interactables = new List<InteractableBase>();
        //public List<EliteBase> Elites = new List<EliteBase>();

        public static HashSet<ItemDef> BlacklistedFromPrinter = new HashSet<ItemDef>();

        public static ConfigEntry<bool> useChapter1;
        public static ConfigEntry<bool> useChapter2;
        public static ConfigEntry<bool> useChapter3;
        public static ConfigEntry<bool> useChapter4;
        public static ConfigEntry<bool> antiFunMode;
        public static ConfigEntry<bool> eliteDisable;

        //public const short TextSyncMsgId = 4242;

        public static Material malachiteOverlayMat = new Material(Addressables.LoadAssetAsync<Material>("RoR2/Base/ElitePoison/matElitePoisonOverlay.mat").WaitForCompletion());


        public void Awake()
        {
            //ModSettingsManager.SetModIcon(MainAssets.LoadAsset<Sprite>("swoon_effect_icon"));
            ModSettingsManager.SetModDescription("Adds various aspects to the game inspired by Deltarune.");
            useChapter1 = Config.Bind("Chapter Settings", "Use Chapter 1 Features", true, "Enable or Disable Chapter 1");
            useChapter2 = Config.Bind("Chapter Settings", "Use Chapter 2 Features", true, "Enable or Disable Chapter 2");
            useChapter3 = Config.Bind("Chapter Settings", "Use Chapter 3 Features", true, "Enable or Disable Chapter 3");
            useChapter4 = Config.Bind("Chapter Settings", "Use Chapter 4 Features", true, "Enable or Disable Chapter 4");
            ModSettingsManager.AddOption(new CheckBoxOption(useChapter1));
            ModSettingsManager.AddOption(new CheckBoxOption(useChapter2));
            ModSettingsManager.AddOption(new CheckBoxOption(useChapter3));
            ModSettingsManager.AddOption(new CheckBoxOption(useChapter4));

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

            #region Elite Initialization
           
            var EliteTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteBase)));
            foreach (var eliteType in EliteTypes)
            {
                EliteBase elites = (EliteBase)System.Activator.CreateInstance(eliteType);
                elites.Init();
              Debug.Log("Elite: " + elites.EliteName + " Initialized!");
            }
            #endregion

            eliteDisable = Config.Bind("Additional Settings", "Disable N.E.O. Elite", false,
                "Disables N.E.O. Elite from spawning.");
            ModSettingsManager.AddOption(new CheckBoxOption(eliteDisable));

            StartCoroutine(LoadSoundBankWhenReady());

            RemoveFromLootPool();

            new NeoMithrixController();

            new Hooks();

            antiFunMode = Config.Bind("Additional Settings", "(NOT RECOMMENDED!) Allow Suspicious Exchange items in Lootpool?", false,
                "(Use only with command, otherwise it just ruins the fun!)");
            ModSettingsManager.AddOption(new CheckBoxOption(antiFunMode));
            antiFunMode.SettingChanged += ToggleItemsForCommand;

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
            ConfigEntry<bool> enabled = Config.Bind("Enable Item?", item.ConfigCategory, true, "Should this item appear in runs?");
            bool itemAlreadyHasBlacklist = item.ItemTags.Contains(RoR2.ItemTag.AIBlacklist);
            var aiBlacklist = Config.Bind(item.ConfigCategory, "Blacklist Item from AI Use?", itemAlreadyHasBlacklist, "Should the AI not be able to obtain this item?").Value;
            var printerBlacklist = Config.Bind(item.ConfigCategory, "Blacklist Item from Printers?", false, "Should the printers be able to print this item?").Value;
            useChapter1.SettingChanged += (sender, args) =>
            {
                if (!useChapter1.Value && item.isChapter1)
                {
                    enabled.Value = false;
                }
            };
            useChapter2.SettingChanged += (sender, args) =>
            {
                if (!useChapter2.Value && item.isChapter2)
                {
                    enabled.Value = false;
                }
            };
            useChapter3.SettingChanged += (sender, args) =>
            {
                if (!useChapter3.Value && item.isChapter3)
                {
                    enabled.Value = false;
                }
            };
            useChapter4.SettingChanged += (sender, args) =>
            {
                if (!useChapter4.Value && item.isChapter4)
                {
                    enabled.Value = false;
                }
            };
            if (enabled.Value)
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
            //ModSettingsManager.AddOption(new CheckBoxOption(enabled));
            return enabled.Value;
        }

        public bool ValidateInteractable(InteractableBase interactable, List<InteractableBase> interactableList)
        {
            ConfigEntry<bool> enabled = Config.Bind("Enable Interactable?", interactable.ConfigCategory, true, "Should this interactable appear in runs?");
            useChapter1.SettingChanged += (sender, args) =>
            {
                if (!useChapter1.Value && interactable.isChapter1)
                {
                    enabled.Value = false;
                }
            };
            useChapter2.SettingChanged += (sender, args) =>
            {
                if (!useChapter2.Value && interactable.isChapter2)
                {
                    enabled.Value = false;
                }
            };
            useChapter3.SettingChanged += (sender, args) =>
            {
                if (!useChapter3.Value && interactable.isChapter3)
                {
                    enabled.Value = false;
                }
            };
            useChapter4.SettingChanged += (sender, args) =>
            {
                if (!useChapter4.Value && interactable.isChapter4)
                {
                    enabled.Value = false;
                }
            };
            if (enabled.Value)
            {
                interactableList.Add(interactable);
            }
            //ModSettingsManager.AddOption(new CheckBoxOption(enabled));
            return enabled.Value;
        }

        public void ToggleItemsForCommand(object sender, EventArgs e)
        {
            List<ItemDef> toggleableItems = new List<ItemDef>();
            toggleableItems.Add(BrokenHeart.instance.ItemDef);
            toggleableItems.Add(CommRing.instance.ItemDef);
            toggleableItems.Add(LightBulb.instance.ItemDef);
            toggleableItems.Add(MalfunctiongCore.instance.ItemDef);
            toggleableItems.Add(Pipis.instance.ItemDef);
            toggleableItems.Add(MrPipis.instance.ItemDef);

            Run.onRunSetRuleBookGlobal += (run, rulebook) =>
            {
                foreach (ItemDef item in toggleableItems)
                {
                    if (antiFunMode.Value)
                    {
                        try
                        {
                            run.availableItems.Add(item.itemIndex);
                        }
                        catch { Debug.Log(item + " is already disabled!"); }
                    }
                    else if (!antiFunMode.Value)
                    {
                        try
                        {
                            run.availableItems.Remove(item.itemIndex);
                        }
                        catch { Debug.Log(item + " is already enabled!"); }
                    }
                }
            };
        }

        public void RemoveFromLootPool()
        {
            List<ItemDef> blacklistedItems = new List<ItemDef>();
            blacklistedItems.Add(BrokenHeart.instance.ItemDef);
            blacklistedItems.Add(CommRing.instance.ItemDef);
            blacklistedItems.Add(LightBulb.instance.ItemDef);
            blacklistedItems.Add(MalfunctiongCore.instance.ItemDef);
            blacklistedItems.Add(Pipis.instance.ItemDef);
            blacklistedItems.Add(MrPipis.instance.ItemDef);
            blacklistedItems.Add(Kromer.instance.ItemDef);
            blacklistedItems.Add(BrokenHeartTradingItem.instance.ItemDef);
            blacklistedItems.Add(CommRingTradingItem.instance.ItemDef);
            blacklistedItems.Add(LightBulbTradingItem.instance.ItemDef);
            blacklistedItems.Add(MalfunctiongCoreTradingItem.instance.ItemDef);
            blacklistedItems.Add(RandomTradingItem.instance.ItemDef);
            blacklistedItems.Add(FinalForm.instance.ItemDef);
            blacklistedItems.Add(ThornRing.instance.ItemDef);
            blacklistedItems.Add(PipisTradingItem.instance.ItemDef);
            blacklistedItems.Add(MrPipisTradingItem.instance.ItemDef);

            Run.onRunSetRuleBookGlobal += (run, rulebook) =>
            {
                foreach (ItemDef item in blacklistedItems)
                {
                    try
                    {
                        run.availableItems.Remove(item.itemIndex);
                    }
                    catch { Debug.Log(item + " is already disabled!"); }
                }
                PickupDropTable.RegenerateAll(run);
            };
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.F10))
            {
                //var testing_item = GasterMask.instance.ItemDef.itemIndex;

                // Spawn all items
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                foreach (ItemBase item in Items)
                {
                    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(item.ItemDef.itemIndex), transform.position, transform.forward * 20f);
                }
            }
            */
        }
    }
}
