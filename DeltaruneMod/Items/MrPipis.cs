using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class MrPipis : ItemBase<Pipis>
    {
        public override string ItemName => "Mr. Pipis";

        public override string ItemLangTokenName => "MR_PIPIS";

        public override string ItemPickupDesc => "Congrats!! You are a [Big Shot]!!!!";

        public override string ItemFullDescription => "Provides ALL elite buffs!";

        public override string ItemLore => "The one and only'.";

        public override ItemTier Tier => ItemTier.Boss;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("lancer_card.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("lancer_card_icon.png");

        public override void Init()
        {
            CreateLang();
            CreateItem();
            Hooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += MrPipisEffect;
        }

        private void MrPipisEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            
        }
    }
}
