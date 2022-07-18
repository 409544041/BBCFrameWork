// ******************************************************************
//       /\ /|       @file       EnumValueType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumValueType
    {
        None = 0,
        [EnumName("整型")] Int = 1,  //整型
        [EnumName("浮点型")] Float = 2,  //浮点型
        [EnumName("布尔型")] Bool = 3,  //布尔型
    }
}