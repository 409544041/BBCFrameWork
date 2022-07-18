// ******************************************************************
//       /\ /|       @file       StateNodeType.cs
//       \ V/        @brief      状态节点类型
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-16 03:26:12
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum StateNodeType
    {
        [EnumName("等待选择")] WaitSelect,
        [EnumName("激活")] Active,
        [EnumName("颜色")] Color,
        [EnumName("灰度")] Gray,
    }
}