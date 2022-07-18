// ******************************************************************
//       /\ /|       @file       UnitMovingState
//       \ V/        @brief      单位移动状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:55
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class UnitMovingState : BaseUnitState
    {
        public UnitMovingState(Unit parent) : base(parent)
        {
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入移动状态");
            animator.SetBool(AnimationDef.MoveHash, true);
        }

        /// <summary>
        /// 动画机去下一个状态之前 不可以清理上一个状态 否则会回滚状态 所以保留数据等动画状态改变完成后清理
        /// </summary>
        public override void OnAfterChange()
        {
            base.OnAfterChange();
            animator.SetBool(AnimationDef.MoveHash, false);
        }
    }
}