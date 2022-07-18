// ******************************************************************
//       /\ /|       @file       ThinkNodeDecorator.cs
//       \ V/        @brief      装饰节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 13:18:21
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using Newtonsoft.Json.Linq;

namespace Rabi
{
    public class ThinkNodeDecorator : BaseThinkNode
    {
        public BaseThinkNode child; //装饰节点只修饰一个子节点

        public override BaseThinkNode Clone()
        {
            var node = new ThinkNodeDecorator
            {
                child = child?.Clone()
            };
            return node;
        }

        public override void Reset()
        {
            state = ThinkState.WaitStart;
            child?.Reset();
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("child", child?.ToJson());
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            var jChild = jObject["child"]?.Value<JObject>();
            if (jChild == null)
            {
                child = null;
                return;
            }

            var classType = jChild["classType"]?.Value<string>();
            if (classType == null)
            {
                Logger.Error($"找不到classType");
                return;
            }

            var type = Type.GetType(classType);
            //实例化
            var instance = (BaseThinkNode) ReflectionEx.CreateInstance(type);
            instance.FromJson(jChild);
            child = instance;
        }
    }
}