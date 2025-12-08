using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeltaruneMod.Characters.RoaringKnight.SkillStates
{
    public class TestSkill
    {
        public TestSkill()
        {
            // First we must load our survivor's Body prefab. For this tutorial, we are making a skill for Commando
            // If you would like to load a different survivor, you can find the key for their Body prefab at the following link
            // https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html
            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

            // We use LanguageAPI to add strings to the game, in the form of tokens
            // Please note that it is instead recommended that you use a language file.
            // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
            LanguageAPI.Add("COMMANDO_PRIMARY_SIMPLEBULLET_NAME", "Simple Bullet Attack");
            LanguageAPI.Add("COMMANDO_PRIMARY_SIMPLEBULLET_DESCRIPTION", $"Fire a bullet from your right pistol for <style=cIsDamage>300% damage</style>.");

            // Now we must create a SkillDef
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            //Check step 2 for the code of the CustomSkillsTutorial.MyEntityStates.SimpleBulletAttack class
            mySkillDef.activationState = new SerializableEntityStateType(typeof(SimpleBulletAttack));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            // For the skill icon, you will have to load a Sprite from your own AssetBundle
            mySkillDef.icon = null;
            mySkillDef.skillDescriptionToken = "COMMANDO_PRIMARY_SIMPLEBULLET_DESCRIPTION";
            mySkillDef.skillName = "COMMANDO_PRIMARY_SIMPLEBULLET_NAME";
            mySkillDef.skillNameToken = "COMMANDO_PRIMARY_SIMPLEBULLET_NAME";

            // This adds our skilldef. If you don't do this, the skill will not work.
            ContentAddition.AddSkillDef(mySkillDef);

            // Now we add our skill to one of the survivor's skill families
            // You can change component.primary to component.secondary, component.utility and component.special
            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            // If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the existing Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        public class SimpleBulletAttack : BaseSkillState
        {
            public float baseDuration = 0.5f;
            private float duration;

            public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;
            public GameObject tracerEffectPrefab = FireBarrage.tracerEffectPrefab;

            //OnEnter() runs once at the start of the skill
            //All we do here is create a BulletAttack and fire it
            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = this.baseDuration / base.attackSpeedStat;
                Ray aimRay = base.GetAimRay();
                base.StartAimMode(aimRay, 2f, false);
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
                RoR2.Util.PlaySound(FireBarrage.fireBarrageSoundString, base.gameObject);
                base.AddRecoil(-0.6f, 0.6f, -0.6f, 0.6f);
                if (FireBarrage.effectPrefab)
                {
                    EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, base.gameObject, "MuzzleRight", false);
                }

                if (base.isAuthority)
                {
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0f,
                        maxSpread = base.characterBody.spreadBloomAngle,
                        bulletCount = 1U,
                        procCoefficient = 1f,
                        damage = base.characterBody.damage * 3f,
                        force = 3,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        tracerEffectPrefab = this.tracerEffectPrefab,
                        muzzleName = "MuzzleRight",
                        hitEffectPrefab = this.hitEffectPrefab,
                        isCrit = base.RollCrit(),
                        HitEffectNormal = false,
                        stopperMask = LayerIndex.world.mask,
                        smartCollision = true,
                        maxDistance = 300f
                    }.Fire();
                }
            }

            //This method runs once at the end
            //Here, we are doing nothing
            public override void OnExit()
            {
                base.OnExit();
            }

            //FixedUpdate() runs almost every frame of the skill
            //Here, we end the skill once it exceeds its intended duration
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if (base.fixedAge >= this.duration && base.isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Skill;
            }
        }
    }
}
