using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.CharacterAI;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier3
{
    public class GasterMask : ItemBase<GasterMask>
    {
        public override string ItemName => "Mystery Man's Mask";

        public override string ItemLangTokenName => "GASTER_MASK";

        public override string ItemPickupDesc => "Enemies that stay at low hp become corrupted allies.";

        public override string ItemFullDescription => "Enemies gain <style=cIsDamage>corruption</style> when hit, when below <style=cIsDamage>10%</style> hp" +
            "\nenemies become corrupted allies for <style=cIsUtility>5</style> seconds <style=cStack>(+5 seconds per stack)</style>.";

        public override string ItemLore => "\"ARE YOU THERE? ARE WE CONNECTED?\"" +
            "\nThe voice rings through your head, pounding, probing..." +
            "\n\"GO FORWARD, MAKE ALLIES, DON'T <style=cMono>GIVE UP</style>.";

        public override ItemTier Tier => ItemTier.VoidTier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("roaring_blade.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("swoon_effect_icon.png");

        public static BuffDef GhasterCorrupt;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            var itemCount = GetCount(sender);
            var happiestItemCount = sender.inventory.GetItemCount((ItemIndex)77);

            if (itemCount > 0 && happiestItemCount > 0)
            {
                sender.inventory.RemoveItem((ItemIndex)77);
                sender.inventory.GiveItem(ItemDef);
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (!NetworkServer.active) return;

            var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            var victimBody = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(attackerBody);
            var thresholdHP = 0.1f; 

            if (victimBody.healthComponent.health <= victimBody.maxHealth * thresholdHP)
                ConvertEnemy(victimBody, attackerBody);
        }

        private void CreateBuff()
        {
            GhasterCorrupt = new BuffDef();
            GhasterCorrupt.name = "ghaster_corruption";
            GhasterCorrupt.canStack = false;
            GhasterCorrupt.isDebuff = true;
            GhasterCorrupt.iconSprite = ItemIcon;

            ContentAddition.AddBuffDef(GhasterCorrupt);
        }

        private void ConvertEnemy(CharacterBody target, CharacterBody owner)
        {
            if (target.isPlayerControlled || target.bodyFlags.HasFlag(CharacterBody.BodyFlags.Mechanical)) return;
            
            CharacterBody targetCopy = target;
            target.healthComponent.Suicide();

            targetCopy.master.teamIndex = TeamIndex.Void;
            targetCopy.teamComponent.teamIndex = TeamIndex.Void;

            targetCopy.inventory.SetEquipmentIndex(DLC1Content.Elites.Void.eliteEquipmentDef.equipmentIndex);

            BaseAI ai = targetCopy.master.GetComponent<BaseAI>();
            if (ai)
            {
                ai.enemyAttention = 0f;
                ai.ForceAcquireNearestEnemyIfNoCurrentEnemy();
            }

            var conversionTime = 5 + GetCount(owner);
            RoR2.Util.TryToCreateGhost(targetCopy, owner, conversionTime);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            Hooks();
        }
    }
}
