using HarmonyLib;
using Nunppong;
using UnityEngine;

namespace LostRuinsMod
{

    [HarmonyPatch]
    class TitleViewPatch
    {
       

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
                                if (CustomGameInfo.ngPlusRect == null)
                                {
                                    CustomGameInfo.ngPlusRect = UnityEngine.Object.Instantiate(obje, obje.transform.parent);

                                    // TODO: Change place?
                                    CustomGameInfo.newGamePlusButton = CustomGameInfo.ngPlusRect.GetComponent<LabelView>();
                                }

                                CustomGameInfo.ngPlusRect.gameObject.name = "NewGamePlus";
                                CustomGameInfo.ngPlusRect.SetSiblingIndex(0);
                                LabelView label = CustomGameInfo.ngPlusRect.GetComponent<LabelView>();
                                label.label = "NewGamePlus";
                                label.SetText("New Game+");

                                break;
                            }
                        }
                    } 
                }
            }
        }

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
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { CustomGameInfo.newGamePlusButton });
            }
            else if (__instance.CurrentButton == __instance.newGameButton && __instance.continueButton.isActiveAndEnabled)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.continueButton }); 
            }
            else if (__instance.CurrentButton == __instance.galleryButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.newGameButton });
            }
            else if (__instance.CurrentButton == __instance.settingsButton)
            {
                if (__instance.galleryButton.isActiveAndEnabled)
                {
                    typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.galleryButton});
                }
                else
                {
                    typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.newGameButton });
                }
            }
            else if (__instance.CurrentButton == __instance.quitButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.settingsButton });
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
            if (__instance.CurrentButton == CustomGameInfo.newGamePlusButton && __instance.continueButton.isActiveAndEnabled)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.continueButton });

            }
            else if (__instance.CurrentButton == __instance.continueButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.newGameButton });
            }
            else if (__instance.CurrentButton == __instance.newGameButton)
            {
                if (__instance.galleryButton.isActiveAndEnabled)
                {
                    typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.galleryButton });
                }
                else
                {
                    typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.settingsButton });
                }
            }
            else if (__instance.CurrentButton == __instance.galleryButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.settingsButton });
            }
            else if (__instance.CurrentButton == __instance.settingsButton)
            {
                typeof(TitleView).GetMethod("SetCurentButton", CustomGameInfo.flags).Invoke(__instance, new object[] { __instance.quitButton });
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
            if(CustomGameInfo.newGamePlusButton != null)
            {
                CustomGameInfo.newGamePlusButton.Disable();
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
            if (__instance.CurrentButton == CustomGameInfo.newGamePlusButton)
            {
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Difficulty);
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
    }
}
