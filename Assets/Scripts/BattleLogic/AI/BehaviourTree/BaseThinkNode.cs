// ******************************************************************
//       /\ /|       @file       BaseThinkNode
//       \ V/        @brief      行为树节点基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-03 23:17
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************


using Newtonsoft.Json.Linq;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Rabi
{
    public abstract class BaseThinkNode : IToJson
    {
        public ThinkState state = ThinkState.WaitStart;
        public Context context; //共享组件
        public Blackboard blackboard; //共享数据
#if UNITY_EDITOR
        public Vector2 position; //在graphView中的坐标
#endif

        public void Update()
        {
            //启动
            if (state == ThinkState.WaitStart)
            {
                OnStart();
                state = ThinkState.Running;
            }

            //运行中
            var cacheState = state;
            if (state == ThinkState.Running)
            {
                OnUpdate();
            }

            //上一帧是运行状态 这一帧有结果 触发停止回调
            if (cacheState == ThinkState.Running && state is ThinkState.Success or ThinkState.Failure)
            {
                OnStop();
            }
        }

        /// <summary>
        /// 中断当前节点以及子节点
        /// </summary>
        public void Abort()
        {
            BehaviourTree.Traverse(this, node =>
            {
                node.state = ThinkState.WaitStart;
                node.OnStop();
            });
        }

        /// <summary>
        /// 序列化为json格式
        /// </summary>
        /// <returns></returns>
        public JObject ToJson()
        {
            var jsonObj = new JObject();
            OnWriteJson(ref jsonObj);
            return jsonObj;
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="jObject"></param>
        public void FromJson(JObject jObject)
        {
            OnReadJson(ref jObject);
        }

        /// <summary>
        /// 写入json数据回调
        /// </summary>
        /// <param name="jObject"></param>
        protected virtual void OnWriteJson(ref JObject jObject)
        {
            jObject.Add("classType", GetType().ToString());
#if UNITY_EDITOR
            jObject.Add("position", position.ToJson());
#endif
        }

        /// <summary>
        /// 读取json数据回调
        /// </summary>
        /// <param name="jObject"></param>
        protected virtual void OnReadJson(ref JObject jObject)
        {
            //classType会由上一层级读取并创建实例
#if UNITY_EDITOR
            var jPosition = jObject["position"]?.Value<JObject>();
            position = VectorEx.FromJson(jPosition);
#endif
        }

        /// <summary>
        /// 克隆实例
        /// </summary>
        /// <returns></returns>
        public abstract BaseThinkNode Clone();

        /// <summary>
        /// 重置节点
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 运行时回调
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// 停止时回调
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// 更新回调
        /// </summary>
        protected abstract void OnUpdate();
    }
}