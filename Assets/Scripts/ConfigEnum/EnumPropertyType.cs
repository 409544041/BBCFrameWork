// ******************************************************************
//       /\ /|       @file       EnumPropertyType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumPropertyType
    {
        None = 0,
        [EnumName("固有属性")] Property = 1,  //固有属性
        [EnumName("固有属性系数")] PropertyFactor = 2,  //固有属性系数
        [EnumName("状态")] Status = 3,  //状态
    }
}