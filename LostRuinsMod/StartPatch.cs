using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LostRuinsMod
{
    [BepInPlugin("org.thyraxx.lostruinsmod", "Lost Ruins Mod", "0.0.2")]
    public class StartPatch : BaseUnityPlugin
    {
        public const string pluginGuid = "org.thyraxx.lostruinsmod";
        Harmony harmony = new Harmony(pluginGuid);

        public new static ManualLogSource Logger;
        public void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo("LostRuinsTestMod Loaded!");

            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            Logger.LogInfo("LostRuinsTestMod Unloaded!");

            // For hot-reload scriptEngine
            harmony.UnpatchSelf();
        }
    }
}
