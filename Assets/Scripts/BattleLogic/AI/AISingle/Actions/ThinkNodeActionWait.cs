// ******************************************************************
//       /\ /|       @file       ThinkNodeActionWait.cs
//       \ V/        @brief      等待节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 19:32:51
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Rabi
{
    public class ThinkNodeActionWait : ThinkNodeAction
    {
        [DrawInInspector] public float delay; //延迟时间 单位秒
        private float _startTime; //节点开始执行的时间

        protected override void OnStart()
        {
            base.OnStart();
            _startTime = Time.realtimeSinceStartup;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Time.realtimeSinceStartup - _startTime > delay)
            {
                state = ThinkState.Success;
            }
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("delay", delay);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            delay = jObject["delay"]?.Value<float>() ?? 0;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionWait
            {
                delay = delay
            };
        }
    }
}