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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TitleView), "Show", new Type[] { typeof(float) })]
        public static bool ShowPatch(float __0, TitleView __instance, CanvasGroup ___hideGroup)
        {
            typeof(GameGUIView).GetMethod("Show", flags).InvokeNotOverride(__instance, new object[] { __0 });
            //MethodInvoker.GetHandler(AccessTools.Method(typeof(GameGUIView), nameof(GameGUIView.Show))).Invoke(__instance, new object[] { __0 });
            Console.Out.WriteLine("AFTER SHOW");
            __instance.hideGroup.alpha = 1f;


            LabelView labelView = __instance.continueButton;
            // LabelView
            if (labelView != null)
            {
                labelView.Show();
            }
            LabelView labelView2 = __instance.newGameButton;
            if (labelView2 != null)
            {
                labelView2.Show();
            }
            LabelView labelView3 = __instance.galleryButton;
            if (labelView3 != null)
            {
                labelView3.Show();
            }
            LabelView labelView4 = __instance.settingsButton;
            if (labelView4 != null)
            {
                labelView4.Show();
            }
            LabelView labelView5 = __instance.quitButton;
            if (labelView5 != null)
            {
                labelView5.Show();
            }

            Console.Out.WriteLine("INVOKING");

            typeof(TitleView).GetMethod("UpdateContinueButton", flags).Invoke(__instance, null);
            typeof(TitleView).GetMethod("UpdateGalleryButton", flags).Invoke(__instance, null);
            typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(__instance, null);
            typeof(TitleView).GetMethod("ShowCurrentButton", flags).Invoke(__instance, null);


            __instance.tooltipViews.SetTooltip(new TooltipInfo
            {
                button1 = InputButtonType.Accept,
                button1Desc = StringManager.Get("AcceptTooltip")
            });
            __instance.tooltipViews.Show(0.2f);
            //typeof(TitleView).GetMethod("HideAllButtons", flags).Invoke(typeof(TitleView), null);

            return false;

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TitleView), "Show")]
        public static void TitleViewAwake(TitleView __instance)
        {
            Console.Out.WriteLine("LABELVIEW AWAKE");
            Console.Out.WriteLine(__instance.continueButton.label);


            //GameObject go = GameObject.Find("UICamera");
            //Console.Out.WriteLine(go.GetComponentsInChildren<GameObject>().Length);
            //Console.Out.WriteLine(go.GetComponents<GameObject>().Length);

            //GameObject go1 = GameObject.Find("UICanvas");
            //Console.Out.WriteLine(go1.GetComponentsInChildren<GameObject>().Length);
            //Console.Out.WriteLine(go1.GetComponents<GameObject>().Length);


            //GameObject go2 = GameObject.Find("TitleView");
            //Console.Out.WriteLine(go2.GetComponentsInChildren<GameObject>().Length);
            //Console.Out.WriteLine(go2.GetComponents<GameObject>().Length);


            //GameObject go3 = GameObject.Find("Menu");
            //Console.Out.WriteLine(go3.GetComponentsInChildren<GameObject>().Length);
            //Console.Out.WriteLine(go3.GetComponents<GameObject>().Length);


            //GameObject go4 = GameObject.Find("Hide");
            //Console.Out.WriteLine(go4.GetComponentsInChildren<GameObject>().Length);
            //Console.Out.WriteLine(go4.GetComponents<GameObject>().Length);

            List<GameObject> objectsInScene = new List<GameObject>();

            bool passed = false;
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.scene.name == "Game" || obj.name == "game")
                {

                    //Console.Out.WriteLine("FOUND SCENE GAME");
                    //Console.Out.WriteLine(obj.name);
                    if (obj.name == "TitleView")
                    {
                        //foreach (GameObject obje in obj.GetComponentsInChildren<GameObject>())
                        //{
                        //    Console.Out.WriteLine("UICAMERA CHILDREN " + obje.name);
                        //}

                        //Console.Out.WriteLine(obj.GetComponentsInChildren<GameObject>().Length);
                    }

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
                                    ngPlus = UnityEngine.Object.Instantiate(obje, obje.transform.parent);

                                ngPlus.gameObject.name = "NewGamePlus";
                                ngPlus.SetSiblingIndex(0);
                                LabelView label = ngPlus.GetComponent<LabelView>();
                                label.label = "TEST";
                                label.SetText("TEST");

                            }

                        }

                        //foreach (TextMeshProUGUI obje in obj.GetComponentsInChildren<TextMeshProUGUI>(false))
                        //{
                        //    Console.Out.WriteLine("Menu TextMeshProUGUI: " + obje.name + " GO: " + obje.gameObject.name + " Layer: " + obje.gameObject.layer + " TYPEOF: " + obje.GetType());
                        //}
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
    }
}
