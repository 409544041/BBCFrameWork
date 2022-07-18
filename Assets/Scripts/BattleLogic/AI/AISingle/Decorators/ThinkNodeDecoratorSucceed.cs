// ******************************************************************
//       /\ /|       @file       ThinkNodeDecoratorSucceed.cs
//       \ V/        @brief      成功节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 22:40:33
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeDecoratorSucceed : ThinkNodeDecorator
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (child == null)
            {
                return;
            }

            child.Update();
            if (child.state != ThinkState.Success && child.state != ThinkState.Failure) return;
            state = ThinkState.Success;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeDecorator
            {
                child = child?.Clone()
            };
        }
    }
}