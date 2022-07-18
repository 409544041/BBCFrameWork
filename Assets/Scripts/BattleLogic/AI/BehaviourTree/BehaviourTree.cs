// ******************************************************************
//       /\ /|       @file       BehaviourTree.cs
//       \ V/        @brief      行为树
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-04 13:10:28
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Rabi
{
    public class BehaviourTree : IToJson
    {
        public string name; //行为树配置文件名称
        public BaseThinkNode rootNode; //根节点
        public ThinkState treeState = ThinkState.WaitStart; //行为状态
        public List<BaseThinkNode> nodes = new List<BaseThinkNode>(); //所有子节点
        public Blackboard blackboard = new Blackboard(); //共享数据

        public BehaviourTree()
        {
        }

        public BehaviourTree(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (rootNode == null)
            {
                return;
            }

            rootNode.Update();
            treeState = rootNode.state;
        }

        /// <summary>
        /// 获取某个节点下的子节点集合
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<BaseThinkNode> GetChildren(BaseThinkNode parent)
        {
            var children = new List<BaseThinkNode>();
            switch (parent)
            {
                case ThinkNodeDecorator {child: { }} decorator:
                    children.Add(decorator.child);
                    break;
                case ThinkNodeRoot {child: { }} rootNode:
                    children.Add(rootNode.child);
                    break;
                case ThinkNodeComposite composite:
                    return composite.children;
            }

            return children;
        }

        /// <summary>
        /// 遍历操作
        /// </summary>
        /// <param name="node"></param>
        /// <param name="visiter"></param>
        public static void Traverse(BaseThinkNode node, Action<BaseThinkNode> visiter)
        {
            if (node == null) return;
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }

        /// <summary>
        /// 克隆实例
        /// </summary>
        /// <returns></returns>
        public BehaviourTree Clone()
        {
            var tree = new BehaviourTree(name) {rootNode = rootNode.Clone()};
            tree.SetSubNodeList();
            return tree;
        }

        /// <summary>
        /// 绑定共享数据
        /// </summary>
        /// <param name="context"></param>
        public void Bind(Context context)
        {
            Traverse(rootNode, node =>
            {
                node.context = context;
                node.blackboard = blackboard;
            });
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public void Serialize()
        {
#if UNITY_EDITOR
            var filePath = $"{AIDef.AIDataFolder}/{name}.json";
            RabiFileUtil.SafeDeleteFile(filePath);
            var fileInfo = new FileInfo(filePath);
            var streamWriter = fileInfo.CreateText();
            streamWriter.Write(ToJson().ToString());
            streamWriter.Close();
            streamWriter.Dispose();
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
        public void Deserialize(string filePath)
        {
            name = filePath.GetFileNameWithoutExtend();
            var textAsset = AssetManager.Instance.LoadAssetSync<TextAsset>(filePath);
            if (textAsset == null)
            {
                Logger.Error($"数据文件加载失败:{filePath}");
                return;
            }

            var strText = textAsset.text;
            if (strText == null)
            {
                Logger.Error($"数据读取失败:{filePath}");
                return;
            }

            var jObject = JsonConvert.DeserializeObject<JObject>(strText);
            FromJson(jObject);
            SetSubNodeList();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public JObject ToJson()
        {
            var jsonObj = new JObject
            {
                {"rootNode", rootNode?.ToJson()}
            };
            return jsonObj;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="jObject"></param>
        public void FromJson(JObject jObject)
        {
            if (jObject == null)
            {
                return;
            }

            // //class完整名称
            var jRootNode = jObject["rootNode"]?.Value<JObject>();
            if (jRootNode == null)
            {
                rootNode = null;
                return;
            }

            var classType = jRootNode["classType"]?.Value<string>();
            if (classType == null)
            {
                Logger.Error($"找不到classType");
                return;
            }

            //type
            var type = Type.GetType(classType);
            //实例化
            var instance = (BaseThinkNode) ReflectionEx.CreateInstance(type);
            instance.FromJson(jRootNode);
            //反序列化
            rootNode = instance;
        }

        /// <summary>
        /// 设置子节点
        /// </summary>
        private void SetSubNodeList()
        {
            //遍历根节点设置子节点
            nodes.Clear();
            Traverse(rootNode, (n) => { nodes.Add(n); });
        }

        #region Editor Compatibility

#if UNITY_EDITOR
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public BaseThinkNode CreateNode(Type type)
        {
            var node = Activator.CreateInstance(type) as BaseThinkNode;
            nodes.Add(node);
            return node;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(BaseThinkNode node)
        {
            nodes.Remove(node);
        }

        /// <summary>
        /// 挂载节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void AddChild(BaseThinkNode parent, BaseThinkNode child)
        {
            if (parent == null)
            {
                return;
            }

            if (child == null)
            {
                return;
            }

            switch (parent)
            {
                case ThinkNodeDecorator decorator:
                    decorator.child = child;
                    break;
                case ThinkNodeRoot thinkNodeRoot:
                    thinkNodeRoot.child = child;
                    break;
                case ThinkNodeComposite composite:
                    composite.children?.Add(child);
                    break;
            }
        }

        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void RemoveChild(BaseThinkNode parent, BaseThinkNode child)
        {
            if (parent == null)
            {
                return;
            }

            if (child == null)
            {
                return;
            }

            switch (parent)
            {
                case ThinkNodeDecorator decorator:
                    decorator.child = null;
                    break;
                case ThinkNodeRoot thinkNodeRoot:
                    thinkNodeRoot.child = null;
                    break;
                case ThinkNodeComposite composite:
                    composite.children?.Remove(child);
                    break;
            }
        }
#endif

        #endregion Editor Compatibility
    }
}