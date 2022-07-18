// ******************************************************************
//       /\ /|       @file       ActionParallel.cs
//       \ V/        @brief      并行节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-18 06:52:40
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;

namespace Rabi
{
    public class ActionParallel : BaseAction
    {
        private readonly List<BaseAction> _subActionList = new List<BaseAction>();
        private int _completeSubActionCount;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="paramStrArray">// 并行 （延时|5） （延时|1）</param>
        public ActionParallel(params string[] paramStrArray) : base(paramStrArray)
        {
            _completeSubActionCount = 0;
            if (paramStrArray.Length < 3)
            {
                Logger.Error($"并行节点需要内置至少2个子节点 paramStrArray.Length:{paramStrArray.Length}");
                return;
            }

            //遍历解析子节点
            for (var i = 0; i < paramStrArray.Length; i++)
            {
                //第一项是并行节点的类别 不是子节点数据
                if (i == 0)
                {
                    continue;
                }

                //分隔符换回通用解析格式 去掉外壳
                var subParamStrArray = paramStrArray[i].Replace('|', '，').Trim('（', '）');
                var subAction = ActionFactory.Create(subParamStrArray);
                _subActionList.Add(subAction);
            }
        }

        public override void Execute(Action onComplete = null)
        {
            base.Execute(onComplete);
            //遍历执行所有子节点
            foreach (var subAction in _subActionList)
            {
                subAction.Execute(OnSubActionComplete);
            }
        }

        /// <summary>
        /// 子节点完成回调
        /// </summary>
        private void OnSubActionComplete()
        {
            _completeSubActionCount++;
            //全部子节点执行完毕
            if (_completeSubActionCount < _subActionList.Count) return;
            Logger.Log("并行节点执行完成");
            onActionComplete?.Invoke();
        }
    }
}