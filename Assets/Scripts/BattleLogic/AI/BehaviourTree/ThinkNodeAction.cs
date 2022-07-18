// ******************************************************************
//       /\ /|       @file       ThinkNodeAction.cs
//       \ V/        @brief      行为节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 13:18:58
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeAction : BaseThinkNode
    {
        public override BaseThinkNode Clone()
        {
            var node = new ThinkNodeAction();
            return node;
        }

        public override void Reset()
        {
            state = ThinkState.WaitStart;
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate()
        {
        }
    }
}