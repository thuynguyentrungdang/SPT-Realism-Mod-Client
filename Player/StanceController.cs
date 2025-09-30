﻿using Comfort.Common;
using Diz.LanguageExtensions;
using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using EFT.InventoryLogic;
using EFT.WeaponMounting;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EFT.Player;

namespace RealismMod
{
    public enum EBracingDirection
    {
        Top,
        Left,
        Right,
        None
    }

    public enum EStance
    {
        None,
        LowReady,
        HighReady,
        ShortStock,
        ActiveAiming,
        PatrolStance,
        Melee,
        PistolCompressed
    }

    public static class StanceController
    {
        //need to change to type WildSpawnType, and somehow get PMC type
        public static string[] _botsToUseTacticalStances = { "bossKolontay", "pmcBEAR", "pmcUSEC", "exUsec", "pmcBot", "bossKnight", "followerBigPipe", "followerBirdEye", "bossGluhar", "followerGluharAssault", "followerGluharScout", "followerGluharSecurity", "followerGluharSnipe" };
        /*        public static Dictionary<string, bool> LightDictionary = new Dictionary<string, bool>();*/

        public static Player.BetterValueBlender StanceBlender = new Player.BetterValueBlender
        {
            Speed = 5f,
            Target = 0f
        };

        private static AnimationCurve _smoothCurve = new(
         new Keyframe(0, 0.05f),
         new Keyframe(0.1f, 0.075f),
         new Keyframe(0.2f, 0.1f),
         new Keyframe(0.3f, 0.2f),
         new Keyframe(0.4f, 0.4f),
         new Keyframe(0.5f, 0.6f),
         new Keyframe(0.6f, 0.7f),
         new Keyframe(0.7f, 0.8f),
         new Keyframe(0.8f, 0.9f),
         new Keyframe(0.9f, 0.95f),
         new Keyframe(1, 1f)
        );

        public static Vector3 MountPos { get; set; }
        public static Vector3 MountDir { get; set; }

        public static bool ShouldBlockAllStances
        {
            get
            {
                return (IsMounting && WeaponStats.BipodIsDeployed) || !MeleeIsToggleable;
            }
        }

        public static bool TreatWeaponAsPistolStance
        {
            get
            {
                return WeaponStats.IsMachinePistol || WeaponStats.IsStocklessPistol;
            }
        }

        public static bool CanDoTacSprint
        {
            get
            {
                return PluginConfig.EnableTacSprint.Value && PlayerState.IsSprinting && CurrentStance != EStance.ActiveAiming
                && (CurrentStance == EStance.HighReady || StoredStance == EStance.HighReady) &&
                WeaponStats.TotalWeaponWeight <= (WeaponStats.IsBullpup ? TAC_SPRINT_WEIGHT_BULLPUP : TAC_SPRINT_WEIGHT_LIMIT)
                && WeaponStats.TotalWeaponLength <= TAC_SPRINT_LENGTH_LIMIT && !PlayerState.IsScav
                && !Plugin.RealHealthController.HealthConditionPreventsTacSprint && WeaponStats.TotalErgo > TAC_SPRINT_ERGO_LIMIT;
            }
        }

        private static float _animationTimer = 0f;
        private static float _animSpeed = 1f;
        private static float _pistolPosSpeed = 1f;

        private static float _currentRifleXPos = 0f;
        private static float _currentRifleYPos = 0f;
        private static float _currentRifleZPos = 0f;
        private static float _currentPistolXPos = 0f;
        private static float _currentPistolYPos = 0f;
        private static float _currentPistolZPos = 0f;

        private static float pistolCameraAlignmentTarget = 0f;
        private static float pistolYTarget = 0f;

        private static float rifleCameraAlignmentTarget = 0f;
        private static float rifleYTarget = 0f;

        public static Vector3 CoverWiggleDirection = Vector3.zero;
        public static Vector3 WeaponOffsetPosition = Vector3.zero;
        public static Vector3 StanceTargetPosition = Vector3.zero;
        private static Vector3 _pistolLocalPosition = Vector3.zero;
        private static Vector3 _rifleLocalPosition = Vector3.zero;

        private const float _clickDelay = 0.2f;
        private static float _doubleClickTime = 0f;
        private static bool _clickTriggered = true;
        public static int StanceIndex = 0;

        public static bool MeleeIsToggleable = true;
        public static bool CanDoMeleeDetection = false;
        public static bool MeleeHitSomething = false;
        public static bool IsFiringFromStance = false;
        public static float StanceShotTime = 0.0f;
        private static float ManipTime = 0.0f;
        public static float ManipTimer = 0.25f;
        public static float DampingTimer = 0.0f;
        public static float MeleeTimer = 0.0f;
        public static bool DoDampingTimer = false;
        public static bool CanResetDamping = true;
        public static bool WasAimingBeforeCollision = false;
        public static bool StopCameraMovement = false;
        public static float CameraMovmentForCollisionSpeed = 0.01f;
        public static bool IsColliding = false;

        public static float HighReadyBlackedArmTime = 0.0f;
        public static bool CanDoHighReadyInjuredAnim = false;

        public static bool ShouldForceLowReady
        {
            get
            {
                return (Plugin.RealHealthController.HealthConditionForcedLowReady || (WeaponStats.TotalWeaponWeight >= 10f && !IsMounting))
                    && !IsAiming && !IsFiringFromStance && CurrentStance != EStance.PistolCompressed
                    && CurrentStance != EStance.PatrolStance && CurrentStance != EStance.ShortStock
                    && CurrentStance != EStance.ActiveAiming && MeleeIsToggleable && !IsBracing;
            }
        }

        public static bool HaveSetAiming = false;
        public static bool HaveSetActiveAim = false;

        public static float HighReadyManipBuff
        {
            get
            {
                return CurrentStance == EStance.HighReady ? 1.18f : 1f;
            }
        }
        public static float ActiveAimManipBuff
        {
            get
            {
                return CurrentStance == EStance.ActiveAiming && PluginConfig.ActiveAimReload.Value ? 1.15f : 1f;
            }
        }
        public static float LowReadyManipBuff
        {
            get
            {
                return CurrentStance == EStance.LowReady ? 1.21f : 1f;
            }
        }

        public static bool CancelPistolStance = false;
        public static bool PistolIsColliding = false;
        public static bool CancelHighReady = false;
        public static bool ModifyHighReady = false;
        public static bool CancelLowReady = false;
        public static bool CancelShortStock = false;
        public static bool CancelActiveAim = false;
        public static bool ShouldResetStances = false;
        private static bool _doMeleeReset = false;

        public static bool HasResetActiveAim = true;
        public static bool HasResetLowReady = true;
        public static bool HasResetHighReady = true;
        public static bool HasResetShortStock = true;
        public static bool HasResetPistolPos = true;
        public static bool HasResetMelee = true;

        public static EStance StoredStance
        {
            get { return _storedStance; }
            set { _storedStance = value; }
        }
        public static EStance CurrentStance
        {
            get { return _currentStance; }
            set
            {
                if (value != _currentStance)
                {
                    _currentStance = value;
                    if (!IsAiming) Utils.GetYourPlayer().ProceduralWeaponAnimation.method_23();
                }
            }
        }
        private static EStance _lastRecordedStanceStamina = EStance.None; //used for stamina drate rate updates
        private static EStance _previousStance = EStance.None;
        private static EStance _currentStance = EStance.None;
        private static EStance _storedStance = EStance.None;
        public static bool FinishedUnPatrolStancing = false;
        private static bool _SkipPistolWiggle = false;
        public static bool WasActiveAim = false;
        public static bool _isLeftShoulder = false;

        public static bool IsLeftShoulder
        {
            get { return _isLeftShoulder; }
            set
            {
                if (value != _isLeftShoulder)
                {
                    _isLeftShoulder = value;
                    Utils.GetYourPlayer().ProceduralWeaponAnimation.method_23();
                }
            }
        }

        public static bool CancelLeftShoulder = false;
        public static bool DoLeftShoulderTransition = false;
        public static bool IsDoingTacSprint = false;
        public const float TAC_SPRINT_WEIGHT_LIMIT = 5.1f;
        public const float TAC_SPRINT_WEIGHT_BULLPUP = 5.75f;
        public const int TAC_SPRINT_LENGTH_LIMIT = 6;
        public const float TAC_SPRINT_ERGO_LIMIT = 35f;

        public static bool IsInForcedLowReady = false;
        public static bool IsAiming = false;
        public static bool DidWeaponSwap = false;
        public static bool IsBlindFiring = false;
        public static bool IsInThirdPerson = false;
        public static bool ToggledLight = false;
        public static bool DidStanceWiggle = false;
        public static bool DidLowReadyResetStanceWiggle = false;
        public static float WiggleReturnSpeed = 1f;

        //arm stamina
        private static bool _regenStam = false;
        private static bool _drainStam = false;
        private static bool _neutral = false;
        private static bool _wasBracing = false;
        private static bool _wasMounting = false;
        private static bool _wasAiming = false;
        public static bool HaveResetStamDrain = false;
        public static bool CanResetAimDrain = false;

        //extra rotaitons
        private static Vector3 _posePosOffest = Vector3.zero;
        private static Vector3 _poseRotOffest = Vector3.zero;
        private static Vector3 _patrolPos = Vector3.zero;
        private static Vector3 _patrolRot = Vector3.zero;

        //patrol
        private static Vector3 _riflePatrolPos = new Vector3(0.2f, 0.025f, 0.1f);
        private static Vector3 _riflePatrolRot = new Vector3(0.05f, -0.05f, -0.5f);
        private static Vector3 _pistolPatrolPos = new Vector3(0.05f, 0f, 0f);
        private static Vector3 _pistolPatrolRot = new Vector3(0.1f, -0.1f, -0.1f);

