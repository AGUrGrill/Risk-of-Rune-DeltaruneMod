using BepInEx;
using BepInEx.Configuration;
using DeltaruneMod.Elites;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Code extracted from Nuxlar's MoreElites mod
namespace DeltaruneMod.Elite
{
    public abstract class EliteBase<T> : EliteBase where T : EliteBase<T>
    {
        public static T instance { get; private set; }

        public EliteBase()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting EliteBase was instantiated twice");
            instance = this as T;
        }
    }

    public abstract class EliteBase
    {
        public enum EliteTier
        {
            None,
            T1,
            T1Honor,
            T1GuildedHonor,
            T1Guilded,
            T2,
            Lunar
        }
        public abstract string EliteName { get; }
        public abstract string EliteAffixDesc { get; }
        public abstract Color EliteColor { get; }
        public abstract float EliteHealthMult { get; }
        public abstract float EliteDamageMult { get; }
        public abstract float EliteAffixDropChance { get; }
        public abstract Material EliteAffixMaterial { get; }
        public abstract Texture2D EliteRamp { get; }
        public abstract Sprite EliteIcon { get; }
        public abstract Sprite EliteAspectIcon { get; }
        public abstract GameObject EliteCrown { get; }
        public abstract EliteTier EliteTierDef { get; }

        public EquipmentDef EliteEquip;
        public BuffDef EliteBuff;
        public EliteDef EliteDefinition;
        public ItemDef EliteItem;
        public virtual CustomElite CustomEliteDef { get; set; }
        public virtual CustomElite CustomEliteDefHonor { get; set; }

        public abstract void Init();

        protected virtual void CreateLang()
        {
            LanguageAPI.Add("ELITE_" + EliteName.ToUpper() + "_NAME", EliteName);
            LanguageAPI.Add("ELITE_MODIFIER_" + EliteName.ToUpper(), EliteName + " {0}");
            LanguageAPI.Add("ELITE_EQUIPMENT_AFFIX_" + EliteName.ToUpper() + "_NAME", $"{EliteName} Name");
            LanguageAPI.Add("ELITE_AFFIX_" + EliteName.ToUpper() + "_DESCRIPTION", $"{EliteName} Aspect");
        }

        protected void AddRamp()
        {
            R2API.EliteRamp.AddRamp(EliteDefinition, EliteRamp);
        }

        protected void AddContent()
        {
            List<CombatDirector.EliteTierDef> tiers = new();
            EliteDef knownT1 = Addressables.LoadAssetAsync<EliteDef>("RoR2/Base/EliteFire/edFire.asset").WaitForCompletion();
            EliteDef knownT1H = Addressables.LoadAssetAsync<EliteDef>("RoR2/Base/EliteFire/edFireHonor.asset").WaitForCompletion();

            switch (EliteTierDef)
            {
                case (EliteTier.T1):
                    AddAllTiersThatContain(knownT1);
                    break;
                case (EliteTier.T1Honor):
                    AddAllTiersThatContain(knownT1H);
                    break;
            }

            void AddAllTiersThatContain(EliteDef def)
            {
                var ctiers = EliteAPI.GetCombatDirectorEliteTiers();

                foreach (CombatDirector.EliteTierDef tier in ctiers)
                {
                    if (tier.eliteTypes.Contains(def))
                    {
                        tiers.Add(tier);
                        Debug.Log("Tier " + def.name + " added to " + EliteName + ".");
                    }
                }
            }

            CustomEliteDef = new CustomElite(EliteDefinition, tiers.ToArray(), EliteRamp);
            EliteAPI.Add(CustomEliteDef);
            ContentAddition.AddBuffDef(EliteBuff);
            ContentAddition.AddEquipmentDef(EliteEquip);
        }

        protected void AddCrown()
        {
            EliteItem = ScriptableObject.CreateInstance<ItemDef>();
            EliteItem.name = "ITEM_" + EliteName + "_CROWN";
            EliteItem.nameToken = "ITEM_" + EliteName + "_CROWN_NAME";
            EliteItem.pickupToken = "ITEM_" + EliteName + "_CROWN_PICKUP";
            EliteItem.descriptionToken = "ITEM_" + EliteName + "_CROWN_DESCRIPTION";
            EliteItem.loreToken = "ITEM_" + EliteName + "_CROWN_LORE";
            EliteItem.pickupModelPrefab = EliteCrown;
            EliteItem.pickupIconSprite = null;
            EliteItem.hidden = true;
            EliteItem.canRemove = false;
            EliteItem.deprecatedTier = ItemTier.NoTier;

            ItemAPI.Add(new CustomItem(EliteItem, CreateItemDisplayRules()));
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateBuff(string buffName)
        {
            EliteBuff = ScriptableObject.CreateInstance<BuffDef>();
            EliteBuff.name = buffName;
            EliteBuff.canStack = false;
            EliteBuff.isCooldown = false;
            EliteBuff.isDebuff = false;
            EliteBuff.buffColor = EliteColor;
            EliteBuff.iconSprite = EliteIcon;
        }

        protected void CreateEquip(string equipmentName)
        {
            EliteEquip = ScriptableObject.CreateInstance<EquipmentDef>();
            EliteEquip.appearsInMultiPlayer = true;
            EliteEquip.appearsInSinglePlayer = true;
            EliteEquip.canBeRandomlyTriggered = false;
            EliteEquip.canDrop = false;
            EliteEquip.colorIndex = ColorCatalog.ColorIndex.Equipment;
            EliteEquip.cooldown = 0.0f;
            EliteEquip.isLunar = false;
            EliteEquip.isBoss = false;
            EliteEquip.passiveBuffDef = EliteBuff;
            EliteEquip.dropOnDeathChance = EliteAffixDropChance;
            EliteEquip.enigmaCompatible = false;
            EliteEquip.pickupIconSprite = EliteAspectIcon;
            EliteEquip.pickupModelPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/EliteFire/PickupEliteFire.prefab").WaitForCompletion(), "PickupAffixEmpowering", false);
            foreach (Renderer componentsInChild in EliteEquip.pickupModelPrefab.GetComponentsInChildren<Renderer>())
                componentsInChild.material = EliteAffixMaterial;
            EliteEquip.nameToken = equipmentName;
            EliteEquip.name = "Affix" + EliteName;
            EliteEquip.pickupToken = "EQUIPMENT_AFFIX_" + EliteName.ToUpper() + "_PICKUP";
            EliteEquip.descriptionToken = EliteAffixDesc;
            EliteEquip.loreToken = "EQUIPMENT_AFFIX_" + EliteName.ToUpper() + "_LORE";
        }

        protected void CreateElite()
        {
            EliteDefinition = ScriptableObject.CreateInstance<EliteDef>();
            EliteDefinition.color = EliteColor;
            EliteDefinition.eliteEquipmentDef = EliteEquip;
            EliteDefinition.modifierToken = "ELITE_MODIFIER_" + EliteName.ToUpper();
            EliteDefinition.name = EliteName;
            EliteDefinition.healthBoostCoefficient = EliteHealthMult;
            EliteDefinition.damageBoostCoefficient = EliteDamageMult;
            EliteBuff.eliteDef = EliteDefinition;
        }

        public virtual void Hooks() { }
    }
}