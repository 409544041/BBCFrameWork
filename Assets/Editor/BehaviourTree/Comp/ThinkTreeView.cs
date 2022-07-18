// ******************************************************************
//       /\ /|       @file       ThinkTreeView.cs
//       \ V/        @brief      行为树视图
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 20:27:21
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rabi
{
    public class ThinkTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<ThinkTreeView, UxmlTraits>
        {
        }

        public Action<ThinkNodeView> onNodeSelected; //节点选中回调
        public Action onGraphViewChanged; //刷新回调
        private BehaviourTree _cacheTree; //当前缓存的树

        private readonly Dictionary<BaseThinkNode, ThinkNodeView> _dictThinkNode2NodeView =
            new Dictionary<BaseThinkNode, ThinkNodeView>(); //树内的全部节点视图 节点地址对视图映射

        public ThinkTreeView()
        {
            Insert(0, new GridBackground());
            //视野缩放能力
            this.AddManipulator(new ContentZoomer());
            //视野拖动能力
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new DoubleClickSelection());
            //节点拖动能力
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AIDef.AIMainStylePath);
            if (styleSheet == null)
            {
                Debug.Log("AI编辑器 样式文件不存在");
                return;
            }

            styleSheets.Add(styleSheet);
            graphViewChanged += OnGraphViewChanged;
        }

        ~ThinkTreeView()
        {
            graphViewChanged -= OnGraphViewChanged;
        }

        /// <summary>
        /// 构建菜单绘制
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            {
                var types = TypeCache.GetTypesDerivedFrom<ThinkNodeAction>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[Action]/{type.Name}", (_) => CreateNode(type, nodePosition));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<ThinkNodeComposite>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[Composite]/{type.Name}", (_) => CreateNode(type, nodePosition));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<ThinkNodeDecorator>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[Decorator]/{type.Name}", (_) => CreateNode(type, nodePosition));
                }
            }
        }

        /// <summary>
        /// 刷新树
        /// </summary>
        /// <param name="tree"></param>
        public void RefreshTree(BehaviourTree tree)
        {
            _cacheTree = tree;
            //清理全部节点
            DeleteElements(graphElements.ToList());
            _dictThinkNode2NodeView.Clear();
            //如果没有根节点 创建根节点
            _cacheTree.rootNode ??= tree.CreateNode(typeof(ThinkNodeRoot));
            //创建树内全部节点的视图
            _cacheTree.nodes.ForEach(CreateNodeView);
            //创建连线
            tree.nodes.ForEach(currentNode =>
            {
                var children = BehaviourTree.GetChildren(currentNode);
                children.ForEach(childNode =>
                {
                    var parentView = _dictThinkNode2NodeView[currentNode];
                    var childView = _dictThinkNode2NodeView[childNode];
                    //子节点的输入端口 连接父节点的输出端口
                    var edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }

        /// <summary>
        /// 获取与给定端口兼容的所有端口
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        /// <summary>
        /// 添加某个节点到选中
        /// </summary>
        /// <param name="baseThinkNode"></param>
        public void AddToSelection(BaseThinkNode baseThinkNode)
        {
            if (_dictThinkNode2NodeView.ContainsKey(baseThinkNode))
            {
                AddToSelection(_dictThinkNode2NodeView[baseThinkNode]);
            }
        }

        /// <summary>
        /// 刷新回调
        /// </summary>
        /// <param name="graphViewChange"></param>
        /// <returns></returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //删除的元素 节点或者连接线
            graphViewChange.elementsToRemove?.ForEach(elem =>
            {
                switch (elem)
                {
                    case ThinkNodeView nodeView:
                        _cacheTree.DeleteNode(nodeView.node);
                        break;
                    case Edge edge:
                    {
                        var parentView = edge.output.node as ThinkNodeView;
                        var childView = edge.input.node as ThinkNodeView;
                        _cacheTree.RemoveChild(parentView?.node, childView?.node);
                        break;
                    }
                }
            });
            //添加的元素 只有节点
            graphViewChange.edgesToCreate?.ForEach(edge =>
            {
                var parentView = edge.output.node as ThinkNodeView;
                var childView = edge.input.node as ThinkNodeView;
                _cacheTree.AddChild(parentView?.node, childView?.node);
            });
            //排序
            nodes.ForEach((n) =>
            {
                if (n is ThinkNodeView thinkNodeView)
                {
                    thinkNodeView.SortChildren();
                }
            });
            onGraphViewChanged?.Invoke();
            return graphViewChange;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        private void CreateNode(Type type, Vector2 position)
        {
            var node = _cacheTree.CreateNode(type);
            node.position = position;
            CreateNodeView(node);
        }

        /// <summary>
        /// 创建节点视图
        /// </summary>
        /// <param name="node"></param>
        private void CreateNodeView(BaseThinkNode node)
        {
            var nodeView = new ThinkNodeView(node)
            {
                onNodeSelected = onNodeSelected
            };
            _dictThinkNode2NodeView.Add(node, nodeView);
            AddElement(nodeView);
        }
    }
}