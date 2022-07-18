// ******************************************************************
//       /\ /|       @file       BaseAction.cs
//       \ V/        @brief      事件节点基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-16 01:16:48
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;

namespace Rabi
{
    public abstract class BaseAction
    {
        protected Action onActionComplete; //当前节点完成回调

        protected BaseAction(params string[] paramStrArray)
        {
        }

        /// <summary>
        /// 节点执行
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void Execute(Action onComplete = null)
        {
            onActionComplete = onComplete;
        }
    }
}