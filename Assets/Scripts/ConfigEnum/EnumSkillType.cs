// ******************************************************************
//       /\ /|       @file       EnumSkillType.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumSkillType
    {
        None = 0,
        [EnumName("触发型技能 只有主动效果")] Trigger = 1,  //触发型技能 只有主动效果
        [EnumName("持有型技能 被动技能里有能力和被动效果")] Holding = 2,  //持有型技能 被动技能里有能力和被动效果
    }
}