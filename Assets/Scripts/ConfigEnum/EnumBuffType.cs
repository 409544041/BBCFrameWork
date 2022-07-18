// ******************************************************************
//       /\ /|       @file       EnumBuffType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumBuffType
    {
        None = 0,
        [EnumName("属性型buff 参数")] Property = 1,  //属性型buff 参数
        [EnumName("间隔触发型buff 参数")] Trigger = 2,  //间隔触发型buff 参数
    }
}