// ******************************************************************
//       /\ /|       @file       TaskUtil
//       \ V/        @brief      异步工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-25 19:34
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Threading.Tasks;

namespace Rabi
{
    public static class TaskUtil
    {
        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="millisecondsDelay"></param>
        /// <param name="callBack"></param>
        public static async void DelayExecute(int millisecondsDelay, Action callBack)
        {
            await Task.Delay(millisecondsDelay);
            callBack?.Invoke();
        }
    }
}