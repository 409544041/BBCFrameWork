// ******************************************************************
//       /\ /|       @file       IToJson.cs
//       \ V/        @brief      json转化接口
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-05 16:02:46
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Newtonsoft.Json.Linq;

namespace Rabi
{
    public interface IToJson
    {
        public JObject ToJson();
        public void FromJson(JObject jObject);
    }
}