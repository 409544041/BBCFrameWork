// ******************************************************************
//       /\ /|       @file       EnumSkillEffectType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumSkillEffectType
    {
        None = 0,
        [EnumName("主动技能效果")] Active = 1,  //主动技能效果
        [EnumName("被动技能效果 满足事件触发主动技能")] Passive = 2,  //被动技能效果 满足事件触发主动技能
        [EnumName("能力 永久属性加成")] Ability = 3,  //能力 永久属性加成
    }
}