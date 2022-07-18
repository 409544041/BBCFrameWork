// ******************************************************************
//       /\ /|       @file       DrawInInspectorAttribute
//       \ V/        @brief      需要绘制的字段
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-07 23:27
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;

namespace Rabi
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DrawInInspectorAttribute : Attribute
    {
    }
}