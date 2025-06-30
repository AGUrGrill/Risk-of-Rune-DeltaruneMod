using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class Kromer : ItemBase<Pipis>
    {
        public override string ItemName => "1 [KROMER]";

        public override string ItemLangTokenName => "KROMER";

        public override string ItemPickupDesc => "DON'T WORRY! FOR OUR [No Money Back Guaranttee]";

        public override string ItemFullDescription => "Does nothing...\n\n\nWhy are you still reading??";

        public override string ItemLore => "Smells like KROMER.";

        public override ItemTier Tier => ItemTier.NoTier;

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
            RecalculateStatsAPI.GetStatCoefficients += KromerEffect;
        }

        private void KromerEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            
        }
    }
}
