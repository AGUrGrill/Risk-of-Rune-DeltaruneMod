using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class BlueRibbon : ItemBase<BlueRibbon>
    {
        public override string ItemName => "Blue Ribbon";

        public override string ItemLangTokenName => "BLUE_RIBBON";

        public override string ItemPickupDesc => "Provides a boost in healing and attack speed.";

        public override string ItemFullDescription => "Gain a 5% to healing and attack speed <style=cStack>(+5% per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => DeltarunePlugin.MainAssets.LoadAsset<Sprite>("blue_ribbon_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.WorldUnique };

        public const float BlueRibbionHealingMult = 0.05f;
        public const float BlueRibbionAttackSpeedMult = 0.05f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (GetCount(sender) > 0)
            {
                args.healthTotalMult += BlueRibbionHealingMult * GetCount(sender);
                args.attackSpeedMultAdd += BlueRibbionAttackSpeedMult * GetCount(sender);
            }
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
