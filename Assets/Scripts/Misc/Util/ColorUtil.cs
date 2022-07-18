// ******************************************************************
//       /\ /|       @file       ColorUtil.cs
//       \ V/        @brief      颜色工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-01 08:10:13
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;

namespace Rabi
{
    public static class ColorUtil
    {
        /// <summary> Html颜色转换为Color </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color GetColor(string hex)
        {
            // 默认黑色
            if (string.IsNullOrEmpty(hex)) return Color.black;
            // 转换颜色
            hex = hex.ToLower();
            if (hex.IndexOf("#", StringComparison.Ordinal) != 0 || hex.Length != 7)
            {
                switch (hex)
                {
                    case "red": return Color.red;
                    case "green": return Color.green;
                    case "blue": return Color.blue;
                    case "yellow": return Color.yellow;
                    case "black": return Color.black;
                    case "white": return Color.white;
                    case "cyan": return Color.cyan;
                    case "gray": return Color.gray;
                    case "grey": return Color.grey;
                    case "magenta": return Color.magenta;
                    default: return Color.black;
                }
            }

            var r = Convert.ToInt32(hex.Substring(1, 2), 16);
            var g = Convert.ToInt32(hex.Substring(3, 2), 16);
            var b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}