// ******************************************************************
//       /\ /|       @file       ThinkNodeCompositeSequencer.cs
//       \ V/        @brief      队列节点 顺序执行直到有子节点失败
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 23:11:08
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeCompositeSequencer : ThinkNodeComposite
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (children == null || children.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < children.Count; ++i)
            {
                var child = children[i];
                child.Update();
                switch (child.state)
                {
                    case ThinkState.Failure:
                        state = ThinkState.Failure;
                        return;
                    case ThinkState.Success:
                        continue;
                }
            }

            state = ThinkState.Success;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeCompositeSequencer {children = children?.ConvertAll(c => c?.Clone())};
        }
    }
}