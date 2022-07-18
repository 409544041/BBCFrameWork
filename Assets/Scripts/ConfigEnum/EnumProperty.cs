// ******************************************************************
//       /\ /|       @file       EnumProperty.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumProperty
    {
        None = 0,
        [EnumName("当前生命值")] CurHp = 1,  //当前生命值
        [EnumName("当前法力值")] CurMp = 2,  //当前法力值
        [EnumName("最大生命值")] MaxHp = 3,  //最大生命值
        [EnumName("最大法力值")] MaxMp = 4,  //最大法力值
        [EnumName("攻击力")] Atk = 5,  //攻击力
    }
}