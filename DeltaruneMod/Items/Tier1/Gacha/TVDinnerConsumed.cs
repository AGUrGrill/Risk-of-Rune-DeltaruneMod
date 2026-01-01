using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class TVDinnerConsumed : ItemBase<TVDinnerConsumed>
    {
        public override string ItemName => "TVDinner (Consumed)";

        public override string ItemLangTokenName => "TV_DINNER_USED";

        public override string ItemPickupDesc => "Not very much of a banquet...";

        public override string ItemFullDescription => "Not very much of a banquet...";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tv_dinner_icon_consumed.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.WorldUnique };

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
           
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
