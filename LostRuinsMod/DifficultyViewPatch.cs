using HarmonyLib;
using Nunppong;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Nunppong.DifficultyView;

namespace LostRuinsMod
{
    [HarmonyPatch]
    class DifficultyViewPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DifficultyView), "SetIcons")]
        public static bool DifficultyViewPrePatch(DifficultyView __instance)
        {
			__instance.Icons.Clear();
			__instance.easyIcon.gameObject.SetActive(true);
			__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Easy, __instance.easyIcon));
			__instance.normalIcon.gameObject.SetActive(true);
			__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Normal, __instance.normalIcon));
			__instance.hardIcon.gameObject.SetActive(true);
			__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Hard, __instance.hardIcon));
			__instance.hardCoreIcon.gameObject.SetActive(true);
			__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Hardcore, __instance.hardCoreIcon));
			if (Singleton<GameSettingsManager>.Instance.unlockGameMode || Singleton<SystemPlatformManager>.Instance.SystemPlatform.IsUnlockStat(StatUnlock.GameMode))
			{
				__instance.witchIcon.gameObject.SetActive(true);
				__instance.assasinIcon.gameObject.SetActive(true);
				__instance.bossIcon.gameObject.SetActive(true);
				__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Witch, __instance.witchIcon));
				__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Assassin, __instance.assasinIcon));
				__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, DifficultyMode.Boss, __instance.bossIcon));

				foreach(RectTransform rect in __instance.GetComponentsInChildren<RectTransform>())
                {
					if(rect.name.Equals("BossIcon"))
                    {
						RectTransform rectT = UnityEngine.Object.Instantiate(rect, rect.transform.parent);
						rectT.gameObject.name = "BossIconTest";
						Image bossRushIcon = rectT.GetComponent<Image>();

						DifficultyMode bossRush = (DifficultyMode)DifficultyModeExtra.BossRush;
						__instance.Icons.Add(new DifficultyView.IconInfo(__instance.Icons.Count, bossRush, bossRushIcon));
					}

					if (rect.name.Equals("DificultyTitle"))
                    {

                    }

						Console.WriteLine(rect.name);
                }

				//test.name = "anotherIcon";
				
            }
			else
			{
				__instance.witchIcon.gameObject.SetActive(false);
				__instance.assasinIcon.gameObject.SetActive(false);
				__instance.bossIcon.gameObject.SetActive(false);
			}
			__instance.CurrentIndex = 1;
			foreach (DifficultyView.IconInfo iconInfo in __instance.Icons)
			{
				iconInfo.ResetIcon(__instance);
			}

			return false;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(DifficultyView), "HandleInput")]
		public static void HandleInputPostPatch(DifficultyView __instance)
        {
			if(__instance.title.text.Equals("DifficultyMode." + (int)DifficultyModeExtra.BossRush))
            {
				__instance.title.text = "Boss Rush";
				__instance.desc.text = "Challenge yourself to defeat all bosses after each other.";
            }
		}
    }
}
