// ******************************************************************
//       /\ /|       @file       MoveControlComp
//       \ V/        @brief      移动控制组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-15 22:47
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class MoveControlComp : UnitControlComp
    {
        private UnitStateController _unitStateController; //状态控制器
        private Rigidbody2D _rigidbody2D; //刚体组件
        private readonly Vector3 _rightDirEuler = Vector3.zero; //右方向旋转
        private readonly Vector3 _leftDirEuler = new Vector3(0, 180, 0); //左方向旋转

        public override void OnInit()
        {
            base.OnInit();
            _rigidbody2D = parent.GetOrAddComponentDontSave<Rigidbody2D>();
            _unitStateController = FsmManager.Instance.GetFsm<UnitStateController>(parent.GetInstanceID().ToString());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //方向
            UpdateDir();
            //动画
            UpdateState();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            //位移
            UpdateMove();
        }

        /// <summary>
        /// 获取移动位移 Move函数必须每帧都执行 否则CharacterController的IsGrounded计算会不准确
        /// </summary>
        /// <returns></returns>
        private void UpdateMove()
        {
            var moveMotion = Vector3.zero;
            //静止或移动状态下接受移动指令
            if (_unitStateController.IsInState(typeof(UnitIdleState)) ||
                _unitStateController.IsInState(typeof(UnitMovingState)))
            {
                moveMotion = InputManager.Instance.currentMovement;
            }

            _rigidbody2D.velocity = moveMotion;
        }

        /// <summary>
        /// 更新动画状态
        /// </summary>
        private void UpdateState()
        {
            //当前帧有移动指令
            if (InputManager.Instance.MovementPressed)
            {
                //当前处于待机状态
                if (_unitStateController.IsInState(typeof(UnitIdleState)))
                {
                    //进入移动状态
                    _unitStateController.ChangeState<UnitMovingState>();
                }

                return;
            }

            //没有移动指令 不在移动中
            if (!_unitStateController.IsInState(typeof(UnitMovingState))) return;
            //没有移动输入 处于移动状态的情况 转为静止
            _unitStateController.ChangeState<UnitIdleState>();
        }

        /// <summary>
        /// 更新方向
        /// </summary>
        private void UpdateDir()
        {
            //静止或移动状态下更新角度朝向
            if (!_unitStateController.IsInState(typeof(UnitIdleState)) &&
                !_unitStateController.IsInState(typeof(UnitMovingState)))
            {
                return;
            }

            //当前帧没有移动指令 不更新
            if (!InputManager.Instance.MovementPressed)
            {
                return;
            }

            //新角度朝向
            var moveX = InputManager.Instance.currentMovement.x;
            if (moveX != 0)
            {
                parent.dir = moveX > 0 ? EnumDirection.East : EnumDirection.West;
            }

            var transform = parent.transform;
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
    }
}