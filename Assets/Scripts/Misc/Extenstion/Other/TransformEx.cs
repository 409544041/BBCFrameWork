// ******************************************************************
//       /\ /|       @file       TransformEx.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-06-23 11:55:13
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;

namespace Rabi
{
    public static class TransformEx
    {
        /// <summary>
        /// 根据施法者位置和方向设置位置
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="caster"></param>
        /// <param name="offset"></param>
        public static void SetPositionEx(this Transform transform, ThingWithComps caster, Vector3 offset)
        {
            transform.position = caster.transform.position + caster.dir switch
            {
                EnumDirection.North => new Vector3(offset.y, offset.x, offset.z),
                EnumDirection.South => new Vector3(offset.y, -offset.x, offset.z),
                EnumDirection.West => new Vector3(-offset.x, offset.y, offset.z),
                EnumDirection.East => offset,
                EnumDirection.None => offset,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="size"></param>
        /// <param name="transform"></param>
        public static void SetSizeEx(this Transform transform, ThingWithComps caster, Vector3 size)
        {
            transform.localScale = caster.dir switch
            {
                EnumDirection.North => new Vector3(size.y, size.x, size.z),
                EnumDirection.South => new Vector3(size.y, -size.x, size.z),
                EnumDirection.West => new Vector3(-size.x, size.y, size.z),
                EnumDirection.East => size,
                EnumDirection.None => size,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// 获取血条挂点
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Transform GetHpSocket(this Transform unit)
        {
            return unit.Find("HpSocket");
        }
    }
}