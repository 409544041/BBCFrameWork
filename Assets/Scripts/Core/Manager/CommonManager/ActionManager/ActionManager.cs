// ******************************************************************
//       /\ /|       @file       ActionManager.cs
//       \ V/        @brief      事件管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-16 01:10:46
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;

namespace Rabi
{
    public sealed class ActionManager : BaseSingleTon<ActionManager>
    {
        private readonly List<BaseAction> _perforceActionList = new List<BaseAction>(); //待执行的事件节点
        private Action _onExecuteAllComplete; //全部事件完成后的回调
        private bool _isPlaying; //正在执行中

        /// <summary>
        /// 尝试执行事件列表
        /// </summary>
        /// <param name="actionListStr"></param>
        /// <param name="onExecuteAllComplete">所有事件完成的回调</param>
        public void TryExecuteActionList(string actionListStr, Action onExecuteAllComplete = null)
        {
            if (_isPlaying)
            {
                return;
            }

            _isPlaying = true;
            AddActionList(actionListStr);
            _onExecuteAllComplete = onExecuteAllComplete;
            StartPerforce();
        }

        /// <summary>
        /// 解析数据 添加事件列表
        /// </summary>
        /// <param name="actionListStr">【延时，0.2】。【延时，0.2】</param>
        private void AddActionList(string actionListStr)
        {
            var actionList = ActionFactory.CreateActionList(actionListStr);
            _perforceActionList.AddRange(actionList);
        }

        /// <summary>
        /// 开始按照事件节点顺序执行
        /// </summary>
        private void StartPerforce()
        {
            //没有事件节点了 执行完毕
            if (_perforceActionList.Count <= 0)
            {
                _onExecuteAllComplete?.Invoke();
                _onExecuteAllComplete = null;
                _isPlaying = false;
                return;
            }

            var action = _perforceActionList[0];
            _perforceActionList.RemoveAt(0);
            action.Execute(StartPerforce);
        }
    }
}