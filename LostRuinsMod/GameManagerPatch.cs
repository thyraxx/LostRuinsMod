using HarmonyLib;
using Nunppong;
using Nunppong.Datasheet;
using System;
using System.Linq;

namespace LostRuinsMod
{
    [HarmonyPatch]
    class GameManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "LoadResourceAssets")]
        public static void GameManagerLoadResourceAssetsPostPatch(GameManager __instance)
        {

            foreach (DropItem dropItem in __instance.DropItemManager.DropItems)
            {
                CustomGameInfo.gameItems.Add(dropItem);
            }
        }
    }
}
