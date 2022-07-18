// ******************************************************************
//       /\ /|       @file       EventId.cs
//       \ V/        @brief      事件id
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-29 11:36:18
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EventId
    {
        #region Input

        [EnumName("按下攻击")] OnAttackPerformed,
        [EnumName("按下技能")] OnSkillPerformed,
        [EnumName("按下跳跃")] OnJumpPerformed,

        #endregion

        #region UI

        [EnumName("logo完成事件")] OnLogoComplete,
        [EnumName("语言更变事件")] OnLanguageChanged,
        [EnumName("单位死亡事件")] OnUnitDie,

        #endregion

        #region 战斗

        #endregion
    }
}