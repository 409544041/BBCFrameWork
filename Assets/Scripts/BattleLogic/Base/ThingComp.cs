// ******************************************************************
//       /\ /|       @file       ThingComp.cs
//       \ V/        @brief      物体组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-02 08:58:57
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public abstract class ThingComp
    {
        protected CompData compData; //组件数据
        protected ThingWithComps parent; //持有者

        /// <summary>
        /// 初始化回调
        /// </summary>
        public virtual void OnInit()
        {
        }

        /// <summary>
        /// 销毁时回调
        /// </summary>
        public virtual void OnDestory()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        /// <summary>
        /// 重置回调
        /// </summary>
        public virtual void OnReset()
        {
            compData?.Reset();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void SetData(ThingWithComps owner, CompData data = null)
        {
            parent = owner;
            compData = data;
        }
    }
}