using HarmonyLib;
using Nunppong;
using Nunppong.Datasheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LostRuinsMod
{
    [HarmonyPatch]
    class PlayerInventoryPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerInventory), "OnLoadStage")]
        public static void StageManagerOnLoadCompletePostPatch(Stage stage, StageLoadMode loadMode, PlayerInventory __instance)
        {
            
            if (loadMode == StageLoadMode.New || loadMode == StageLoadMode.Reset || loadMode == StageLoadMode.LoadSceneEvent)
            {
                // Only activate if New Game+ is selected
                if (CustomGameInfo.IsNGP)
                {
                    // Reflection, get the original hashItems list for lookup
                    HashSet<DropItem> hashItems = (HashSet<DropItem>)typeof(PlayerInventory).GetProperty("PickedItems").GetValue(__instance, null);

                    foreach (DropItem newDrop in CustomGameInfo.gameItems)
                    {
                        DropItem dropItem = Singleton<DropItemManager>.Instance.FindAndCheckDropItem(newDrop.id);

                        if (dropItem != null)
                        {

                            // Special cases, we don't want certain items in specific difficulties
                            switch (dropItem.id)
                            {

                                case "FloppyDisk":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    continue;

                                case "BlessingOfGodess":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Easy)
                                    {
                                        continue;
                                    }
                                    break;

                                case "BraveHeart":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Hard)
                                    {
                                        continue;
                                    }
                                    break;

                                case "DeathlyHallow":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Hardcore)
                                    {
                                        continue;
                                    }
                                    break;

                                case "Trinity":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Boss)
                                    {
                                        continue;
                                    }
                                    break;

                                case "AssassinsGreed":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Assassin)
                                    {
                                        continue;
                                    }
                                    break;

                                case "WitchsHat":
                                    //Console.Out.WriteLine("Found :" + dropItem.id);
                                    if (Singleton<GameManager>.Instance.difficulty != DifficultyMode.Witch)
                                    {
                                        continue;
                                    }
                                    break;
                            }

                            // Check if the item isn't already given
                            if (!hashItems.Any(r => r.id == dropItem.id))
                            {
                                __instance.Pick(dropItem, 1, false);
                                __instance.AddKnownItem(dropItem);
                            }
                        }
                    }

                    __instance.UpdateKnownItems();
                }
            }
        }
    }
}
