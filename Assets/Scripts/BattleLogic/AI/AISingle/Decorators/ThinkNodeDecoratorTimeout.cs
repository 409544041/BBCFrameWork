// ******************************************************************
//       /\ /|       @file       ThinkNodeDecoratorTimeout.cs
//       \ V/        @brief      超时节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 22:29:05
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public class ThinkNodeDecoratorTimeout : ThinkNodeDecorator
    {
        [DrawInInspector] public float duration = 1.0f;
        private float _startTime;

        protected override void OnStart()
        {
            base.OnStart();
            _startTime = Time.realtimeSinceStartup;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Time.realtimeSinceStartup - _startTime > duration)
            {
                state = ThinkState.Failure;
                return;
            }

            child?.Update();
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("duration", duration);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            duration = jObject["duration"]?.Value<float>() ?? 0;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeDecoratorTimeout
            {
                child = child?.Clone(),
                duration = duration
            };
        }
    }
}