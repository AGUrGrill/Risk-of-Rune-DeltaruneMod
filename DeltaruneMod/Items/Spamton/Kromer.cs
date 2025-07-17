using DeltaruneMod.Items.Yellow;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Spamton
{
    public class Kromer : ItemBase<Kromer>
    {
        public override string ItemName => "1 [KROMER]";

        public override string ItemLangTokenName => "KROMER";

        public override string ItemPickupDesc => "DON'T WORRY! FOR OUR <style=cShrine>[No Money Back Guaranttee]</style>";

        public override string ItemFullDescription => "Does nothing...\n\n\nWhy are you still reading??";

        public override string ItemLore => "Smells like <style=cKeywordName>KROMER</style>.";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("kromer.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("kromer.png");

        public override void Init()
        {
            ItemModel.transform.localScale = new Vector3(2f, 2f, 2f);
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
            var existing = sender.GetComponent<KromerEffectComponenent>();
            if(sender.inventory && GetCount(sender) > 50 & !existing)
            {
                existing = sender.gameObject.AddComponent<KromerEffectComponenent>();
                existing.body = sender;
                existing.enabled = true;
            }
        }
    }

    public class KromerEffectComponenent : CharacterBody.ItemBehavior
    {
        public CharacterBody body;
        private void Start()
        {
            body.master.luck -= 1;
            Debug.Log("Thats too much Kromer... (-1 Luck)");
        }
    }
}
