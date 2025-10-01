﻿using BepInEx.Configuration;
using System;
using UnityEngine;


namespace RealismMod
{
    public static class PluginConfig
    {
        //movement
        public static ConfigEntry<bool> EnableMaterialSpeed { get; set; }
        public static ConfigEntry<bool> EnableSlopeSpeed { get; set; }

        //attchment + recoil overhaul
        public static ConfigEntry<bool> EnableZeroShift { get; set; }
        public static ConfigEntry<bool> IncreaseCOI { get; set; }

        //malf changes
        public static ConfigEntry<float> DuraMalfReductionThreshold { get; set; }
        public static ConfigEntry<float> DuraMalfThreshold { get; set; }
        public static ConfigEntry<float> MalfMulti { get; set; }

        //recoil
        public static ConfigEntry<float> AimPunchMulti { get; set; }
        public static ConfigEntry<float> ShotResetDelay { get; set; }
        public static ConfigEntry<float> SwayIntensity { get; set; }
        public static ConfigEntry<float> ProceduralIntensity { get; set; }
        public static ConfigEntry<float> RecoilIntensity { get; set; }
        public static ConfigEntry<float> VertMulti { get; set; }
        public static ConfigEntry<float> PistolVertMulti { get; set; }
        public static ConfigEntry<float> HorzMulti { get; set; }
        public static ConfigEntry<float> DispMulti { get; set; }
        public static ConfigEntry<float> CamMulti { get; set; }
        public static ConfigEntry<float> CamWiggle { get; set; }
        public static ConfigEntry<float> CamReturn { get; set; }
        public static ConfigEntry<bool> EnableAngle { get; set; }
        public static ConfigEntry<float> RecoilAngleMulti { get; set; }
        public static ConfigEntry<float> ConvergenceMulti { get; set; }
        public static ConfigEntry<float> PistolConvergenceMulti { get; set; }
        public static ConfigEntry<float> RecoilDampingMulti { get; set; }
        public static ConfigEntry<float> HandsDampingMulti { get; set; }
        public static ConfigEntry<bool> EnableCrank { get; set; }
        public static ConfigEntry<bool> EnableAdditionalRec { get; set; }
        public static ConfigEntry<float> VisRecoilMulti { get; set; }
        public static ConfigEntry<float> RecoilClimbFactor { get; set; }
        public static ConfigEntry<float> RecoilClimbSpeed { get; set; }
        public static ConfigEntry<float> PistolRecClimbFactor { get; set; }
        public static ConfigEntry<float> RecoilDispersionFactor { get; set; }
        public static ConfigEntry<float> RecoilDispersionSpeed { get; set; }
        public static ConfigEntry<float> AfterRecoilRandomness { get; set; }
        public static ConfigEntry<bool> UseFpsRecoilFactor { get; set; }
        public static ConfigEntry<float> RecoilRandomness { get; set; }
        public static ConfigEntry<bool> EnableMuzzleEffects { get; set; }

        //stat display
        public static ConfigEntry<bool> ShowBalance { get; set; }
        public static ConfigEntry<bool> ShowCamRecoil { get; set; }
        public static ConfigEntry<bool> ShowDispersion { get; set; }
        public static ConfigEntry<bool> ShowRecoilAngle { get; set; }
        public static ConfigEntry<bool> ShowSemiROF { get; set; }

        //reloading
        public static ConfigEntry<float> PistolGlobalAimSpeedModifier { get; set; }
        public static ConfigEntry<float> GlobalAimSpeedModifier { get; set; }
        public static ConfigEntry<float> GlobalReloadSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalFixSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalUBGLReloadMulti { get; set; }
        public static ConfigEntry<float> GlobalRechamberSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalShotgunRackSpeedFactor { get; set; }
        public static ConfigEntry<float> GlobalCheckChamberSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalCheckChamberShotgunSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalCheckChamberPistolSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalCheckAmmoPistolSpeedMulti { get; set; }
        public static ConfigEntry<float> GlobalCheckAmmoMulti { get; set; }
        public static ConfigEntry<float> QuickReloadSpeedMulti { get; set; }
        public static ConfigEntry<float> InternalMagReloadMulti { get; set; }
        public static ConfigEntry<float> GlobalBoltSpeedMulti { get; set; }
        public static ConfigEntry<float> RechamberPistolSpeedMulti { get; set; }

        //audio
        public static ConfigEntry<float> DeafenResetDelay { get; set; }
        public static ConfigEntry<int> HeadsetGain { get; set; }
        public static ConfigEntry<bool> EnableAmbientChanges { get; set; }
        public static ConfigEntry<float> OutdoorAmbientMulti { get; set; }
        public static ConfigEntry<float> IndoorAmbientMulti { get; set; }
        public static ConfigEntry<int> HeadsetNoiseReduction { get; set; }
        public static ConfigEntry<float> GunshotVolume { get; set; }
        public static ConfigEntry<float> PlayerMovementVolume { get; set; }
        public static ConfigEntry<float> NPCMovementVolume { get; set; }
        public static ConfigEntry<float> SharedMovementVolume { get; set; }
        public static ConfigEntry<float> ADSVolume { get; set; }
        public static ConfigEntry<KeyboardShortcut> IncGain { get; set; }
        public static ConfigEntry<KeyboardShortcut> DecGain { get; set; }

        //ballistics
        public static ConfigEntry<float> ArmorDurabilityModifier { get; set; }
        public static ConfigEntry<float> DragModifier { get; set; }
        public static ConfigEntry<bool> EnablePlateChanges { get; set; }
        public static ConfigEntry<float> GlobalDamageModifier { get; set; }
        public static ConfigEntry<bool> EnableBodyHitZones { get; set; }
        public static ConfigEntry<bool> EnableHitSounds { get; set; }
        public static ConfigEntry<float> FleshHitSoundMulti { get; set; }
        public static ConfigEntry<float> ArmorCloseHitSoundMulti { get; set; }
        public static ConfigEntry<float> ArmorFarHitSoundMulti { get; set; }
        public static ConfigEntry<bool> CanDisarmPlayer { get; set; }
        public static ConfigEntry<bool> CanDisarmBot { get; set; }
        public static ConfigEntry<float> DisarmBaseChance { get; set; }
        public static ConfigEntry<bool> CanFellPlayer { get; set; }
        public static ConfigEntry<bool> CanFellBot { get; set; }
        public static ConfigEntry<float> FallBaseChance { get; set; }
        public static ConfigEntry<bool> EnableAmmoStats { get; set; }
        public static ConfigEntry<float> RagdollForceModifier { get; set; }
        public static ConfigEntry<bool> EnableRagdollFix { get; set; }

        //hazard zones
        public static ConfigEntry<bool> ShowRadEffects { get; set; }
        public static ConfigEntry<bool> ShowGasEffects { get; set; }
        public static ConfigEntry<float> DeviceVolume { get; set; }
        public static ConfigEntry<float> GasMaskBreathVolume { get; set; }
        public static ConfigEntry<bool> EnableTrueHazardRates { get; set; }
        public static ConfigEntry<KeyboardShortcut> MuteGeigerKey { get; set; }
        public static ConfigEntry<KeyboardShortcut> MuteGasAnalyserKey { get; set; }
        public static ConfigEntry<KeyboardShortcut> ToggleGasMaskKey { get; set; }

        //medical
        public static ConfigEntry<bool> EnableMedNotes { get; set; }
        public static ConfigEntry<bool> ResourceRateChanges { get; set; }
        public static ConfigEntry<bool> PassiveRegen { get; set; }
        public static ConfigEntry<float> EnergyRateMulti { get; set; }
        public static ConfigEntry<float> HydrationRateMulti { get; set; }
        public static ConfigEntry<bool> GearBlocksHeal { get; set; }
        public static ConfigEntry<bool> GearBlocksEat { get; set; }
        public static ConfigEntry<bool> EnableAdrenaline { get; set; }
        public static ConfigEntry<bool> EnableTrnqtEffect { get; set; }
        public static ConfigEntry<bool> EnableHealthEffects { get; set; }
        public static ConfigEntry<KeyboardShortcut> DropGearKeybind { get; set; }

        //stances
        public static ConfigEntry<KeyboardShortcut> ActiveAimKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> LowReadyKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> HighReadyKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> ShortStockKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> CycleStancesKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> PatrolKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> MeleeKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> StanceWheelComboKeyBind { get; set; }
        public static ConfigEntry<bool> EnableAnimationFixes { get; set; }
        public static ConfigEntry<bool> ModifyBSGCollision { get; set; }
        public static ConfigEntry<bool> OverrideCollision { get; set; }
        public static ConfigEntry<bool> OverrideMounting { get; set; }
        public static ConfigEntry<bool> UseMouseWheelStance { get; set; }
        public static ConfigEntry<bool> UseMouseWheelPlusKey { get; set; }
        public static ConfigEntry<bool> EnableFSPatch { get; set; }
        public static ConfigEntry<bool> EnableNVGPatch { get; set; }
        public static ConfigEntry<bool> EnableMountUI { get; set; }
        public static ConfigEntry<bool> ToggleActiveAim { get; set; }
        public static ConfigEntry<bool> ActiveAimReload { get; set; }
        public static ConfigEntry<bool> EnableAltPistol { get; set; }
        public static ConfigEntry<bool> EnableAltRifle { get; set; }
        public static ConfigEntry<bool> EnableIdleStamDrain { get; set; }
        public static ConfigEntry<float> IdleStamDrainModi { get; set; }
        public static ConfigEntry<bool> EnableStanceStamChanges { get; set; }
        public static ConfigEntry<bool> EnableTacSprint { get; set; }
        public static ConfigEntry<bool> BlockFiring { get; set; }
        public static ConfigEntry<bool> RememberStanceFiring { get; set; }
        public static ConfigEntry<bool> RememberStanceItem { get; set; }
        public static ConfigEntry<bool> EnableExtraProcEffects { get; set; }
        public static ConfigEntry<bool> EnableSprintPenalty { get; set; }
        public static ConfigEntry<bool> EnableMouseSensPenalty { get; set; }
        public static ConfigEntry<float> LeftShoulderOffset { get; set; }
        public static ConfigEntry<float> StanceRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> StanceTransitionSpeedMulti { get; set; }
        public static ConfigEntry<float> ThirdPersonPositionSpeed { get; set; }
        public static ConfigEntry<float> ThirdPersonRotationSpeed { get; set; }


