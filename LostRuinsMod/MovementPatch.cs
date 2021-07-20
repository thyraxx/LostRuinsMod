using HarmonyLib;
using Nunppong;
using UnityEngine;

namespace LostRuinsMod
{
	[HarmonyPatch]
	class MovementPatch
	{

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerHandleWeapon), "UpdateWeaponState")]
        public static bool UpdateWeaponState_PrePatch(PlayerHandleWeapon __instance)
        {
            //System.Console.WriteLine("INIT");
            if (__instance.State.Current == PlayerHandleWeapon.States.Attack)
            {
                //__instance.State.Current = PlayerHandleWeapon.States.Idle;
                //__instance.Player.ForceController.SetHorizontalForce(__instance.Player.ForceController.CurrentForce.x);
                __instance.Player.ForceController.SetHorizontalForce(__instance.Player.ForceController.CurrentForce.x);
                if (__instance.State.StateChangedTime + __instance.CurrentWeaponSetting.WeaponDuration < Time.time)
                {
                    __instance.StopWeapon();
                    __instance.State.Current = PlayerHandleWeapon.States.Delay;
                    __instance.State.Current = PlayerHandleWeapon.States.DelayToIdle;
                    //return false;


                }
            }
            //else if (__instance.State.Current == PlayerHandleWeapon.States.Delay)
            //{
            //    __instance.State.Current = PlayerHandleWeapon.States.Idle;

            //    __instance.Player.ForceController.SetHorizontalForce(__instance.Player.ForceController.CurrentForce.x);
            //    return false;
            //}
            return true;
        }

        //      [HarmonyPrefix]
        //[HarmonyPatch(typeof(PlayerHandleWeapon), "TryAttack")]
        //public static bool WeaponUseResult_PrePatch(PlayerHandleWeapon __instance, WeaponUseResult __result, WeaponItem weapon)
        //{
        //	System.Console.WriteLine("WEAPON USE");

        //	if (!__instance.Player.CanMove())
        //	{
        //		System.Console.WriteLine("CANT MOVE");

        //		__result = WeaponUseResult.Ok;
        //		return false;
        //	}

        //	return true;
        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerHandleWeapon), "CanMove")]
        public static void CanMove(bool __result)
        {
            __result = false;
        }
    }
}
