// ******************************************************************
//       /\ /|       @file       ThinkNodeActionRandomWalk
//       \ V/        @brief      随机寻路节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-10 21:45
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class ThinkNodeActionRandomWalk : ThinkNodeAction
    {
        private bool _isLeftMoving;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //存在目标
            if (!blackboard.targetPos.Equals(Vector3.positiveInfinity))
            {
                state = ThinkState.Failure;
                return;
            }

            var position1 = context.transform.position;
            if (_isLeftMoving)
            {
                blackboard.targetPos = new Vector3(position1.x + 1, position1.y, 0);
                Logger.Log($"目的地:{blackboard.targetPos}");
                state = ThinkState.Success;
                _isLeftMoving = !_isLeftMoving;
                return;
            }

            blackboard.targetPos = new Vector3(position1.x - 1, position1.y, 0);
            Logger.Log($"目的地:{blackboard.targetPos}");
            _isLeftMoving = !_isLeftMoving;
            state = ThinkState.Success;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionRandomWalk();
        }
    }
}