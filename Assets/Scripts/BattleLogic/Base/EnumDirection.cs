// ******************************************************************
//       /\ /|       @file       EnumDirection.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumDirection
    {
        None = 0,
        [EnumName("北")] North = 1,  //北
        [EnumName("南")] South = 2,  //南
        [EnumName("西")] West = 3,  //西
        [EnumName("东")] East = 4,  //东
    }
}