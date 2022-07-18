// ******************************************************************
//       /\ /|       @file       ThinkNodeRoot.cs
//       \ V/        @brief      根节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 13:24:23
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using Newtonsoft.Json.Linq;

namespace Rabi
{
    public class ThinkNodeRoot : BaseThinkNode
    {
        public BaseThinkNode child; //根节点下只允许一个子节点

        public override void Reset()
        {
            state = ThinkState.WaitStart;
            child.state = ThinkState.WaitStart;
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override void OnUpdate()
        {
            if (child == null)
            {
                return;
            }

            child.Update();
            state = child.state;
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

        public override BaseThinkNode Clone()
        {
            var node = new ThinkNodeRoot
            {
                child = child?.Clone()
            };
            return node;
        }
    }
}