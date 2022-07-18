// ******************************************************************
//       /\ /|       @file       ThinkNodeComposite.cs
//       \ V/        @brief      组合节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 13:16:54
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Rabi
{
    public class ThinkNodeComposite : BaseThinkNode
    {
        public List<BaseThinkNode> children = new List<BaseThinkNode>();

        public override BaseThinkNode Clone()
        {
            var node = new ThinkNodeComposite
            {
                children = children?.ConvertAll(c => c?.Clone())
            };
            return node;
        }

        public override void Reset()
        {
            state = ThinkState.WaitStart;
            if (children == null)
            {
                return;
            }

            foreach (var child in children)
            {
                child.Reset();
            }
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
            if (children == null)
            {
                return;
            }

            var array = new JArray();
            foreach (var child in children)
            {
                array.Add(child.ToJson());
            }

            jObject.Add("children", array);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            var jChildren = jObject["children"]?.Value<JArray>();
            if (jChildren == null || jChildren.Count <= 0)
            {
                children = null;
                return;
            }

            foreach (var jChild in jChildren)
            {
                var jObjectChild = jChild.Value<JObject>();
                if (jObjectChild == null)
                {
                    Logger.Error("children下存在空数据");
                    continue;
                }

                var classType = jObjectChild["classType"]?.Value<string>();
                if (classType == null)
                {
                    Logger.Error($"找不到classType");
                    continue;
                }

                var type = Type.GetType(classType);
                //实例化
                var instance = (BaseThinkNode) ReflectionEx.CreateInstance(type);
                instance.FromJson(jObjectChild);
                children.Add(instance);
            }
        }
    }
}