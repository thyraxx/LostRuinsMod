using HarmonyLib;
using Nunppong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEditor;

namespace LostRuinsMod
{

    [HarmonyPatch]
    class TitleViewPatch
    {
        public static BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        public static RectTransform ngPlus;
        public static LabelView newGamePlusButton;

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(TitleView), "Show", new Type[] { typeof(float) })]
        //public static bool ShowPatch(float __0, TitleView __instance, CanvasGroup ___hideGroup)
        //{
        //    typeof(GameGUIView).GetMethod("Show", flags).InvokeNotOverride(__instance, new object[] { __0 });
        //    //MethodInvoker.GetHandler(AccessTools.Method(typeof(GameGUIView), nameof(GameGUIView.Show))).Invoke(__instance, new object[] { __0 });
        //    Console.Out.WriteLine("AFTER SHOW");
        //    __instance.hideGroup.alpha = 1f;


        //    LabelView labelView = __instance.continueButton;
        //    if (labelView != null)
        //    {
        //        labelView.Show();
        //    }
        //    LabelView labelView2 = __instance.newGameButton;
        //    if (labelView2 != null)
        //    {
        //        labelView2.Show();
        //    }
        //    LabelView labelView3 = __instance.galleryButton;
        //    if (labelView3 != null)
        //    {
        //        labelView3.Show();
        //    }
        //    LabelView labelView4 = __instance.settingsButton;
        //    if (labelView4 != null)
        //    {
        //        labelView4.Show();
        //    }
        //    LabelView labelView5 = __instance.quitButton;
        //    if (labelView5 != null)
        //    {
        //        labelView5.Show();
        //    }

        //    Console.Out.WriteLine("INVOKING");

        //    typeof(TitleView).GetMethod("UpdateContinueButton", flags).Invoke(__instance, null);
        //    typeof(TitleView).GetMethod("UpdateGalleryButton", flags).Invoke(__instance, null);
        //    typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(__instance, null);
        //    typeof(TitleView).GetMethod("ShowCurrentButton", flags).Invoke(__instance, null);


        //    __instance.tooltipViews.SetTooltip(new TooltipInfo
        //    {
        //        button1 = InputButtonType.Accept,
        //        button1Desc = StringManager.Get("AcceptTooltip")
        //    });
        //    __instance.tooltipViews.Show(0.2f);
        //    //typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(typeof(TitleView), null);

        //    return false;

        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TitleView), "Show")]
        public static void TitleViewAwake(TitleView __instance)
        {
            

            Console.Out.WriteLine("LABELVIEW AWAKE");
            Console.Out.WriteLine(__instance.continueButton.label);

            List<GameObject> objectsInScene = new List<GameObject>();

            bool passed = false;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.scene.name == "Game" || obj.name == "game")
                {

                    //Console.Out.WriteLine("FOUND SCENE GAME");
                    //Console.Out.WriteLine(obj.name);

                    if (obj.name == "Menu")
                    {
                        Console.Out.WriteLine("------- " + obj.GetComponentsInChildren<RectTransform>(true).Length);

                        foreach (RectTransform obje in obj.GetComponentsInChildren<RectTransform>(false))
                        {
                            Console.Out.WriteLine("Menu Child: " + obje.name + " GO: " + obje.gameObject.name + " Layer: " + obje.gameObject.layer + " TYPEOF: " + obje.GetType());
                            if (obje.name == "NewGame" && !passed)
                            {
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
                                label.label = "TEST";
                                label.SetText("New Game+");
                            }
                        }
                    } 
                }
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "UpdateContinueButton")]
        public static void UpdateContinueButtonPrePatch()
        {
            Console.Out.WriteLine("UpdateContinueButton PASSED");
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
                Singleton<GUIManager>.Instance.SetState(GUIManager.GameGUIState.Load);
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
