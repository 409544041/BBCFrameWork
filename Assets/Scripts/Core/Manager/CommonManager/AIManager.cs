// ******************************************************************
//       /\ /|       @file       AIManager.cs
//       \ V/        @brief      AI管理器
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-08 10:27:42
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;

namespace Rabi
{
    public class AIManager : BaseSingleTon<AIManager>
    {
        private readonly Dictionary<int, BehaviourTree>
            _name2AITree = new Dictionary<int, BehaviourTree>(); //AI文件名称对树映射

        /// <summary>
        /// 加载AI原型
        /// </summary>
        public void LoadAIProto()
        {
            foreach (var rowCfgAI in ConfigManager.Instance.cfgAI.AllConfigs)
            {
                var aiTreeProto = new BehaviourTree();
                aiTreeProto.Deserialize(rowCfgAI.path);
                _name2AITree.Add(rowCfgAI.id, aiTreeProto);
            }
        }

        /// <summary>
        /// 创建AI实例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public BehaviourTree CreateAIInstance(int id, ThingWithComps owner)
        {
            if (!_name2AITree.ContainsKey(id))
            {
                Logger.Error($"找不到原型体 id:{id}");
                return null;
            }

            var tree = _name2AITree[id].Clone();
            var context = Context.CreateFromGameObject(owner);
            tree.Bind(context);
            return tree;
        }

        /// <summary>
        /// 创建AI实例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public BehaviourTree CreateAIInstance(EnumAI id, ThingWithComps owner)
        {
            return CreateAIInstance(id.GetHashCode(), owner);
        }
    }
}