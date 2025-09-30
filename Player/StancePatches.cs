﻿using ChartAndGraph;
using Comfort.Common;
using EFT;
using EFT.Animations;
using EFT.Ballistics;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static EFT.Player;
using CollisionLayerClass = GClass3449;
using ReloadClass = EFT.Player.FirearmController.GClass1806;

namespace RealismMod
{
    class SprintPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(PlayerAnimator).GetMethod("EnableSprint", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static bool Prefix(PlayerAnimator __instance, bool enabled)
        {
            if (enabled && StanceController.CanDoMeleeDetection && WeaponStats.HasBayonet)
            {
                __instance.Animator.SetBool(PlayerAnimator.SPRINT_PARAM_HASH, false);
                return false;
            }
            return true;
        }
    }

    public class DisableAimOnReloadPatch : ModulePatch
    {
        private static FieldInfo _playerField;
        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(ReloadClass), "player_0");
            return typeof(ReloadClass).GetMethod("DisableAimingOnReload");
        }

        [PatchPrefix]
        private static bool PatchPreFix(ReloadClass __instance)
        {
            Player player = (Player)_playerField.GetValue(__instance);
            if (player.IsYourPlayer && StanceController.IsMounting)
            {
                return false;
            }
            return true;
        }
    }

    public class ChangeScopePatch : ModulePatch
    {
        private static FieldInfo _playerField;
        private static FieldInfo _weaponManagerClassField;
        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(FirearmController), "_player");
            _weaponManagerClassField = AccessTools.Field(typeof(FirearmController), "weaponManagerClass");
            return typeof(Player.FirearmController).GetMethod("ChangeAimingMode", []);
        }

        [PatchPrefix]
        private static bool PatchPreFix(FirearmController __instance)
        {
            Player player = (Player)_playerField.GetValue(__instance);
            if (player.IsYourPlayer && PluginConfig.OverrideMounting.Value && Plugin.ServerConfig.enable_stances)
            {
                if (WeaponStats.BipodIsDeployed && StanceController.IsMounting)
                {
                    WeaponManagerClass weaponManagerClass = (WeaponManagerClass)_weaponManagerClassField.GetValue(__instance);
                    var scopeIndex = __instance.Item.AimIndex.Value + 1;
                    if (scopeIndex >= weaponManagerClass.ProceduralWeaponAnimation.ScopeAimTransforms.Count)
                    {
                        scopeIndex = 0;
                    }
                    while (scopeIndex != __instance.Item.AimIndex.Value && Mathf.Abs(weaponManagerClass.ProceduralWeaponAnimation.ScopeAimTransforms[scopeIndex].Rotation) >= EFTHardSettings.Instance.SCOPE_ROTATION_THRESHOLD)
                    {
                        scopeIndex++;
                        if (scopeIndex >= weaponManagerClass.ProceduralWeaponAnimation.ScopeAimTransforms.Count)
                        {
                            scopeIndex = 0;
                        }
                    }
                    if (scopeIndex == __instance.Item.AimIndex.Value || Mathf.Abs(weaponManagerClass.ProceduralWeaponAnimation.ScopeAimTransforms[scopeIndex].Rotation) >= EFTHardSettings.Instance.SCOPE_ROTATION_THRESHOLD)
                    {
                        return false;
                    }
                    __instance.Item.AimIndex.Value = scopeIndex;
                    __instance.UpdateSensitivity();
                    player.RaiseSightChangedEvent(player.ProceduralWeaponAnimation.CurrentAimingMod);
                    return false;
                }
            }
            return true;
        }
    }

    public class MountingAndCollisionPatch : ModulePatch
    {
        private static FieldInfo _playerField;
        private static FieldInfo _fcField;
        private static FieldInfo _blendField;
        private static FieldInfo _smoothInField;
        private static FieldInfo _smoothOutField;
        private static float _mountClamp = 0f;
        private static float _collidingModifier = 1f;
        private static float _finalStateEndTimer = 0f;
        private static float _finalStateDelayTimer = 0f;
        private static bool _delayFinalState = false;
        private static float _adsResetTimer = 0f;
        private static float _collisionOverrideTimer = 0f;
        private static float _collisionTimer = 0f;
        private static float _collisionResetTimer = 0f;
        private static float _previousOverlapValue = 0f;
        private static float _currentOverlapValue = 0f;
        private static float _smoothedOverlapValue = 0f;
        private static float _lastDistance = 0f;
        private static bool _isColliding = false;
        private static bool _wasInFinalState = false;
        private static float _stanceFactor = 0f;
        private static float _stanceInverseFactor = 0f;

        private static Vector3 _initialPos = new Vector3(0.01f, -0.075f, -0.13f);
        private static Vector3 _initialRot = new Vector3(-0.025f, 0.005f, 0.005f);
        private static Vector3 _finalPos = Vector3.zero;
        private static Vector3 _finalRot = Vector3.zero;

        private static Vector3 _collisionPos = Vector3.zero;
        private static Vector3 _collisionRot = Vector3.zero;


        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(FirearmController), "_player");
            _fcField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");
            _blendField = AccessTools.Field(typeof(TurnAwayEffector), "_blendSpeed");
            _smoothInField = AccessTools.Field(typeof(TurnAwayEffector), "_inSmoothTime");
            _smoothOutField = AccessTools.Field(typeof(TurnAwayEffector), "_outSmoothTime");
            return typeof(ProceduralWeaponAnimation).GetMethod("AvoidObstacles", BindingFlags.Instance | BindingFlags.Public);
        }

        private static void DoMounting(Player player, ProceduralWeaponAnimation pwa)
        {
            if (StanceController.IsMounting)
            {
                _mountClamp = Mathf.Lerp(_mountClamp, 2.5f, 0.1f);
            }
            else
            {
                _mountClamp = Mathf.Lerp(_mountClamp, 0f, 0.1f);
            }
            float pivotPoint = WeaponStats.BipodIsDeployed ? 1.5f : 0.75f;
            float aimPivot = WeaponStats.BipodIsDeployed ? 0.15f : 0.25f;
            StanceController.MountingPivotUpdate(player, pwa, _mountClamp, StanceController.GetDeltaTime(), pivotPoint, aimPivot);
        }

        private static void ModifyBSGCollisions(ProceduralWeaponAnimation pwa, FirearmController fc)
        {
            _currentOverlapValue = fc.OverlapValue;
            float _smoothingFactor = 0.1f; //0.1f
            _smoothedOverlapValue = _smoothedOverlapValue + _smoothingFactor * (_currentOverlapValue - _smoothedOverlapValue);
            //_smoothedOverlapValue = !__instance.OverlappingAllowsBlindfire ? Mathf.Max(_smoothedOverlapValue, _previousOverlapValue) : Mathf.Min(_smoothedOverlapValue, _previousOverlapValue); //this was for when I was trying my own rotaiton + position

            bool isIncreasing = _smoothedOverlapValue > _previousOverlapValue;
            bool isDecreasing = _smoothedOverlapValue < _previousOverlapValue;
            bool isStable = Utils.AreFloatsEqual(_smoothedOverlapValue, _previousOverlapValue, 0.0001f);
            float normalSpeed = 0.1f * Time.deltaTime; //0.1f
            float delaySpeed = 0.2f * Time.deltaTime; //0.2f
            float slowDown = 0.15f; //0.05
            float resetTime = 2.5f; //2
            float delayTime = 0.15f; //0.1

            if (isStable)
            {
                _collisionTimer = 0;
                _collisionResetTimer = 0f;
                _collidingModifier = Mathf.MoveTowards(_collidingModifier, 1f, normalSpeed);

            }
            else if (isIncreasing)
            {
                _collisionTimer += Time.deltaTime;
                if (_collisionTimer <= delayTime)
                {
                    _collidingModifier = Mathf.MoveTowards(_collidingModifier, slowDown, delaySpeed);
                }
                else
                {
                    _collidingModifier = Mathf.MoveTowards(_collidingModifier, 1f, normalSpeed);
                }

                _collisionResetTimer = 0f;
            }
            else if (isDecreasing)
            {
                _collisionTimer = 0;
                _collisionResetTimer += Time.deltaTime;
                if (_collisionResetTimer <= resetTime)
                {
                    _collidingModifier = Mathf.MoveTowards(_collidingModifier, slowDown, delaySpeed);
                }
                else
                {
                    _collidingModifier = Mathf.MoveTowards(_collidingModifier, 1f, normalSpeed);
                }
            }
            _previousOverlapValue = _smoothedOverlapValue;

            _blendField.SetValue(pwa.TurnAway, 4.5f * _collidingModifier);
            _smoothInField.SetValue(pwa.TurnAway, 14f * _collidingModifier);
            _smoothOutField.SetValue(pwa.TurnAway, 8f * _collidingModifier);
        }

        private static void SetStanceSpeedModi(bool isPistol)
        {
            if (isPistol)
            {
                _stanceFactor = 1f;
                _stanceInverseFactor = 1f;
                StanceController.CameraMovmentForCollisionSpeed = 0.06f;
            }
            else if (StanceController.CurrentStance == EStance.ShortStock || StanceController.StoredStance == EStance.ShortStock)
            {
                _stanceFactor = 1.15f;
                _stanceInverseFactor = 0.85f;
                StanceController.CameraMovmentForCollisionSpeed = 0.1f;
            }
            else if (StanceController.CurrentStance == EStance.HighReady || StanceController.StoredStance == EStance.HighReady)
            {
                _stanceFactor = 1.1f;
                _stanceInverseFactor = 0.89f;
                StanceController.CameraMovmentForCollisionSpeed = 0.2f;
            }
            else if (StanceController.CurrentStance == EStance.LowReady || StanceController.StoredStance == EStance.LowReady)
            {
                _stanceFactor = 1.07f;
                _stanceInverseFactor = 0.92f;
                StanceController.CameraMovmentForCollisionSpeed = 0.16f;
            }
            else if (StanceController.CurrentStance == EStance.ActiveAiming || StanceController.StoredStance == EStance.ActiveAiming)
            {
                _stanceFactor = 1.03f;
                _stanceInverseFactor = 0.95f;
                StanceController.CameraMovmentForCollisionSpeed = 0.08f;
            }
            else if (StanceController.CurrentStance == EStance.PatrolStance)
            {
                _stanceFactor = 1f;
                _stanceInverseFactor = 1f;
                StanceController.CameraMovmentForCollisionSpeed = 0.1f;
            }
            else
            {
                _stanceFactor = 1f;
                _stanceInverseFactor = 1f;
                StanceController.CameraMovmentForCollisionSpeed = 0.07f;
            }
        }

        private static void AssignFinalTransforms(bool isPistol, float length)
        {
            if (isPistol)
            {
                _finalPos = new Vector3(0.15f, -0.6f, 0.1f);
                _finalRot = new Vector3(-0.9f, -0.01f, -0.01f);
            }
            else if (StanceController.CurrentStance == EStance.ShortStock || StanceController.StoredStance == EStance.ShortStock)
            {
                _finalPos = new Vector3(0f, 0f, -0.5f);
                _finalRot = new Vector3(0.01f, 0.1f, -0.05f);

            }
            else if (StanceController.CurrentStance == EStance.HighReady || StanceController.StoredStance == EStance.HighReady)
            {
                _finalPos = new Vector3(0.08f, -0.34f, -0.4f);
                _finalRot = new Vector3(-0.25f, -0.05f, -0.025f);
            }
            else if (StanceController.CurrentStance == EStance.LowReady || StanceController.StoredStance == EStance.LowReady)
            {
                _finalPos = new Vector3(0f, 0f, -0.15f);
                _finalRot = new Vector3(0.15f, -0.4f, 0f);
            }
            else if (StanceController.CurrentStance == EStance.ActiveAiming || StanceController.StoredStance == EStance.ActiveAiming)
            {
                _finalPos = new Vector3(0.05f, -0.2f, 0.1f);
                _finalRot = new Vector3(-0.5f, -0.5f, -0.5f);
            }
            else if (StanceController.CurrentStance == EStance.PatrolStance)
            {
                _finalPos = Vector3.zero;
                _finalRot = Vector3.zero;
            }
            else
            {
                _finalPos = new Vector3(0f, 0f, -0.15f);
                _finalRot = new Vector3(0.2f, -0.1f, -0.1f);
            }
        }

        private static void CollisionOverride(ProceduralWeaponAnimation pwa, FirearmController fc)
        {
            _blendField.SetValue(pwa.TurnAway, 0f);
            _smoothInField.SetValue(pwa.TurnAway, 0f);
            _smoothOutField.SetValue(pwa.TurnAway, 0f);

            Vector3 rayStart = pwa.HandsContainer.WeaponRoot.position;
            Vector3 forward = -pwa.HandsContainer.WeaponRoot.transform.up;
            bool treatAsPistol = WeaponStats.IsStocklessPistol || WeaponStats.IsMachinePistol;
            float weaponLn = WeaponStats.NewWeaponLength;
            float weaponLengthFactor = weaponLn * (treatAsPistol ? 1.05f : 1.25f); //stance should be a factor here too

            _isColliding = false;

            if (!StanceController.IsMounting && EFTPhysicsClass.Raycast(new Ray(rayStart, forward), out RaycastHit raycastHit, weaponLengthFactor, LayerMaskClass.HighPolyWithTerrainMask))
            {
                _lastDistance = raycastHit.distance;
                _isColliding = true;
            }

            SetStanceSpeedModi(treatAsPistol);
            bool isStocklessRifle = !WeaponStats.IsPistol && !WeaponStats.HasShoulderContact;
            float stanceBonus = (isStocklessRifle ? 1.15f : 1f) * _stanceFactor;
            float stanceInverseBonus = (isStocklessRifle ? 0.85f : 1f) * _stanceInverseFactor;
            float collisionTimerSpeed = 0.015f * WeaponStats.ErgoFactor * stanceInverseBonus;
            float adsTimerSpeed = 0.03f * WeaponStats.ErgoFactor * _stanceInverseFactor;
            float finalStateTimerSpeed = 0.5f * _stanceFactor;

            if (_isColliding)
            {
                _collisionOverrideTimer = 0;
                StanceController.IsColliding = true;
            }
            else
            {
                _collisionOverrideTimer += Time.deltaTime;
                if (_collisionOverrideTimer >= collisionTimerSpeed)
                {
                    StanceController.IsColliding = false;
                }
                else
                {
                    StanceController.IsColliding = true;
                }
            }

            StanceController.StopCameraMovement = false;
            if (_isColliding) //&& _lastDistance <= 1.5f
            {
                _adsResetTimer = 0;
                StanceController.StopCameraMovement = true;
            }
            else
            {
                _adsResetTimer += Time.deltaTime;
                if (_adsResetTimer >= adsTimerSpeed) //this delay needs to factor in the weapon's ADS speed. 0.25 feels good for SKS with supp, 0.5 at least for full length mosin
                {
                    StanceController.StopCameraMovement = false;
                }
                else StanceController.StopCameraMovement = true;
            }

            //weapon length ranges around 0.5-1.4, need to modify collision reaction based on the length of the weapon, particularly the threshold.
            //shorter guns need to react less, maybe have a different Pow value for inverseDistance derived from length
            //length = 1.4, threshold = 0.65
            //try using relative distance instead ? distance / length of raycast

            AssignFinalTransforms(treatAsPistol, weaponLn);
            float speed = 0.15f * WeaponStats.TotalErgo * stanceBonus;
            float baseThrehold = treatAsPistol ? 0.45f : 0.5f;
            float threshold = baseThrehold * Mathf.Pow(weaponLn, 1.15f);
            bool doIntiialState = _lastDistance >= threshold || _delayFinalState;

            float distanceFactor = Mathf.InverseLerp(weaponLn, 0, _lastDistance);
            float smoothedInverseDistance = Mathf.Pow(distanceFactor, 0.45f);

            float offsetFactor = (weaponLn * -0.65f) * (1 - (_lastDistance / weaponLengthFactor));

            float intitalPosZ = offsetFactor; // treatAsPistol ? -0.145f : treatAsPistol && pwa.IsAiming ? -0.16f : -0.13f;
            Vector3 initialPos = new Vector3(0.025f, -0.15f, 0f) * smoothedInverseDistance;
            initialPos.z = intitalPosZ;
            Vector3 lastPos = _finalPos * smoothedInverseDistance; //

            Vector3 initialRot = _initialRot * smoothedInverseDistance;
            Vector3 lastRot = _finalRot * smoothedInverseDistance; //
            Vector3 targetRot = !StanceController.IsColliding ? Vector3.zero : doIntiialState ? initialRot : lastRot;

            bool isInFinalState;
            Vector3 targetPos = Vector3.zero;

            if (!StanceController.IsColliding) isInFinalState = false;
            else if (doIntiialState) isInFinalState = false;
            else
            {
                isInFinalState = true;
                _wasInFinalState = true;
            }

            bool reset = !StanceController.IsColliding && !_wasInFinalState;
            bool initial = doIntiialState && !_wasInFinalState;
            targetPos = reset ? Vector3.zero : initial ? initialPos : lastPos;
            targetRot = reset ? Vector3.zero : initial ? initialRot : lastRot;

            _collisionPos = Vector3.Lerp(_collisionPos, targetPos, speed * Time.deltaTime);
            pwa.HandsContainer.WeaponRoot.localPosition += _collisionPos;

            _collisionRot = Vector3.Lerp(_collisionRot, targetRot, speed * Time.deltaTime);
            Quaternion newRot = Quaternion.identity;
            newRot.x = _collisionRot.x;
            newRot.y = _collisionRot.y;
            newRot.z = _collisionRot.z;

            pwa.HandsContainer.WeaponRoot.localRotation *= newRot;

            if (_isColliding)
            {
                _finalStateDelayTimer += Time.deltaTime;
                if (_finalStateDelayTimer >= 0.05f)
                {
                    _delayFinalState = false;
                }
                else _delayFinalState = true;
            }
            else
            {
                _finalStateDelayTimer = 0f;
                _delayFinalState = true;
            }

            if (_wasInFinalState && !isInFinalState)
            {
                _finalStateEndTimer += Time.deltaTime;
                if (_finalStateEndTimer >= finalStateTimerSpeed)
                {
                    _wasInFinalState = false;
                }
            }
            else
            {
                _finalStateEndTimer = 0f;
            }
        }


        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            FirearmController firearmController = (FirearmController)_fcField.GetValue(__instance);
            if (firearmController == null) return;
            Player player = (Player)_playerField.GetValue(firearmController);
            if (player != null && player.IsYourPlayer && player.MovementContext.CurrentState.Name != EPlayerState.Stationary)
            {

                if (PluginConfig.OverrideCollision.Value && Plugin.FOVFixPresent) CollisionOverride(__instance, firearmController);
                else if (PluginConfig.ModifyBSGCollision.Value) ModifyBSGCollisions(__instance, firearmController);

                DoMounting(player, __instance);
            }
        }
    }

    public class OnWeaponDrawPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(SkillManager).GetMethod("OnWeaponDraw", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix(SkillManager __instance, Item item)
        {
            if (item?.Owner?.ID != null && item.Owner.ID == Singleton<GameWorld>.Instance.MainPlayer.ProfileId && StanceController.CurrentStance == EStance.PistolCompressed)
            {
                StanceController.DidWeaponSwap = true;
            }
        }
    }

    public class SetFireModePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(FirearmsAnimator).GetMethod("SetFireMode", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static bool Prefix(FirearmsAnimator __instance, Weapon.EFireMode fireMode, bool skipAnimation = false)
        {
            __instance.ResetLeftHand();
            skipAnimation = StanceController.CurrentStance == EStance.HighReady && PlayerState.IsSprinting ? true : skipAnimation;
            WeaponAnimationSpeedControllerClass.SetFireMode(__instance.Animator, (float)fireMode);
            if (!skipAnimation)
            {
                WeaponAnimationSpeedControllerClass.TriggerFiremodeSwitch(__instance.Animator);
            }
            return false;
        }
    }

    public class OperateStationaryWeaponPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("OperateStationaryWeapon", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix(Player __instance)
        {
            if (__instance.IsYourPlayer)
            {
                StanceController.CancelAllStances();
                StanceController.StanceBlender.Target = 0f;
                StanceController.StanceTargetPosition = Vector3.zero;

            }
        }
    }

    public class CollisionPatch : ModulePatch
    {
        private static FieldInfo _playerField;
        private static FieldInfo _hitIgnoreField;

        private static int _timer = 0;
        private static MaterialType[] _allowedMats = { MaterialType.Helmet, MaterialType.BodyArmor, MaterialType.Body, MaterialType.Glass, MaterialType.GlassShattered, MaterialType.GlassVisor };

        private static Vector3 _startLeftDir = new Vector3(0.143f, 0f, 0f);
        private static Vector3 _startRightDir = new Vector3(-0.143f, 0f, 0f);
        private static Vector3 _startDownDir = new Vector3(0f, 0f, -0.19f);

        private static Vector3 _wiggleLeftDir = new Vector3(2.5f, 7.5f, -5) * 0.5f;
        private static Vector3 _wiggleRightDir = new Vector3(2.5f, -7.5f, -5f) * 0.5f;
        private static Vector3 _wiggleDownDir = new Vector3(7.5f, 2.5f, -5f) * 0.5f;

        private static int PlayerMask = LayerMask.NameToLayer("Player");


        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            _hitIgnoreField = AccessTools.Field(typeof(EFT.Player.FirearmController), "func_2");

            return typeof(Player.FirearmController).GetMethod("method_11", BindingFlags.Instance | BindingFlags.Public);
        }

        private static void SetMountingStatus(EBracingDirection coverDir)
        {
            if (!StanceController.IsMounting)
            {
                StanceController.BracingDirection = coverDir;
            }
            StanceController.IsBracing = true;
        }

        private static Vector3 GetWiggleDir(EBracingDirection coverDir)
        {
            {
                switch (coverDir)
                {
                    case EBracingDirection.Right:
                        return _wiggleRightDir;
                    case EBracingDirection.Left:
                        return _wiggleLeftDir;
                    case EBracingDirection.Top:
                        return _wiggleDownDir;
                    default: return Vector3.zero;
                }
            }
        }

        private static bool IsBracingProne(Player player)
        {
            if (player.IsInPronePose)
            {
                SetMountingStatus(EBracingDirection.Top);
                return true;
            }
            return false;
        }

        private static bool CheckForCoverCollision(EBracingDirection coverDir, Vector3 start, Vector3 direction, Vector3 spherePos, float radius)
        {
            RaycastHit raycastHit;
            if (Physics.Linecast(start, direction, out raycastHit, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS))
            {
                if (raycastHit.collider.gameObject.layer != PlayerMask)
                {
                    SetMountingStatus(coverDir);
                    StanceController.CoverWiggleDirection = GetWiggleDir(coverDir);
                    return true;
                }
            }

            Collider[] hitColliders = Physics.OverlapSphere(spherePos, radius, EFTHardSettings.Instance.WEAPON_OCCLUSION_LAYERS);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.layer != PlayerMask)
                {
                    SetMountingStatus(coverDir);
                    StanceController.CoverWiggleDirection = GetWiggleDir(coverDir);
                    return true;
                }
            }

            return false;
        }

        private static void DetectBracing(FirearmController fc, Player player, float ln)
        {
            _timer += 1;
            if (_timer >= 60)
            {
                _timer = 0;
                Transform weapTransform = player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim;
                Vector3 linecastDirection = weapTransform.TransformDirection(Vector3.up);

                Vector3 downDir = WeaponStats.BipodIsDeployed ? new Vector3(_startDownDir.x, _startDownDir.y, _startDownDir.z + -0.21f) : _startDownDir;

                Vector3 startDown = weapTransform.position + weapTransform.TransformDirection(downDir);
                Vector3 startLeft = weapTransform.position + weapTransform.TransformDirection(_startLeftDir);
                Vector3 startRight = weapTransform.position + weapTransform.TransformDirection(_startRightDir);

                Vector3 sphereDown = weapTransform.position + weapTransform.TransformDirection(new Vector3(0f, -0.45f, -0.1f));
                Vector3 sphereLeft = weapTransform.position + weapTransform.TransformDirection(new Vector3(0.05f, -0.5f, -0.065f));
                Vector3 sphereRight = weapTransform.position + weapTransform.TransformDirection(new Vector3(-0.05f, -0.5f, -0.065f));

                Vector3 forwardDirection = startDown - linecastDirection * ln;
                Vector3 leftDirection = startLeft - linecastDirection * ln;
                Vector3 rightDirection = startRight - linecastDirection * ln;

                if (PluginConfig.EnableGeneralLogging.Value)
                {
                    DebugGizmos.SingleObjects.Line(startDown, forwardDirection, Color.red, 0.02f, true, 0.3f, true);
                    DebugGizmos.SingleObjects.Sphere(sphereDown, 0.1f, Color.red, true, 0.3f);
                    DebugGizmos.SingleObjects.Line(startLeft, leftDirection, Color.green, 0.02f, true, 0.3f, true);
                    DebugGizmos.SingleObjects.Sphere(sphereLeft, 0.1f, Color.green, true, 0.3f);
                    DebugGizmos.SingleObjects.Line(startRight, rightDirection, Color.yellow, 0.02f, true, 0.3f, true);
                    DebugGizmos.SingleObjects.Sphere(sphereRight, 0.1f, Color.yellow, true, 0.3f);
                }

                if (PluginConfig.OverrideMounting.Value && Plugin.ServerConfig.enable_stances)
                {
                    if (IsBracingProne(player) ||
                    CheckForCoverCollision(EBracingDirection.Top, startDown, forwardDirection, sphereDown, 0.045f) ||
                    CheckForCoverCollision(EBracingDirection.Left, startLeft, leftDirection, sphereLeft, 0.09f) ||
                    CheckForCoverCollision(EBracingDirection.Right, startRight, rightDirection, sphereRight, 0.09f))
                    {
                        return;
                    }
                }
                StanceController.IsBracing = false;
            }

            if (StanceController.IsBracing || StanceController.IsMounting)
            {
                float mountOrientationBonus = StanceController.BracingDirection == EBracingDirection.Top ? 0.75f : 1f;
                float mountingRecoilLimit = StanceController.TreatWeaponAsPistolStance ? 0.25f : 0.75f;
                float recoilBonus =
                    StanceController.IsMounting && fc.Weapon.IsBeltMachineGun && WeaponStats.BipodIsDeployed ? 0.4f :
                    StanceController.IsMounting && fc.Weapon.IsBeltMachineGun ? 0.75f :
                    StanceController.IsMounting && WeaponStats.BipodIsDeployed ? 0.45f :
                    StanceController.IsMounting ? 0.85f :
                    0.95f;
                float swayBonus = StanceController.IsMounting && WeaponStats.BipodIsDeployed ? 0.05f : StanceController.IsMounting ? 0.35f : 0.65f;
                StanceController.BracingRecoilBonus = Mathf.Lerp(StanceController.BracingRecoilBonus, recoilBonus * mountOrientationBonus, 0.04f);
                StanceController.BracingSwayBonus = Mathf.Lerp(StanceController.BracingSwayBonus, swayBonus * mountOrientationBonus, 0.04f);
            }
            else
            {
                StanceController.BracingSwayBonus = Mathf.Lerp(StanceController.BracingSwayBonus, 1f, 0.05f);
                StanceController.BracingRecoilBonus = Mathf.Lerp(StanceController.BracingRecoilBonus, 1f, 0.05f);
            }
        }

        private static void DoMelee(FirearmController fc, Player player, float ln)
        {
            if (StanceController.CurrentStance == EStance.Melee && StanceController.CanDoMeleeDetection && !StanceController.MeleeHitSomething)
            {
                Transform weapTransform = player.ProceduralWeaponAnimation.HandsContainer.WeaponRootAnim;
                Vector3 linecastDirection = weapTransform.TransformDirection(Vector3.up);
                Vector3 startMeleeDir = new Vector3(0, -0.5f, -0.025f);
                Vector3 meleeStart = weapTransform.position + weapTransform.TransformDirection(startMeleeDir);
                Vector3 meleeDir = meleeStart - linecastDirection * (ln - (WeaponStats.HasBayonet ? 0.1f : 0.25f));

                if (PluginConfig.EnablePWALogging.Value)
                {
                    DebugGizmos.SingleObjects.Line(meleeStart, meleeDir, Color.red, 0.02f, true, 0.3f, true);
                }

                BallisticCollider hitBalls = null;
                if (Physics.Linecast(meleeStart, meleeDir, out RaycastHit raycastHit, CollisionLayerClass.HitMask))
                {
                    Collider col = raycastHit.collider;
                    BaseBallistic baseballComp = col.GetComponent<BaseBallistic>();

                    if (baseballComp != null)
                    {
                        hitBalls = baseballComp.Get(raycastHit.point);
                    }

                    float weaponWeight = fc.Weapon.TotalWeight;
                    float damage = 19f + WeaponStats.BaseMeleeDamage * (1f + player.Skills.StrengthBuffMeleePowerInc) * (1f + (weaponWeight / 10f));

                    damage *= player.Physical.HandsStamina.Exhausted ? Singleton<BackendConfigSettingsClass>.Instance.Stamina.ExhaustedMeleeDamageMultiplier : 1f;
                    damage *= player.Speed;

                    float pen = 15f + WeaponStats.BaseMeleePen * (1f + (weaponWeight / 10f));
                    bool shouldSkipHit = false;

                    if (hitBalls as BodyPartCollider != null)
                    {
                        player.ExecuteSkill(new Action(() => player.Skills.FistfightAction.Complete(1f)));
                    }

                    if (hitBalls.TypeOfMaterial == MaterialType.Glass || hitBalls.TypeOfMaterial == MaterialType.GlassShattered)
                    {
                        int rndNum = UnityEngine.Random.Range(1, 11);
                        if (rndNum > (4f + WeaponStats.BaseMeleeDamage))
                        {
                            shouldSkipHit = true;
                        }
                    }

                    bool isAllowableHit = _allowedMats.Contains(hitBalls.TypeOfMaterial) && !shouldSkipHit;
                    if ((WeaponStats.HasBayonet || isAllowableHit) && raycastHit.collider.gameObject.layer != PlayerMask)
                    {
                        Vector3 position = fc.CurrentFireport.position;
                        Vector3 vector = fc.WeaponDirection;
                        Vector3 shotPosition = position;
                        fc.AdjustShotVectors(ref shotPosition, ref vector);
                        Vector3 shotDirection = vector;
                        DamageInfoStruct damageInfo = new DamageInfoStruct
                        {
                            SourceId = fc.Weapon.Id,
                            DamageType = EDamageType.Melee,
                            Damage = damage,
                            PenetrationPower = pen,
                            ArmorDamage = 10f + (damage / 10f),
                            Direction = shotDirection.normalized,
                            HitCollider = col,
                            HitPoint = raycastHit.point,
                            Player = Singleton<GameWorld>.Instance.GetAlivePlayerBridgeByProfileID(player.ProfileId),
                            HittedBallisticCollider = hitBalls,
                            HitNormal = raycastHit.normal,
                            Weapon = fc.Item as Item,
                            IsForwardHit = true,
                            StaminaBurnRate = 5f
                        };
                        ShotInfoClass result = Singleton<GameWorld>.Instance.HackShot(damageInfo);
                    }
                    float vol = WeaponStats.HasBayonet ? 10f : 12f;
                    Singleton<BetterAudio>.Instance.PlayDropItem(baseballComp.SurfaceSound, JsonType.EItemDropSoundType.Rifle, raycastHit.point, vol);
                    /*                  StanceController.DoWiggleEffects(player, player.ProceduralWeaponAnimation, fc, new Vector3(-10f, 10f, 0f), true, 1.5f);
                    */
                    player.Physical.ConsumeAsMelee(0.2f * (1f + (weaponWeight * 0.1f)));

                    StanceController.CanDoMeleeDetection = false;
                    StanceController.MeleeHitSomething = true;
                    return;
                }
            }
        }


        [PatchPrefix]
        private static void PatchPrefix(Player.FirearmController __instance, Vector3 origin, float ln, Vector3? weaponUp = null)
        {
            Player player = (Player)_playerField.GetValue(__instance);
            if (player.IsYourPlayer)
            {
                DoMelee(__instance, player, ln);
                DetectBracing(__instance, player, ln);
            }
        }
    }

    public class WeaponOverlapViewPatch : ModulePatch
    {
        private static FieldInfo playerField;

        protected override MethodBase GetTargetMethod()
        {
            playerField = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            return typeof(Player.FirearmController).GetMethod("WeaponOverlapView", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player.FirearmController __instance)
        {
            Player player = (Player)playerField.GetValue(__instance);
            if (player.IsYourPlayer && (StanceController.IsMounting || StanceController.IsColliding))
            {
                return false;
            }
            return true;
        }
    }

    public class WeaponLengthPatch : ModulePatch
    {
        private static FieldInfo playerField;
        private static FieldInfo weapLn;

        protected override MethodBase GetTargetMethod()
        {
            playerField = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            weapLn = AccessTools.Field(typeof(EFT.Player.FirearmController), "WeaponLn");
            return typeof(Player.FirearmController).GetMethod("method_10", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix(Player.FirearmController __instance)
        {
            Player player = (Player)playerField.GetValue(__instance);
            float length = (float)weapLn.GetValue(__instance);
            if (player.IsYourPlayer)
            {
                WeaponStats.BaseWeaponLength = length;
                WeaponStats.NewWeaponLength = length < 0.92f ? length * 0.95f : length * 1.05f; //length >= 0.92f ? length * 1.12f : length
            }
        }
    }


    public class WeaponOverlappingPatch : ModulePatch
    {
        private static FieldInfo playerField;
        private static FieldInfo weaponLnField;

        protected override MethodBase GetTargetMethod()
        {
            playerField = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            weaponLnField = AccessTools.Field(typeof(EFT.Player.FirearmController), "WeaponLn");
            return typeof(Player.FirearmController).GetMethod("WeaponOverlapping", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static void Prefix(Player.FirearmController __instance)
        {
            Player player = (Player)playerField.GetValue(__instance);
            if (player.IsYourPlayer)
            {
                if (StanceController.CurrentStance == EStance.PatrolStance)
                {
                    weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.75f);
                    return;
                }

                if (__instance.Item.WeapClass == "pistol")
                {
                    weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.85f);
                }
                else
                {
                    if (Plugin.FikaPresent) //collisions acts funky with stances from another client's perspective
                    {
                        weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.8f);
                        return;
                    }
                    if (StanceController.CurrentStance == EStance.ShortStock)
                    {
                        weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.9f);
                        return;
                    }
                    if (StanceController.CurrentStance == EStance.HighReady)
                    {
                        weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.95f);
                        return;
                    }
                    if (StanceController.CurrentStance == EStance.LowReady)
                    {
                        weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength * 0.98f);
                        return;
                    }
                }
                weaponLnField.SetValue(__instance, WeaponStats.NewWeaponLength);
            }
        }
    }

    public class ShouldMoveWeapCloserPatch : ModulePatch
    {
        private static FieldInfo _playerField;
        private static FieldInfo _fcField;

        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(EFT.Player.FirearmController), "_player");
            _fcField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");
            return typeof(EFT.Animations.ProceduralWeaponAnimation).GetMethod("CheckShouldMoveWeaponCloser", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance, ref bool ____shouldMoveWeaponCloser)
        {
            FirearmController firearmController = (FirearmController)_fcField.GetValue(__instance);

            if (firearmController == null) return;

            Player player = (Player)_playerField.GetValue(firearmController);

            if (player != null && player.MovementContext.CurrentState.Name != EPlayerState.Stationary && player.IsYourPlayer)
            {
                ____shouldMoveWeaponCloser = false;
            }
        }
    }

    public class InitTransformsPatch : ModulePatch
    {
        private static FieldInfo playerField;
        private static FieldInfo fcField;

        protected override MethodBase GetTargetMethod()
        {
            playerField = AccessTools.Field(typeof(FirearmController), "_player");
            fcField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");

            return typeof(ProceduralWeaponAnimation).GetMethod("InitTransforms", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            FirearmController firearmController = (FirearmController)fcField.GetValue(__instance);

            if (firearmController == null) return;

            Player player = (Player)playerField.GetValue(firearmController);

            if (player != null && player.MovementContext.CurrentState.Name != EPlayerState.Stationary && player.IsYourPlayer)
            {
                Vector3 baseOffset = StanceController.GetWeaponOffsets().TryGetValue(firearmController.Weapon.TemplateId, out Vector3 offset) ? offset : Vector3.zero;
                Vector3 newPos = PluginConfig.EnableAltRifle.Value ? new Vector3(0.08f, -0.075f, 0f) : PluginConfig.WeapOffset.Value;

                newPos += baseOffset;

                if (!PluginConfig.EnableAltRifle.Value) newPos += __instance.HandsContainer.WeaponRoot.localPosition;

                StanceController.WeaponOffsetPosition = newPos;
                __instance.HandsContainer.WeaponRoot.localPosition = newPos;

                if (!Plugin.FOVFixPresent) __instance.HandsContainer.CameraOffset = new Vector3(0.04f, 0.04f, 0.025f);
            }
        }
    }

    public class ZeroAdjustmentsPatch : ModulePatch
    {
        private static FieldInfo blindfireStrengthField;
        private static FieldInfo blindfireRotationField;
        private static PropertyInfo overlappingBlindfireField;
        private static FieldInfo blindfirePositionField;
        private static FieldInfo firearmControllerField;
        private static FieldInfo playerField;

        private static Vector3 targetPosition = Vector3.zero;

        protected override MethodBase GetTargetMethod()
        {
            blindfireStrengthField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_blindfireStrength");
            blindfireRotationField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_blindFireRotation");
            overlappingBlindfireField = AccessTools.Property(typeof(EFT.Animations.ProceduralWeaponAnimation), "Single_3");
            blindfirePositionField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_blindFirePosition");
            firearmControllerField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");
            playerField = AccessTools.Field(typeof(FirearmController), "_player");

            return typeof(EFT.Animations.ProceduralWeaponAnimation).GetMethod("ZeroAdjustments", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static bool PatchPrefix(EFT.Animations.ProceduralWeaponAnimation __instance)
        {
            FirearmController firearmController = (FirearmController)firearmControllerField.GetValue(__instance);
            if (firearmController == null)
            {
                return true;
            }
            Player player = (Player)playerField.GetValue(firearmController);
            if (player != null && player.IsYourPlayer) // player.MovementContext.CurrentState.Name != EPlayerState.Stationary && player.IsYourPlayer
            {
                float collidingModifier = (float)overlappingBlindfireField.GetValue(__instance);
                Vector3 blindfirePosition = (Vector3)blindfirePositionField.GetValue(__instance);

                __instance.PositionZeroSum.y = (__instance._shouldMoveWeaponCloser ? 0.05f : 0f);
                __instance.RotationZeroSum.y = __instance.SmoothedTilt * __instance.PossibleTilt;
                float stanceBlendValue = StanceController.StanceBlender.Value;
                float blindFireBlendValue = __instance.BlindfireBlender.Value;
                if (Mathf.Abs(blindFireBlendValue) > 0f)
                {
                    StanceController.IsBlindFiring = true;
                    float strength = ((Mathf.Abs(__instance.Pitch) < 45f) ? 1f : ((90f - Mathf.Abs(__instance.Pitch)) / 45f));
                    blindfireStrengthField.SetValue(__instance, strength);
                    __instance.BlindFireEndPosition = ((blindFireBlendValue > 0f) ? __instance.BlindFireOffset : __instance.SideFireOffset);
                    __instance.BlindFireEndPosition *= strength;
                }
                else
                {
                    StanceController.IsBlindFiring = false;
                    blindfirePositionField.SetValue(__instance, Vector3.zero);
                    blindfireRotationField.SetValue(__instance, Vector3.zero);
                }

                if (Mathf.Abs(stanceBlendValue) > 0f)
                {
                    float strength = ((Mathf.Abs(__instance.Pitch) < 45f) ? 1f : ((90f - Mathf.Abs(__instance.Pitch)) / 45f));
                    blindfireStrengthField.SetValue(__instance, strength);
                    targetPosition = StanceController.StanceTargetPosition * stanceBlendValue;
                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + (float)blindfireStrengthField.GetValue(__instance) * targetPosition;
                    __instance.HandsContainer.HandsRotation.Zero = __instance.RotationZeroSum;
                    return false;
                }
                else
                {
                    targetPosition = Vector3.zero;
                }

                __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + targetPosition + (Vector3)blindfirePositionField.GetValue(__instance) * (float)blindfireStrengthField.GetValue(__instance) * collidingModifier;
                __instance.HandsContainer.HandsRotation.Zero = __instance.RotationZeroSum;
                return false;
            }
            return true;
        }
    }

    public class ChangePosePatch : ModulePatch
    {
        private static FieldInfo movementContextField;
        private static FieldInfo playerField;

        protected override MethodBase GetTargetMethod()
        {
            movementContextField = AccessTools.Field(typeof(MovementState), "MovementContext");
            playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return typeof(MovementState).GetMethod("ChangePose", BindingFlags.Instance | BindingFlags.Public);
        }


        [PatchPrefix]
        private static void Prefix(MovementState __instance)
        {
            MovementContext movementContext = (MovementContext)movementContextField.GetValue(__instance);
            Player player = (Player)playerField.GetValue(movementContext);

            if (player.IsYourPlayer)
            {
                StanceController.IsMounting = false;
            }
        }
    }


    public class SetTiltPatch : ModulePatch
    {
        private static FieldInfo movementContextField;
        private static FieldInfo playerField;
        public static float tiltBeforeMount = 0f;

        protected override MethodBase GetTargetMethod()
        {
            movementContextField = AccessTools.Field(typeof(MovementState), "MovementContext");
            playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return typeof(MovementState).GetMethod("SetTilt", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static void Prefix(MovementState __instance, float tilt)
        {
            MovementContext movementContext = (MovementContext)movementContextField.GetValue(__instance);
            Player player = (Player)playerField.GetValue(movementContext);

            if (player.IsYourPlayer)
            {
                float tiltTolerance = WeaponStats.BipodIsDeployed ? 0.5f : 2.5f;
                if (!StanceController.IsMounting)
                {
                    tiltBeforeMount = tilt;
                }
                else if (Math.Abs(tiltBeforeMount - tilt) > tiltTolerance)
                {
                    StanceController.IsMounting = false;
                    tiltBeforeMount = 0f;
                }
            }
        }
    }

    public class ApplySimpleRotationPatch : ModulePatch
    {
        private static FieldInfo aimSpeedField;
        private static FieldInfo blindFireStrength;
        private static FieldInfo scopeRotationField;
        private static FieldInfo weapRotationField;
        private static FieldInfo isAimingField;
        private static FieldInfo weaponPositionField;
        private static FieldInfo currentRotationField;
        private static FieldInfo firearmControllerField;
        private static FieldInfo playerField;

        private static bool hasResetActiveAim = true;
        private static bool hasResetLowReady = true;
        private static bool hasResetHighReady = true;
        private static bool hasResetShortStock = true;
        private static bool hasResetPistolPos = true;
        private static bool hasResetMelee = true;

        private static bool isResettingActiveAim = false;
        private static bool isResettingLowReady = false;
        private static bool isResettingHighReady = false;
        private static bool isResettingShortStock = false;
        private static bool isResettingPistol = false;
        private static bool isResettingMelee = false;
        private static bool didHalfMeleeAnim = false;

        private static Quaternion currentRotation = Quaternion.identity;
        private static Quaternion stanceRotation = Quaternion.identity;
        private static Vector3 mountWeapPosition = Vector3.zero;

        private static Vector3 lowReadyTargetRotation = new Vector3(18.0f, 5.0f, -1.0f);
        private static Quaternion lowReadyTargetQuaternion = Quaternion.Euler(lowReadyTargetRotation);
        private static Vector3 lowReadyTargetPostion = new Vector3(0.06f, 0.04f, 0.0f);
        private static Vector3 highReadyTargetRotation = new Vector3(-15.0f, 3.0f, 3.0f);
        private static Quaternion highReadyTargetQuaternion = Quaternion.Euler(highReadyTargetRotation);
        private static Vector3 highReadyTargetPostion = new Vector3(0.05f, 0.1f, -0.12f);
        private static Vector3 activeAimTargetRotation = new Vector3(0.0f, -40.0f, 0.0f);
        private static Quaternion activeAimTargetQuaternion = Quaternion.Euler(activeAimTargetRotation);
        private static Vector3 activeAimTargetPostion = new Vector3(0.0f, 0.0f, 0.0f);
        private static Vector3 shortStockTargetRotation = new Vector3(0.0f, -28.0f, 0.0f);
        private static Quaternion shortStockTargetQuaternion = Quaternion.Euler(shortStockTargetRotation);
        private static Vector3 shortStockTargetPostion = new Vector3(0.05f, 0.18f, -0.2f);
        private static Vector3 tacPistolTargetRotation = new Vector3(0.0f, -20.0f, 0.0f);
        private static Quaternion tacPistolTargetQuaternion = Quaternion.Euler(tacPistolTargetRotation);
        private static Vector3 tacPistolTargetPosition = new Vector3(-0.1f, 0.1f, -0.05f);
        private static Vector3 normalPistolTargetRotation = new Vector3(0f, -5.0f, 0.0f);
        private static Quaternion normalPistolTargetQuaternion = Quaternion.Euler(normalPistolTargetRotation);
        private static Vector3 normalPistolTargetPosition = new Vector3(-0.05f, 0.0f, 0.0f);

        private static float stanceRotationSpeed = 1f;

        protected override MethodBase GetTargetMethod()
        {
            aimSpeedField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_aimingSpeed");
            blindFireStrength = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_blindfireStrength");
            weaponPositionField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_temporaryPosition");
            scopeRotationField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_targetScopeRotation");
            weapRotationField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_temporaryRotation");
            isAimingField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_isAiming");
            currentRotationField = AccessTools.Field(typeof(EFT.Animations.ProceduralWeaponAnimation), "_cameraIdenity");
            firearmControllerField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");
            playerField = AccessTools.Field(typeof(FirearmController), "_player");

            return typeof(EFT.Animations.ProceduralWeaponAnimation).GetMethod("ApplySimpleRotation", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void Postfix(EFT.Animations.ProceduralWeaponAnimation __instance, float dt)
        {
            FirearmController firearmController = (FirearmController)firearmControllerField.GetValue(__instance);
            if (firearmController == null)
            {
                return;
            }
            Player player = (Player)playerField.GetValue(firearmController);
            if (player != null && player.MovementContext.CurrentState.Name != EPlayerState.Stationary)
            {
                float pitch = (float)blindFireStrength.GetValue(__instance);
                Quaternion scopeRotation = (Quaternion)scopeRotationField.GetValue(__instance);
                Vector3 weaponPosition = (Vector3)weaponPositionField.GetValue(__instance);
                Quaternion weapRotation = (Quaternion)weapRotationField.GetValue(__instance);

                if (player.IsYourPlayer)
                {
                    StanceController.IsInThirdPerson = true;

                    float aimSpeed = (float)aimSpeedField.GetValue(__instance);
                    bool isAiming = (bool)isAimingField.GetValue(__instance);

                    bool allStancesReset = hasResetActiveAim && hasResetLowReady && hasResetHighReady && hasResetShortStock && hasResetPistolPos;
                    bool isInStance =
                        StanceController.CurrentStance == EStance.HighReady ||
                        StanceController.CurrentStance == EStance.LowReady ||
                        StanceController.CurrentStance == EStance.ShortStock ||
                        StanceController.CurrentStance == EStance.ActiveAiming ||
                        StanceController.CurrentStance == EStance.Melee;
                    bool isInShootableStance =
                        StanceController.CurrentStance == EStance.ShortStock ||
                        StanceController.CurrentStance == EStance.ActiveAiming ||
                        StanceController.TreatWeaponAsPistolStance ||
                        StanceController.CurrentStance == EStance.Melee;
                    bool cancelBecauseShooting = !(PluginConfig.RememberStanceFiring.Value && isAiming) && StanceController.IsFiringFromStance && !isInShootableStance;
                    bool doStanceRotation = (isInStance || !allStancesReset || StanceController.CurrentStance == EStance.PistolCompressed) && !cancelBecauseShooting;
                    bool allowActiveAimReload = PluginConfig.ActiveAimReload.Value && PlayerState.IsInReloadOpertation && !PlayerState.IsAttemptingToReloadInternalMag && !PlayerState.IsQuickReloading;
                    bool cancelStance =
                        (StanceController.CancelActiveAim && StanceController.CurrentStance == EStance.ActiveAiming && !allowActiveAimReload) ||
                        (StanceController.CancelHighReady && StanceController.CurrentStance == EStance.HighReady) ||
                        (StanceController.CancelLowReady && StanceController.CurrentStance == EStance.LowReady) ||
                        (StanceController.CancelShortStock && StanceController.CurrentStance == EStance.ShortStock); //|| (StanceController.CancelPistolStance && StanceController.PistolIsCompressed)

                    currentRotation = Quaternion.Slerp(currentRotation, __instance.IsAiming && allStancesReset ? scopeRotation : doStanceRotation ? stanceRotation : Quaternion.identity, doStanceRotation ? stanceRotationSpeed * PluginConfig.StanceRotationSpeedMulti.Value : __instance.IsAiming ? 8f * aimSpeed * dt : 8f * dt);

                    __instance.HandsContainer.WeaponRootAnim.SetPositionAndRotation(weaponPosition, weapRotation * currentRotation);

                    if (StanceController.TreatWeaponAsPistolStance && PluginConfig.EnableAltPistol.Value) // && StanceController.CurrentStance != EStance.PatrolStance
                    {
                        if (StanceController.CurrentStance == EStance.PistolCompressed && !StanceController.IsAiming && !isResettingPistol && !StanceController.IsBlindFiring)
                        {
                            StanceController.StanceBlender.Target = 1f;
                        }
                        else
                        {
                            StanceController.StanceBlender.Target = 0f;
                        }

                        if ((StanceController.CurrentStance != EStance.PistolCompressed && !StanceController.IsAiming && !isResettingPistol) || (StanceController.IsBlindFiring))
                        {
                            StanceController.StanceTargetPosition = Vector3.Lerp(StanceController.StanceTargetPosition, Vector3.zero, 5f * dt);
                        }

                        hasResetActiveAim = true;
                        hasResetHighReady = true;
                        hasResetLowReady = true;
                        hasResetShortStock = true;
                        StanceController.DoPistolStances(true, __instance, ref stanceRotation, dt, ref hasResetPistolPos, player, ref stanceRotationSpeed, ref isResettingPistol, firearmController, Vector3.zero);
                    }
                    else if (!StanceController.TreatWeaponAsPistolStance || WeaponStats.HasShoulderContact)
                    {
                        if ((!isInStance && allStancesReset) || (cancelBecauseShooting && !isInShootableStance) || StanceController.IsAiming || cancelStance || StanceController.IsBlindFiring)
                        {
                            StanceController.StanceBlender.Target = 0f;
                        }
                        else if (isInStance)
                        {
                            StanceController.StanceBlender.Target = 1f;
                        }

                        if (((!isInStance && allStancesReset) && !cancelBecauseShooting && !StanceController.IsAiming) || (StanceController.IsBlindFiring))
                        {
                            StanceController.StanceTargetPosition = Vector3.Lerp(StanceController.StanceTargetPosition, Vector3.zero, 5f * dt);
                        }

                        hasResetPistolPos = true;
                        StanceController.DoRifleStances(player, firearmController, true, __instance, ref stanceRotation, dt, ref isResettingShortStock, ref hasResetShortStock, ref hasResetLowReady, ref hasResetActiveAim, ref hasResetHighReady, ref isResettingHighReady, ref isResettingLowReady, ref isResettingActiveAim, ref stanceRotationSpeed, ref hasResetMelee, ref isResettingMelee, ref didHalfMeleeAnim, Vector3.zero);
                    }

                    StanceController.HasResetActiveAim = hasResetActiveAim;
                    StanceController.HasResetHighReady = hasResetHighReady;
                    StanceController.HasResetLowReady = hasResetLowReady;
                    StanceController.HasResetShortStock = hasResetShortStock;
                    StanceController.HasResetPistolPos = hasResetPistolPos;
                    StanceController.HasResetMelee = hasResetMelee;

                }
                else if (player.IsAI && !player.AIData.UseZombieSimpleAnimator)
                {
                    Quaternion targetRotation = Quaternion.identity;
                    Quaternion currentRotation = (Quaternion)currentRotationField.GetValue(__instance);
                    aimSpeedField.SetValue(__instance, 1f);

                    FaceShieldComponent fsComponent = player.FaceShieldObserver.Component;
                    NightVisionComponent nvgComponent = player.NightVisionObserver.Component;
                    bool nvgIsOn = nvgComponent != null && (nvgComponent.Togglable == null || nvgComponent.Togglable.On);
                    bool fsIsON = fsComponent != null && (fsComponent.Togglable == null || fsComponent.Togglable.On);

                    float lastDistance = player.AIData.BotOwner.AimingManager.CurrentAiming.LastDist2Target;
                    Vector3 distanceVect = player.AIData.BotOwner.AimingManager.CurrentAiming.RealTargetPoint - player.AIData.BotOwner.MyHead.position;
                    float realDistance = distanceVect.magnitude;

                    bool isTacBot = StanceController._botsToUseTacticalStances.IndexOf(player.AIData.BotOwner.Profile.Info.Settings.Role.ToString()) != -1;
                    bool isPeace = player.AIData.BotOwner.Memory.IsPeace;
                    bool notShooting = !player.AIData.BotOwner.ShootData.Shooting && Time.time - player.AIData.BotOwner.ShootData.LastTriggerPressd > 15f;
                    bool isInStance = false;
                    float stanceSpeed = 1f;

                    if (player.MovementContext.BlindFire == 0 && player.MovementContext.StationaryWeapon == null)
                    {
                        if (isPeace && !player.IsSprintEnabled && !__instance.IsAiming && !firearmController.IsInReloadOperation() && !firearmController.IsInventoryOpen() && !firearmController.IsInInteractionStrictCheck() && !firearmController.IsInSpawnOperation() && !firearmController.IsHandsProcessing()) // && player.AIData.BotOwner.WeaponManager.IsWeaponReady &&  player.AIData.BotOwner.WeaponManager.InIdleState()
                        {
                            isInStance = true;
                            player.MovementContext.SetPatrol(true);
                        }
                        else
                        {
                            player.MovementContext.SetPatrol(false);
                            if (firearmController.Weapon.WeapClass != "pistol")
                            {
                                ////low ready//// 
                                if (!isTacBot && !firearmController.IsInReloadOperation() && !player.IsSprintEnabled && !__instance.IsAiming && notShooting && (lastDistance >= 25f || lastDistance == 0f))    // (Time.time - player.AIData.BotOwner.Memory.LastEnemyTimeSeen) > 1f
                                {
                                    isInStance = true;
                                    stanceSpeed = 12f * dt;
                                    targetRotation = lowReadyTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * lowReadyTargetPostion;
                                }

                                ////high ready////
                                if (isTacBot && !firearmController.IsInReloadOperation() && !__instance.IsAiming && notShooting && (lastDistance >= 25f || lastDistance == 0f))
                                {
                                    isInStance = true;
                                    player.BodyAnimatorCommon.SetFloat(PlayerAnimator.WEAPON_SIZE_MODIFIER_PARAM_HASH, 2);
                                    stanceSpeed = 10.8f * dt;
                                    targetRotation = highReadyTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * highReadyTargetPostion;
                                }
                                else
                                {
                                    player.BodyAnimatorCommon.SetFloat(PlayerAnimator.WEAPON_SIZE_MODIFIER_PARAM_HASH, (float)firearmController.Item.CalculateCellSize().X);
                                }

                                ///active aim//// 
                                if (isTacBot && (((nvgIsOn || fsIsON) && !player.IsSprintEnabled && !firearmController.IsInReloadOperation() && lastDistance < 25f && lastDistance > 2f && lastDistance != 0f) || (__instance.IsAiming && (nvgIsOn && __instance.CurrentScope.IsOptic || fsIsON))))
                                {
                                    isInStance = true;
                                    stanceSpeed = 6f * dt;
                                    targetRotation = activeAimTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * activeAimTargetPostion;
                                }

                                ///short stock//// 
                                if (isTacBot && !player.IsSprintEnabled && !firearmController.IsInReloadOperation() && lastDistance <= 2f && lastDistance != 0f)
                                {
                                    isInStance = true;
                                    stanceSpeed = 12f * dt;
                                    targetRotation = shortStockTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * shortStockTargetPostion;
                                }
                            }
                            else
                            {
                                if (!isTacBot && !player.IsSprintEnabled && !__instance.IsAiming && notShooting)
                                {
                                    isInStance = true;
                                    stanceSpeed = 6f * dt;
                                    targetRotation = normalPistolTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * normalPistolTargetPosition;
                                }

                                if (isTacBot && !player.IsSprintEnabled && !__instance.IsAiming && notShooting)
                                {
                                    isInStance = true;
                                    stanceSpeed = 6f * dt;
                                    targetRotation = tacPistolTargetQuaternion;
                                    __instance.HandsContainer.HandsPosition.Zero = __instance.PositionZeroSum + pitch * tacPistolTargetPosition;
                                }
                            }
                        }
                    }

                    currentRotation = Quaternion.Slerp(currentRotation, __instance.IsAiming && !isInStance ? scopeRotation : isInStance ? targetRotation : Quaternion.identity, isInStance ? stanceSpeed : 8f * dt);
                    __instance.HandsContainer.WeaponRootAnim.SetPositionAndRotation(weaponPosition, weapRotation * currentRotation);
                    currentRotationField.SetValue(__instance, currentRotation);
                }
            }
        }
    }

    public class ApplyComplexRotationPatch : ModulePatch
    {
        private static FieldInfo _aimSpeedField;
        private static FieldInfo _compensatoryField;
        private static FieldInfo _displacementStrField;
        private static FieldInfo _scopeRotationField;
        private static FieldInfo _weapTempRotationField;
        private static FieldInfo _weapTempPositionField;
        private static FieldInfo _isAimingField;
        private static FieldInfo _firearmControllerField;
        private static FieldInfo _playerField;

        private static bool _hasResetActiveAim = true;
        private static bool _hasResetLowReady = true;
        private static bool _hasResetHighReady = true;
        private static bool _hasResetShortStock = true;
        private static bool _hasResetPistolPos = true;
        private static bool _hasResetMelee = true;

        private static bool _isResettingActiveAim = false;
        private static bool _isResettingLowReady = false;
        private static bool _isResettingHighReady = false;
        private static bool _isResettingShortStock = false;
        private static bool _isResettingPistol = false;
        private static bool _isResettingMelee = false;
        private static bool _didHalfMeleeAnim = false;

        private static Quaternion _currentRotation = Quaternion.identity;
        private static Quaternion _stanceRotation = Quaternion.identity;
        private static Vector3 _mountWeapPosition = Vector3.zero;
        private static Vector3 _currentRecoil = Vector3.zero;
        private static Vector3 _targetRecoil = Vector3.zero;
        private static float _stanceRotationSpeed = 1f;

        protected override MethodBase GetTargetMethod()
        {
            _aimSpeedField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_aimingSpeed");
            _compensatoryField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_compensatoryScale");
            _displacementStrField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_displacementStr");
            _scopeRotationField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_targetScopeRotation");
            _weapTempPositionField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_temporaryPosition");
            _weapTempRotationField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_temporaryRotation");
            _isAimingField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_isAiming");
            _firearmControllerField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_firearmController");
            _playerField = AccessTools.Field(typeof(FirearmController), "_player");

            return typeof(ProceduralWeaponAnimation).GetMethod("ApplyComplexRotation", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPostfix]
        private static void Postfix(ProceduralWeaponAnimation __instance, float dt, Vector3 ____vCameraTarget)
        {
            FirearmController firearmController = (FirearmController)_firearmControllerField.GetValue(__instance);
            if (firearmController == null)
            {
                return;
            }
            Player player = (Player)_playerField.GetValue(firearmController);
            if (player != null && player.IsYourPlayer && player.MovementContext.CurrentState.Name != EPlayerState.Stationary)
            {
                FirearmController fc = player.HandsController as FirearmController;

                StanceController.IsInThirdPerson = false;

                float aimSpeed = (float)_aimSpeedField.GetValue(__instance);
                float compensatoryScale = (float)_compensatoryField.GetValue(__instance);
                float displacementStr = (float)_displacementStrField.GetValue(__instance);
                Quaternion scopeRotation = (Quaternion)_scopeRotationField.GetValue(__instance);
                Vector3 weapTempPosition = (Vector3)_weapTempPositionField.GetValue(__instance);
                Quaternion weapTempRotation = (Quaternion)_weapTempRotationField.GetValue(__instance);
                bool isAiming = (bool)_isAimingField.GetValue(__instance);

                Vector3 handsRotation = __instance.HandsContainer.HandsRotation.Get();
                Vector3 sway = __instance.HandsContainer.SwaySpring.Value;

                handsRotation += displacementStr * (isAiming ? __instance.AimingDisplacementStr : 1f) * new Vector3(sway.x, 0f, sway.z);
                handsRotation += sway;

                Vector3 rotationCenter = __instance._shouldMoveWeaponCloser ? __instance.HandsContainer.RotationCenterWoStock : __instance.HandsContainer.RotationCenter;
                Vector3 weapRootPivot = __instance.HandsContainer.WeaponRootAnim.TransformPoint(rotationCenter);

                bool allStancesAreReset = _hasResetActiveAim && _hasResetLowReady && _hasResetHighReady && _hasResetShortStock && _hasResetPistolPos && !StanceController.DoLeftShoulderTransition;
                bool isInStance =
                    StanceController.CurrentStance == EStance.HighReady ||
                    StanceController.CurrentStance == EStance.LowReady ||
                    StanceController.CurrentStance == EStance.ShortStock ||
                    StanceController.CurrentStance == EStance.ActiveAiming ||
                    StanceController.CurrentStance == EStance.Melee ||
                    StanceController.DoLeftShoulderTransition;
                bool isInShootableStance =
                    StanceController.CurrentStance == EStance.ShortStock ||
                    StanceController.CurrentStance == EStance.ActiveAiming ||
                    StanceController.TreatWeaponAsPistolStance ||
                    StanceController.CurrentStance == EStance.Melee;
                bool cancelBecauseShooting = PluginConfig.RememberStanceFiring.Value && !isAiming && StanceController.IsFiringFromStance && !isInShootableStance;
                bool doStanceRotation = (isInStance || !allStancesAreReset || StanceController.CurrentStance == EStance.PistolCompressed) && !cancelBecauseShooting;
                bool allowActiveAimReload = PluginConfig.ActiveAimReload.Value && PlayerState.IsInReloadOpertation && !PlayerState.IsAttemptingToReloadInternalMag && !PlayerState.IsQuickReloading;
                bool cancelStance =
                    (StanceController.CancelActiveAim && StanceController.CurrentStance == EStance.ActiveAiming && !allowActiveAimReload) ||
                    (StanceController.CancelHighReady && StanceController.CurrentStance == EStance.HighReady) ||
                    (StanceController.CancelLowReady && StanceController.CurrentStance == EStance.LowReady) ||
                    (StanceController.CancelShortStock && StanceController.CurrentStance == EStance.ShortStock); // || (StanceController.CancelPistolStance && StanceController.PistolIsCompressed)

                _currentRotation = Quaternion.Slerp(_currentRotation, __instance.IsAiming && allStancesAreReset ? Quaternion.identity : doStanceRotation ? _stanceRotation : Quaternion.identity, doStanceRotation ? _stanceRotationSpeed * PluginConfig.StanceRotationSpeedMulti.Value : __instance.IsAiming ? 7f * aimSpeed * dt : 8f * dt); //__instance.IsAiming ? 8f * aimSpeed * dt

                if (PluginConfig.EnableAdditionalRec.Value)
                {
                    ShootController.DoVisualRecoil(ref _targetRecoil, ref _currentRecoil, ref weapTempRotation);
                }

                __instance.HandsContainer.WeaponRootAnim.SetPositionAndRotation(weapTempPosition, weapTempRotation * _currentRotation);

                if (!Plugin.ServerConfig.enable_stances) return;

                if (StanceController.TreatWeaponAsPistolStance)//&& StanceController.CurrentStance != EStance.PatrolStance
                {
                    if (StanceController.CurrentStance == EStance.PistolCompressed && !StanceController.IsAiming && !_isResettingPistol && !StanceController.IsBlindFiring) //&& !__instance.LeftStance
                    {
                        StanceController.StanceBlender.Target = 1f;
                    }
                    else
                    {
                        StanceController.StanceBlender.Target = 0f;
                    }

                    if ((StanceController.CurrentStance != EStance.PistolCompressed && !StanceController.IsAiming && !_isResettingPistol) || StanceController.IsBlindFiring) // || __instance.LeftStance
                    {
                        StanceController.StanceTargetPosition = Vector3.Lerp(StanceController.StanceTargetPosition, Vector3.zero, 5f * dt);
                    }

                    _hasResetActiveAim = true;
                    _hasResetHighReady = true;
                    _hasResetLowReady = true;
                    _hasResetShortStock = true;
                    _hasResetMelee = true;
                    StanceController.DoPistolStances(false, __instance, ref _stanceRotation, dt, ref _hasResetPistolPos, player, ref _stanceRotationSpeed, ref _isResettingPistol, fc, ____vCameraTarget);
                }
                else if (!StanceController.TreatWeaponAsPistolStance || WeaponStats.HasShoulderContact)
                {
                    if ((!isInStance && allStancesAreReset) || (cancelBecauseShooting && !isInShootableStance) || StanceController.IsAiming || cancelStance || StanceController.IsBlindFiring || StanceController.IsLeftShoulder)
                    {
                        StanceController.StanceBlender.Target = 0f;
                    }
                    else if (isInStance)
                    {
                        StanceController.StanceBlender.Target = 1f;
                    }

                    if (((!isInStance && allStancesAreReset) && !cancelBecauseShooting && !StanceController.IsAiming) || StanceController.IsBlindFiring || StanceController.IsLeftShoulder)
                    {
                        StanceController.StanceTargetPosition = Vector3.Lerp(StanceController.StanceTargetPosition, Vector3.zero, 5f * dt);
                    }

                    _hasResetPistolPos = true;
                    StanceController.DoRifleStances(player, fc, false, __instance, ref _stanceRotation, dt, ref _isResettingShortStock, ref _hasResetShortStock, ref _hasResetLowReady, ref _hasResetActiveAim, ref _hasResetHighReady, ref _isResettingHighReady, ref _isResettingLowReady, ref _isResettingActiveAim, ref _stanceRotationSpeed, ref _hasResetMelee, ref _isResettingMelee, ref _didHalfMeleeAnim, ____vCameraTarget);
                }

                if (PluginConfig.EnableExtraProcEffects.Value) StanceController.DoExtraPosAndRot(__instance, player);
                StanceController.DoPatrolStance(__instance, player);

                StanceController.HasResetActiveAim = _hasResetActiveAim;
                StanceController.HasResetHighReady = _hasResetHighReady;
                StanceController.HasResetLowReady = _hasResetLowReady;
                StanceController.HasResetShortStock = _hasResetShortStock;
                StanceController.HasResetPistolPos = _hasResetPistolPos;
                StanceController.HasResetMelee = _hasResetMelee;
            }
        }
    }
}
