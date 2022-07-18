// ******************************************************************
//       /\ /|       @file       BaseUnitState
//       \ V/        @brief      单位状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:53
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class BaseUnitState : IFsmState
    {
        protected Unit owner; //拥有者
        protected Animator animator;
        protected UnitStateController unitStateController; //状态控制器

        protected BaseUnitState(Unit parent)
        {
            owner = parent;
        }

        public virtual void OnEnter(params object[] args)
        {
            unitStateController = FsmManager.Instance.GetFsm<UnitStateController>(owner.GetInstanceID().ToString());
            animator = owner.GetOrAddComponentDontSave<Animator>();
        }

        public virtual void OnExit()
        {
        }

        /// <summary>
        /// 转换状态后回调
        /// </summary>
        public virtual void OnAfterChange()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }
    }
}