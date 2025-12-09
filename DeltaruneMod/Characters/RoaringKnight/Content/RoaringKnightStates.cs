using DeltaruneMod.Characters.RoaringKnight.SkillStates;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeltaruneMod.Characters.RoaringKnight.Content
{
    public class RoaringKnightStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(DarkSlash));
            Modules.Content.AddEntityState(typeof(CrystalBarrage));
            Modules.Content.AddEntityState(typeof(ShadowlessCloak));
        }
    }
}
