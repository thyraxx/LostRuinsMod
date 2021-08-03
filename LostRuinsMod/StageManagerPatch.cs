using HarmonyLib;
using Nunppong;

namespace LostRuinsMod
{
    [HarmonyPatch]
    class StageManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StageManager), "StartNewGame")]
        public static void StageManagerStartNewGamePrePatch()
        {
            CustomGameInfo.IsNGP = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StageManager), "LoadTitle")]
        public static void SaveLoadManagerClearPrePatch()
        {
            CustomGameInfo.IsNGP = false;
        }
    }
}
