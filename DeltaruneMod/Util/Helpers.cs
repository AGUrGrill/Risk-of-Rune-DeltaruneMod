using BepInEx.Configuration;
using DeltaruneMod.Items;
using DeltaruneMod.Items.Tier2;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace DeltaruneMod.Util
{
    public class Helpers
    {
        #region Item Defs
        // 99: ALL, 0: Tier 1, 1: Tier 2, 2: Tier, 3: Tier Boss
        public static List<ItemDef> GetItems(int tierIndex)
        {
            List<ItemDef> items = new List<ItemDef>();
            for (ItemIndex i = 0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef item = ItemCatalog.GetItemDef(i);

                if (item == null) continue;

                if (tierIndex == 99) items.Add(item);
                else if (tierIndex == 0 && item.tier == ItemTier.Tier1) items.Add(item);
                else if (tierIndex == 1 && item.tier == ItemTier.Tier2) items.Add(item);
                else if (tierIndex == 2 && item.tier == ItemTier.Tier3) items.Add(item);
                else if (tierIndex == 3 && item.tier == ItemTier.Lunar) items.Add(item);
                else if (tierIndex == 4 && item.tier == ItemTier.Boss) items.Add(item);
                else if (tierIndex == 5 && item.tier == ItemTier.NoTier) items.Add(item);
                else if (tierIndex == 6 && item.tier == ItemTier.VoidTier1) items.Add(item);
                else if (tierIndex == 7 && item.tier == ItemTier.VoidTier2) items.Add(item);
                else if (tierIndex == 8 && item.tier == ItemTier.VoidTier3) items.Add(item);
                else if (tierIndex == 9 && item.tier == ItemTier.VoidBoss) items.Add(item);
                else if (tierIndex == 10 && item.tier == ItemTier.AssignedAtRuntime) items.Add(item);
            }
            return items;
        }
        public static List<ItemDef> GetAllItemsFromInventory(Inventory inv)
        {
            List<ItemDef> items = new List<ItemDef>();
            for (ItemIndex i = 0; i < (ItemIndex)ItemCatalog.itemCount; i++)
            {
                ItemDef item = ItemCatalog.GetItemDef(i);
                if (item == null) continue;
                if (inv.GetItemCount(i) > 0) items.Add(item);
            }
            return items;
        }
        #endregion

        #region Buff Defs
        // 99: ALL, 0: Buffs?, 1: Debuffs, 2: Affixs
        public static List<BuffDef> GetBuffs(int type)
        {
            List<BuffDef> buffs = new List<BuffDef>();
            FieldInfo[] fields = typeof(RoR2Content.Buffs).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.GetValue(null) is BuffDef buff)
                {
                    if (buff.isHidden) continue;
                    if (type == 0 && !buff.isElite && !buff.isDebuff) buffs.Add(buff);
                    else if (type == 1 && buff.isDebuff) buffs.Add(buff);
                    else if (type == 2 && buff.isElite) buffs.Add(buff);
                    else if (type == 99) buffs.Add(buff);
                }
            }
            fields = typeof(DLC1Content.Buffs).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.GetValue(null) is BuffDef buff)
                {
                    if (buff.isHidden) continue;
                    if (type == 0 && !buff.isElite && !buff.isDebuff) buffs.Add(buff);
                    else if (type == 1 && buff.isDebuff) buffs.Add(buff);
                    else if (type == 2 && buff.isElite) buffs.Add(buff);
                    else if (type == 99) buffs.Add(buff);
                }
            }
            fields = typeof(DLC2Content.Buffs).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.GetValue(null) is BuffDef buff)
                {
                    if (buff.isHidden) continue;
                    if (type == 0 && !buff.isElite && !buff.isDebuff) buffs.Add(buff);
                    else if (type == 1 && buff.isDebuff) buffs.Add(buff);
                    else if (type == 2 && buff.isElite) buffs.Add(buff);
                    else if (type == 99) buffs.Add(buff);
                }
            }
            return buffs;
        }
        #endregion

        #region Make Prefabs
        public static void CreateNetworkedEffectPrefab(GameObject obj, bool isFollower)
        {
            if (!obj.GetComponent<NetworkIdentity>()) obj.AddComponent<NetworkIdentity>();
            var effect = obj.GetComponent<EffectComponent>();
            if (!effect) effect = obj.AddComponent<EffectComponent>();
            effect.applyScale = isFollower;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = isFollower;
            effect.positionAtReferencedTransform = isFollower;

            PrefabAPI.RegisterNetworkPrefab(obj);
            ContentAddition.AddEffect(obj);
        }
        public static void AddEffectPrefabToContentAddition(GameObject obj)
        {
            if (!obj.GetComponent<NetworkIdentity>()) obj.AddComponent<NetworkIdentity>();
            if (!obj.GetComponent<EffectComponent>()) obj.AddComponent<EffectComponent>();
            ContentAddition.AddEffect(obj);
        }
        public static void CreateNetworkedObjectPrefab(GameObject obj)
        {
            if (!obj.GetComponent<NetworkIdentity>()) obj.AddComponent<NetworkIdentity>();
            PrefabAPI.RegisterNetworkPrefab(obj);
            ContentAddition.AddNetworkedObject(obj);
        }
        public static void CreateNetworkedProjectilePrefab(GameObject obj)
        {
            if (!obj.GetComponent<ProjectileController>()) obj.AddComponent<ProjectileController>();
            if (!obj.GetComponent<ProjectileSimple>()) obj.AddComponent<ProjectileSimple>();
            if (!obj.GetComponent<NetworkIdentity>()) obj.AddComponent<NetworkIdentity>();
            PrefabAPI.RegisterNetworkPrefab(obj);
            ContentAddition.AddProjectile(obj);
        }
        /// <summary>
        /// Loads a prefab from RoR2 addressable assets, clones it without awakening it, applies a modifier function to the clone, then performs a second InstantiateClone operation to freeze the modified version into a new named prefab.
        /// </summary>
        public static GameObject ModifyVanillaPrefab(string addressablePath, string newName, bool shouldNetwork, System.Func<GameObject, GameObject> modifierCallback)
        {
            var origObj = Addressables.LoadAssetAsync<GameObject>(addressablePath)
                .WaitForCompletion()
                .InstantiateClone("Temporary Setup Prefab", false);
            var newObj = modifierCallback(origObj);
            var newObjPrefabified = newObj.InstantiateClone(newName, shouldNetwork);
            GameObject.Destroy(origObj);
            GameObject.Destroy(newObj);
            return newObjPrefabified;
        }

        // Using Nuxlar's Sound Solution, cause im too dum for this
        public static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            ContentAddition.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }
        #endregion

        public static void GetAllComponentNames(GameObject obj)
        {
            foreach (var componenet in obj.GetComponents<Component>())
            {
                Debug.Log(componenet);
            }
        }
    }
}