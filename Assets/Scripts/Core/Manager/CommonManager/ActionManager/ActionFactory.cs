// ******************************************************************
//       /\ /|       @file       ActionFactory.cs
//       \ V/        @brief      事件工厂模式
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-16 01:32:38
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace Rabi
{
    public class ActionFactory : BaseSingleTon<ActionFactory>
    {
        /// <summary>
        /// 创建事件节点列表
        /// </summary>
        /// <param name="actionListStr">【延时，0.2】。【延时，0.2】</param>
        /// <returns></returns>
        public static IEnumerable<BaseAction> CreateActionList(string actionListStr)
        {
            var perforceActionList = new List<BaseAction>();
            var actionStrArray = actionListStr.Split('。');
            for (var i = 0; i < actionStrArray.Length; i++)
            {
                var actionStr = actionStrArray[i].Trim('【', '】');
                perforceActionList.Add(Create(actionStr));
            }

            return perforceActionList;
        }

        /// <summary>
        /// 创建事件节点
        /// </summary>
        /// <param name="actionStr">延时，0.2</param>
        /// <returns></returns>
        public static BaseAction Create(string actionStr)
        {
            var paramStrArray = actionStr.Split('，');
            if (paramStrArray.Length < 1)
            {
                Logger.Error($"事件解析失败:{actionStr}");
                return default;
            }

            switch (paramStrArray[0])
            {
                case "延时":
                    return new ActionDelay(paramStrArray);
                case "并行":
                    return new ActionParallel(paramStrArray);
                default:
                    return default;
            }
        }
    }
}