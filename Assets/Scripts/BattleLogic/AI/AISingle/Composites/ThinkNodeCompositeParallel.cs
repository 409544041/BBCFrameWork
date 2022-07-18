// ******************************************************************
//       /\ /|       @file       ThinkNodeCompositeParallel.cs
//       \ V/        @brief      并行节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 23:11:28
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeCompositeParallel : ThinkNodeComposite
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            var isRunning = false;
            if (children == null)
            {
                return;
            }

            foreach (var child in children)
            {
                child.Update();
                //存在节点运行中
                if (child.state is ThinkState.Running)
                {
                    isRunning = true;
                }
            }

            state = isRunning ? ThinkState.Running : ThinkState.Success;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeCompositeParallel {children = children?.ConvertAll(c => c?.Clone())};
        }
    }
}