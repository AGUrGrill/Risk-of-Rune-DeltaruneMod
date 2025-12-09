using DeltaruneMod.Characters.Modules;
using DeltaruneMod.Characters.Modules.BaseContent.Characters;
using DeltaruneMod.Characters.RoaringKnight.SkillStates;
using DeltaruneMod.Unlocks;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DeltaruneMod.Characters.RoaringKnight.Content
{
    public class RoaringKnightSurvivor : SurvivorBase<RoaringKnightSurvivor>
    {
        public override string masterName => "RoaringKnightMaster";

        public override string displayPrefabName => "RoaringKnightDisplay";

        public override string survivorTokenPrefix => "ROARING_KNIGHT";

        public override UnlockableDef characterUnlockableDef => RiskOfRuneUnlocks.roaringKnightUnlockableDef;

        public override string assetBundleName => "Empty";

        public override string bodyName => "RaoringKnightBody";

        public override string modelPrefabName => "mdlRoaringKnight";

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = survivorTokenPrefix + "_NAME",
            subtitleNameToken = survivorTokenPrefix + "_SUBTITLE",

            characterPortrait = DeltarunePlugin.MainAssets.LoadAsset<Sprite>("swoon_effect_icon.png").texture,
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override GameObject displayPrefab { get; protected set; }
        public override AssetBundle assetBundle { get; protected set; }
        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }


        public static string ROARING_KNIGHT_PREFIX = "ROARING_KNIGHT";
        GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

        public override void Initialize()
        {
            base.Initialize();
        }

        public void TestInit()
        {
            RoaringKnightStates.Init();
            RoaringKnightTokens.Init();
            InitializeSkills();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            RiskOfRuneUnlocks.Init();

            base.InitializeCharacter();

            //HenryConfig.Init();
            RoaringKnightStates.Init();
            RoaringKnightTokens.Init();

            //HenryAssets.Init(assetBundle);
            //HenryBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            //AdditionalBodySetup();

            //AddHooks();
        }

        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            //HenryAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        public override void InitializeEntityStateMachines()
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(commandoBodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(commandoBodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(commandoBodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(commandoBodyPrefab, "Weapon2");
        }

        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(commandoBodyPrefab);
            //add our own
            //AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            //AddSpecialSkills();
        }

        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }

        public void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(commandoBodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "DarkSlash",
                    survivorTokenPrefix + "_NAME",
                    survivorTokenPrefix + "_DESCRIPTION",
                    DeltarunePlugin.MainAssets.LoadAsset<Sprite>("roaring_blade_icon.png"),
                    new EntityStates.SerializableEntityStateType(typeof(DarkSlash)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef.stepCount = 2;
            primarySkillDef.stepGraceDuration = 0.5f;

            Skills.AddPrimarySkills(commandoBodyPrefab, primarySkillDef);
        }

        public void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(commandoBodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Crystal Barrage",
                skillNameToken = survivorTokenPrefix + "_NAME",
                skillDescriptionToken = survivorTokenPrefix + "_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = DeltarunePlugin.MainAssets.LoadAsset<Sprite>("golden_idol_icon.png"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(CrystalBarrage)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(commandoBodyPrefab, secondarySkillDef);
        }

        public void AddUtilitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(commandoBodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = survivorTokenPrefix + "_NAME",
                skillDescriptionToken = survivorTokenPrefix + "_DESCRIPTION",
                skillIcon = DeltarunePlugin.MainAssets.LoadAsset<Sprite>("swoon_effect_icon.png"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ShadowlessCloak)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(commandoBodyPrefab, utilitySkillDef);
        }
    }
}
