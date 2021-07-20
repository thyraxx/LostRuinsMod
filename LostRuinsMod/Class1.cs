using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LostRuinsMod
{
    [BepInPlugin("org.thyraxx.lostruinstestmod", "Lost Ruins Movement unlock", "0.0.1")]
    public class Class1 : BaseUnityPlugin
    {
        public const string pluginGuid = "org.thyraxx.lostruinstestmod";
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

            // For hot-reload
            harmony.UnpatchSelf();
        }
    }
}
