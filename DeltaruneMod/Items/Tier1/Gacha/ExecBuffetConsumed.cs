using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class ExecBuffetConsumed : ItemBase<ExecBuffetConsumed>
    {
        public override string ItemName => "ExecBuffet (Consumed)";

        public override string ItemLangTokenName => "EXEC_BUFFET_USED";

        public override string ItemPickupDesc => "A dinner for executives, just not a good one...";

        public override string ItemFullDescription => "A dinner for executives, just not a good one...";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("exec_buffet_icon_consumed.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        private readonly float healAmount = 100f;

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
