using HarmonyLib;
using Nunppong;
using System;
using UnityEngine;

namespace LostRuinsMod
{
    [HarmonyPatch]
    class PadVibratePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PadVibrate), "GetMagnitude", new Type[] { typeof(Vector2) })]
        public static void PadVibratePrePatch(ref float ___power, ref float ___maxRange, ref float ___minRange, ref float ___magnitude)
        {
            //Console.Out.WriteLine("BEFORE {0}", ___power);
            ___power = 0.1f;
            //Console.Out.WriteLine("AFTER {0}", ___power);
            //Console.Out.WriteLine("BEFORE {0}", ___minRange);
            ___minRange = 0.1f;
            //Console.Out.WriteLine("AFTER {0}", ___minRange);
            //Console.Out.WriteLine("BEFORE {0}", ___maxRange);
            ___maxRange = 0.5f;
            //Console.Out.WriteLine("AFTER {0}", ___maxRange);
            //Console.Out.WriteLine("{0}", ___magnitude);
        }
    }
}
