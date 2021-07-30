using HarmonyLib;
using Nunppong;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Nunppong.Datasheet;

namespace LostRuinsMod
{

    [HarmonyPatch]
    class TitleViewPatch
    {
        public static BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        public static RectTransform ngPlus;
        public static LabelView newGamePlusButton;
        public static List<DropItem> gameItems = new List<DropItem>();
        public static bool IsNGP { get; set; } = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TitleView), "Show")]
        public static void TitleViewAwake(TitleView __instance)
        {
            bool passed = false;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                // This is retarded, need to fix this
                if (passed)
                    break;

                if (obj.scene.name == "Game" || obj.name == "game")
                {

                    //Console.Out.WriteLine("FOUND SCENE GAME");
                    //Console.Out.WriteLine(obj.name);

                    if (obj.name == "Menu")
                    {
                        //Console.Out.WriteLine("------- " + obj.GetComponentsInChildren<RectTransform>(true).Length);

                        foreach (RectTransform obje in obj.GetComponentsInChildren<RectTransform>(false))
                        {
                            //Console.Out.WriteLine("Menu Child: " + obje.name + " GO: " + obje.gameObject.name + " Layer: " + obje.gameObject.layer + " TYPEOF: " + obje.GetType());
                            if (obje.name == "NewGame" && !passed)
                            {
                                // Ugly stuff, w/e
                                passed = true;
                                if (ngPlus == null)
                                {
                                    ngPlus = UnityEngine.Object.Instantiate(obje, obje.transform.parent);
                                    
                                    // TODO: Change place?
                                    newGamePlusButton = ngPlus.GetComponent<LabelView>();
                                }

                                ngPlus.gameObject.name = "NewGamePlus";
                                ngPlus.SetSiblingIndex(0);
                                LabelView label = ngPlus.GetComponent<LabelView>();
                                label.label = "NewGamePlus";
                                label.SetText("New Game+");

                                break;
                            }
                        }
                    } 
                }
            }
        }


        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(TitleView), "UpdateContinueButton")]
        //public static void UpdateContinueButtonPrePatch()
        //{
        //    Console.Out.WriteLine("UpdateContinueButton PASSED");
        //}

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "PrevButton")]
        public static bool PrevButtonPrePatch(TitleView __instance)
        {
            if (__instance.CurrentButton == null)
            {
                return false;
            }
            if (__instance.CurrentButton == __instance.continueButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { newGamePlusButton });
                
            }
            else if (__instance.CurrentButton == __instance.newGameButton && __instance.continueButton.isActiveAndEnabled)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.continueButton }); 
            }
            else if (__instance.CurrentButton == __instance.galleryButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.newGameButton });
            }
            else if (__instance.CurrentButton == __instance.settingsButton)
            {
                if (__instance.galleryButton.isActiveAndEnabled)
                {
                    typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.galleryButton});
                }
                else
                {
                    typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.newGameButton });
                }
            }
            else if (__instance.CurrentButton == __instance.quitButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.settingsButton });
            }
            
            //typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(__instance, null);
            MethodInvoker.GetHandler(AccessTools.Method(typeof(TitleView), "HideAllButtons")).Invoke(__instance, null);
            MethodInvoker.GetHandler(AccessTools.Method(typeof(TitleView), "ShowCurrentButton")).Invoke(__instance, null);
            Singleton<SoundManager>.Instance.PlayUISound(GUISoundType.Navigate);

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "NextButton")]
        public static bool NextButtonPrePatch(TitleView __instance)
        {
            if (__instance.CurrentButton == newGamePlusButton && __instance.continueButton.isActiveAndEnabled)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.continueButton });

            }
            else if (__instance.CurrentButton == __instance.continueButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.newGameButton });
            }
            else if (__instance.CurrentButton == __instance.newGameButton)
            {
                if (__instance.galleryButton.isActiveAndEnabled)
                {
                    typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.galleryButton });
                }
                else
                {
                    typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.settingsButton });
                }
            }
            else if (__instance.CurrentButton == __instance.galleryButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.settingsButton });
            }
            else if (__instance.CurrentButton == __instance.settingsButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", flags).Invoke(__instance, new object[] { __instance.quitButton });
            }

            //typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(__instance, null);
            //typeof(TitleView).GetMethod("ShowCurrentButton", flags).Invoke(__instance, null);
            MethodInvoker.GetHandler(AccessTools.Method(typeof(TitleView), "HideAllButtons")).Invoke(__instance, null);
            MethodInvoker.GetHandler(AccessTools.Method(typeof(TitleView), "ShowCurrentButton")).Invoke(__instance, null);
            
            Singleton<SoundManager>.Instance.PlayUISound(GUISoundType.Navigate);

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "HideAllButtons")]
        public static bool HideAllButtonsPrePatch(TitleView __instance)
        {
            if(newGamePlusButton != null)
            {
                newGamePlusButton.Disable();
            }

            LabelView labelView = __instance.continueButton;
            if (labelView != null)
            {
                labelView.Disable();
            }
            LabelView labelView2 = __instance.newGameButton;
            if (labelView2 != null)
            {
                labelView2.Disable();
            }
            LabelView labelView3 = __instance.galleryButton;
            if (labelView3 != null)
            {
                labelView3.Disable();
            }
            LabelView labelView4 = __instance.settingsButton;
            if (labelView4 != null)
            {
                labelView4.Disable();
            }
            LabelView labelView5 = __instance.quitButton;
            if (labelView5 == null)
            {
                return false;
            }
            labelView5.Disable();

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "ClickCurrentButton")]
        public static bool ClickCurrentButtonPrePatch(TitleView __instance)
        {
            if (__instance.CurrentButton == null)
            {
                return false;
            }
            Singleton<SoundManager>.Instance.PlayUISound(GUISoundType.Toggle);
            if (__instance.CurrentButton == newGamePlusButton)
            {
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Difficulty);
                IsNGP = true;
                return false;
            }
            if (__instance.CurrentButton == __instance.continueButton)
            {
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Load);
                return false;
            }
            if (__instance.CurrentButton == __instance.newGameButton)
            {
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Difficulty);
                return false;
            }
            if (__instance.CurrentButton == __instance.galleryButton)
            {
                Singleton<StageManager>.Instance.LoadGallery();
                return false;
            }
            if (__instance.CurrentButton == __instance.settingsButton)
            {
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Settings);
                return false;
            }
            if (__instance.CurrentButton == __instance.quitButton)
            {
                Application.Quit();
            }

            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "LoadResourceAssets")]
        public static void GameManagerLoadResourceAssetsPostPatch(GameManager __instance)
        {
            foreach (DropItem dropItem in __instance.DropItemManager.DropItems)
            {
                //Console.Out.WriteLine("WEAPON: " + dropItem.name);
                gameItems.Add(dropItem);
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerInventory), "OnLoadStage")]
        public static void StageManagerOnLoadCompletePostPatch(Stage stage, StageLoadMode loadMode, PlayerInventory __instance)
        {
            

            if (loadMode == StageLoadMode.New || loadMode == StageLoadMode.Reset || loadMode == StageLoadMode.LoadSceneEvent)
            {
                __instance.Slots.Clear();
                
                if (IsNGP)
                {
                    foreach (DropItem dropItem in gameItems)
                    {
                        __instance.AddItem(dropItem, 1, __instance.NextSlotOrder);
                    }
                }

                foreach (PlayerInitItems playerInitItems in Singleton<GameManager>.Instance.PlayerInitItems)
                {
                    if (playerInitItems.CheckDifficulty(Singleton<GameManager>.Instance.difficulty))
                    {
                        foreach (PlayerInitItems.InitItem initItem in playerInitItems.initItems)
                        {
                            __instance.Pick(initItem.item, initItem.count, false);
                        }
                    }
                }
            }
            if (loadMode == StageLoadMode.Load && Singleton<GameManager>.Instance.difficulty == DifficultyMode.Hardcore)
            {
                __instance.AddItem(Singleton<DropItemManager>.Instance.dropItemSetting.diskItem, Singleton<SaveLoadManager>.Instance.LastDiskCount, 0);
            }
            if (loadMode == StageLoadMode.Load)
            {
                Singleton<GameEventManager>.Instance.OnPlayerInventoryLoad();
            }
        }
    }
}
