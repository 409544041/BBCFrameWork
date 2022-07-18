// ******************************************************************
//       /\ /|       @file       ThinkNodeActionMove.cs
//       \ V/        @brief      移动节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 21:42:45
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public class ThinkNodeActionMove : ThinkNodeAction
    {
        [DrawInInspector] public float stoppingDistance = 0.1f; //终止距离
        private readonly Vector3 _rightDirEuler = Vector3.zero; //右方向旋转
        private readonly Vector3 _leftDirEuler = new Vector3(0, 180, 0); //左方向旋转

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //todo 没路了 移动失败
            //没有目标
            if (blackboard.targetPos.Equals(Vector3.positiveInfinity))
            {
                state = ThinkState.Failure;
                return;
            }

            //到达目的地
            if (Mathf.Abs(context.transform.position.x - blackboard.targetPos.x) < stoppingDistance)
            {
                blackboard.targetPos = Vector3.positiveInfinity;
                //没有移动输入 处于移动状态的情况 转为静止
                if (context.unitStateController.IsInState(typeof(UnitMovingState)))
                {
                    context.unitStateController.ChangeState<UnitIdleState>();
                }

                state = ThinkState.Success;
                return;
            }

            //todo 寻路撞了 重新寻路
            //方向
            UpdateDir();
            //动画
            UpdateState();
            //位移
            UpdateMove();
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("stoppingDistance", stoppingDistance);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            stoppingDistance = jObject["stoppingDistance"]?.Value<float>() ?? 0;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionMove {stoppingDistance = stoppingDistance};
        }

        /// <summary>
        /// 更新方向
        /// </summary>
        private void UpdateDir()
        {
            var dir = blackboard.targetPos - context.self.transform.position;
            //新角度朝向
            var moveX = dir.x;
            if (moveX != 0)
            {
                context.self.dir = moveX > 0 ? EnumDirection.East : EnumDirection.West;
            }

            var transform = context.transform;
            var newAngle = moveX switch
            {
                > 0 => _rightDirEuler,
                < 0 => _leftDirEuler,
                _ => transform.eulerAngles
            };

            //更新角度
            if (transform.eulerAngles != newAngle)
            {
                transform.eulerAngles = newAngle;
            }
        }

        /// <summary>
        /// 更新动画状态
        /// </summary>
        private void UpdateState()
        {
            //当前处于待机状态
            if (context.unitStateController.IsInState(typeof(UnitIdleState)))
            {
                //进入移动状态
                context.unitStateController.ChangeState<UnitMovingState>();
            }
        }

        /// <summary>
        /// 获取移动位移 Move函数必须每帧都执行 否则CharacterController的IsGrounded计算会不准确
        /// </summary>
        /// <returns></returns>
        private void UpdateMove()
        {
            var moveMotion = Vector3.zero;
            //静止或移动状态下接受移动指令
            if (context.unitStateController.IsInState(typeof(UnitIdleState)) ||
                context.unitStateController.IsInState(typeof(UnitMovingState)))
            {
                var dir = context.self.dir == EnumDirection.East ? 1 : -1;
                moveMotion = new Vector3(1 * dir, 0, 0);
            }

            context.rigidbody2D.velocity = moveMotion;
        }
    }
}