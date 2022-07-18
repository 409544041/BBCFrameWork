// ******************************************************************
//       /\ /|       @file       ThinkNodeCompositeSelector.cs
//       \ V/        @brief      选择节点 顺序执行直到有子节点成功
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 23:12:25
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class ThinkNodeCompositeSelector : ThinkNodeComposite
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
                    //执行成功
                    case ThinkState.Success:
                        state = ThinkState.Success;
                        return;
                    //当前子节点执行失败 换下一个节点
                    case ThinkState.Failure:
                        continue;
                }
            }

            //执行失败
            state = ThinkState.Failure;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeCompositeSelector {children = children?.ConvertAll(c => c?.Clone())};
        }
    }
}