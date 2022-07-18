// ******************************************************************
//       /\ /|       @file       ThinkNodeDecoratorFailure.cs
//       \ V/        @brief      失败装饰节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 22:18:42
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeDecoratorFailure : ThinkNodeDecorator
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            child?.Update();
            if (child?.state != ThinkState.Success && child?.state != ThinkState.Failure) return;
            state = ThinkState.Failure;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeDecoratorFailure
            {
                child = child?.Clone()
            };
        }
    }
}