        //mounting
        private static Quaternion _makeQuaternionDelta(Quaternion from, Quaternion to) => to * Quaternion.Inverse(from); //yeah I don't know what this is either
        private static float _mountAimSmoothed = 0f;
        public static float _cumulativeMountPitch = 0f;
        public static float _cumulativeMountYaw = 0f;
        static Vector2 _lastMountYawPitch;
        public static EBracingDirection BracingDirection = EBracingDirection.None;
        public static bool IsBracing = false;
        public static bool _isRealismMounting = false;
        public static bool IsMounting
        {
            get
            {
                return _isRealismMounting;
            }
            set
            {
                if (value != _isRealismMounting)
                {
                    Player player = Utils.GetYourPlayer();
                    FirearmController fc = player.HandsController as FirearmController;

                    if (fc == null)
                    {
                        value = false;
                        return;
                    }

                    _isRealismMounting = value;

                    if (player.ProceduralWeaponAnimation != null) player.ProceduralWeaponAnimation.method_23();

                    float accuracy = fc.Item.GetTotalCenterOfImpact(false); //forces accuracy to update

                    AccessTools.Field(typeof(Player.FirearmController), "float_3").SetValue(fc, accuracy); //update weapon accuracy
                    player.ProceduralWeaponAnimation.UpdateTacticalReload(); //gives better chamber animations
                    //player.MovementContext.PlayerAnimator.SetProneBipodMount(player.MovementContext.IsInPronePose && WeaponStats.BipodIsDeployed && value); //this causes camera to detatch from weapon, could be a good effect if I could get camera to follow it.
                    fc.FirearmsAnimator.SetMounted(value);
                    //player.ProceduralWeaponAnimation.SetMountingData(value, BracingDirection == EBracingDirection.Top);
                }
            }
        }
        public static float BracingSwayBonus = 1f;
        public static float BracingRecoilBonus = 1f;

        private static float _tacSprintTime = 0.0f;
        private static bool _canDoTacSprintTimer = false;

        public static Dictionary<string, Vector3> GetWeaponOffsets()
        {
            return new Dictionary<string, Vector3> {
            { "5aafa857e5b5b00018480968", new Vector3(0f, 0f, -0.1f)}, //m1a
            { "5b0bbe4e5acfc40dc528a72d", new Vector3(0f, 0f, -0.035f)}, //sa58
            { "676176d362e0497044079f4c", new Vector3(0f, -0.0135f, 0.02f)}, //x17
            { "6183afd850224f204c1da514", new Vector3(0f, -0.0135f, 0.02f)}, //mk17
            { "6165ac306ef05c2ce828ef74", new Vector3(0f, -0.0135f, 0.02f)}, //mk17 fde
            { "6184055050224f204c1da540", new Vector3(0f, -0.0135f, 0.02f)}, //mk16
            { "618428466ef05c2ce828f218", new Vector3(0f, -0.0135f, 0.02f)}, //mk16 fde
            { "5ae08f0a5acfc408fb1398a1", new Vector3(0f, 0f, -0.005f)}, //mosin 
            { "5bfd297f0db834001a669119", new Vector3(0f, 0f, -0.005f)}, //mosin s
            { "54491c4f4bdc2db1078b4568", new Vector3(0f, 0f, -0.01f)}, //mp133
            { "56dee2bdd2720bc8328b4567", new Vector3(0f, 0f, -0.01f)}, //mp153
            { "606dae0ab0e443224b421bb7", new Vector3(0f, 0f, -0.01f)}, //mp155
            { "6259b864ebedf17603599e88", new Vector3(0f, 0f, -0.02f)}, //M3
            { "6783ae5bb52da6ed912e3d01", new Vector3(0f, 0f, -0.02f)}, //M3 mechanic
            };
        }

        private static float GetRestoreRate()
        {
            float baseRestoreRate = 0f;
            if (IsMounting && WeaponStats.BipodIsDeployed)
            {
                baseRestoreRate = 5f;
            }
            if (CurrentStance == EStance.PatrolStance || IsMounting)
            {
                baseRestoreRate = 4f;
            }
            else if (CurrentStance == EStance.LowReady || CurrentStance == EStance.PistolCompressed || IsBracing)
            {
                baseRestoreRate = 2.4f;
            }
            else if (CurrentStance == EStance.HighReady)
            {
                baseRestoreRate = 1.85f;
            }
            else if (CurrentStance == EStance.ShortStock)
            {
                baseRestoreRate = 1.3f;
            }
            else if (IsIdle() && !PluginConfig.EnableIdleStamDrain.Value)
            {
                baseRestoreRate = 1f;
            }
            else
            {
                baseRestoreRate = 1f;
            }
            float formfactor = WeaponStats.IsBullpup ? 1.05f : 1f;
            return (1f - ((WeaponStats.ErgoFactor * formfactor) / 100f)) * baseRestoreRate * PlayerState.HealthStamRegenFactor;
        }

        private static float GetDrainRate(Player player)
        {
            float baseDrainRate = 0f;
            if (player.Physical.HoldingBreath)
            {
                baseDrainRate = IsMounting && WeaponStats.BipodIsDeployed ? 0.025f : IsMounting ? 0.05f : IsBracing ? 0.1f : 0.5f;
            }
            else if (IsAiming)
            {
                baseDrainRate = 0.15f;
            }
            else if (IsDoingTacSprint)
            {
                baseDrainRate = 0.15f;
            }
            else if (CurrentStance == EStance.ActiveAiming)
            {
                baseDrainRate = 0.075f;
            }
            else
            {
                baseDrainRate = 0.1f;
            }
            float formfactor = WeaponStats.IsBullpup ? 0.4f : 1f;
            return WeaponStats.ErgoFactor * formfactor * baseDrainRate * ((1f - PlayerState.HealthStamRegenFactor) + 1f) * (1f - (PlayerState.StrengthSkillAimBuff)) * PluginConfig.IdleStamDrainModi.Value;
        }


        //this method makes baby Jesus cry
        public static void SetStanceStamina(Player player)
        {
            bool isUsingStationaryWeapon = player.MovementContext.CurrentState.Name == EPlayerState.Stationary;
            bool isInRegenableStance = CurrentStance == EStance.HighReady || CurrentStance == EStance.LowReady || CurrentStance == EStance.PatrolStance || CurrentStance == EStance.ShortStock || (IsIdle() && !PluginConfig.EnableIdleStamDrain.Value);
            bool isInRegenableState = (!player.Physical.HoldingBreath && (IsMounting || IsBracing)) || player.IsInPronePose || CurrentStance == EStance.PistolCompressed || isUsingStationaryWeapon;
            bool doRegen = ((isInRegenableStance && !IsAiming && !IsFiringFromStance) || isInRegenableState) && !PlayerState.IsSprinting;
            bool shouldDoIdleDrain = IsIdle() && PluginConfig.EnableIdleStamDrain.Value;
            bool shouldInterruptRegen = isInRegenableStance && (IsAiming || IsFiringFromStance);
            bool doNeutral = PlayerState.IsSprinting || player.IsInventoryOpened || (CurrentStance == EStance.ActiveAiming && player.Pose == EPlayerPose.Duck);
            bool doDrain = ((shouldInterruptRegen || !isInRegenableStance || shouldDoIdleDrain) && !isInRegenableState && !doNeutral) || (IsDoingTacSprint && PluginConfig.EnableIdleStamDrain.Value);
            EStance stance = CurrentStance;

            if (HaveResetStamDrain || DidWeaponSwap || IsAiming != _wasAiming || _regenStam != doRegen || _drainStam != doDrain || _neutral != doNeutral || _lastRecordedStanceStamina != CurrentStance || IsMounting != _wasMounting || IsBracing != _wasBracing)
            {
                if (doDrain)
                {
                    player.Physical.Aim(1f);
                }
                else if (doRegen)
                {
                    player.Physical.Aim(0f);
                }
                else if (doNeutral)
                {
                    player.Physical.Aim(1f);
                }
                HaveResetStamDrain = false;
            }

            //drain
            if (doDrain)
            {
                player.Physical.HandsStamina.Multiplier = GetDrainRate(player);
            }
            //regen
            else if (doRegen)
            {
                player.Physical.HandsStamina.Multiplier = GetRestoreRate();
            }
            //no drain or regen
            else if (doNeutral)
            {
                player.Physical.HandsStamina.Multiplier = 0f;
            }

            _regenStam = doRegen;
            _drainStam = doDrain;
            _neutral = doNeutral;
            _wasBracing = IsBracing;
            _wasMounting = IsMounting;
            _wasAiming = IsAiming;
            _lastRecordedStanceStamina = CurrentStance;
        }

        public static void ResetStanceStamina()
        {
            _regenStam = false;
            _drainStam = false;
            _neutral = false;
            _wasBracing = false;
            _wasMounting = false;
            _wasAiming = false;
            _lastRecordedStanceStamina = EStance.None;
        }

        public static void UnarmedStanceStamina(Player player)
        {
            player.Physical.Aim(0f);
            player.Physical.HandsStamina.Multiplier = 1f;
            ResetStanceStamina();
        }

        public static bool IsIdle()
        {
            return CurrentStance == EStance.None && StoredStance == EStance.None && HasResetActiveAim && HasResetHighReady && HasResetLowReady && HasResetShortStock && HasResetPistolPos && HasResetMelee ? true : false;
        }

        public static void CancelAllStances()
        {
            StanceBlender.Target = 0f;
            CurrentStance = EStance.None;
            StoredStance = EStance.None;
            DidStanceWiggle = false;
            WasActiveAim = false;
            IsLeftShoulder = false;
        }

        private static void StanceManipCancelTimer()
        {
            ManipTime += Time.deltaTime;

            if (ManipTime >= ManipTimer)
            {
                CancelHighReady = false;
                ModifyHighReady = false;
                CancelLowReady = false;
                CancelShortStock = false;
                CancelPistolStance = false;
                CancelActiveAim = false;
                ShouldResetStances = false;
                CancelLeftShoulder = false;
                ManipTimer = 0.25f;
                ManipTime = 0f;
            }
        }

        private static void StanceDampingTimer()
        {
            DampingTimer += Time.deltaTime;

            if (DampingTimer >= 0.01f) //0.05f
            {
                CanResetDamping = true;
                DoDampingTimer = false;
                DampingTimer = 0f;
            }
        }

        public static void StanceShotTimer()
        {
            StanceShotTime += Time.deltaTime;

            if (StanceShotTime >= 0.55f)
            {
                IsFiringFromStance = false;
                StanceShotTime = 0f;
            }
        }

        private static void MeleeCooldownTimer()
        {
            MeleeTimer += Time.deltaTime;

            if (MeleeTimer >= 0.25f)
            {
                _doMeleeReset = false;
                MeleeIsToggleable = true;
                MeleeTimer = 0f;
            }
        }

