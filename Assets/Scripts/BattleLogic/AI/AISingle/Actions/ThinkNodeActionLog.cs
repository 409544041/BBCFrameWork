// ******************************************************************
//       /\ /|       @file       ThinkNodeActionLog.cs
//       \ V/        @brief      日志节点
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 19:12:40
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;

namespace Rabi
{
    public class ThinkNodeActionLog : ThinkNodeAction
    {
        [DrawInInspector] public string message;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Logger.Log(message);
            state = ThinkState.Success;
        }

        protected override void OnWriteJson(ref JObject jObject)
        {
            base.OnWriteJson(ref jObject);
            jObject.Add("message", message);
        }

        protected override void OnReadJson(ref JObject jObject)
        {
            base.OnReadJson(ref jObject);
            message = jObject["message"]?.Value<string>();
        }

        public override BaseThinkNode Clone()
        {
            return new ThinkNodeActionLog { message = message };
        }
    }
}