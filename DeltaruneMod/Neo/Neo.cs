using BepInEx.Configuration;
using DeltaruneMod.Items.Tier2;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Neo
{
    public class Neo
    {
        public static Color AffixNeoColor = new Color(255f/255f, 192f/255f, 203f/255f);
        public static EquipmentDef AffixNeoEquipment;
        public static BuffDef AffixNeoBuff;
        public static EliteDef AffixNeoElite;
        public static float healthMult = 1f;
        public static float damageMult = 3f;
        public static float affixDropChance = 0.00025f;
        public static GameObject ItemModel = MainAssets.LoadAsset<GameObject>("neo_wings.prefab");
        //private static Material NeoMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/WardOnLevel/matWarbannerBuffRing.mat").WaitForCompletion();
        private static Texture2D eliteRamp = Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Common/ColorRamps/texRampMagmaWorm.png").WaitForCompletion();
        private static Sprite eliteIcon = MainAssets.LoadAsset<Sprite>("swoon_effect_icon.png");
        // RoR2/Base/Common/ColorRamps/texRampWarbanner.png 
        private int maxBuffs = 3;
        private List<BuffDef> allBuffs = new List<BuffDef>();
        private List<BuffDef> currBuffs = new List<BuffDef>();

        public Neo()
        {
            AddLanguageTokens();
            SetupBuff();
            SetupEquipment();
            SetupElite();
            AddContent();
            EliteRamp.AddRamp(AffixNeoElite, eliteRamp);
            ContentAddition.AddEquipmentDef(AffixNeoEquipment);
            On.RoR2.CharacterBody.OnBuffFirstStackGained += CharacterBody_OnBuffFirstStackGained;
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            On.RoR2.CombatDirector.Init += CombatDirector_Init;
        }

        private void CombatDirector_Init(On.RoR2.CombatDirector.orig_Init orig)
        {
            orig();
            if (EliteAPI.VanillaEliteTiers.Length > 2)
            {
                // HONOR
                CombatDirector.EliteTierDef targetTier = EliteAPI.VanillaEliteTiers[2];
                List<EliteDef> elites = targetTier.eliteTypes.ToList();
                AffixNeoElite.healthBoostCoefficient = 0.5f;
                AffixNeoElite.damageBoostCoefficient = 2f;
                elites.Add(AffixNeoElite);
                targetTier.eliteTypes = elites.ToArray();
            }
            if (EliteAPI.VanillaEliteTiers.Length > 1)
            {
                CombatDirector.EliteTierDef targetTier = EliteAPI.VanillaEliteTiers[1];
                List<EliteDef> elites = targetTier.eliteTypes.ToList();
                AffixNeoElite.healthBoostCoefficient = 1f;
                AffixNeoElite.damageBoostCoefficient = 3f;
                elites.Add(AffixNeoElite);
                targetTier.eliteTypes = elites.ToArray();
            }
        }

        private void CharacterBody_OnBuffFirstStackGained(
            On.RoR2.CharacterBody.orig_OnBuffFirstStackGained orig, CharacterBody self,BuffDef buffDef)
        {
            orig(self, buffDef);

            if (buffDef != AffixNeoBuff) return;

            if (allBuffs.Count <= 0) allBuffs = Util.Helpers.GetBuffs(0);

            for (int i = 0; i < maxBuffs; i++)
            {
                BuffDef ranBuff = allBuffs[Random.Range(0, allBuffs.Count)];
                currBuffs.Add(ranBuff);
                self.AddBuff(ranBuff);
            }
        }

        private void CharacterBody_OnBuffFinalStackLost(
            On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            orig(self, buffDef);

            if (buffDef != AffixNeoBuff) return;

            for (int i = 0; i < currBuffs.Count; i++)
            {
                self.RemoveBuff(currBuffs[i]);
            }
            currBuffs.Clear();
        }

        private void AddContent()
        {
            ItemDisplayRuleDict itemDisplays = new ItemDisplayRuleDict();
            ContentAddition.AddEliteDef(AffixNeoElite);
            ContentAddition.AddBuffDef(AffixNeoBuff);
        }

        private void SetupBuff()
        {
            AffixNeoBuff = ScriptableObject.CreateInstance<BuffDef>();
            AffixNeoBuff.name = "EliteNeoBuff";
            AffixNeoBuff.canStack = false;
            AffixNeoBuff.isCooldown = false;
            AffixNeoBuff.isDebuff = false;
            AffixNeoBuff.buffColor = AffixNeoColor;
            AffixNeoBuff.iconSprite = eliteIcon;
        }

        private void SetupEquipment()
        {
            AffixNeoEquipment = ScriptableObject.CreateInstance<EquipmentDef>();
            AffixNeoEquipment.appearsInMultiPlayer = true;
            AffixNeoEquipment.appearsInSinglePlayer = true;
            AffixNeoEquipment.canBeRandomlyTriggered = false;
            AffixNeoEquipment.canDrop = false;
            AffixNeoEquipment.colorIndex = ColorCatalog.ColorIndex.Equipment;
            AffixNeoEquipment.cooldown = 0.0f;
            AffixNeoEquipment.isLunar = false;
            AffixNeoEquipment.isBoss = false;
            AffixNeoEquipment.passiveBuffDef = AffixNeoBuff;
            AffixNeoEquipment.dropOnDeathChance = affixDropChance;
            AffixNeoEquipment.enigmaCompatible = false;
            //AffixNeoEquipment.pickupModelPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/EliteFire/PickupEliteFire.prefab").WaitForCompletion(), "PickupAffixNeo", false);
            //foreach (Renderer componentsInChild in AffixNeoEquipment.pickupModelPrefab.GetComponentsInChildren<Renderer>())
            //    componentsInChild.material = NeoMat;
            AffixNeoEquipment.pickupModelPrefab = ItemModel;
            AffixNeoEquipment.nameToken = "EQUIPMENT_AFFIX_NEO_NAME";
            AffixNeoEquipment.descriptionToken = "EQUIPMENT_AFFIX_NEO_DESC";
            AffixNeoEquipment.pickupToken = "EQUIPMENT_AFFIX_NEO_PICKUP";
            AffixNeoEquipment.loreToken = "EQUIPMENT_AFFIX_NEO_LORE";
            AffixNeoEquipment.name = "AffixNeo";
        }

        private void SetupElite()
        {
            AffixNeoElite = ScriptableObject.CreateInstance<EliteDef>();
            AffixNeoElite.color = AffixNeoColor;
            AffixNeoElite.eliteEquipmentDef = AffixNeoEquipment;
            AffixNeoElite.modifierToken = "ELITE_MODIFIER_NEO";
            AffixNeoElite.name = "EliteNeo";
            AffixNeoElite.healthBoostCoefficient = healthMult;
            AffixNeoElite.damageBoostCoefficient = damageMult;
            AffixNeoBuff.eliteDef = AffixNeoElite;
        }

        private void AddLanguageTokens()
        {
            LanguageAPI.Add("ELITE_MODIFIER_NEO", "Neo {0}");
            LanguageAPI.Add("EQUIPMENT_AFFIX_NEO_NAME", "NEO Armor");
            LanguageAPI.Add("EQUIPMENT_AFFIX_NEO_PICKUP", "Gain NEO armor.");
            LanguageAPI.Add("EQUIPMENT_AFFIX_NEO_DESC", "Gain NEO armor. Lower HP, higher damage, and 3 random buffs.");
            LanguageAPI.Add("EQUIPMENT_AFFIX_NEO_LORE", "I'LL ADMIT YOU'VE GOT SOME [Guts] KID!" +
                "\nBUT IN A [1 for 1] BATTLE, NEO NEVER LOSES!!!" +
                "\nIT'S TIME FOR A LITTLE [Bluelight Specil]." +
                "\nDIDN'T YOU KNOW [Neo] IS FAMOUS FOR ITS HIGH DEFENSE!?" +
                "\nNOW... ENJ0Y THE FIR3WORKS, KID!!!");
        }
    }
}
