// ******************************************************************
//       /\ /|       @file       ListEx.cs
//       \ V/        @brief      List结构扩展
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-05 01:29:41
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Rabi
{
    public static class ListEx
    {
        /// <summary>
        /// 打乱顺序
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            var i = list.Count;
            while (i > 1)
            {
                i--;
                var index = Random.Range(0, i);
                (list[index], list[i]) = (list[i], list[index]);
            }
        }
    }
}