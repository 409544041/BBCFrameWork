// ******************************************************************
//       /\ /|       @file       UnitIdleState
//       \ V/        @brief      单位待机状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:54
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class UnitIdleState : BaseUnitState
    {
        public UnitIdleState(Unit parent) : base(parent)
        {
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入待机状态");
            if (!animator.enabled)
            {
                animator.enabled = true;
            }
        }
    }
}