// ******************************************************************
//       /\ /|       @file       EnumSkillEffectLogicType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumSkillEffectLogicType
    {
        None = 0,
        [EnumName("修改{propertyId}状态值{value}")] UpdateStatus = 1,  //修改{propertyId}状态值{value}
        [EnumName("满足事件{eventId}触发主动技能{skillEffectId}")] Passive = 2,  //满足事件{eventId}触发主动技能{skillEffectId}
        [EnumName("修改{propertyId}属性值{value}")] Ability = 3,  //修改{propertyId}属性值{value}
        [EnumName("添加buff{buffId}")] AddBuff = 4,  //添加buff{buffId}
        [EnumName("普通攻击 走伤害公式")] Attack = 5,  //普通攻击 走伤害公式
    }
}