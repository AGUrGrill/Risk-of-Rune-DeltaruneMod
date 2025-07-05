using DeltaruneMod.Items.Tier2;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Lunar
{
    public class ThornRing : ItemBase<ThornRing>
    {
        public override string ItemName => "Thorn Ring";

        public override string ItemLangTokenName => "THORN_RING";

        public override string ItemPickupDesc => "Receive <style=cDeath>pain</style> to become <style=cIsUtility><style=cMono>stronger</style></style>.";

        public override string ItemFullDescription => "Apply <style=cIsUtility>1</style> stack of <style=cIsUtility>frostbite on hit </style><style=cStack>(+1 per stack)</style>" +
            "\nStacks of frostbite cause enemies to <style=cIsUtility>freeze</style>." +
            "\nLose <style=cIsHealth>-5% hp</style> on hit <style=cStack>(-3% hp per stack)</style>." +
            "\nYou cannot lose more than <style=cIsHealth>5% hp</style>.";

        public override string ItemLore => "<style=cShrine>[Angel]</style>, <style=cShrine>[Angel]</style> \nARE YOU LOOKING FOR THE <style=cIsUtility>[Ring]</style>\n OF <style=cDeath>[Thorns]</style> ?";

        public override ItemTier Tier => ItemTier.Lunar;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("thorn_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("thorn_ring_icon.png");

        public static BuffDef frostbite;

        public static bool healthAmputated = false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        // Blacklist from lunar shop
        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += ThornRingEffect;
            RecalculateStatsAPI.GetStatCoefficients += CommRingToThornRing;
        }

        private void CommRingToThornRing(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            int itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                ItemDef commRing = CommRing.instance.ItemDef;
                int commRingItemCount = sender.inventory.GetItemCount(commRing);
                if (commRingItemCount > 0)
                {
                    for (int i = 0; i < commRingItemCount; i++)
                    {
                        sender.inventory.RemoveItem(commRing);
                        sender.inventory.GiveItem(ItemDef);
                    }
                }
                if (!healthAmputated)
                {
                    sender.SetAmputateMaxHealth(sender.maxHealth * 0.3f);
                    healthAmputated = true;
                }
            }
        }

        public void ThornRingEffect(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (!NetworkServer.active) return;

            var attacker = damageInfo.attacker;
            CharacterBody sender = attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();

            int itemCount = GetCount(sender);

            frostbite = DLC2Content.Buffs.Frost;
            frostbite.name = "FrostbiteDebuff";
            frostbite.buffColor = Color.red;
            frostbite.isDebuff = true;

            //float thornDmg = sender.baseMaxHealth * (0.05f + (0.03f * (itemCount-1)));
            //int stacksFrostbite = victimBody.GetBuffCount(frostbite);

            if (sender.inventory && itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    victimBody.AddBuff(frostbite);
                }
            }
            orig(self, damageInfo, victim);
        }
    }
}
