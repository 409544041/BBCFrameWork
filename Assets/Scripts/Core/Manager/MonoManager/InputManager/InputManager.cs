// ******************************************************************
//       /\ /|       @file       InputManager
//       \ V/        @brief      输入管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-15 13:30
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;
using UnityEngine.InputSystem;

namespace Rabi
{
    public class InputManager : BaseSingleTon<InputManager>, IMonoManager
    {
        private readonly InputControl _inputControl = new InputControl(); //按键控制映射
        public Vector2 currentMovement; //当前的移动输入值
        public bool MovementPressed => currentMovement.x != 0 || currentMovement.y != 0; //移动键有被按下

        public void OnInit()
        {
            _inputControl.Enable();
            _inputControl.PlayerControl.Attack.performed += OnAttackPerformed;
            _inputControl.PlayerControl.Jump.performed += OnJumpPerformed;
            _inputControl.PlayerControl.Movement.performed += OnMovementPerformed;
            _inputControl.PlayerControl.Skill.performed += OnSkillPerformed;
            Logger.Log("输入管理器初始化");
        }

        public void OnClear()
        {
            _inputControl.PlayerControl.Attack.performed -= OnAttackPerformed;
            _inputControl.PlayerControl.Jump.performed -= OnJumpPerformed;
            _inputControl.PlayerControl.Jump.performed -= OnMovementPerformed;
            _inputControl.PlayerControl.Skill.performed -= OnSkillPerformed;
            _inputControl.Disable();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
        }


        /// <summary>
        /// 按下攻击
        /// </summary>
        /// <param name="obj"></param>
        private static void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            EventManager.Instance.Dispatch(EventId.OnAttackPerformed);
        }

        /// <summary>
        /// 按下跳跃
        /// </summary>
        /// <param name="obj"></param>
        private static void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            EventManager.Instance.Dispatch(EventId.OnJumpPerformed);
        }

        /// <summary>
        /// 按下方向键
        /// </summary>
        /// <param name="obj"></param>
        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            currentMovement = obj.ReadValue<Vector2>();
        }

        /// <summary>
        /// 按下技能键
        /// </summary>
        /// <param name="obj"></param>
        private static void OnSkillPerformed(InputAction.CallbackContext obj)
        {
            EventManager.Instance.Dispatch(EventId.OnSkillPerformed);
        }
    }
}