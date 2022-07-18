// ******************************************************************
//       /\ /|       @file       AttackControlComp
//       \ V/        @brief      攻击控制组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-16 0:13
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class AttackControlComp : UnitControlComp
    {
        private UnitStateController _unitStateController; //状态控制器

        public override void OnInit()
        {
            base.OnInit();
            _unitStateController = FsmManager.Instance.GetFsm<UnitStateController>(parent.GetInstanceID().ToString());
            EventManager.Instance.AddListener(EventId.OnAttackPerformed, OnAttackPerformed);
        }

        /// <summary>
        /// 按下攻击键回调
        /// </summary>
        private void OnAttackPerformed()
        {
            //没激活
            if (!Enable)
            {
                return;
            }

            //非移动静止状态  不接收攻击输入
            if (!_unitStateController.IsInState(typeof(UnitIdleState)) &&
                !_unitStateController.IsInState(typeof(UnitMovingState)) &&
                !_unitStateController.IsInState(typeof(UnitAttackingState)))
                return;
            //处于攻击连段1 进入连段2
            if (_unitStateController.IsInState(typeof(UnitAttackingState)))
            {
                _unitStateController.ChangeState<UnitAttackingState2>();
                return;
            }

            //进入攻击连段1
            _unitStateController.ChangeState<UnitAttackingState>();
        }
    }
}