        private static void DoMeleeEffect()
        {
            Player player = Singleton<GameWorld>.Instance.MainPlayer;
            Player.FirearmController fc = player.HandsController as Player.FirearmController;
            if (WeaponStats.HasBayonet)
            {

                int rndNum = UnityEngine.Random.Range(1, 11);
                string track = rndNum <= 5 ? "knife_1.wav" : "knife_2.wav";
                Singleton<BetterAudio>.Instance.PlayAtPoint(player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim.position, Plugin.RealismAudioController.HitAudioClips[track], 2, BetterAudio.AudioSourceGroupType.Distant, 100, 2, EOcclusionTest.Continuous);
            }
            player.Physical.ConsumeAsMelee(2f + (WeaponStats.ErgoFactor / 100f));
        }

        private static void ToggleStance(EStance targetStance, bool setPrevious = false, bool setPrevisousAsCurrent = false)
        {
            _previousStance = _currentStance;
            if (IsLeftShoulder) IsLeftShoulder = false;
            if (setPrevious) StoredStance = CurrentStance;
            if (CurrentStance == targetStance) CurrentStance = EStance.None;
            else CurrentStance = targetStance;
            if (setPrevisousAsCurrent) StoredStance = CurrentStance;
        }

        private static void ToggleHighReady()
        {
            StanceBlender.Target = StanceBlender.Target == 0f ? 1f : 0f;
            ToggleStance(EStance.HighReady, false, true);
            WasActiveAim = false;
            DidStanceWiggle = false;

            if (CurrentStance == EStance.HighReady && (Plugin.RealHealthController.HealthConditionForcedLowReady))
            {
                CanDoHighReadyInjuredAnim = true;
            }
        }

        private static void ToggleLowReady()
        {
            StanceBlender.Target = StanceBlender.Target == 0f ? 1f : 0f;
            ToggleStance(EStance.LowReady, false, true);
            WasActiveAim = false;
            DidStanceWiggle = false;
        }

        private static void HandleScrollInput(float scrollIncrement)
        {
            if (scrollIncrement == -1)
            {
                if (CurrentStance == EStance.HighReady)
                {
                    ToggleHighReady();
                }
                else if (CurrentStance != EStance.LowReady && HasResetHighReady)
                {
                    ToggleLowReady();
                }
            }
            if (scrollIncrement == 1 && CurrentStance != EStance.HighReady)
            {
                if (CurrentStance == EStance.LowReady && !Plugin.RealHealthController.HealthConditionForcedLowReady)
                {
                    ToggleLowReady();
                }
                else if (CurrentStance != EStance.HighReady && HasResetLowReady)
                {
                    ToggleHighReady();
                }
            }
        }

        public static void ToggleLeftShoulder()
        {
            IsLeftShoulder = !IsLeftShoulder;
            if (!TreatWeaponAsPistolStance)
            {
                CurrentStance = EStance.None;
                StoredStance = EStance.None;
                WasActiveAim = false;
                HaveSetActiveAim = false;
                DidStanceWiggle = false;
                StanceBlender.Target = 0f;
            }
        }

        public static void StanceUpdate()
        {
            if (Utils.WeaponIsReady && Utils.GetYourPlayer().MovementContext.CurrentState.Name != EPlayerState.Stationary)
            {
                if (DoDampingTimer)
                {
                    StanceDampingTimer();
                }

                if (_doMeleeReset)
                {
                    MeleeCooldownTimer();
                }

                //patrol
                if (!ShouldBlockAllStances && Input.GetKeyDown(PluginConfig.PatrolKeybind.Value.MainKey) && PluginConfig.PatrolKeybind.Value.Modifiers.All(Input.GetKey))
                {
                    Utils.GetYourPlayer().method_58(0.5f);
                    ToggleStance(EStance.PatrolStance);
                    StoredStance = EStance.None;
                    StanceBlender.Target = 0f;
                    DidStanceWiggle = false;
                }

                if (!PlayerState.IsSprinting && !PlayerState.IsInInventory && !TreatWeaponAsPistolStance)
                {
                    //cycle stances
                    if (!ShouldBlockAllStances && Input.GetKeyUp(PluginConfig.CycleStancesKeybind.Value.MainKey))
                    {
                        if (Time.time <= _doubleClickTime)
                        {
                            _clickTriggered = true;
                            StanceBlender.Target = 0f;
                            StanceIndex = 0;
                            CancelAllStances();
                            DidStanceWiggle = false;
                        }
                        else
                        {
                            _clickTriggered = false;
                            _doubleClickTime = Time.time + _clickDelay;
                        }
                    }
                    else if (!_clickTriggered)
                    {
                        if (Time.time > _doubleClickTime)
                        {
                            IsLeftShoulder = false;
                            StanceBlender.Target = 1f;
                            _clickTriggered = true;
                            StanceIndex++;
                            StanceIndex = StanceIndex > 3 ? 1 : StanceIndex;
                            CurrentStance = (EStance)StanceIndex;
                            StoredStance = CurrentStance;
                            DidStanceWiggle = false;
                            if (CurrentStance == EStance.HighReady && Plugin.RealHealthController.HealthConditionForcedLowReady)
                            {
                                CanDoHighReadyInjuredAnim = true;
                            }
                        }
                    }

                    //active aim
                    if (!PluginConfig.ToggleActiveAim.Value)
                    {
                        if ((!IsAiming && !ShouldBlockAllStances && Input.GetKey(PluginConfig.ActiveAimKeybind.Value.MainKey) && PluginConfig.ActiveAimKeybind.Value.Modifiers.All(Input.GetKey)) || (Input.GetKey(KeyCode.Mouse1) && !PlayerState.IsAllowedADS))
                        {
                            if (!HaveSetActiveAim)
                            {
                                DidStanceWiggle = false;
                            }
                            IsLeftShoulder = false;
                            StanceBlender.Target = 1f;
                            CurrentStance = EStance.ActiveAiming;
                            WasActiveAim = true;
                            HaveSetActiveAim = true;
                        }
                        else if (HaveSetActiveAim)
                        {
                            StanceBlender.Target = 0f;
                            CurrentStance = StoredStance;
                            WasActiveAim = false;
                            HaveSetActiveAim = false;
                            DidStanceWiggle = false;
                        }
                    }
                    else
                    {
                        if ((!IsAiming && !ShouldBlockAllStances && Input.GetKeyDown(PluginConfig.ActiveAimKeybind.Value.MainKey) && PluginConfig.ActiveAimKeybind.Value.Modifiers.All(Input.GetKey)) || (Input.GetKeyDown(KeyCode.Mouse1) && !PlayerState.IsAllowedADS))
                        {
                            StanceBlender.Target = StanceBlender.Target == 0f ? 1f : 0f;
                            ToggleStance(EStance.ActiveAiming);
                            WasActiveAim = CurrentStance == EStance.ActiveAiming ? true : false;
                            DidStanceWiggle = false;
                            if (CurrentStance != EStance.ActiveAiming)
                            {
                                CurrentStance = StoredStance;
                            }
                        }
                    }

                    if (!ShouldBlockAllStances && PluginConfig.UseMouseWheelStance.Value && !IsAiming)
                    {
                        if ((Input.GetKey(PluginConfig.StanceWheelComboKeyBind.Value.MainKey) && PluginConfig.UseMouseWheelPlusKey.Value) || (!PluginConfig.UseMouseWheelPlusKey.Value && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.C)))
                        {
                            float scrollDelta = Input.mouseScrollDelta.y;
                            if (scrollDelta != 0f)
                            {
                                HandleScrollInput(scrollDelta);
                            }
                        }
                    }

                    //Melee
                    if (!IsAiming && MeleeIsToggleable && Input.GetKeyDown(PluginConfig.MeleeKeybind.Value.MainKey) && PluginConfig.MeleeKeybind.Value.Modifiers.All(Input.GetKey))
                    {
                        IsMounting = false;
                        IsLeftShoulder = false;
                        CurrentStance = EStance.Melee;
                        StoredStance = EStance.None;
                        WasActiveAim = false;
                        DidStanceWiggle = false;
                        StanceBlender.Target = 1f;
                        MeleeIsToggleable = false;
                        MeleeHitSomething = false;
                    }

                    //short-stock
                    if (!ShouldBlockAllStances && Input.GetKeyDown(PluginConfig.ShortStockKeybind.Value.MainKey) && PluginConfig.ShortStockKeybind.Value.Modifiers.All(Input.GetKey))
                    {
                        StanceBlender.Target = StanceBlender.Target == 0f ? 1f : 0f;
                        ToggleStance(EStance.ShortStock, false, true);
                        WasActiveAim = false;
                        DidStanceWiggle = false;
                    }

                    //high ready
                    if (!ShouldBlockAllStances && !IsInForcedLowReady && Input.GetKeyDown(PluginConfig.HighReadyKeybind.Value.MainKey) && PluginConfig.HighReadyKeybind.Value.Modifiers.All(Input.GetKey))
                    {
                        ToggleHighReady();
                    }

                    //low ready
                    if (!ShouldBlockAllStances && !IsInForcedLowReady && Input.GetKeyDown(PluginConfig.LowReadyKeybind.Value.MainKey) && PluginConfig.LowReadyKeybind.Value.Modifiers.All(Input.GetKey))
                    {
                        ToggleLowReady();
                    }

                    //cancel if aiming
                    if (IsAiming)
                    {
                        if (CurrentStance == EStance.ActiveAiming || WasActiveAim)
                        {
                            StoredStance = EStance.None;
                        }
                        CurrentStance = EStance.None;
                        HaveSetAiming = true;
                    }
                    else if (HaveSetAiming)
                    {
                        CurrentStance = WasActiveAim ? EStance.ActiveAiming : StoredStance;
                        HaveSetAiming = false;
                    }
                }


                if (ShootController.IsFiring) //stnace specific firing check is too slow
                {
                    bool rememberStance = PluginConfig.RememberStanceFiring.Value && IsAiming;
                    bool isActiveAim = CurrentStance == EStance.ActiveAiming && !IsAiming;
                    bool keepStance = rememberStance || (isActiveAim || CurrentStance == EStance.ShortStock || CurrentStance == EStance.PistolCompressed);

                    if (!keepStance)
                    {
                        CurrentStance = EStance.None;
                        StoredStance = EStance.None;
                        StanceBlender.Target = 0f;
                    }
                }

                if (CanDoHighReadyInjuredAnim)
                {
                    HighReadyBlackedArmTime += Time.deltaTime;
                    if (HighReadyBlackedArmTime >= 0.35f)
                    {
                        CanDoHighReadyInjuredAnim = false;
                        CurrentStance = EStance.LowReady;
                        StoredStance = EStance.LowReady;
                        HighReadyBlackedArmTime = 0f;
                    }
                }

                if (ShouldForceLowReady)
                {
                    StanceBlender.Target = 1f;
                    CurrentStance = EStance.LowReady;
                    StoredStance = EStance.LowReady;
                    WasActiveAim = false;
                    IsLeftShoulder = false;
                    IsInForcedLowReady = true;
                }
                else IsInForcedLowReady = false;
            }

