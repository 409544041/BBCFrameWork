// ******************************************************************
//       /\ /|       @file       ThinkNodeActionRandomFailure.cs
//       \ V/        @brief      随机失败节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 21:39:45
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public class ThinkNodeActionRandomFailure : ThinkNodeAction
    {
        [Range(0, 1)] [DrawInInspector] public float chanceOfFailure = 0.5f;

        protected override void OnUpdate()
        {
            var value = Random.value;
            state = value > chanceOfFailure ? ThinkState.Failure : ThinkState.Success;
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("chanceOfFailure", chanceOfFailure);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            chanceOfFailure = jObject["chanceOfFailure"]?.Value<float>() ?? 0;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionRandomFailure
            {
                chanceOfFailure = chanceOfFailure
            };
        }
    }
}