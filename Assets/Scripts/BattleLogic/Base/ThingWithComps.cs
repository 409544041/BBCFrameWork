// ******************************************************************
//       /\ /|       @file       ThingWithComps.cs
//       \ V/        @brief      带组件的物体
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-02 08:59:29
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;

namespace Rabi
{
    public abstract class ThingWithComps : Thing
    {
        /// <summary>
        /// 挂载的组件
        /// </summary>
        public readonly Dictionary<Type, ThingComp> compDict = new Dictionary<Type, ThingComp>();

        public override void OnInit()
        {
            base.OnInit();
            OnCreateComps();
            //初始化所有组件
            foreach (var comp in compDict.Values)
            {
                comp.OnInit();
            }
        }

        protected virtual void Update()
        {
            foreach (var comp in compDict.Values)
            {
                comp.OnUpdate();
            }
        }

        protected virtual void FixedUpdate()
        {
            foreach (var comp in compDict.Values)
            {
                comp.OnFixedUpdate();
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var comp in compDict.Values)
            {
                comp.OnDestory();
            }

            compDict.Clear();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        protected virtual void OnCreateComps()
        {
        }

        /// <summary>
        /// 受击回调
        /// </summary>
        /// <param name="damage"></param>
        public virtual void OnHit(int damage)
        {
        }

        /// <summary>
        /// 死亡回调
        /// </summary>
        protected virtual void OnDie()
        {
            //死亡动画
        }


        /// <summary>
        /// 回收时回调
        /// </summary>
        public override void OnRecycle()
        {
            base.OnRecycle();
            foreach (var comp in compDict.Values)
            {
                comp.OnReset();
            }
        }

        /// <summary>
        /// 获取某个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T TryGetComp<T>() where T : ThingComp
        {
            if (compDict is not { Count: > 0 })
            {
                return default;
            }

            if (compDict.ContainsKey(typeof(T)) && compDict[typeof(T)] is T targetComp)
            {
                return targetComp;
            }

            return default;
        }

        /// <summary>
        /// 是否拥有某个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HaveComp<T>() where T : ThingComp
        {
            return compDict is { Count: > 0 } && compDict.ContainsKey(typeof(T));
        }
    }
}