            if (ShouldResetStances)
            {
                StanceManipCancelTimer();
            }

            if (DidWeaponSwap || (!PluginConfig.RememberStanceItem.Value && !Utils.WeaponIsReady) || !Utils.PlayerIsReady)
            {
                IsLeftShoulder = false;
                IsMounting = false;
                CurrentStance = EStance.None;
                StoredStance = EStance.None;
                StanceBlender.Target = 0f;
                StanceIndex = 0;
                WasActiveAim = false;
                DidWeaponSwap = false;
                ResetStanceStamina();
            }
        }

        //I've no idea wtf is going on here but it sort of works
        private static void DoAltPistolAndLeftShoulder(Player player, Player.FirearmController fc, ProceduralWeaponAnimation pwa, float stanceMulti, float dt, Vector3 camTarget)
        {
            float speedFactorTarget = IsAiming && !IsLeftShoulder && _animationTimer == 0f ? PluginConfig.PistolPosResetSpeedMulti.Value * stanceMulti : PluginConfig.PistolPosSpeedMulti.Value * stanceMulti;
            _pistolPosSpeed = Mathf.Lerp(_pistolPosSpeed, speedFactorTarget, dt * 10f);
            float xTarget = !IsBlindFiring && IsLeftShoulder && !CancelLeftShoulder ? -0.08f : !IsBlindFiring ? 0.04f : 0f;
            float zTarget = 0f;

            if (!IsAiming)
            {
                pistolYTarget = -0.04f; // this might not be necessary anymore, just use the pistol offset and don't do this here
                pistolCameraAlignmentTarget = camTarget.y;
            }
            else
            {
                float tolerance = 0.001f;    // how close is "good enough"
                float speed = 0.5f * PluginConfig.PistolPosSpeedMulti.Value * stanceMulti;       // tuning: how fast it corrects

                // Calculate difference
                float error = pistolCameraAlignmentTarget - camTarget.y;

                if (Mathf.Abs(error) > tolerance)
                {
                    // Convert error into a vertical offset
                    // (positive error = move weapon upward, negative = downward)
                    float adjustment = error * speed * dt;

                    pistolYTarget += adjustment;
                }
            }

            //this is sus, needs investigating
            if (!Utils.AreFloatsEqual(_currentPistolXPos, xTarget, 0.05f))
            {
                //pistolYTarget += 0.03f;
                zTarget += 0.05f;
                _animationTimer += 1.9f * stanceMulti * dt;
                _animSpeed = _smoothCurve.Evaluate(_animationTimer);
            }
            else
            {
                _animationTimer = 0f;
                _animSpeed = 1f;
            }

            //_currentPistolXPos = Mathf.SmoothDamp(_currentPistolXPos, xTarget, ref _currentPistolXPosVelocity, 0.25f, speedFactor, dt);
            _currentPistolXPos = Mathf.Lerp(_currentPistolXPos, xTarget, dt * _pistolPosSpeed * _animSpeed);
            _currentPistolYPos = Mathf.Lerp(_currentPistolYPos, pistolYTarget, dt * _pistolPosSpeed); //do not apply animSpeed to pistol
            _currentPistolZPos = Mathf.Lerp(_currentPistolZPos, zTarget, dt * _pistolPosSpeed * _animSpeed);

            _pistolLocalPosition.x = _currentPistolXPos;
            _pistolLocalPosition.y = _currentPistolYPos;
            _pistolLocalPosition.z = _currentPistolZPos;
            pwa.HandsContainer.WeaponRoot.localPosition = _pistolLocalPosition;
        }

        public static void DoPistolStances(bool isThirdPerson, EFT.Animations.ProceduralWeaponAnimation pwa, ref Quaternion stanceRotation, float dt, ref bool hasResetPistolPos, Player player, ref float rotationSpeed, ref bool isResettingPistol, Player.FirearmController fc, Vector3 camTarget)
        {
            bool useThirdPersonStance = isThirdPerson;//  || Plugin.IsUsingFika
            float totalPlayerWeight = PlayerState.TotalModifiedWeightMinusWeapon;
            float playerWeightFactor = 1f + (totalPlayerWeight / 100f);
            float ergoMulti = Mathf.Clamp(WeaponStats.ErgoStanceSpeed * Mathf.Pow(WeaponStats.TotalWeaponHandlingModi, 0.5f), 0.65f, 1.45f);
            float stanceMulti = Mathf.Clamp(ergoMulti * PlayerState.StanceInjuryMulti * Plugin.RealHealthController.AdrenalineStanceBonus * (Mathf.Max(PlayerState.RemainingArmStamFactor, 0.55f)), 0.5f, 1.45f);

            //float balanceFactor = 1f + (WeaponStats.Balance / 100f);
            // float rotationBalanceFactor = WeaponStats.Balance <= -9f ? -balanceFactor : balanceFactor;
            //float wiggleBalanceFactor = Mathf.Abs(WeaponStats.Balance) > 4f ? balanceFactor : Mathf.Abs(WeaponStats.Balance) <= 4f ? 0.75f : Mathf.Abs(WeaponStats.Balance) <= 3f ? 0.5f : 0.25f;
            float resetErgoMulti = (1f - stanceMulti) + 1f;

            float wiggleErgoMulti = Mathf.Clamp((WeaponStats.ErgoStanceSpeed * 0.25f), 0.1f, 1f);
            WiggleReturnSpeed = (1f - (PlayerState.AimSkillADSBuff * 0.5f)) * wiggleErgoMulti * PlayerState.StanceInjuryMulti * playerWeightFactor * (Mathf.Max(PlayerState.RemainingArmStamFactor, 0.65f));

            float movementFactor = PlayerState.IsMoving ? 0.8f : 1f;

            Quaternion pistolRevertQuaternion = Quaternion.Euler(PluginConfig.PistolResetRotation.Value); // * rotationBalanceFactor
            Vector3 pistolPMCTargetPosition = useThirdPersonStance ? PluginConfig.PistolThirdPersonPosition.Value : PluginConfig.PistolOffset.Value;
            Vector3 pistolScavTargetPosition = useThirdPersonStance ? new Vector3(0.01f, 0.025f, -0.015f) : new Vector3(0.01f, 0.025f, -0.015f);
            Vector3 pistolTargetPosition = PlayerState.IsScav ? pistolScavTargetPosition : pistolPMCTargetPosition;
            Vector3 pistolPMCTargetRotation = useThirdPersonStance ? PluginConfig.PistolThirdPersonRotation.Value : PluginConfig.PistolRotation.Value;
            Vector3 pistolScavTargetRotation = useThirdPersonStance ? new Vector3(2f, -10f, 0f) : new Vector3(2f, -10f, 0f);
            Vector3 pistolTargetRotation = PlayerState.IsScav ? pistolScavTargetRotation : pistolPMCTargetRotation;
            Quaternion pistolTargetQuaternion = Quaternion.Euler(pistolTargetRotation);
            Quaternion pistolMiniTargetQuaternion = Quaternion.Euler(PluginConfig.PistolAdditionalRotation.Value);

            //I've no idea wtf is going on here but it sort of works
            if (!WeaponStats.HasShoulderContact && PluginConfig.EnableAltPistol.Value)
            {
                DoAltPistolAndLeftShoulder(player, fc, pwa, stanceMulti, dt, camTarget);
            }

            if (CurrentStance == EStance.PatrolStance) return;

            if (!pwa.IsAiming && !IsBlindFiring && !PistolIsColliding && !WeaponStats.HasShoulderContact && PluginConfig.EnableAltPistol.Value) //!CancelPistolStance && !pwa.LeftStance
            {
                if (CurrentStance == EStance.PatrolStance || _previousStance == EStance.PatrolStance) _SkipPistolWiggle = true;
                CurrentStance = EStance.PistolCompressed;
                StoredStance = EStance.None;
                isResettingPistol = false;
                hasResetPistolPos = false;

                StanceBlender.Speed = PluginConfig.PistolPosSpeedMulti.Value * stanceMulti;
                StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, pistolTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * stanceMulti * dt);

                if (StanceBlender.Value < 1f)
                {
                    rotationSpeed = 4f * stanceMulti * dt * PluginConfig.PistolAdditionalRotationSpeedMulti.Value * stanceMulti;
                    stanceRotation = pistolMiniTargetQuaternion;
                }
                else
                {
                    rotationSpeed = 4f * stanceMulti * dt * PluginConfig.PistolRotationSpeedMulti.Value * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f);
                    stanceRotation = pistolTargetQuaternion;
                }