        public static ConfigEntry<float> ActiveAimSpeedMulti { get; set; }
        public static ConfigEntry<float> ActiveAimResetSpeedMulti { get; set; }
        public static ConfigEntry<float> ActiveAimRotationMulti { get; set; }
        public static ConfigEntry<float> PistolRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> HighReadyRotationMulti { get; set; }
        public static ConfigEntry<float> LowReadyRotationMulti { get; set; }
        public static ConfigEntry<float> ShortStockRotationMulti { get; set; }
        public static ConfigEntry<float> ShortStockAdditionalRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> ActiveAimAdditionalRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> HighReadyAdditionalRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> LowReadyAdditionalRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> PistolAdditionalRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> ActiveAimResetRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> PistolResetRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> HighReadyResetRotationMulti { get; set; }
        public static ConfigEntry<float> LowReadyResetRotationMulti { get; set; }
        public static ConfigEntry<float> ShortStockResetRotationSpeedMulti { get; set; }
        public static ConfigEntry<float> HighReadySpeedMulti { get; set; }
        public static ConfigEntry<float> HighReadyResetSpeedMulti { get; set; }
        public static ConfigEntry<float> LowReadySpeedMulti { get; set; }
        public static ConfigEntry<float> LowReadyResetSpeedMulti { get; set; }
        public static ConfigEntry<float> PistolPosSpeedMulti { get; set; }
        public static ConfigEntry<float> PistolPosResetSpeedMulti { get; set; }
        public static ConfigEntry<float> ShortStockSpeedMulti { get; set; }
        public static ConfigEntry<float> ShortStockResetSpeedMulti { get; set; }

        public static ConfigEntry<Vector3> WeapOffset { get; set; }
        public static ConfigEntry<Vector3> ActiveAimRotation { get; set; }
        public static ConfigEntry<Vector3> PistolRotation { get; set; }
        public static ConfigEntry<Vector3> ActiveThirdPersonPosition { get; set; }
        public static ConfigEntry<Vector3> ActiveThirdPersonRotation { get; set; }
        public static ConfigEntry<Vector3> ActiveAimAdditionalRotation { get; set; }
        public static ConfigEntry<Vector3> ActiveAimResetRotation { get; set; }
        public static ConfigEntry<Vector3> HighReadyThirdPersonPosition { get; set; }
        public static ConfigEntry<Vector3> HighReadyThirdPersonRotation { get; set; }
        public static ConfigEntry<Vector3> HighReadyAdditionalRotation { get; set; }
        public static ConfigEntry<Vector3> HighReadyResetRotation { get; set; }
        public static ConfigEntry<Vector3> LowReadyThirdPersonPosition { get; set; }
        public static ConfigEntry<Vector3> LowReadyThirdPersonRotation { get; set; }
        public static ConfigEntry<Vector3> LowReadyAdditionalRotation { get; set; }
        public static ConfigEntry<Vector3> LowReadyResetRotation { get; set; }
        public static ConfigEntry<Vector3> PistolAdditionalRotation { get; set; }
        public static ConfigEntry<Vector3> PistolResetRotation { get; set; }
        public static ConfigEntry<Vector3> ShortStockAdditionalRotation { get; set; }
        public static ConfigEntry<Vector3> ShortStockResetRotation { get; set; }
        public static ConfigEntry<Vector3> PistolThirdPersonPosition { get; set; }
        public static ConfigEntry<Vector3> PistolThirdPersonRotation { get; set; }
        public static ConfigEntry<Vector3> PistolOffset { get; set; }
        public static ConfigEntry<Vector3> ActiveAimOffset { get; set; }
        public static ConfigEntry<Vector3> LowReadyOffset { get; set; }
        public static ConfigEntry<Vector3> LowReadyRotation { get; set; }
        public static ConfigEntry<Vector3> HighReadyOffset { get; set; }
        public static ConfigEntry<Vector3> HighReadyRotation { get; set; }
        public static ConfigEntry<Vector3> ShortStockThirdPersonPosition { get; set; }
        public static ConfigEntry<Vector3> ShortStockThirdPersonRotation { get; set; }
        public static ConfigEntry<Vector3> ShortStockOffset { get; set; }
        public static ConfigEntry<Vector3> ShortStockRotation { get; set; }
        public static ConfigEntry<Vector3> ShortStockReadyOffset { get; set; }
        public static ConfigEntry<Vector3> ShortStockReadyRotation { get; set; }

        //dev config options
        public static ConfigEntry<bool> DevMode { get; set; }
        public static ConfigEntry<bool> ZoneDebug { get; set; }
        public static ConfigEntry<string> TargetZone { get; set; }
        public static ConfigEntry<bool> EnableGeneralLogging { get; set; }
        public static ConfigEntry<bool> EnableMedicalLogging { get; set; }
        public static ConfigEntry<bool> EnableReloadLogging { get; set; }
        public static ConfigEntry<bool> EnablePWALogging { get; set; }
        public static ConfigEntry<bool> EnableRecoilLogging { get; set; }
        public static ConfigEntry<bool> EnableBallisticsLogging { get; set; }
        public static ConfigEntry<float> test1 { get; set; }
        public static ConfigEntry<float> test2 { get; set; }
        public static ConfigEntry<float> test3 { get; set; }
        public static ConfigEntry<float> test4 { get; set; }
        public static ConfigEntry<float> test5 { get; set; }
        public static ConfigEntry<float> test6 { get; set; }
        public static ConfigEntry<float> test7 { get; set; }
        public static ConfigEntry<float> test8 { get; set; }
        public static ConfigEntry<float> test9 { get; set; }
        public static ConfigEntry<float> test10 { get; set; }
        public static ConfigEntry<KeyboardShortcut> AddEffectKeybind { get; set; }
        public static ConfigEntry<KeyboardShortcut> AddZone { get; set; }
        public static ConfigEntry<int> AddEffectBodyPart { get; set; }
        public static ConfigEntry<string> AddEffectType { get; set; }

