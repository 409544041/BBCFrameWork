// ******************************************************************
//       /\ /|       @file       AnimationDef
//       \ V/        @brief      动画定义
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-18 12:39
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public static class AnimationDef
    {
        public static readonly string Move = "Move"; //移动
        public static readonly int MoveHash = Animator.StringToHash(Move);
    }
}