                if (StanceTargetPosition == pistolTargetPosition && StanceBlender.Value >= 1f && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != pistolTargetPosition || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                if (StanceBlender.Value < 0.95f || CancelPistolStance)
                {
                    DidStanceWiggle = false;
                }
                if ((StanceBlender.Value >= 1f && StanceTargetPosition == pistolTargetPosition) && !DidStanceWiggle)
                {
                    if (!_SkipPistolWiggle) DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-12.5f, 5f, 1f) * movementFactor);
                    DidStanceWiggle = true;
                    CancelPistolStance = false;
                    _SkipPistolWiggle = false;
                }

            }
            else if (StanceBlender.Value > 0f && !hasResetPistolPos && !PistolIsColliding)
            {
                CanResetDamping = false;

                isResettingPistol = true;
                rotationSpeed = 4f * stanceMulti * dt * PluginConfig.PistolResetRotationSpeedMulti.Value * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f);
                stanceRotation = pistolRevertQuaternion;
                StanceBlender.Speed = PluginConfig.PistolPosResetSpeedMulti.Value * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
            }
            else if (StanceBlender.Value == 0f && !hasResetPistolPos && !PistolIsColliding)
            {
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }

                DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-10f, 0f, -20f) * movementFactor); //new Vector3(10f, 1f, -30f) * wiggleBalanceFactor * rotationBalanceFactor  * wiggleBalanceFactor

                isResettingPistol = false;
                CurrentStance = EStance.None;
                stanceRotation = Quaternion.identity;
                hasResetPistolPos = true;
            }
        }

        private static void DoRiflePosAndLeftShoulder(Player player, Player.FirearmController fc, ProceduralWeaponAnimation pwa, float stanceMulti, float movementFactor, float dt, ref float rotationSpeed, ref Quaternion stanceRotation, Vector3 camTarget)
        {
            bool doAltRifle = PluginConfig.EnableAltRifle.Value;

            float stanceFactor = Mathf.Min(stanceMulti, 0.6f);
            float ySpeedFactor = doAltRifle && IsAiming ? 5f : doAltRifle ? stanceFactor : 1f;
            float shoulderSpeed = 3f * stanceFactor;
            float xTarget = IsLeftShoulder && !CancelLeftShoulder ? PluginConfig.LeftShoulderOffset.Value + WeaponOffsetPosition.x : doAltRifle && IsAiming ? 0.075f : WeaponOffsetPosition.x;
            float yTarget = doAltRifle && IsAiming ? -0.05f : WeaponOffsetPosition.y;
            float zTarget = WeaponOffsetPosition.z;

            if (!Utils.AreFloatsEqual(_currentRifleXPos, xTarget, 0.05f))
            {
                zTarget += 0.08f;
                yTarget += 0.05f;
                _animationTimer += dt * 3f * stanceFactor;
                _animSpeed = _smoothCurve.Evaluate(_animationTimer);
            }
            else
            {
                zTarget = WeaponOffsetPosition.z;
                _animationTimer = 0f;
                _animSpeed = 1f;
            }

            if (!Utils.AreFloatsEqual(_currentRifleXPos, xTarget, 0.045f))
            {
                rotationSpeed = 2.1f * stanceFactor * _animSpeed * dt;
                stanceRotation = Quaternion.Euler(new Vector3(-50f, 150f * (IsLeftShoulder ? 1f : -1f), -40f * (IsLeftShoulder ? 1f : -1f)) * 0.2f);
                DoLeftShoulderTransition = true;
            }
            else if (DoLeftShoulderTransition)
            {
                DoLeftShoulderTransition = false;
                DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-2f, 2f, 10f) * movementFactor, true);
            }

            _currentRifleXPos = Mathf.Lerp(_currentRifleXPos, xTarget, dt * shoulderSpeed * 3.5f * _animSpeed);
            _currentRifleYPos = Mathf.Lerp(_currentRifleYPos, yTarget + rifleYTarget, dt * ySpeedFactor * _animSpeed); //if trying to fix stance ADS, animspeed might be fucking with things
            _currentRifleZPos = Mathf.Lerp(_currentRifleZPos, zTarget, dt * shoulderSpeed * _animSpeed);

            _rifleLocalPosition.x = _currentRifleXPos;
            _rifleLocalPosition.y = _currentRifleYPos;
            _rifleLocalPosition.z = _currentRifleZPos;
            pwa.HandsContainer.WeaponRoot.localPosition = _rifleLocalPosition;
        }

        private static void DoTacSprint(Player.FirearmController fc, Player player)
        {
            if (CanDoTacSprint)
            {
                IsDoingTacSprint = true;
                player.BodyAnimatorCommon.SetFloat(PlayerAnimator.WEAPON_SIZE_MODIFIER_PARAM_HASH, 2f);
                _tacSprintTime = 0f;
                _canDoTacSprintTimer = true;
            }
            else if (PluginConfig.EnableTacSprint.Value && _canDoTacSprintTimer)
            {
                _tacSprintTime += Time.deltaTime;
                if (_tacSprintTime >= 0.5f)
                {
                    player.BodyAnimatorCommon.SetFloat(PlayerAnimator.WEAPON_SIZE_MODIFIER_PARAM_HASH, WeaponStats.TotalWeaponLength);
                    _tacSprintTime = 0f;
                    _canDoTacSprintTimer = false;
                }
                IsDoingTacSprint = false;
            }
            else
            {
                IsDoingTacSprint = false;
            }
        }

        public static void DoRifleStances(Player player, Player.FirearmController fc, bool isThirdPerson, EFT.Animations.ProceduralWeaponAnimation pwa, ref Quaternion stanceRotation, float dt, ref bool isResettingShortStock, ref bool hasResetShortStock, ref bool hasResetLowReady, ref bool hasResetActiveAim, ref bool hasResetHighReady, ref bool isResettingHighReady, ref bool isResettingLowReady, ref bool isResettingActiveAim, ref float rotationSpeed, ref bool hasResetMelee, ref bool isResettingMelee, ref bool didHalfMeleeAnim, Vector3 camTarget)
        {
            float weightLimit = 8f;
            float movementFactor = PlayerState.IsMoving ? 1.1f : 1f;
            float chonkerFactor = WeaponStats.TotalWeaponWeight >= weightLimit ? 0.7f : 1f;
            bool useThirdPersonStance = isThirdPerson; // || Plugin.IsUsingFika
            float totalPlayerWeight = PlayerState.TotalModifiedWeightMinusWeapon;
            float playerWeightFactor = 1f + (totalPlayerWeight / 150f);
            float lowerBaseLimit = WeaponStats.TotalWeaponWeight >= weightLimit ? 0.45f : 0.55f;
            float ergoMulti = Mathf.Clamp(1.15f * WeaponStats.ErgoStanceSpeed * Mathf.Pow(WeaponStats.TotalWeaponHandlingModi, 0.4f), lowerBaseLimit, 1.2f);
            float lowerSpeedLimit = WeaponStats.TotalWeaponWeight >= weightLimit ? 0.3f : 0.4f;
            float stanceMulti = Mathf.Clamp(ergoMulti * PlayerState.StanceInjuryMulti * Plugin.RealHealthController.AdrenalineStanceBonus * (Mathf.Max(PlayerState.RemainingArmStamFactor, 0.65f)), lowerSpeedLimit, 1.18f);
            float resetErgoMulti = (1f - stanceMulti) + 1f;
            float highReadyStanceMulti = Mathf.Clamp(stanceMulti, 0.5f, 0.98f);
            float lowReadyStanceMulti = Mathf.Clamp(stanceMulti, 0.5f, 0.98f);
            float highReadyXWiggleFactor = WeaponStats.TotalErgo <= 49f ? -1f : 1f;
            float highReadyZWiggleFactor = WeaponStats.TotalErgo <= 40f ? 1f : 2f;
            bool pauseStance = PlayerState.IsInInventory || IsBlindFiring || IsLeftShoulder;
            float wiggleErgoMulti = Mathf.Clamp(WeaponStats.ErgoStanceSpeed * 0.5f, 0.1f, 1f);
            float stocklessModifier = WeaponStats.HasShoulderContact ? 1f : 0.5f;
            WiggleReturnSpeed = (1f - (PlayerState.AimSkillADSBuff * 0.5f)) * wiggleErgoMulti * PlayerState.StanceInjuryMulti * stocklessModifier * playerWeightFactor * (Mathf.Max(PlayerState.RemainingArmStamFactor, 0.55f));

            Vector3 activeTargetRoation = useThirdPersonStance ?
                 PluginConfig.ActiveThirdPersonRotation.Value :
                 PluginConfig.ActiveAimRotation.Value;

            Quaternion activeAimMiniTargetQuaternion = Quaternion.Euler(PluginConfig.ActiveAimAdditionalRotation.Value * resetErgoMulti);

            Quaternion activeAimRevertQuaternion = Quaternion.Euler(PluginConfig.ActiveAimResetRotation.Value * resetErgoMulti);

            Vector3 activeAimTargetPosition = useThirdPersonStance ?
                PluginConfig.ActiveThirdPersonPosition.Value :
                PluginConfig.ActiveAimOffset.Value;

            Quaternion activeAimTargetQuaternion = Quaternion.Euler(activeTargetRoation);

            Vector3 lowTargetRotation = useThirdPersonStance ?
                PluginConfig.LowReadyThirdPersonRotation.Value :
                new Vector3(
                    PluginConfig.LowReadyRotation.Value.x * resetErgoMulti,
                    PluginConfig.LowReadyRotation.Value.y,
                    PluginConfig.LowReadyRotation.Value.z);

            Quaternion lowReadyTargetQuaternion = Quaternion.Euler(lowTargetRotation);

            Quaternion lowReadyMiniTargetQuaternion = Quaternion.Euler(PluginConfig.LowReadyAdditionalRotation.Value * resetErgoMulti);

            Quaternion lowReadyRevertQuaternion = Quaternion.Euler(PluginConfig.LowReadyResetRotation.Value * resetErgoMulti);

            Vector3 lowReadyTargetPosition = useThirdPersonStance ?
                PluginConfig.LowReadyThirdPersonPosition.Value :
                PluginConfig.LowReadyOffset.Value;

            Vector3 highTargetRotation = useThirdPersonStance ?
                PluginConfig.HighReadyThirdPersonRotation.Value :
                new Vector3(
                    PluginConfig.HighReadyRotation.Value.x * stanceMulti,
                    PluginConfig.HighReadyRotation.Value.y * stanceMulti * (ModifyHighReady ? -1f : 1f),
                    PluginConfig.HighReadyRotation.Value.z * stanceMulti);

            Vector3 highReadyTargetPosition = useThirdPersonStance ?
                PluginConfig.HighReadyThirdPersonPosition.Value :
                new Vector3(
                    PluginConfig.HighReadyOffset.Value.x,
                    PluginConfig.HighReadyOffset.Value.y * (ModifyHighReady ? 0.25f : 1f),
                    PluginConfig.HighReadyOffset.Value.z);

            Quaternion highReadyTargetQuaternion = Quaternion.Euler(highTargetRotation);

            Quaternion highReadyMiniTargetQuaternion = Quaternion.Euler(PluginConfig.HighReadyAdditionalRotation.Value * resetErgoMulti);

            Quaternion highReadyRevertQuaternion = Quaternion.Euler(PluginConfig.HighReadyResetRotation.Value * resetErgoMulti);

            Vector3 shortTargetRotation = useThirdPersonStance ?
                PluginConfig.ShortStockThirdPersonRotation.Value :
                PluginConfig.ShortStockRotation.Value * stanceMulti;

            Quaternion shortStockTargetQuaternion = Quaternion.Euler(shortTargetRotation);

            Quaternion shortStockMiniTargetQuaternion = Quaternion.Euler(PluginConfig.ShortStockAdditionalRotation.Value * resetErgoMulti);

            Quaternion shortStockRevertQuaternion = Quaternion.Euler(PluginConfig.ShortStockResetRotation.Value * resetErgoMulti);

            Vector3 shortStockTargetPosition = useThirdPersonStance ?
                PluginConfig.ShortStockThirdPersonPosition.Value :
                PluginConfig.ShortStockOffset.Value;

            Quaternion meleeInitialQuaternion = Quaternion.Euler(new Vector3(2.5f * resetErgoMulti, -15f * resetErgoMulti, -1f));
            Quaternion meleeFinalQuaternion = Quaternion.Euler(new Vector3(-1.5f * resetErgoMulti, -7.5f * resetErgoMulti, -0.5f));
            Vector3 meleeInitialPos = new Vector3(0f, 0.06f, 0f);
            Vector3 meleeFinalPos = new Vector3(0f, -0.0275f, 0f);

            //for setting baseline position
            if (!IsBlindFiring) // && !pwa.LeftStance
            {
                DoRiflePosAndLeftShoulder(player, fc, pwa, stanceMulti, movementFactor, dt, ref rotationSpeed, ref stanceRotation, camTarget);
            }

            DoTacSprint(fc, player);

            ////short-stock////
            if (CurrentStance == EStance.ShortStock && !pwa.IsAiming && !CancelShortStock && !IsBlindFiring && !pwa.LeftStance && !PlayerState.IsSprinting && !pauseStance)
            {
                float activeToShort = 1f;
                float highToShort = 1f;
                float lowToShort = 1f;
                isResettingShortStock = false;
                hasResetShortStock = false;
                hasResetMelee = true;

                if (StanceTargetPosition != shortStockTargetPosition)
                {
                    if (!hasResetActiveAim)
                    {
                        activeToShort = 0.55f;
                    }
                    if (!hasResetHighReady)
                    {
                        highToShort = 0.78f;
                    }
                    if (!hasResetLowReady)
                    {
                        lowToShort = 0.55f;
                    }
                }
                else
                {
                    hasResetActiveAim = true;
                    hasResetHighReady = true;
                    hasResetLowReady = true;
                }

                if (StanceTargetPosition == shortStockTargetPosition && StanceBlender.Value >= 1f && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != shortStockTargetPosition || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                float transitionPositionFactor = activeToShort * highToShort * lowToShort;
                float transitionRotationFactor = activeToShort * highToShort * lowToShort;

                if (StanceBlender.Value < 1f)
                {
                    rotationSpeed = 4f * stanceMulti * dt * PluginConfig.ShortStockAdditionalRotationSpeedMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f) * transitionRotationFactor;
                    stanceRotation = shortStockMiniTargetQuaternion;
                }
                else
                {
                    rotationSpeed = 4f * stanceMulti * dt * PluginConfig.ShortStockRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f) * transitionRotationFactor;
                    stanceRotation = shortStockTargetQuaternion;
                }

                StanceBlender.Speed = PluginConfig.ShortStockSpeedMulti.Value * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
                StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, shortStockTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * stanceMulti * transitionPositionFactor * dt);

                if ((StanceBlender.Value >= 0.9f || StanceTargetPosition == shortStockTargetPosition) && !DidStanceWiggle && !useThirdPersonStance)
                {
                    DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(5f, -2.5f, 30f) * movementFactor, true);
                    DidStanceWiggle = true;
                }
            }
            else if (StanceBlender.Value > 0f && !hasResetShortStock && CurrentStance != EStance.LowReady && CurrentStance != EStance.ActiveAiming && CurrentStance != EStance.HighReady && !isResettingActiveAim && !isResettingHighReady && !isResettingLowReady && !isResettingMelee)
            {
                CanResetDamping = false;
                isResettingShortStock = true;
                rotationSpeed = 4f * stanceMulti * dt * PluginConfig.ShortStockResetRotationSpeedMulti.Value;
                stanceRotation = shortStockRevertQuaternion;
                StanceBlender.Speed = PluginConfig.ShortStockResetSpeedMulti.Value * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
            }
            else if (StanceBlender.Value == 0f && !hasResetShortStock)
            {
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }

                if (!useThirdPersonStance) DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-4f, -2f, -30f) * movementFactor, true);
                DidStanceWiggle = false;
                stanceRotation = Quaternion.identity;
                isResettingShortStock = false;
                hasResetShortStock = true;
            }

            ////high ready////
            if (CurrentStance == EStance.HighReady && !pwa.IsAiming && !IsFiringFromStance && !CancelHighReady && !pauseStance)
            {
                float shortToHighMulti = 1.0f;
                float lowToHighMulti = 1.0f;
                float activeToHighMulti = 1.0f;
                isResettingHighReady = false;
                hasResetHighReady = false;
                hasResetMelee = true;

                if (StanceTargetPosition != highReadyTargetPosition)
                {
                    if (!hasResetShortStock)
                    {
                        shortToHighMulti = 0.82f;
                    }
                    if (!hasResetActiveAim)
                    {
                        activeToHighMulti = 1f;
                    }
                    if (!hasResetLowReady)
                    {
                        lowToHighMulti = 1f;
                    }
                }
                else
                {
                    hasResetActiveAim = true;
                    hasResetLowReady = true;
                    hasResetShortStock = true;
                }

                if (StanceTargetPosition == highReadyTargetPosition && StanceBlender.Value == 1 && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != highReadyTargetPosition || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                float transitionPositionFactor = shortToHighMulti * lowToHighMulti * activeToHighMulti;
                float transitionRotationFactor = shortToHighMulti * lowToHighMulti * activeToHighMulti * (transitionPositionFactor != 1f ? 0.9f : 1f);

                if (CanDoHighReadyInjuredAnim)
                {
                    if (StanceBlender.Value < 0.3f)
                    {
                        rotationSpeed = 3f * highReadyStanceMulti * dt * PluginConfig.HighReadyRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.7f : 1f) * (WeaponStats.IsPistol ? 0.5f : 1f);
                        stanceRotation = lowReadyTargetQuaternion;
                    }
                    else
                    {
                        rotationSpeed = 3f * highReadyStanceMulti * dt * PluginConfig.HighReadyAdditionalRotationSpeedMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.2f : 1f) * (WeaponStats.IsPistol ? 0.5f : 1f);
                        stanceRotation = highReadyMiniTargetQuaternion;
                    }
                }
                else
                {
                    if (StanceBlender.Value < 0.3f)
                    {
                        rotationSpeed = 4f * highReadyStanceMulti * dt * PluginConfig.HighReadyAdditionalRotationSpeedMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.2f : 1f) * transitionRotationFactor * (WeaponStats.IsPistol ? 0.5f : 1f);
                        stanceRotation = highReadyMiniTargetQuaternion;
                    }
                    else
                    {
                        rotationSpeed = 4f * highReadyStanceMulti * dt * PluginConfig.HighReadyRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.7f : 1f) * transitionRotationFactor * (WeaponStats.IsPistol ? 0.5f : 1f);
                        stanceRotation = highReadyTargetQuaternion;
                    }
                }

                StanceBlender.Speed = PluginConfig.HighReadySpeedMulti.Value * highReadyStanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
                StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, highReadyTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * highReadyStanceMulti * transitionPositionFactor * dt);

                if ((StanceBlender.Value >= 1f || StanceTargetPosition == highReadyTargetPosition) && !DidStanceWiggle && !useThirdPersonStance)
                {
                    if (!WeaponStats.IsPistol) DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(5f, 5f, 5f) * movementFactor, true);//new Vector3(11f, 5.5f, 50f)
                    DidStanceWiggle = true;
                }
            }
            else if (StanceBlender.Value > 0f && !hasResetHighReady && CurrentStance != EStance.LowReady && CurrentStance != EStance.ActiveAiming && CurrentStance != EStance.ShortStock && !isResettingActiveAim && !isResettingLowReady && !isResettingShortStock && !isResettingMelee)
            {
                CanResetDamping = false;
                isResettingHighReady = true;
                rotationSpeed = 4f * highReadyStanceMulti * dt * PluginConfig.HighReadyResetRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f);
                stanceRotation = highReadyRevertQuaternion;
                StanceBlender.Speed = PluginConfig.HighReadyResetSpeedMulti.Value * highReadyStanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
            }
            else if (StanceBlender.Value <= 0f && !hasResetHighReady)
            {
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }

                if (!useThirdPersonStance && !WeaponStats.IsPistol) DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(highReadyXWiggleFactor * 10f, highReadyXWiggleFactor * 1f, highReadyZWiggleFactor * -10f) * movementFactor, true); //(1.5f, 3.75f, -30)
                DidStanceWiggle = false;
                stanceRotation = Quaternion.identity;
                isResettingHighReady = false;
                hasResetHighReady = true;
            }

            ////low ready////
            if (CurrentStance == EStance.LowReady && !pwa.IsAiming && !IsFiringFromStance && !CancelLowReady && !pauseStance)
            {
                float highToLow = 1.0f;
                float shortToLow = 1.0f;
                float activeToLow = 1.0f;
                isResettingLowReady = false;
                hasResetLowReady = false;
                hasResetMelee = true;

                if (StanceTargetPosition != lowReadyTargetPosition)
                {
                    if (!hasResetHighReady)
                    {
                        highToLow = 0.95f;
                    }
                    if (!hasResetShortStock)
                    {
                        shortToLow = 0.7f;
                    }
                    if (!hasResetActiveAim)
                    {
                        activeToLow = 0.87f;
                    }
                }
                else
                {
                    hasResetHighReady = true;
                    hasResetShortStock = true;
                    hasResetActiveAim = true;
                }

                if (StanceTargetPosition == lowReadyTargetPosition && StanceBlender.Value >= 1f && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != lowReadyTargetPosition || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                float transitionPositionFactor = highToLow * shortToLow * activeToLow;
                float transitionRotationFactor = highToLow * shortToLow * activeToLow * (transitionPositionFactor != 1f ? 1.025f : 1f);

                if (StanceBlender.Value < 1f)
                {
                    rotationSpeed = 4f * lowReadyStanceMulti * dt * PluginConfig.LowReadyAdditionalRotationSpeedMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.8f : 1f) * transitionRotationFactor;
                    stanceRotation = lowReadyMiniTargetQuaternion;
                }
                else
                {
                    rotationSpeed = 4f * lowReadyStanceMulti * dt * PluginConfig.LowReadyRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.8f : 1f) * transitionRotationFactor;
                    stanceRotation = lowReadyTargetQuaternion;
                }

                StanceBlender.Speed = PluginConfig.LowReadySpeedMulti.Value * lowReadyStanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value * 0.8f : 1f);
                StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, lowReadyTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * lowReadyStanceMulti * transitionPositionFactor * dt);

                if ((StanceBlender.Value >= 0.5f || StanceTargetPosition == lowReadyTargetPosition) && !DidStanceWiggle && !useThirdPersonStance)
                {
                    DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(7f, 7f, 0f) * movementFactor, true);
                    DidStanceWiggle = true;
                }
                DidLowReadyResetStanceWiggle = false;
            }
            else if (StanceBlender.Value > 0f && !hasResetLowReady && CurrentStance != EStance.ActiveAiming && CurrentStance != EStance.HighReady && CurrentStance != EStance.ShortStock && !isResettingActiveAim && !isResettingHighReady && !isResettingShortStock && !isResettingMelee)
            {
                CanResetDamping = false;

                isResettingLowReady = true;
                rotationSpeed = 4f * lowReadyStanceMulti * dt * PluginConfig.LowReadyResetRotationMulti.Value * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value * 0.8f : 1f);
                stanceRotation = lowReadyRevertQuaternion;

                StanceBlender.Speed = PluginConfig.LowReadyResetSpeedMulti.Value * lowReadyStanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value * 0.8f : 1f);

                if (!useThirdPersonStance && StanceBlender.Value <= 0.65f && !DidLowReadyResetStanceWiggle)
                {
                    DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-10f, 4f, 10f) * movementFactor, true); //new Vector3(-4f, 2.5f, 10f)
                    DidLowReadyResetStanceWiggle = true;
                }
            }
            else if (StanceBlender.Value == 0f && !hasResetLowReady)
            {
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                stanceRotation = Quaternion.identity;
                isResettingLowReady = false;
                hasResetLowReady = true;
            }

            ////active aiming////
            if (CurrentStance == EStance.ActiveAiming && !CancelActiveAim && !pauseStance)
            {
                float ergoFactor = WeaponStats.TotalErgo <= 40f ? 0.75f : 1f;
                float shortToActive = 1f;
                float shortToActiveRotation = 1f;
                float highToActive = 1f;
                float lowToActive = 1f;
                float highToActiveRotation = 1f;
                float lowToActiveRotation = 1f;
                isResettingActiveAim = false;
                hasResetActiveAim = false;
                hasResetMelee = true;

                if (StanceTargetPosition != activeAimTargetPosition)
                {
                    if (!hasResetShortStock)
                    {
                        shortToActive = 0.45f;
                        shortToActiveRotation = 0.9f;
                    }
                    if (!hasResetHighReady)
                    {
                        highToActive = 1.15f;
                        highToActiveRotation = 1.15f;
                    }
                    if (!hasResetLowReady)
                    {
                        lowToActive = 1.29f;
                        lowToActiveRotation = 1.37f;
                    }
                }
                else
                {
                    hasResetShortStock = true;
                    hasResetHighReady = true;
                    hasResetLowReady = true;
                }

                if (StanceTargetPosition == activeAimTargetPosition && StanceBlender.Value == 1 && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != activeAimTargetPosition || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                float transitionPositionFactor = shortToActive * highToActive * lowToActive;
                float transitionRotationFactor = shortToActiveRotation * highToActiveRotation * lowToActiveRotation; //(transitionPositionFactor != 1f ? 0.9f : 1f)

                if (StanceBlender.Value < 1f)
                {
                    StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, activeAimTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * stanceMulti * transitionPositionFactor * dt);
                    rotationSpeed = 4f * stanceMulti * dt * ergoFactor * PluginConfig.ActiveAimAdditionalRotationSpeedMulti.Value * chonkerFactor * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f) * transitionRotationFactor;
                    stanceRotation = activeAimMiniTargetQuaternion;
                }
                else
                {
                    StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, activeAimTargetPosition, PluginConfig.StanceTransitionSpeedMulti.Value * stanceMulti * transitionPositionFactor * dt);
                    rotationSpeed = 4f * stanceMulti * dt * ergoFactor * PluginConfig.ActiveAimRotationMulti.Value * chonkerFactor * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f) * transitionRotationFactor;
                    stanceRotation = activeAimTargetQuaternion;
                }

                StanceBlender.Speed = PluginConfig.ActiveAimSpeedMulti.Value * stanceMulti * ergoFactor * chonkerFactor * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);

                if ((StanceBlender.Value >= 1f || StanceTargetPosition == activeAimTargetPosition) && !DidStanceWiggle && !useThirdPersonStance)
                {
                    DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-10f, -10f, 0f), true);
                    DidStanceWiggle = true;
                }
            }
            else if (StanceBlender.Value > 0f && !hasResetActiveAim && CurrentStance != EStance.LowReady && CurrentStance != EStance.HighReady && CurrentStance != EStance.ShortStock && !isResettingLowReady && !isResettingHighReady && !isResettingShortStock && !isResettingMelee)
            {
                CanResetDamping = false;

                isResettingActiveAim = true;
                rotationSpeed = stanceMulti * dt * PluginConfig.ActiveAimResetRotationSpeedMulti.Value * chonkerFactor * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f);
                stanceRotation = activeAimRevertQuaternion;
                StanceBlender.Speed = PluginConfig.ActiveAimResetSpeedMulti.Value * stanceMulti * chonkerFactor * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
            }
            else if (StanceBlender.Value == 0f && !hasResetActiveAim)
            {
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }

                if (!useThirdPersonStance) DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-5f, 1.5f, 0f) * movementFactor, true);
                DidStanceWiggle = false;

                stanceRotation = Quaternion.identity;

                isResettingActiveAim = false;
                hasResetActiveAim = true;
            }

            ////Melee////
            if (CurrentStance == EStance.Melee && !pwa.IsAiming && !pauseStance) //&& !PlayerValues.IsSprinting
            {
                isResettingMelee = false;
                hasResetMelee = false;
                hasResetActiveAim = true;
                hasResetHighReady = true;
                hasResetLowReady = true;
                hasResetShortStock = true;

                if (StanceTargetPosition == meleeFinalPos && StanceBlender.Value >= 1f && !CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                else if (StanceTargetPosition != meleeFinalPos || StanceBlender.Value < 1)
                {
                    CanResetDamping = false;
                }

                rotationSpeed = 10f * Mathf.Clamp(stanceMulti, 0.8f, 1f) * dt * (useThirdPersonStance ? PluginConfig.ThirdPersonRotationSpeed.Value : 1f);

                float initialPosDistance = Vector3.Distance(StanceTargetPosition, meleeInitialPos);
                float finalPosDistance = Vector3.Distance(StanceTargetPosition, meleeFinalPos);

                bool holdBackStab = Input.GetKey(PluginConfig.MeleeKeybind.Value.MainKey) && WeaponStats.HasBayonet && !MeleeHitSomething;

                if (initialPosDistance > 0.001f && !didHalfMeleeAnim)
                {
                    stanceRotation = meleeInitialQuaternion;
                    StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, meleeInitialPos, PluginConfig.StanceTransitionSpeedMulti.Value * Mathf.Clamp(stanceMulti, 0.75f, 1f) * dt * 1.5f * chonkerFactor);
                }
                else
                {
                    didHalfMeleeAnim = true;
                    if (!holdBackStab)
                    {
                        stanceRotation = meleeFinalQuaternion;
                        StanceTargetPosition = Vector3.Lerp(StanceTargetPosition, meleeFinalPos, PluginConfig.StanceTransitionSpeedMulti.Value * Mathf.Clamp(stanceMulti, 0.75f, 1f) * dt * 2f * chonkerFactor);
                    }
                }

                StanceBlender.Speed = 50f * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);

                if (StanceBlender.Value >= 0.9f && !DidStanceWiggle && !MeleeHitSomething && !holdBackStab) // && finalPosDistance <= 0.001f
                {
                    DoMeleeEffect();
                    DoWiggleEffects(player, pwa, fc.Weapon, new Vector3(-20f, -10f, -90f) * movementFactor, true, 1.5f);
                    DidStanceWiggle = true;
                }

                if (StanceBlender.Value >= 0.9f && didHalfMeleeAnim)
                {
                    CanDoMeleeDetection = true;
                }

                if (StanceBlender.Value >= 1f && finalPosDistance <= 0.001f)
                {
                    CurrentStance = StoredStance;
                    StanceBlender.Target = 0f;
                }
            }
            else if (StanceBlender.Value > 0f && !hasResetMelee) //&& !IsLowReady && !IsActiveAiming && !IsHighReady && !IsShortStock && !isResettingActiveAim && !isResettingHighReady && !isResettingLowReady && !isResettingShortStock
            {
                CanDoMeleeDetection = false;
                CanResetDamping = false;
                isResettingMelee = true;
                rotationSpeed = 10f * stanceMulti * dt;
                stanceRotation = Quaternion.identity;
                StanceBlender.Speed = 15f * stanceMulti * (useThirdPersonStance ? PluginConfig.ThirdPersonPositionSpeed.Value : 1f);
            }
            else if (StanceBlender.Value == 0f && !hasResetMelee)
            {
                _doMeleeReset = true;
                if (!CanResetDamping)
                {
                    DoDampingTimer = true;
                }
                stanceRotation = Quaternion.identity;
                isResettingMelee = false;
                hasResetMelee = true;
                didHalfMeleeAnim = false;
            }

        }

        public static void DoWiggleEffects(Player player, ProceduralWeaponAnimation pwa, Weapon weapon, Vector3 wiggleDirection, bool playSound = false, float volume = 1.05f, float wiggleFactor = 1f, bool isADS = false)
        {
            if (playSound)
            {
                player.method_58(volume);
            }

            NewRecoilShotEffect newRecoil = pwa.Shootingg.CurrentRecoilEffect as NewRecoilShotEffect;
            if (isADS)
            {
                newRecoil.HandRotationRecoil.ReturnTrajectoryDumping = 0.3f * wiggleFactor;
                pwa.Shootingg.CurrentRecoilEffect.HandRotationRecoilEffect.Damping = 0.3f * wiggleFactor;
            }
            player.ProceduralWeaponAnimation.Shootingg.CurrentRecoilEffect.RecoilProcessValues[3].IntensityMultiplicator = 0;
            player.ProceduralWeaponAnimation.Shootingg.CurrentRecoilEffect.RecoilProcessValues[4].IntensityMultiplicator = 0;
            float count = pwa.Shootingg.CurrentRecoilEffect.RecoilProcessValues.Length;
            for (int i = 0; i < count; i++)
            {
                pwa.Shootingg.CurrentRecoilEffect.RecoilProcessValues[i].Process(wiggleDirection);
            }
            player.ProceduralWeaponAnimation.Shootingg.CurrentRecoilEffect.RecoilProcessValues[3].IntensityMultiplicator = 0;
            player.ProceduralWeaponAnimation.Shootingg.CurrentRecoilEffect.RecoilProcessValues[4].IntensityMultiplicator = 0;
        }

        public static void DoPatrolStance(ProceduralWeaponAnimation pwa, Player player)
        {
            Vector3 patrolPos = StanceController.CurrentStance != EStance.PatrolStance ? Vector3.zero : WeaponStats.IsStocklessPistol || WeaponStats.IsMachinePistol ? _pistolPatrolPos : _riflePatrolPos;
            _patrolPos = Vector3.Lerp(_patrolPos, patrolPos, 5.5f * Time.deltaTime);
            pwa.HandsContainer.WeaponRoot.localPosition += _patrolPos;

            Vector3 patrolRot = StanceController.CurrentStance != EStance.PatrolStance ? Vector3.zero : WeaponStats.IsStocklessPistol || WeaponStats.IsMachinePistol ? _pistolPatrolRot : _riflePatrolRot;
            _patrolRot = Vector3.Lerp(_patrolRot, patrolRot, 5.5f * Time.deltaTime);

            Quaternion newRot = Quaternion.identity;
            newRot.x = _patrolRot.x;
            newRot.y = _patrolRot.y;
            newRot.z = _patrolRot.z;
            pwa.HandsContainer.WeaponRoot.localRotation *= newRot;

            if (Vector3.Distance(_patrolPos, Vector3.zero) <= 0.05f) StanceController.FinishedUnPatrolStancing = true;
            else
            {
                StanceController.FinishedUnPatrolStancing = false;
            }
        }

        public static void DoExtraPosAndRot(ProceduralWeaponAnimation pwa, Player player)
        {
            //position
            float stockOffset = !WeaponStats.IsPistol && !WeaponStats.HasShoulderContact ? -0.04f : 0f;
            float stockPosOffset = WeaponStats.StockPosition * 0.01f;
            float posOffsetMulti = WeaponStats.HasShoulderContact ? -0.04f : 0.04f;
            float posePosOffset = (1f - player.MovementContext.PoseLevel) * posOffsetMulti;

            float targetPosXOffset = pwa.IsAiming ? 0f : 0f;
            float targetPosYOffset = pwa.IsAiming ? 0f : 0f;
            float targetPosZOffset = pwa.IsAiming ? 0f : Mathf.Clamp(posePosOffset + stockOffset + stockPosOffset, -0.05f, 0.05f);
            Vector3 targetPos = new Vector3(targetPosXOffset, targetPosYOffset, targetPosZOffset);

            _posePosOffest = Vector3.Lerp(_posePosOffest, targetPos, 5f * Time.deltaTime);
            pwa.HandsContainer.WeaponRoot.localPosition += _posePosOffest;

            //rotation
            bool isMountedWithBipod = WeaponStats.BipodIsDeployed && StanceController.IsMounting;
            bool doCantedSightOffset = Mathf.Abs(pwa.CurrentScope.Rotation) >= EFTHardSettings.Instance.SCOPE_ROTATION_THRESHOLD && StanceController.IsAiming;
            bool doMaskOffset = !doCantedSightOffset && !isMountedWithBipod && (GearController.HasGasMask || (GearController.FSIsActive && GearController.GearBlocksMouth)) && !WeaponStats.WeaponCanFSADS && pwa.IsAiming && WeaponStats.HasShoulderContact && !WeaponStats.IsStocklessPistol && !WeaponStats.IsMachinePistol;
            bool doLongMagOffset = WeaponStats.HasLongMag && player.IsInPronePose && !isMountedWithBipod;
            float cantedOffsetBase = -0.41f;
            float magOffset = doCantedSightOffset ? 0f : doLongMagOffset && !pwa.IsAiming ? -0.35f : doLongMagOffset && pwa.IsAiming ? -0.12f : 0f;
            float ergoOffset = WeaponStats.ErgoFactor * -0.001f;
            float poseRotOffset = (1f - player.MovementContext.PoseLevel) * -0.03f;
            poseRotOffset += player.IsInPronePose ? -0.03f : 0f;
            float maskFactor = doMaskOffset ? -0.025f + ergoOffset : 0f;
            float baseRotOffset = pwa.IsAiming || StanceController.IsMounting || StanceController.IsBracing ? 0f : poseRotOffset + ergoOffset;
            float cantedSightOffset = doCantedSightOffset ? cantedOffsetBase : 0f;

            float rotX = 0f;
            float rotY = Mathf.Clamp(baseRotOffset + maskFactor + magOffset, -0.5f, 0f) + cantedSightOffset;
            float rotZ = 0f;
            Vector3 targetRot = new Vector3(rotX, rotY, rotZ);

            _poseRotOffest = Vector3.Lerp(_poseRotOffest, targetRot, 5f * Time.deltaTime);

            Quaternion newRot = Quaternion.identity;
            newRot.x = _poseRotOffest.x;
            newRot.y = _poseRotOffest.y;
            newRot.z = _poseRotOffest.z;
            pwa.HandsContainer.WeaponRoot.localRotation *= newRot;
        }

        ///
        //thanks and credit to lualeet's deadzone mod for this code, 0 jank compared to Realism's previous mounting system
        ///
        static void SetRotationWrapped(ref float yaw, ref float pitch)
        {
            // I prefer using (-180; 180) euler angle range over (0; 360)
            // However, wrapping the angles is easier with (0; 360), so temporarily cast it
            if (yaw < 0) yaw += 360;
            if (pitch < 0) pitch += 360;

            pitch %= 360;
            yaw %= 360;

            // Now cast it back
            if (yaw > 180) yaw -= 360;
            if (pitch > 180) pitch -= 360;
        }

        static void SetRotationClamped(ref float yaw, ref float pitch, float maxAngle)
        {
            Vector2 clampedVector
                = Vector2.ClampMagnitude(
                    new Vector2(yaw, pitch),
                    maxAngle
                );

            yaw = clampedVector.x;
            pitch = clampedVector.y;
        }

        static void UpdateAimSmoothed(ProceduralWeaponAnimation pwa, float deltaTime)
        {
            _mountAimSmoothed = Mathf.Lerp(_mountAimSmoothed, pwa.IsAiming ? 1f : 0f, deltaTime * 6f);
        }

        static void UpdateMountRotation(Vector2 currentYawPitch, float clamp)
        {
            Quaternion lastRotation = Quaternion.Euler(_lastMountYawPitch.x, _lastMountYawPitch.y, 0);
            Quaternion currentRotation = Quaternion.Euler(currentYawPitch.x, currentYawPitch.y, 0);

            _lastMountYawPitch = currentYawPitch;
            lastRotation = Quaternion.SlerpUnclamped(currentRotation, lastRotation, 0.115f);

            Vector3 delta = _makeQuaternionDelta(lastRotation, currentRotation).eulerAngles;

            _cumulativeMountYaw += delta.x;
            _cumulativeMountPitch += delta.y;

            SetRotationWrapped(ref _cumulativeMountYaw, ref _cumulativeMountPitch);
            SetRotationClamped(ref _cumulativeMountYaw, ref _cumulativeMountPitch, clamp);
        }

        static void ApplyPivotPoint(ProceduralWeaponAnimation pwa, Player player, float pivotPoint, float aimPivot)
        {
            float aimMultiplier = 1f - ((1f - aimPivot) * _mountAimSmoothed);

            Transform weaponRootAnim = pwa.HandsContainer.WeaponRootAnim;

            if (weaponRootAnim == null) return;

            weaponRootAnim.LocalRotateAround(Vector3.up * -pivotPoint, new Vector3(_cumulativeMountPitch * aimMultiplier, 0, _cumulativeMountYaw * aimMultiplier));

            // Not doing this messes up pivot for all offsets after this
            weaponRootAnim.LocalRotateAround(
                Vector3.up * pivotPoint,
                Vector3.zero
            );
        }

        public static void MountingPivotUpdate(Player player, ProceduralWeaponAnimation pwa, float clamp, float deltaTime, float pivotPoint = 0.75f, float aimPivot = 0.25f)
        {
            Vector2 currentYawPitch = new(player.MovementContext.Yaw, player.MovementContext.Pitch);

            UpdateMountRotation(currentYawPitch, clamp);
            UpdateAimSmoothed(pwa, deltaTime);
            ApplyPivotPoint(pwa, player, pivotPoint, aimPivot);
        }

        static readonly System.Diagnostics.Stopwatch aimWatch = new();
        public static float GetDeltaTime()
        {
            float deltaTime = aimWatch.Elapsed.Milliseconds / 1000f;
            aimWatch.Reset();
            aimWatch.Start();
            return deltaTime;
        }

        public static void ToggleMounting(Player player, ProceduralWeaponAnimation pwa, Player.FirearmController fc)
        {
            if (IsMounting && PlayerState.IsMoving)
            {
                IsMounting = false;
            }
        }
    }
}

