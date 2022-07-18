// ******************************************************************
//       /\ /|       @file       ThinkNodeActionRandomPosition.cs
//       \ V/        @brief      漫步节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 21:32:41
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public class ThinkNodeActionRandomPosition : ThinkNodeAction
    {
        [DrawInInspector] public Vector2 min = Vector2.one * -10;
        [DrawInInspector] public Vector2 max = Vector2.one * 10;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            blackboard.targetPos.x = Random.Range(min.x, max.x);
            blackboard.targetPos.z = Random.Range(min.y, max.y);
            state = ThinkState.Success;
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("min", min.ToJson());
            jObject.Add("max", max.ToJson());
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            var jMin = jObject["min"]?.Value<JObject>();
            var jMax = jObject["max"]?.Value<JObject>();
            min = VectorEx.FromJson(jMin);
            max = VectorEx.FromJson(jMax);
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionRandomPosition
            {
                min = min,
                max = max
            };
        }
    }
}