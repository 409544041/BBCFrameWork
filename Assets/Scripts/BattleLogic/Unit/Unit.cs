// ******************************************************************
//       /\ /|       @file       Unit
//       \ V/        @brief      单位基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-28 13:18
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public abstract class Unit : ThingWithComps
    {
        public override bool IsDie => unitStateController.IsInState<UnitOnDieState>();
        protected UnitStateController unitStateController; //状态控制器

        public override void OnInit()
        {
            base.OnInit();
            unitStateController = FsmManager.Instance.GetFsm<UnitStateController>(GetInstanceID().ToString());
        }

        protected override void OnDestroy()
        {
            FsmManager.Instance.DestroyFsm(GetInstanceID().ToString());
            base.OnDestroy();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            unitStateController.ChangeState<UnitIdleState>();
        }

        public override void OnRecycle()
        {
            //终止状态机
            unitStateController.Stop();
            base.OnRecycle();
        }

        /// <summary>
        /// 创建组件
        /// </summary>
        protected override void OnCreateComps()
        {
            base.OnCreateComps();
        }

        /// <summary>
        /// 受击回调
        /// </summary>
        /// <param name="damage"></param>
        public override void OnHit(int damage)
        {
            //受击动画
            unitStateController.ChangeState<UnitOnHitState>(true);
        }

        /// <summary>
        /// 死亡回调
        /// </summary>
        protected override void OnDie()
        {
            unitStateController.ChangeState<UnitOnDieState>();
        }
    }
}