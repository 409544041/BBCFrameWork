// ******************************************************************
//       /\ /|       @file       EnumSkill.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumSkill
    {
        None = 0,
        [EnumName("施法者生命-5,目标生命+10")] EnumSkill1 = 1,  //施法者生命-5,目标生命+10
        [EnumName("受击时 生命恢复10;生命50%以下时，攻击力5")] EnumSkill2 = 2,  //受击时 生命恢复10;生命50%以下时，攻击力5
        [EnumName("施法者获得buff1，目标生命-5")] EnumSkill3 = 3,  //施法者获得buff1，目标生命-5
        [EnumName("生命最大值+10")] EnumSkill4 = 4,  //生命最大值+10
        [EnumName("近战普通攻击")] EnumSkill5 = 5,  //近战普通攻击
    }
}