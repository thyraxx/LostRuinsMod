using Nunppong;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LostRuinsMod
{
    static class CustomGameInfo
    {
        public static BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
        public static RectTransform ngPlusRect;
        public static LabelView newGamePlusButton;
        public static List<DropItem> gameItems = new List<DropItem>();
        public static bool IsNGP { get; set; } = false;
    }

    // To add our own Difficulty enums
    enum DifficultyModeExtra
    {
        BossRush = 6
    }
}
