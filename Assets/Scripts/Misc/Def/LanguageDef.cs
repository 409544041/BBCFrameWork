// ******************************************************************
//       /\ /|       @file       LanguageDef.cs
//       \ V/        @brief      语言相关定义
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 06:23:20
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace Rabi
{
    public static class LanguageDef
    {
        //id映射后缀
        private static readonly Dictionary<int, string> LanguageIdToSuffix = new Dictionary<int, string>()
        {
            { LanguageId.English.GetHashCode(), "_en.png" },
            { LanguageId.Chinese.GetHashCode(), "_cn.png" },
            { LanguageId.Japanese.GetHashCode(), "_jp.png" },
        };

        //id映射后缀
        private static readonly Dictionary<string, int> FontName2FontId = new Dictionary<string, int>()
        {
            { "simfang", 1 },
            { "SourceHanSansBoldCn", 2 }
        };

        /// <summary>
        /// 获取后缀 默认英文
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public static string GetSuffix(int languageId)
        {
            return LanguageIdToSuffix.ContainsKey(languageId)
                ? LanguageIdToSuffix[languageId]
                : "_en.png";
        }

        /// <summary>
        /// 由于初版以中文为基准 所以字体一定传中文的
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <param name="defaultFontId">默认使用的多语言字体id</param>
        /// <returns></returns>
        public static int FindFontId(string fontName, int defaultFontId = 1)
        {
            //找不到的字体默认1号配置
            return FontName2FontId.ContainsKey(fontName) ? FontName2FontId[fontName] : defaultFontId;
        }
    }
}