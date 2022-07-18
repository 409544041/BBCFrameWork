// ******************************************************************
//       /\ /|       @file       ActionDelay.cs
//       \ V/        @brief      延时节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-16 01:20:06
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections;
using UnityEngine;

namespace Rabi
{
    public class ActionDelay : BaseAction
    {
        private readonly float _secondsDelay; //延时 秒

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramStrArray">延时 0.2</param>
        public ActionDelay(params string[] paramStrArray) : base(paramStrArray)
        {
            if (paramStrArray.Length < 2)
            {
                _secondsDelay = 0;
                return;
            }

            _secondsDelay = float.Parse(paramStrArray[1]);
        }

        public override void Execute(Action onComplete = null)
        {
            base.Execute(onComplete);
            Logger.Log($"执行延时:{_secondsDelay}");
            //Delay();
            //协成异步写法
            GameManager.Instance.StartCoroutine(Delay());
        }

        // private async void Delay()
        // {
        //     await Task.Delay((int) (_secondsDelay * 1000));
        //     LogUtil.Log($"完成延时:{_secondsDelay}");
        //     onActionComplete?.Invoke();
        // }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_secondsDelay);
            onActionComplete?.Invoke();
        }
    }
}