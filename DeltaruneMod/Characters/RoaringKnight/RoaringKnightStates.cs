using DeltaruneMod.Characters.RoaringKnight.SkillStates;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeltaruneMod.Characters.RoaringKnight
{
    public class RoaringKnightStates
    {
        public RoaringKnightStates()
        {
            var darkSlashAdded = false;
            ContentAddition.AddEntityState(typeof(DarkSlash), out darkSlashAdded);
            var shadowlessCloakAdded = false;
            ContentAddition.AddEntityState(typeof(ShadowlessCloak), out shadowlessCloakAdded);
        }

    }
}
