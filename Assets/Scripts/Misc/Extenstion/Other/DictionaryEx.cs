// ******************************************************************
//       /\ /|       @file       DictionaryEx.cs
//       \ V/        @brief      字典扩展
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-03 05:43:12
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace Rabi
{
    public static class DictionaryEx
    {
        /// <summary>
        /// 从字典中查找字段并转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Find<T>(this Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key)) return dict[key].ToT<T>();
            Logger.Error($"找不到key:{key}");
            return default;
        }
    }
}