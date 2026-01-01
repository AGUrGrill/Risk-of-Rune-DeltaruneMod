using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class GingerGuard : ItemBase<GingerGuard>
    {
        public override string ItemName => "Ginger Guard";

        public override string ItemLangTokenName => "GINGER_GUARD";

        public override string ItemPickupDesc => "Provides a boost in armor.";

        public override string ItemFullDescription => "Gain a 5% increase to armor <style=cStack>(+5% per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => DeltarunePlugin.MainAssets.LoadAsset<Sprite>("ginger_guard_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.WorldUnique };

        public const float GingerGuardArmorMult = 0.05f;

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
                args.armorTotalMult += GingerGuardArmorMult * GetCount(sender);
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
