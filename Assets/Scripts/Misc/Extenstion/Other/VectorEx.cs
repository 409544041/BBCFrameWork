// ******************************************************************
//       /\ /|       @file       VectorEx.cs
//       \ V/        @brief      向量扩展
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-05 16:52:04
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public static class VectorEx
    {
        public static JObject ToJson(this Vector2 vector2)
        {
            var jObject = new JObject
            {
                { "x", vector2.x },
                { "y", vector2.y }
            };
            return jObject;
        }

        public static Vector2 FromJson(JObject jObject)
        {
            if (jObject == null)
            {
                return Vector2.zero;
            }

            var vector2 = new Vector2
            {
                x = jObject["x"]?.Value<float>() ?? 0f,
                y = jObject["y"]?.Value<float>() ?? 0f
            };
            return vector2;
        }
    }
}