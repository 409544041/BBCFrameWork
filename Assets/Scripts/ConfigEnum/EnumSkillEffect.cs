// ******************************************************************
//       /\ /|       @file       EnumSkillEffect.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumSkillEffect
    {
        None = 0,
        [EnumName("生命-5")] EnumSkillEffect1 = 1,  //生命-5
        [EnumName("目标生命+10")] EnumSkillEffect2 = 2,  //目标生命+10
        [EnumName("受击时 生命恢复10")] EnumSkillEffect3 = 3,  //受击时 生命恢复10
        [EnumName("生命50%以下时，攻击力5")] EnumSkillEffect4 = 4,  //生命50%以下时，攻击力5
        [EnumName("添加buff1")] EnumSkillEffect5 = 5,  //添加buff1
        [EnumName("生命最大值+10")] EnumSkillEffect6 = 6,  //生命最大值+10
        [EnumName("近战普通攻击")] EnumSkillEffect7 = 7,  //近战普通攻击
    }
}