// ******************************************************************
//       /\ /|       @file       ThinkNodeCompositeRandomSelector.cs
//       \ V/        @brief      随机选择节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 23:12:00
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class ThinkNodeCompositeRandomSelector : ThinkNodeComposite
    {
        protected int current;

        protected override void OnStart()
        {
            current = Random.Range(0, children.Count);
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate()
        {
            if (children == null || children.Count <= 0)
            {
                return;
            }

            if (current >= children.Count)
            {
                Logger.Error("索引越界");
                current = 0;
                return;
            }

            var child = children[current];
            child.Update();
            state = child.state;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeCompositeRandomSelector {children = children?.ConvertAll(c => c?.Clone())};
        }
    }
}