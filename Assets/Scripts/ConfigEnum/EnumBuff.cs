// ******************************************************************
//       /\ /|       @file       EnumBuff.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumBuff
    {
        None = 0,
        [EnumName("最大生命+5")] Buff1 = 1,  //最大生命+5
        [EnumName("每秒 当前生命-5")] Buff2 = 2,  //每秒 当前生命-5
    }
}