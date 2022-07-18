// ******************************************************************
//       /\ /|       @file       ThinkNodeDecoratorInverter.cs
//       \ V/        @brief      翻转节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 22:26:00
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeDecoratorInverter : ThinkNodeDecorator
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            child?.Update();
            switch (child?.state)
            {
                case ThinkState.Success:
                    state = ThinkState.Failure;
                    return;
                case ThinkState.Failure:
                    state = ThinkState.Success;
                    return;
            }
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeDecoratorInverter
            {
                child = child?.Clone()
            };
        }
    }
}