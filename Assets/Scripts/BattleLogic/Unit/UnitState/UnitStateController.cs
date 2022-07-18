// ******************************************************************
//       /\ /|       @file       UnitStateController
//       \ V/        @brief      单位状态控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:57
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;

namespace Rabi
{
    public class UnitStateController : BaseFsm
    {
        public override void ChangeState<T>(bool canRepeat = false, params object[] args)
        {
            base.ChangeState<T>(canRepeat, args);
            previousState?.OnAfterChange();
        }

        public override void ChangeState(Type fsmType, bool canRepeat = false, params object[] args)
        {
            base.ChangeState(fsmType, canRepeat, args);
            previousState?.OnAfterChange();
        }
    }
}