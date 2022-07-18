// ******************************************************************
//       /\ /|       @file       SkillControlComp
//       \ V/        @brief      技能控制组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-19 10:15
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class SkillControlComp : UnitControlComp
    {
        private UnitStateController _unitStateController; //状态控制器

        public override void OnInit()
        {
            base.OnInit();
            _unitStateController = FsmManager.Instance.GetFsm<UnitStateController>(parent.GetInstanceID().ToString());
            EventManager.Instance.AddListener(EventId.OnSkillPerformed, OnSkillPerformed);
        }

        /// <summary>
        /// 按下攻击键回调
        /// </summary>
        private void OnSkillPerformed()
        {
            //非移动静止状态  不接收攻击输入
            if (!_unitStateController.IsInState(typeof(UnitIdleState)) &&
                !_unitStateController.IsInState(typeof(UnitMovingState)))
                return;
            //进入技能状态
            _unitStateController.ChangeState<UnitCastingState>();
        }
    }
}