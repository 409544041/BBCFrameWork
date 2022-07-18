// ******************************************************************
//       /\ /|       @file       ThinkNodeDecoratorRepeat.cs
//       \ V/        @brief      循环节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 22:41:20
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;

namespace Rabi
{
    public class ThinkNodeDecoratorRepeat : ThinkNodeDecorator
    {
        [DrawInInspector] public bool needRestartWhenSuccess = true; //成功时重新开始
        [DrawInInspector] public bool needRestartWhenFailure; //失败时重新开始

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (child == null)
            {
                return;
            }

            child?.Update();
            state = child.state;
            //重新运行
            if (state == ThinkState.Failure && needRestartWhenFailure ||
                state == ThinkState.Success && needRestartWhenSuccess)
            {
                //重置子节点状态
                Reset();
            }
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("needRestartWhenSuccess", needRestartWhenSuccess);
            jObject.Add("needRestartWhenFailure", needRestartWhenFailure);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            needRestartWhenSuccess = jObject["needRestartWhenSuccess"]?.Value<bool>() ?? false;
            needRestartWhenFailure = jObject["needRestartWhenFailure"]?.Value<bool>() ?? false;
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeDecoratorRepeat
            {
                child = child?.Clone(),
                needRestartWhenSuccess = needRestartWhenSuccess,
                needRestartWhenFailure = needRestartWhenFailure
            };
        }
    }
}