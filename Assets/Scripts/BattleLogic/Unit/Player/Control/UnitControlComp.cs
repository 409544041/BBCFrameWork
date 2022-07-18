// ******************************************************************
//       /\ /|       @file       UnitControlComp
//       \ V/        @brief      单位控制组件基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-16 0:16
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public abstract class UnitControlComp : ThingComp
    {
        protected Animator animator; //动画状态机
        protected Unit unit; //被控制的单位
        public bool Enable { get; set; }

        public override void OnInit()
        {
            base.OnInit();
            unit = (Unit)parent;
            animator = parent.GetOrAddComponentDontSave<Animator>();
            Enable = true;
        }
    }
}