        public static void InitConfigBindings(ConfigFile config)
        {
            string testing = ".0. Testing";
            string miscSettings = ".1. Misc. Settings.";
            string ballSettings = ".2. Ballistics Settings.";
            string recoilSettings = ".3. Recoil Settings.";
            string advancedRecoilSettings = ".4. Advanced Recoil Settings.";
            string statSettings = ".5. Stat Display Settings.";
            string waponSettings = ".6. Weapon Settings.";
            string healthSettings = ".7. Health and Meds Settings.";
            string zoneSettings = ".8. Hazard Zone Settings.";
            string moveSettings = ".9. Movement Settings.";
            string deafSettings = ".10. Deafening and Audio.";
            string speed = "11. Weapon Speed Modifiers.";
            string weapAimAndPos = "12. Weapon Stances And Position.";
            string stanceBinds = "13. Weapon Stances Keybinds.";
            string activeAim = "14. Active Aim.";
            string highReady = "15. High Ready.";
            string lowReady = "16. Low Ready.";
            string pistol = "17. Pistol Position And Stance.";
            string shortStock = "18. Short-Stocking.";
            string thirdPerson = "19. Third Person Animations.";

            test1 = config.Bind<float>(testing, "test 1", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 170, IsAdvanced = true, Browsable = true }));
            test2 = config.Bind<float>(testing, "test 2", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 160, IsAdvanced = true, Browsable = true }));
            test3 = config.Bind<float>(testing, "test 3", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 150, IsAdvanced = true, Browsable = true }));
            test4 = config.Bind<float>(testing, "test 4", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 140, IsAdvanced = true, Browsable = true }));
            test5 = config.Bind<float>(testing, "test 5", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 130, IsAdvanced = true, Browsable = true }));
            test6 = config.Bind<float>(testing, "test 6", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 120, IsAdvanced = true, Browsable = true }));
            test7 = config.Bind<float>(testing, "test 7", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 110, IsAdvanced = true, Browsable = true }));
            test8 = config.Bind<float>(testing, "test 8", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true, Browsable = true }));
            test9 = config.Bind<float>(testing, "test 9", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 90, IsAdvanced = true, Browsable = true }));
            test10 = config.Bind<float>(testing, "test 10", 1f, new ConfigDescription("", new AcceptableValueRange<float>(-5000f, 5000f), new ConfigurationManagerAttributes { Order = 80, IsAdvanced = true, Browsable = true }));
            AddZone = config.Bind(testing, "Create Debug Zone", new KeyboardShortcut(KeyCode.None), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 70, IsAdvanced = true, Browsable = true }));
            TargetZone = config.Bind<string>(testing, "TargetZone", "", new ConfigDescription("DebugZone", null, new ConfigurationManagerAttributes { Order = 65, IsAdvanced = true, Browsable = true }));
            AddEffectType = config.Bind<string>(testing, "Effect Type", "", new ConfigDescription("HeavyBleeding, LightBleeding, Fracture, removeHP, addHP.", null, new ConfigurationManagerAttributes { Order = 60, IsAdvanced = true, Browsable = true }));
            AddEffectBodyPart = config.Bind<int>(testing, "Body Part Index", 1, new ConfigDescription("Head = 0, Chest = 1, Stomach = 2, Letft Arm, Right Arm, Left Leg, Right Leg, Common (whole body)", null, new ConfigurationManagerAttributes { Order = 50, IsAdvanced = true, Browsable = true }));
            AddEffectKeybind = config.Bind(testing, "Add Effect Keybind", new KeyboardShortcut(KeyCode.None), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 40, IsAdvanced = true, Browsable = true }));
            ZoneDebug = config.Bind<bool>(testing, "Enable Zone Debug", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 30, IsAdvanced = true, Browsable = true }));
            DevMode = config.Bind<bool>(testing, "Enable Dev Mode", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 29, IsAdvanced = true, Browsable = true }));
            EnableBallisticsLogging = config.Bind<bool>(testing, "Enable Ballistics Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 20, IsAdvanced = true, Browsable = true }));
            EnableGeneralLogging = config.Bind<bool>(testing, "Enable General Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true, Browsable = true }));
            EnableMedicalLogging = config.Bind<bool>(testing, "Enable Medical Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true, Browsable = true }));
            EnableReloadLogging = config.Bind<bool>(testing, "Enable Reload Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true, Browsable = true }));
            EnablePWALogging = config.Bind<bool>(testing, "Enable PWA Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true, Browsable = true }));
            EnableRecoilLogging = config.Bind<bool>(testing, "Enable Recoil Logging", false, new ConfigDescription("Enables Logging For Debug And Dev", null, new ConfigurationManagerAttributes { Order = 10, IsAdvanced = true, Browsable = true }));

            RecoilIntensity = config.Bind<float>(recoilSettings, "Recoil Intensity", 1.75f, new ConfigDescription("Changes The Overall Intenisty Of Recoil. This Will Increase/Decrease Horizontal Recoil, Dispersion, Vertical Recoil. Does Not Affect Recoil Climb Much, Mostly Spread And Visual.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 50, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            VertMulti = config.Bind<float>(recoilSettings, "Rifle Vertical Recoil Multi.", 1.05f, new ConfigDescription("Up/Down. Will Also Increase Recoil Climb.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 40, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            PistolVertMulti = config.Bind<float>(recoilSettings, "Pistol Vertical Recoil Multi.", 1.5f, new ConfigDescription("Up/Down. Will Also Increase Recoil Climb.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 40, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            HorzMulti = config.Bind<float>(recoilSettings, "Horizontal Recoil Multi", 1.0f, new ConfigDescription("Forward/Back. Will Also Increase Weapon Shake While Firing.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            DispMulti = config.Bind<float>(recoilSettings, "Dispersion Recoil Multi", 1.0f, new ConfigDescription("Spread. Will Also Increase S-Pattern Size.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            CamMulti = config.Bind<float>(recoilSettings, "Camera Recoil Multi", 0.85f, new ConfigDescription("Visual Camera Recoil.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            EnableAngle = config.Bind<bool>(recoilSettings, "Enable Recoil Angle", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Weapons Will Recoil At Different Angles, And Weight Out Front Will Make The Angle More Steep. If Disabled All Recoil Will Be At 90 Degrees.", null, new ConfigurationManagerAttributes { Order = 7, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilAngleMulti = config.Bind<float>(recoilSettings, "Recoil Angle Multi", 1.0f, new ConfigDescription("Multiplier For Recoil Angle, Lower = Steeper Angle.", new AcceptableValueRange<float>(0.8f, 1.2f), new ConfigurationManagerAttributes { Order = 5, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ConvergenceMulti = config.Bind<float>(recoilSettings, "Rifle Convergence Multi", 0.6f, new ConfigDescription("AKA Auto-Compensation. Higher = Snappier Recoil, Faster Reset And Tighter Recoil Pattern.", new AcceptableValueRange<float>(0f, 40f), new ConfigurationManagerAttributes { Order = 2, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            PistolConvergenceMulti = config.Bind<float>(recoilSettings, "Pistol Convergence Multi", 1.3f, new ConfigDescription("AKA Auto-Compensation. Higher = Snappier Recoil, Faster Reset And Tighter Recoil Pattern.", new AcceptableValueRange<float>(0f, 40f), new ConfigurationManagerAttributes { Order = 1, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            UseFpsRecoilFactor = config.Bind<bool>(advancedRecoilSettings, "Use FPS Recoil Factor", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Factors in current FPS to keep recoil climb consistent", null, new ConfigurationManagerAttributes { Order = 142, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            AfterRecoilRandomness = config.Bind<float>(advancedRecoilSettings, "Reset Recoil Randomness Multi", 1.15f, new ConfigDescription("Higher = More Deviation From Point Of Aim After Firing", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 140, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilRandomness = config.Bind<float>(advancedRecoilSettings, "Recoil Randomness", 2.8f, new ConfigDescription("Higher = Recoil Bounces Around More, More Erratic Recoil Pattern", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 135, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            CamReturn = config.Bind<float>(advancedRecoilSettings, "Camera Recoil Speed", 0.05f, new ConfigDescription("Higher = More Faster Camera Recoil", new AcceptableValueRange<float>(0f, 0.5f), new ConfigurationManagerAttributes { Order = 132, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            CamWiggle = config.Bind<float>(advancedRecoilSettings, "Camera Recoil Wiggle", 0.81f, new ConfigDescription("Higher = More Camera Wiggle", new AcceptableValueRange<float>(0f, 0.9f), new ConfigurationManagerAttributes { Order = 130, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            EnableAdditionalRec = config.Bind<bool>(advancedRecoilSettings, "Enable Additional Visual Recoil", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Enables Additonal Visual Recoil Elements. Makes The Weapon Visually Move More In New Directions While Firing, Doesn't Have A Significant Effect On Spread.", null, new ConfigurationManagerAttributes { Order = 120, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            VisRecoilMulti = config.Bind<float>(advancedRecoilSettings, "Visual Recoil Multi", 1f, new ConfigDescription("Multi For All Of The Mod's Visual Recoil Elements, Makes The Weapon Vibrate More While Firing. Visual Recoil Is Affected By Weapon Stats.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 110, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilClimbSpeed = config.Bind<float>(advancedRecoilSettings, "Recoil Climb Speed Multi", 4f, new ConfigDescription("How Fast The Recoil CLimb Is, Can Make It Feel Smoother Or Snappier.", new AcceptableValueRange<float>(0.1f, 20f), new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilClimbFactor = config.Bind<float>(advancedRecoilSettings, "Recoil Climb Multi", 4f, new ConfigDescription("Multiplier For How Much Non-Pistols Climbs Vertically Per Shot. Weapon's Vertical Recoil Stat Increases This.", new AcceptableValueRange<float>(0f, 50f), new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            PistolRecClimbFactor = config.Bind<float>(advancedRecoilSettings, "Pistol Recoil Climb Multi", 0.55f, new ConfigDescription("Multiplier For How Much Pistols Vertically Per Shot. Weapon's Vertical Recoil Stat Increases This.", new AcceptableValueRange<float>(0f, 50f), new ConfigurationManagerAttributes { Order = 29, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilDispersionFactor = config.Bind<float>(advancedRecoilSettings, "S-Pattern Multi.", 1.2f, new ConfigDescription("Increases The Size The Classic S Pattern. Weapon's Dispersion Stat Increases This.", new AcceptableValueRange<float>(0f, 50f), new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilDispersionSpeed = config.Bind<float>(advancedRecoilSettings, "S-Pattern Speed Multi", 3f, new ConfigDescription("Increases The Speed At Which Recoil Makes The Classic S Pattern.", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ShotResetDelay = config.Bind<float>(advancedRecoilSettings, "Reset Delay", 0.14f, new ConfigDescription("The Time In Seconds That Has To Be Elapsed Before Firing Is Considered Over, Recoil Will Not Reset Until It Is Over.", new AcceptableValueRange<float>(-0.1f, 0.5f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 4, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            EnableCrank = config.Bind<bool>(advancedRecoilSettings, "Rearward Recoil", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Makes Recoil Go Towards Player's Shoulder Instead Of Forward.", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 3, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            HandsDampingMulti = config.Bind<float>(advancedRecoilSettings, "Rearward Recoil Wiggle Multi", 1f, new ConfigDescription("The Amount Of Rearward Wiggle After Firing.", new AcceptableValueRange<float>(0.1f, 1.5f), new ConfigurationManagerAttributes { Order = 2, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RecoilDampingMulti = config.Bind<float>(advancedRecoilSettings, "Vertical Recoil Wiggle Multi", 1f, new ConfigDescription("The Amount Of Vertical Wiggle After Firing.", new AcceptableValueRange<float>(0.1f, 1.5f), new ConfigurationManagerAttributes { Order = 1, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            EnableMaterialSpeed = config.Bind<bool>(moveSettings, "Enable Ground Material Speed Modifier", Plugin.ServerConfig.movement_changes, new ConfigDescription("Enables Movement Speed Being Affected By Ground Material (Concrete, Grass, Metal, Glass Etc.)", null, new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.movement_changes }));
            EnableSlopeSpeed = config.Bind<bool>(moveSettings, "Enable Ground Slope Speed Modifier", false, new ConfigDescription("Enables Slopes Slowing Down Movement. Can Cause Random Speed Slowdowns In Some Small Spots Due To BSG's Bad Map Geometry.", null, new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.movement_changes }));
            EnableSprintPenalty = config.Bind<bool>(moveSettings, "Enable Sprint Aim Penalties", Plugin.ServerConfig.movement_changes, new ConfigDescription("ADS Out Of Sprint Has A Short Delay, Reduced Aim Speed And Increased Sway. The Longer You Sprint The Bigger The Penalty.", null, new ConfigurationManagerAttributes { Order = 5, Browsable = Plugin.ServerConfig.enable_stances }));

            DeviceVolume = config.Bind<float>(zoneSettings, "Device Volume", 0.3f, new ConfigDescription("Volume Modifier For Geiger And Gas Analyser.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            GasMaskBreathVolume = config.Bind<float>(zoneSettings, "Gas Mask Breath Volume", 0.3f, new ConfigDescription("Volume Modifier For Gas Mask SFX.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            EnableTrueHazardRates = config.Bind<bool>(zoneSettings, "Display True Hazard Rates", false, new ConfigDescription("Enable To Show The 'True' Hazard Rate, I.E Not Factoring Meds Or Gas Mask.", null, new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            ShowRadEffects = config.Bind<bool>(zoneSettings, "Visualize Radiation", Plugin.ServerConfig.enable_hazard_zones, new ConfigDescription("Radiation Causes Visual Noise, The Strength Of Which Depends On Current Radiation Rate And Total Radiation Poisoning.", null, new ConfigurationManagerAttributes { Order = 40, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            ShowGasEffects = config.Bind<bool>(zoneSettings, "Visualize Gas", Plugin.ServerConfig.enable_hazard_zones, new ConfigDescription("Gas Will Become Visible", null, new ConfigurationManagerAttributes { Order = 40, Browsable = Plugin.ServerConfig.enable_hazard_zones })); MuteGasAnalyserKey = config.Bind(zoneSettings, "Mute Gas Analysed Key", new KeyboardShortcut(KeyCode.M, new[] { KeyCode.LeftControl }), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 50, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            MuteGeigerKey = config.Bind(zoneSettings, "Mute Geiger Key", new KeyboardShortcut(KeyCode.M, new[] { KeyCode.LeftAlt }), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 60, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            MuteGasAnalyserKey = config.Bind(zoneSettings, "Mute Gas Analysed Key", new KeyboardShortcut(KeyCode.M, new[] { KeyCode.LeftControl }), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 70, Browsable = Plugin.ServerConfig.enable_hazard_zones }));
            ToggleGasMaskKey = config.Bind(zoneSettings, "Toggle Equip Gas Mask", new KeyboardShortcut(KeyCode.Z, new[] { KeyCode.LeftControl }), new ConfigDescription("Will Equip Gas Mask From Tac Rig, Armband (Used By Belt Mods), Or Pockets/Special Slots. When Un-Equiping It Will Try To Move Back To Original Slot.", null, new ConfigurationManagerAttributes { Order = 80, Browsable = Plugin.ServerConfig.enable_hazard_zones }));

            EnableMedNotes = config.Bind<bool>(healthSettings, "Medical Notifications", Plugin.ServerConfig.med_changes, new ConfigDescription("Enables Notifications For Medical Status Effects, Healing Etc..", null, new ConfigurationManagerAttributes { Order = 130, Browsable = Plugin.ServerConfig.med_changes }));
            ResourceRateChanges = config.Bind<bool>(healthSettings, "Enable Hydration/Energy Loss Rate Changes", Plugin.ServerConfig.med_changes, new ConfigDescription("Enables Changes To How Hydration And Energy Loss Rates Are Calculated. They Are Increased By Injuries, Drug Use, Sprinting And Weight.", null, new ConfigurationManagerAttributes { Order = 120, Browsable = Plugin.ServerConfig.med_changes }));
            PassiveRegen = config.Bind<bool>(healthSettings, "Enable Passive Regen", Plugin.ServerConfig.med_changes, new ConfigDescription("Enables Regen Under Certain Conditions, And If THe Player Has Not Taken Damage For Some Time", null, new ConfigurationManagerAttributes { Order = 115, Browsable = Plugin.ServerConfig.med_changes }));
            HydrationRateMulti = config.Bind<float>(healthSettings, "Hydration Drain Rate Multi.", 0.5f, new ConfigDescription("Lower = Less Drain", new AcceptableValueRange<float>(0.1f, 1.5f), new ConfigurationManagerAttributes { Order = 110, Browsable = Plugin.ServerConfig.med_changes }));
            EnergyRateMulti = config.Bind<float>(healthSettings, "Energy Drain Rate Multi.", 0.3f, new ConfigDescription("Lower = Less Drain", new AcceptableValueRange<float>(0.1f, 1.5f), new ConfigurationManagerAttributes { Order = 100, Browsable = Plugin.ServerConfig.med_changes }));
            EnableTrnqtEffect = config.Bind<bool>(healthSettings, "Enable Tourniquet Effect", Plugin.ServerConfig.med_changes, new ConfigDescription("Tourniquet Will Drain HP Of The Limb They Are Applied To.", null, new ConfigurationManagerAttributes { Order = 90, Browsable = Plugin.ServerConfig.med_changes }));
            GearBlocksEat = config.Bind<bool>(healthSettings, "Gear Blocks Consumption", Plugin.ServerConfig.med_changes, new ConfigDescription("Gear Blocks Eating & Drinking. This Includes Some Masks & NVGs & Faceshields That Are Toggled On.", null, new ConfigurationManagerAttributes { Order = 80, Browsable = Plugin.ServerConfig.med_changes }));
            GearBlocksHeal = config.Bind<bool>(healthSettings, "Gear Blocks Healing", false, new ConfigDescription("Gear Blocks Use Of Meds If The Wound Is Covered By It.", null, new ConfigurationManagerAttributes { Order = 70, Browsable = Plugin.ServerConfig.med_changes }));
            EnableAdrenaline = config.Bind<bool>(healthSettings, "Adrenaline", Plugin.ServerConfig.med_changes, new ConfigDescription("If The Player Is Shot or Shot At They Will Get A Painkiller Effect, As Well As Tunnel Vision and Tremors. The Duration And Strength Of These Effects Are Determined By The Stress Resistence Skill.", null, new ConfigurationManagerAttributes { Order = 55, Browsable = Plugin.ServerConfig.med_changes }));
            DropGearKeybind = config.Bind(healthSettings, "Remove Gear Keybind (Double Press)", new KeyboardShortcut(KeyCode.P), new ConfigDescription("Removes Any Gear That Is Blocking The Healing Of A Wound, It's A Double Press Like Bag Keybind Is.", null, new ConfigurationManagerAttributes { Order = 50, Browsable = Plugin.ServerConfig.med_changes }));

            EnableFSPatch = config.Bind<bool>(miscSettings, "Enable Faceshield Patch", Plugin.ServerConfig.enable_stances, new ConfigDescription("Faceshields Block ADS Unless The Specfic Stock/Weapon/Faceshield Allows It.", null, new ConfigurationManagerAttributes { Order = 4, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableNVGPatch = config.Bind<bool>(miscSettings, "Enable NVG/Thermal ADS Patch", Plugin.ServerConfig.enable_stances, new ConfigDescription("Magnified Optics Block ADS When Using NVGs. Can't Aim With Sights When Using Thermal Goggles.", null, new ConfigurationManagerAttributes { Order = 5, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableMouseSensPenalty = config.Bind<bool>(miscSettings, "Enable Weight Mouse Sensitivity Penalty", Plugin.ServerConfig.gear_weight, new ConfigDescription("Instead Of Using Gear Mouse Sens Penalty Stats, It Is Calculated Based On The Gear + Content's Weight As Modified By The Comfort Stat.", null, new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.gear_weight }));
            EnableZeroShift = config.Bind<bool>(miscSettings, "Enable Zero Shift", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Sights Simulate Losing Zero While Firing. The Reticle Has A Chance To Move Off Target. The Chance Is Determined By The Scope And Its Mount's Accuracy Stat, And The Weapon's Recoil. High Quality Scopes And Mounts Won't Lose Zero. SCAR-H Has Worse Zero-Shift.", null, new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            EnableAmmoStats = config.Bind<bool>(ballSettings, "Display Ammo Stats", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("Requiures Restart.", null, new ConfigurationManagerAttributes { Order = 160, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ArmorDurabilityModifier = config.Bind<float>(ballSettings, "Armor Durability Loss Modifier", 1.25f, new ConfigDescription("Modified Armor Durabiltiy Loss Per Shot", new AcceptableValueRange<float>(0.25f, 2f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 151, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            DragModifier = config.Bind<float>(ballSettings, "Ballistic Coefficient Modifier", 1.25f, new ConfigDescription("Determines The Amount Of Drag On Projectiles. Higher Value = Slower Flight Time And More Drop.", new AcceptableValueRange<float>(0.5f, 5f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 150, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            GlobalDamageModifier = config.Bind<float>(ballSettings, "Global Damage Modifier", 1f, new ConfigDescription("Lower = Less Damage Received (Except Head) For Bots And Player.", new AcceptableValueRange<float>(0.1f, 2f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 140, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            EnablePlateChanges = config.Bind<bool>(ballSettings, "Enable Armor Plate Hitbox Changes", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("Reduces The Size Of Armor Plate Hitboxes To Be Closer To Real Life, And Closer To How They Were When First Implemented.", null, new ConfigurationManagerAttributes { Order = 130, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            EnableBodyHitZones = config.Bind<bool>(ballSettings, "Enable Body Hit Zones", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("Divides Body Into A, C and D Hit Zones Like On IPSC Targets. In Addtion, There Are Upper Arm, Forearm, Thigh, Calf, Neck, Spine And Heart Hit Zones. Each Zone Modifies Damage And Bleed Chance. ", null, new ConfigurationManagerAttributes { Order = 120, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            EnableHitSounds = config.Bind<bool>(ballSettings, "Enable Hit Sounds", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("Enables Additional Sounds To Be Played When Hitting The New Body Zones And Armor Hit Sounds By Material.", null, new ConfigurationManagerAttributes { Order = 110, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            FleshHitSoundMulti = config.Bind<float>(ballSettings, "Flesh Hit Sound Multi", 1f, new ConfigDescription("Raises/Lowers New Hit Sounds Volume.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 100, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            ArmorCloseHitSoundMulti = config.Bind<float>(ballSettings, "Close Armor Hit Sound Multi", 1f, new ConfigDescription("Raises/Lowers New Hit Sounds Volume.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 90, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            ArmorFarHitSoundMulti = config.Bind<float>(ballSettings, "Distant Armor Hit Sound Mutli", 1f, new ConfigDescription("Raises/Lowers New Hit Sounds Volume.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 80, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            EnableRagdollFix = config.Bind<bool>(ballSettings, "Enable Ragdoll Fix (Experimental)", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("Requiures Restart. Enables Fix For Ragdolls Flying Into The Stratosphere.", null, new ConfigurationManagerAttributes { Order = 70, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            RagdollForceModifier = config.Bind<float>(ballSettings, "Ragdoll Force Modifier", 0.01f, new ConfigDescription("Requires Ragdoll Fix To Be Enabled.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = 60, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            DisarmBaseChance = config.Bind<float>(ballSettings, "Disarm Base Chance.", 1f, new ConfigDescription("The Base Chance To Be Disarmed. 1 = 1% Chance. This Value Is Increased By The Bullet's Kinetic Energy, Reduced By Armor Armor If Hit, And Doubled If Forearm Is Hit.", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, IsAdvanced = true, Order = 50, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            FallBaseChance = config.Bind<float>(ballSettings, "Fall Base Chance", 20f, new ConfigDescription("The Base Chance To Toggle Prone If Shot In Leg. 1 = 1% Chance. This Value Is Increased By The Bullet's Kinetic Energy And Doubled If Calf Is Hit.", new AcceptableValueRange<float>(0f, 100f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, IsAdvanced = true, Order = 40, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            CanFellBot = config.Bind<bool>(ballSettings, "Enable Bot Knockdown", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("If Hit In The Leg And The Leg Has/Will Have 0 HP, There Is A Chance That Prone Will Be Toggled. Chance Is Modified By Bullet Kinetic EnergyAnd Doubled If Calf Is Hit.", null, new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            CanFellPlayer = config.Bind<bool>(ballSettings, "Enable Player Knockdown", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("If Hit In The Leg And The Leg Has/Will Have 0 HP, There Is A Chance That Prone Will Be Toggled. Chance Is Modified By Bullet Kinetic Energy And Doubled If Calf Is Hit.", null, new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            CanDisarmBot = config.Bind<bool>(ballSettings, "Can Disarm Bot.", false, new ConfigDescription("If Hit In The Arms, There Is A Chance That The Currently Equipped Weapon Will Be Dropped. Chance Is Modified By Bullet Kinetic Energy And Reduced If Hit Arm Armor, And Doubled If Forearm Is Hit. WARNING: Disarmed Bots Will Become Passive And Not Attack Player, So This Is Disabled By Default.", null, new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.realistic_ballistics }));
            CanDisarmPlayer = config.Bind<bool>(ballSettings, "Can Disarm Player", Plugin.ServerConfig.realistic_ballistics, new ConfigDescription("If Hit In The Arms, There Is A Chance That The Currently Equipped Weapon Will Be Dropped. Chance Is Modified By Bullet Kinetic Energy And Reduced If Hit Arm Armor, And Doubled If Forearm Is Hit.", null, new ConfigurationManagerAttributes { Order = 1, Browsable = Plugin.ServerConfig.realistic_ballistics }));

            ShowBalance = config.Bind<bool>(statSettings, "Show Balance Stat", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Requiures Restart. Warning: Showing Too Many Stats On Weapons With Lots Of Slots Makes The Inspect Menu UI Difficult To Use.", null, new ConfigurationManagerAttributes { Order = 5, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ShowCamRecoil = config.Bind<bool>(statSettings, "Show Camera Recoil Stat", false, new ConfigDescription("Requiures Restart. Warning: Showing Too Many Stats On Weapons With Lots Of Slots Makes The Inspect Menu UI Difficult To Use.", null, new ConfigurationManagerAttributes { Order = 4, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ShowDispersion = config.Bind<bool>(statSettings, "Show Dispersion Stat", false, new ConfigDescription("Requiures Restart. Warning: Showing Too Many Stats On Weapons With Lots Of Slots Makes The Inspect Menu UI Difficult To Use.", null, new ConfigurationManagerAttributes { Order = 3, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ShowRecoilAngle = config.Bind<bool>(statSettings, "Show Recoil Angle Stat", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Requiures Restart. Warning: Showing Too Many Stats On Weapons With Lots Of Slots Makes The Inspect Menu UI Difficult To Use..", null, new ConfigurationManagerAttributes { Order = 2, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ShowSemiROF = config.Bind<bool>(statSettings, "Show Semi Auto ROF Stat", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Requiures Restart. Warning: Showing Too Many Stats On Weapons With Lots Of Slots Makes The Inspect Menu UI Difficult To Use.", null, new ConfigurationManagerAttributes { Order = 1, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            EnableMuzzleEffects = config.Bind<bool>(waponSettings, "Enable Muzzle Effects.", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Enanbes Changes To Muzzle Flash, Smoke, Etc. And Makes Their Intensity Dependent On Caliber, Weapon Condition, Attachments Etc.", null, new ConfigurationManagerAttributes { Order = 40, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            AimPunchMulti = config.Bind<float>(waponSettings, "Aim Punch Intensity.", 0.8f, new ConfigDescription("Amount Of Aim Punch.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 35, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            SwayIntensity = config.Bind<float>(waponSettings, "Sway Intensity.", 1f, new ConfigDescription("Changes The Intensity Of Aim Sway.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 30, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            ProceduralIntensity = config.Bind<float>(waponSettings, "Procedural Intensity.", 1f, new ConfigDescription("Changes The Intensity Of Procedural Animations, Including Sway, Weapon Movement, And Weapon Inertia.", new AcceptableValueRange<float>(0f, 3f), new ConfigurationManagerAttributes { Order = 20, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            DuraMalfReductionThreshold = config.Bind<float>(waponSettings, "Malfunction Reduction Durability Threshold.", 90f, new ConfigDescription("Durabiltiy-Based Malfunction Chance Is Reduced Until This Durability Threshold Is Exceeded.", new AcceptableValueRange<float>(1f, 100f), new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.malf_changes }));
            DuraMalfThreshold = config.Bind<float>(waponSettings, "Malfunction Durability Threshold", 98f, new ConfigDescription("Malfunction Chance Is Almost 0 Till This Durabiltiy Threshold Is Met, Unless One Of A Number Of Critera Or Thresholds Is Met (Heat, Burst Round Count, Mag Malf Chance, Ammo Malf Chance, Weapon Mod Malf Chance, Subsonic Ammo + Gun That Can't Cycle It, Etc.)", new AcceptableValueRange<float>(1f, 100f), new ConfigurationManagerAttributes { Order = 8, Browsable = Plugin.ServerConfig.malf_changes }));
            MalfMulti = config.Bind<float>(waponSettings, "Malfunction Multi", 0.75f, new ConfigDescription("Malfunction Chance Multiplier.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { Order = 10, Browsable = Plugin.ServerConfig.malf_changes }));
            IncreaseCOI = config.Bind<bool>(waponSettings, "Enable Increased Inaccuracy", Plugin.ServerConfig.recoil_attachment_overhaul, new ConfigDescription("Requires Restart. Increases The Innacuracy Of All Weapons So That MOA/Accuracy Is A More Important Stat.", null, new ConfigurationManagerAttributes { Order = 1, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            DecGain = config.Bind(deafSettings, "Reduce Gain Keybind", new KeyboardShortcut(KeyCode.KeypadMinus), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, Browsable = Plugin.ServerConfig.headset_changes }));
            IncGain = config.Bind(deafSettings, "Increase Gain Keybind", new KeyboardShortcut(KeyCode.KeypadPlus), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, Browsable = Plugin.ServerConfig.headset_changes }));
            HeadsetNoiseReduction = config.Bind<int>(deafSettings, "Headset Impulse Noise Reduction", 0, new ConfigDescription("To What Level Of Amplification The Headset Reduces To When There's Gunfire Or Explosions. It Is Hard-Coded To Not Exceed Current Headset Gain Value", new AcceptableValueRange<int>(-10, 15), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 100, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            HeadsetGain = config.Bind<int>(deafSettings, "Headset Gain", 2, new ConfigDescription("WARNING: BE CAREFUL INCREASING THIS TOO HIGH! IT MAY DAMAGE YOUR HEARING! Adjusts The Gain Of Equipped Headsets In Real Time, Acts Just Like The Volume Control On IRL Ear Defenders.", new AcceptableValueRange<int>(-5, 15), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 90, Browsable = Plugin.ServerConfig.headset_changes }));
            GunshotVolume = config.Bind<float>(deafSettings, "Gunshot Volume", 1f, new ConfigDescription("Multiplier For Gunshot Volume, Player and NPC. Higher = Louder.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 80, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            EnableAmbientChanges = config.Bind<bool>(deafSettings, "Enable Ambient Audio Changes", false, new ConfigDescription("Enable The Use Of The Ambient Audio Multis. Can Cause Audio Glitches When Transitioning From Indoors To Outdoors ", null, new ConfigurationManagerAttributes { Order = 73, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            OutdoorAmbientMulti = config.Bind<float>(deafSettings, "Outdoor Ambient Audio Offset", 0f, new ConfigDescription("Adjusts The Ambient Volume With And Without Headsets. Higher = Louder.", new AcceptableValueRange<float>(-60f, 50f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 72, Browsable = Plugin.ServerConfig.headset_changes }));
            IndoorAmbientMulti = config.Bind<float>(deafSettings, "Indoor Ambient Audio Offset", -20f, new ConfigDescription("Adjusts The Ambient Volume With And Without Headsets. Higher = Louder.", new AcceptableValueRange<float>(-60f, 50f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 70, Browsable = Plugin.ServerConfig.headset_changes }));
            SharedMovementVolume = config.Bind<float>(deafSettings, "Shared Movement Volume Multi", 1f, new ConfigDescription("Multiplier For Player + NPC Sprint Volume. Has To Be Shared Due To BSG Jank.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 60, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            NPCMovementVolume = config.Bind<float>(deafSettings, "NPC Movement Volume Multi", 1f, new ConfigDescription("Multiplier For NPC Movement Volume. Includes Walking And Equipment Rattle.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 50, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            PlayerMovementVolume = config.Bind<float>(deafSettings, "Player Movement Volume Multi", 1f, new ConfigDescription("Multiplier For Player Movment Volume.  Includes Walking And Equipment Rattle.", new AcceptableValueRange<float>(0f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 40, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            ADSVolume = config.Bind<float>(deafSettings, "ADS Volume Multi", 2.5f, new ConfigDescription("ADS Volume. Higher = Louder.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 20, IsAdvanced = false, Browsable = Plugin.ServerConfig.headset_changes }));
            DeafenResetDelay = config.Bind<float>(deafSettings, "Deafen Reset Delay", 1f, new ConfigDescription("Dekay Before Deafening And Tunnel Vision To Start Resetting.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 10, Browsable = Plugin.ServerConfig.headset_changes }));

            PistolGlobalAimSpeedModifier = config.Bind<float>(speed, "Pistol Aim Speed Multi.", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 17, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalAimSpeedModifier = config.Bind<float>(speed, "Aim Speed Multi.", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 16, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalReloadSpeedMulti = config.Bind<float>(speed, "Magazine Reload Speed Multi", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 15, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalFixSpeedMulti = config.Bind<float>(speed, "Malfunction Fix Speed Multi", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 14, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalUBGLReloadMulti = config.Bind<float>(speed, "UBGL Reload Speed Multi", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 13, IsAdvanced = true, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            RechamberPistolSpeedMulti = config.Bind<float>(speed, "Pistol Rechamber Speed Multi", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 12, IsAdvanced = true, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalRechamberSpeedMulti = config.Bind<float>(speed, "Rechamber Speed Multi", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 11, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalBoltSpeedMulti = config.Bind<float>(speed, "Bolt Speed Multi", 1.0f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 10, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalShotgunRackSpeedFactor = config.Bind<float>(speed, "Shotgun Rack Speed Multi", 1.0f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 9, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalCheckChamberSpeedMulti = config.Bind<float>(speed, "Chamber Check Speed Multi", 1.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 8, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalCheckChamberShotgunSpeedMulti = config.Bind<float>(speed, "Shotgun Chamber Check Speed Multi", 1.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 7, IsAdvanced = true, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalCheckChamberPistolSpeedMulti = config.Bind<float>(speed, "Pistol Chamber Check Speed Multi", 1.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 6, IsAdvanced = true, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalCheckAmmoPistolSpeedMulti = config.Bind<float>(speed, "Pistol Check Ammo Multi", 1.25f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 5, IsAdvanced = true, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            GlobalCheckAmmoMulti = config.Bind<float>(speed, "Check Ammo Multi.", 1.3f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 4, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            QuickReloadSpeedMulti = config.Bind<float>(speed, "Quick Reload Multi", 1.45f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 2, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));
            InternalMagReloadMulti = config.Bind<float>(speed, "Internal Magazine Reload", 1.15f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 10.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 1, Browsable = Plugin.ServerConfig.recoil_attachment_overhaul }));

            EnableAnimationFixes = config.Bind<bool>(weapAimAndPos, "De-Jank EFT Animations", Plugin.ServerConfig.enable_stances, new ConfigDescription("Attempts To Make EFT Certain Animations Less Janky, Like Inventory And Door Animations.", null, new ConfigurationManagerAttributes { Order = 430, Browsable = Plugin.ServerConfig.enable_stances }));
            ModifyBSGCollision = config.Bind<bool>(weapAimAndPos, "Modify BSG Collision", Plugin.ServerConfig.enable_stances, new ConfigDescription("If 'Override Collision' Is Enabled, This Will Be Ignored. Tweaks BSG's Handling Of Weapon Collisions. Slows It Down, Makes It Less Janky.", null, new ConfigurationManagerAttributes { Order = 420, Browsable = Plugin.ServerConfig.enable_stances }));
            OverrideCollision = config.Bind<bool>(weapAimAndPos, "Override Collision (WIP)", false, new ConfigDescription("If This Is Enabled, 'Modify BSG Collision' Will Be Ignored. Overrides BSG Collision Handling Complately With Custom Solution. Requires FOV Fix Mod To Function Correctly.", null, new ConfigurationManagerAttributes { Order = 410, Browsable = Plugin.ServerConfig.enable_stances }));
            OverrideMounting = config.Bind<bool>(weapAimAndPos, "Use Realism Mounting System", Plugin.ServerConfig.enable_stances, new ConfigDescription("Overrides BSG's Mounting System WIth Realism's (That Was Implemented First). Recoil, Stance and Sway Mechanics Are All Built Around Realism's Mounting And Won't Function Correctly With BSG's.", null, new ConfigurationManagerAttributes { Order = 300, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableExtraProcEffects = config.Bind<bool>(weapAimAndPos, "Enable Extra Weapon Position/Rotation Effects", Plugin.ServerConfig.enable_stances, new ConfigDescription("Weapon Has A Slight Cant To It based On Ergo. ADS With Gasmask/Faceshield Is Canted. Weapon Cant Increases When Crouching, And Moves Closer To You. Other Sublte Effects.", null, new ConfigurationManagerAttributes { Order = 280, Browsable = Plugin.ServerConfig.enable_stances }));
            RememberStanceItem = config.Bind<bool>(weapAimAndPos, "Remember Stance After Using Item", Plugin.ServerConfig.enable_stances, new ConfigDescription("Remember Stance After Actions (Using Items)", null, new ConfigurationManagerAttributes { Order = 260, Browsable = Plugin.ServerConfig.enable_stances }));
            RememberStanceFiring = config.Bind<bool>(weapAimAndPos, "Remember Stance After Firing", Plugin.ServerConfig.enable_stances, new ConfigDescription("Remember Stance After Firing If The Player Was Aiming.", null, new ConfigurationManagerAttributes { Order = 260, Browsable = Plugin.ServerConfig.enable_stances }));
            BlockFiring = config.Bind<bool>(weapAimAndPos, "Block Shooting While In Stance", false, new ConfigDescription("Blocks Firing While In A Stance, Will Cancel Stance If Attempting To Fire.", null, new ConfigurationManagerAttributes { Order = 250, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableTacSprint = config.Bind<bool>(weapAimAndPos, "Enable High Ready Sprint Animation", Plugin.ServerConfig.enable_stances, new ConfigDescription("Enables Usage Of High Ready Sprint Animation When Sprinting From High Ready Position.", null, new ConfigurationManagerAttributes { Order = 230, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableAltPistol = config.Bind<bool>(weapAimAndPos, "Enable Alternative Pistol Position And ADS", Plugin.ServerConfig.enable_stances, new ConfigDescription("Pistol Will Be Held Centered And In A Compressed Stance. ADS Will Be Animated.", null, new ConfigurationManagerAttributes { Order = 229, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableAltRifle = config.Bind<bool>(weapAimAndPos, "Enable Alternative Rifle Position And ADS (WIP)", false, new ConfigDescription("Rfile Will Move Closer To Camera When Aiming, Leading To Smoother ADS From Stances. Also Standardizes All Rifle Positions. Ignores 'Rifle Position' Settings.", null, new ConfigurationManagerAttributes { Order = 220, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableIdleStamDrain = config.Bind<bool>(weapAimAndPos, "Enable Idle Arm Stamina Drain", Plugin.ServerConfig.enable_stances, new ConfigDescription("Arm Stamina Will Drain When Not In A Stance (High And Low Ready, Short-Stocking).", null, new ConfigurationManagerAttributes { Order = 210, Browsable = Plugin.ServerConfig.enable_stances }));
            IdleStamDrainModi = config.Bind<float>(weapAimAndPos, "Idle Stam Drain Modifer", 1f, new ConfigDescription("", new AcceptableValueRange<float>(0.1f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 200, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableStanceStamChanges = config.Bind<bool>(weapAimAndPos, "Enable Stance Stamina And Movement Effects", Plugin.ServerConfig.enable_stances, new ConfigDescription("Enabled Stances And Mounting To Affect Stamina And Movement Speed. Stamina Drain May Not Work Correctly If Disabled. High + Low Ready, Short-Stocking And Pistol Idle Will Regenerate Stamina Faster And Optionally Idle With Rifles Drains Stamina. High Ready Has Faster Sprint Speed And Sprint Accel, Low Ready Has Faster Sprint Accel. Arm Stamina Won't Drain Regular Stamina If It Reaches 0.", null, new ConfigurationManagerAttributes { Order = 183, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimReload = config.Bind<bool>(weapAimAndPos, "Allow Reload From Active Aim", false, new ConfigDescription("Allows Reload From Magazine While In Active Aim With Speed Bonus.", null, new ConfigurationManagerAttributes { Order = 190, Browsable = Plugin.ServerConfig.enable_stances }));
            EnableMountUI = config.Bind<bool>(weapAimAndPos, "Enable Mounting UI", Plugin.ServerConfig.enable_stances, new ConfigDescription("If Enabled, An Icon On Screen Will Indicate If Player Is Bracing, Mounting And What Side Of Cover They Are On.", null, new ConfigurationManagerAttributes { Order = 179, Browsable = Plugin.ServerConfig.enable_stances }));
            LeftShoulderOffset = config.Bind<float>(weapAimAndPos, "Left Shoulder Offset", -0.13f, new ConfigDescription("", new AcceptableValueRange<float>(-0.2f, 0.1f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 153, Browsable = Plugin.ServerConfig.enable_stances }));
            WeapOffset = config.Bind<Vector3>(weapAimAndPos, "Rifle Position Offset", new Vector3(-0.04f, -0.015f, 0f), new ConfigDescription("Adjusts The Starting Position Of Rifle On Screen If Alt Rifle Is Disabled", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 152, Browsable = Plugin.ServerConfig.enable_stances }));
            StanceRotationSpeedMulti = config.Bind<float>(weapAimAndPos, "Stance Rotation Speed Multi", 1f, new ConfigDescription("Adjusts The Speed Of Stance Rotation Changes.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 146, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            StanceTransitionSpeedMulti = config.Bind<float>(weapAimAndPos, "Stance Transition Speed.", 15.0f, new ConfigDescription("Adjusts The Position Change Speed Between Stances", new AcceptableValueRange<float>(1f, 35f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 145, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            CycleStancesKeybind = config.Bind(stanceBinds, "Cycle Stances Keybind", new KeyboardShortcut(KeyCode.None), new ConfigDescription("Cycles Between High, Low Ready and Short-Stocking. Double Click Returns To Idle.", null, new ConfigurationManagerAttributes { Order = 80, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimKeybind = config.Bind(stanceBinds, "Active Aim Keybind", new KeyboardShortcut(KeyCode.Mouse4), new ConfigDescription("Cants The Weapon Sideways, Improving Hipfire Accuracy.", null, new ConfigurationManagerAttributes { Order = 90, Browsable = Plugin.ServerConfig.enable_stances }));
            ToggleActiveAim = config.Bind<bool>(stanceBinds, "Use Toggle For Active Aim", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 100, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyKeybind = config.Bind(stanceBinds, "High Ready Keybind", new KeyboardShortcut(KeyCode.Mouse3, new[] { KeyCode.LeftAlt }), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 110, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyKeybind = config.Bind(stanceBinds, "Low Ready Keybind", new KeyboardShortcut(KeyCode.Mouse3, new[] { KeyCode.LeftControl }), new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 120, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockKeybind = config.Bind(stanceBinds, "Short-Stock Keybind", new KeyboardShortcut(KeyCode.J), new ConfigDescription("Tucks The Weapon's Stock Under Player's Arm, Shortening The Overall Length Of The Wweapon To Prevent Muzzle Being Pushed Away From Target.", null, new ConfigurationManagerAttributes { Order = 130, Browsable = Plugin.ServerConfig.enable_stances }));
            PatrolKeybind = config.Bind(stanceBinds, "Patrol/Neutral Stance Keybind", new KeyboardShortcut(KeyCode.K), new ConfigDescription("Puts The Weapon In A Neutral Position, Improving Arm Stam Regen And Walk Speed. For Maximum Larping.", null, new ConfigurationManagerAttributes { Order = 155, Browsable = Plugin.ServerConfig.enable_stances }));
            MeleeKeybind = config.Bind(stanceBinds, "Melee Keybind", new KeyboardShortcut(KeyCode.None), new ConfigDescription("Strike With Muzzle Or Bayonet Of Equipped Weapon.", null, new ConfigurationManagerAttributes { Order = 150, Browsable = Plugin.ServerConfig.enable_stances }));
            UseMouseWheelStance = config.Bind<bool>(stanceBinds, "Enable Mouse Wheel Stance Switching", Plugin.ServerConfig.enable_stances, new ConfigDescription("Switches Between High And Low Ready Via Mouse Wheel.", null, new ConfigurationManagerAttributes { Order = 160, Browsable = Plugin.ServerConfig.enable_stances }));
            UseMouseWheelPlusKey = config.Bind<bool>(stanceBinds, "Require Key + Mouse Wheel", Plugin.ServerConfig.enable_stances, new ConfigDescription("Require Keybind + Mouse Wheel To Change Stance.", null, new ConfigurationManagerAttributes { Order = 170, Browsable = Plugin.ServerConfig.enable_stances }));
            StanceWheelComboKeyBind = config.Bind(stanceBinds, "Keybind To Use With Mouse Wheel", new KeyboardShortcut(KeyCode.LeftControl), new ConfigDescription("Key Used In Combination With Mouse Wheel If Enabled ", null, new ConfigurationManagerAttributes { Order = 180, Browsable = Plugin.ServerConfig.enable_stances }));

            ThirdPersonRotationSpeed = config.Bind<float>(thirdPerson, "Third Person Rotation Speed Multi", 1.5f, new ConfigDescription("Speed Of Stance Rotation Change In Third Person.", new AcceptableValueRange<float>(0.1f, 20f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 1000, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ThirdPersonPositionSpeed = config.Bind<float>(thirdPerson, "Third Person Position Speed Multi", 1.0f, new ConfigDescription("Speed Of Stance Position Change In Third Person.", new AcceptableValueRange<float>(0.1f, 20f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 1100, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            PistolThirdPersonPosition = config.Bind<Vector3>(thirdPerson, "Pistol Third Person Position", new Vector3(-0.03f, 0.04f, -0.05f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 260, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolThirdPersonRotation = config.Bind<Vector3>(thirdPerson, "Pistol Third Person Rotation", new Vector3(0f, 15f, 0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 230, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ShortStockThirdPersonPosition = config.Bind<Vector3>(thirdPerson, "Short-Stock Third Person Position", new Vector3(0.03f, 0.065f, -0.075f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 200, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockThirdPersonRotation = config.Bind<Vector3>(thirdPerson, "Short-Stock Third Person Rotation", new Vector3(0f, -15f, 0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 170, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ActiveThirdPersonPosition = config.Bind<Vector3>(thirdPerson, "Active Aim Third Person Position", new Vector3(-0.02f, -0.02f, 0.02f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 140, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveThirdPersonRotation = config.Bind<Vector3>(thirdPerson, "Active Aim Third Person Rotation", new Vector3(0f, -35f, 0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 110, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            HighReadyThirdPersonPosition = config.Bind<Vector3>(thirdPerson, "High Ready Third Person Position", new Vector3(0.02f, 0.05f, -0.045f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 80, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyThirdPersonRotation = config.Bind<Vector3>(thirdPerson, "High Ready Third Person Rotation", new Vector3(-8f, -25f, 0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 50, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            LowReadyThirdPersonPosition = config.Bind<Vector3>(thirdPerson, "Low Ready Third Person Position", new Vector3(0.01f, -0.025f, 0f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 20, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyThirdPersonRotation = config.Bind<Vector3>(thirdPerson, "Low Ready Third Person Rotation", new Vector3(24f, 10f, -1f), new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 8, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ActiveAimAdditionalRotationSpeedMulti = config.Bind<float>(activeAim, "Active Aim Additonal Rotation Speed Multi.", 2f, new ConfigDescription("", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 145, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimResetRotationSpeedMulti = config.Bind<float>(activeAim, "Active Aim Reset Rotation Speed Multi.", 3.5f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 10f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 145, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimRotationMulti = config.Bind<float>(activeAim, "Active Aim Rotation Speed Multi.", 2f, new ConfigDescription("", new AcceptableValueRange<float>(0.0f, 10f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 144, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimSpeedMulti = config.Bind<float>(activeAim, "Active Aim Speed Multi", 15f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 100f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 143, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimResetSpeedMulti = config.Bind<float>(activeAim, "Active Aim Reset Speed Multi", 6f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 100f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 142, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ActiveAimOffset = config.Bind<Vector3>(activeAim, "Active Aim Position", new Vector3(-0.02f, 0.008f, 0f), new ConfigDescription("Weapon Position When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 135, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimRotation = config.Bind<Vector3>(activeAim, "Active Aim Rotation", new Vector3(0.0f, -35f, 0f), new ConfigDescription("Weapon Rotation When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 122, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimAdditionalRotation = config.Bind<Vector3>(activeAim, "Active Aiming Additional Rotation", new Vector3(0f, -35f, 0f), new ConfigDescription("Additional Seperate Weapon Rotation When Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 111, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ActiveAimResetRotation = config.Bind<Vector3>(activeAim, "Active Aiming Reset Rotation", new Vector3(0f, 20f, -1f), new ConfigDescription("Weapon Rotation When Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 102, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            HighReadyAdditionalRotationSpeedMulti = config.Bind<float>(highReady, "High Ready Additonal Rotation Speed Multi.", 0.1f, new ConfigDescription("How Fast The Weapon Rotates Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 94, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyResetRotationMulti = config.Bind<float>(highReady, "High Ready Reset Rotation Speed Multi.", 1.5f, new ConfigDescription("How Fast The Weapon Rotates Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 93, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyRotationMulti = config.Bind<float>(highReady, "High Ready Rotation Speed Multi.", 2f, new ConfigDescription("How Fast The Weapon Rotates Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 92, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyResetSpeedMulti = config.Bind<float>(highReady, "High Ready Reset Speed Multi", 6.5f, new ConfigDescription("How Fast The Weapon Moves Going Out Of Stance", new AcceptableValueRange<float>(1f, 100.1f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 91, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadySpeedMulti = config.Bind<float>(highReady, "High Ready Speed Multi", 6f, new ConfigDescription("How Fast The Weapon Moves Going Into Stance", new AcceptableValueRange<float>(1f, 100.1f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 90, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            HighReadyOffset = config.Bind<Vector3>(highReady, "High Ready Position", new Vector3(0.005f, 0.035f, -0.04f), new ConfigDescription("Weapon Position When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 85, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyRotation = config.Bind<Vector3>(highReady, "High Ready Rotation", new Vector3(-8.0f, -20f, 0f), new ConfigDescription("Weapon Rotation When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 72, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyAdditionalRotation = config.Bind<Vector3>(highReady, "High Ready Additional Rotation", new Vector3(-50.0f, -25f, -5f), new ConfigDescription("Additional Seperate Weapon Rotation When Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 69, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            HighReadyResetRotation = config.Bind<Vector3>(highReady, "High Ready Reset Rotation", new Vector3(0f, 2f, 0f), new ConfigDescription("Weapon Rotation When Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 66, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            LowReadyAdditionalRotationSpeedMulti = config.Bind<float>(lowReady, "Low Ready Additonal Rotation Speed Multi", 0.75f, new ConfigDescription("How Fast The Weapon Rotates.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 64, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyResetRotationMulti = config.Bind<float>(lowReady, "Low Ready Reset Rotation Speed Multi", 2.25f, new ConfigDescription("How Fast The Weapon Rotates.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 63, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyRotationMulti = config.Bind<float>(lowReady, "Low Ready Rotation Speed Multi", 1.5f, new ConfigDescription("How Fast The Weapon Rotates.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 62, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadySpeedMulti = config.Bind<float>(lowReady, "Low Ready Speed Multi.", 14f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 61, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyResetSpeedMulti = config.Bind<float>(lowReady, "Low Ready Reset Speed Multi", 8.7f, new ConfigDescription("", new AcceptableValueRange<float>(0.01f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 60, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            LowReadyOffset = config.Bind<Vector3>(lowReady, "Low Ready Position", new Vector3(0f, -0.01f, 0f), new ConfigDescription("Weapon Position When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 55, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyRotation = config.Bind<Vector3>(lowReady, "Low Ready Rotation", new Vector3(8f, -5f, -1f), new ConfigDescription("Weapon Rotation When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 42, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyAdditionalRotation = config.Bind<Vector3>(lowReady, "Low Ready Additional Rotation", new Vector3(12.0f, -1f, 0f), new ConfigDescription("Additional Seperate Weapon Rotation When Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 39, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            LowReadyResetRotation = config.Bind<Vector3>(lowReady, "Low Ready Reset Rotation", new Vector3(-1.0f, 0f, 0f), new ConfigDescription("Weapon Rotation When Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 36, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            PistolAdditionalRotationSpeedMulti = config.Bind<float>(pistol, "Pistol Additional Rotation Speed Multi", 0.1f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.0f, 20f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 35, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolResetRotationSpeedMulti = config.Bind<float>(pistol, "Pistol Reset Rotation Speed Multi", 0.5f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.0f, 20f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 34, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolRotationSpeedMulti = config.Bind<float>(pistol, "Pistol Rotation Speed Multi", 1f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.0f, 20f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 33, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolPosSpeedMulti = config.Bind<float>(pistol, "Pistol Position Speed Multi", 6.0f, new ConfigDescription("", new AcceptableValueRange<float>(1.0f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 32, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolPosResetSpeedMulti = config.Bind<float>(pistol, "Pistol Position Reset Speed Multi", 10.0f, new ConfigDescription("", new AcceptableValueRange<float>(1.0f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 30, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            PistolOffset = config.Bind<Vector3>(pistol, "Pistol Position", new Vector3(0f, 0.04f, -0.015f), new ConfigDescription("Weapon Position When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 25, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolRotation = config.Bind<Vector3>(pistol, "Pistol Rotation", new Vector3(0.0f, -5f, 0f), new ConfigDescription("Weapon Rotation When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 12, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolAdditionalRotation = config.Bind<Vector3>(pistol, "Pistol Ready Additional Rotation", new Vector3(0.0f, 0f, 0f), new ConfigDescription("Additional Seperate Weapon Rotation When Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 6, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            PistolResetRotation = config.Bind<Vector3>(pistol, "Pistol Ready Reset Rotation", new Vector3(-1f, 0f, 0f), new ConfigDescription("Weapon Rotation When Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 3, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ShortStockAdditionalRotationSpeedMulti = config.Bind<float>(shortStock, "Short-Stock Additional Rotation Speed Multi", 1.5f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.1f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 35, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockResetRotationSpeedMulti = config.Bind<float>(shortStock, "Short-Stock Reset Rotation Speed Multi", 1.0f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.1f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 34, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockRotationMulti = config.Bind<float>(shortStock, "Short-Stock Rotation Speed Multi", 1.0f, new ConfigDescription("How Fast The Weapon Rotates.", new AcceptableValueRange<float>(0.1f, 5f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 33, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockSpeedMulti = config.Bind<float>(shortStock, "Short-Stock Position Speed Multi.", 4f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 32, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockResetSpeedMulti = config.Bind<float>(shortStock, "Short-Stock Position Reset Speed Mult", 3.8f, new ConfigDescription("", new AcceptableValueRange<float>(1f, 100.0f), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 30, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

            ShortStockOffset = config.Bind<Vector3>(shortStock, "Short-Stock Position", new Vector3(0.02f, 0.1f, -0.025f), new ConfigDescription("Weapon Position When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 25, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockRotation = config.Bind<Vector3>(shortStock, "Short-Stock Rotation", new Vector3(0f, -15f, 0f), new ConfigDescription("Weapon Rotation When In Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 12, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockAdditionalRotation = config.Bind<Vector3>(shortStock, "Short-Stock Ready Additional Rotation", new Vector3(-3.0f, -15f, 1f), new ConfigDescription("Additional Seperate Weapon Rotation When Going Into Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 6, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));
            ShortStockResetRotation = config.Bind<Vector3>(shortStock, "Short-Stock Ready Reset Rotation", new Vector3(-1.5f, 2f, 0f), new ConfigDescription("Weapon Rotation When Going Out Of Stance.", null, new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 3, IsAdvanced = true, Browsable = Plugin.ServerConfig.enable_stances }));

        }



        private static void ResetToDefault(ConfigFile config, ConfigDefinition definition)
        {
            Utils.Logger.LogWarning("definition key: " + definition.Key);
            Utils.Logger.LogWarning("definition section: " + definition.Section);
            var defaultValue = config[definition].DefaultValue;
            config[definition].BoxedValue = defaultValue;
        }
    }
}
