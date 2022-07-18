// ******************************************************************
//       /\ /|       @file       AtlasUtil.cs
//       \ V/        @brief      图集工具
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-18 12:38:10
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Text;

namespace Rabi
{
    public static class AtlasUtil
    {
        /// <summary>
        /// 获取sprite在图集中的名字
        /// </summary>
        /// <param name="spritePath"></param>
        /// <returns></returns>
        public static string GetSpriteName(string spritePath)
        {
            if (string.IsNullOrEmpty(spritePath))
            {
                Logger.Error("路径为空");
                return string.Empty;
            }

            var folderNameArray = spritePath.Split('/');
            if (folderNameArray.Length <= 2)
            {
                Logger.Error($"路径格式错误 path:{spritePath}");
                return string.Empty;
            }

            var fileName = folderNameArray[folderNameArray.Length - 1];
            var spriteNameArray = fileName.Split('.');
            if (spriteNameArray.Length == 2)
            {
                return spriteNameArray[0];
            }

            Logger.Error($"路径格式错误 path:{spritePath}");
            return string.Empty;
        }

        /// <summary>
        /// 获取sp所在图集的路径
        /// </summary>
        /// <param name="spritePath"></param>
        /// <returns></returns>
        public static string GetAtlasPath(string spritePath)
        {
            if (string.IsNullOrEmpty(spritePath))
            {
                Logger.Error("路径为空");
                return string.Empty;
            }

            var folderNameArray = spritePath.Split('/');
            if (folderNameArray.Length < 2)
            {
                Logger.Error($"路径格式错误 path:{spritePath}");
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < folderNameArray.Length - 1; i++)
            {
                stringBuilder.Append($"{folderNameArray[i]}/");
            }

            stringBuilder.Append($"{folderNameArray[folderNameArray.Length - 2]}.spriteatlas");
            return stringBuilder.ToString();
        }
    }
}