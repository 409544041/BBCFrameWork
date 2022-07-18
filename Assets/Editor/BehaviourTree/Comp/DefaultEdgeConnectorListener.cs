// ******************************************************************
//       /\ /|       @file       DefaultEdgeConnectorListener
//       \ V/        @brief      边缘连接监听
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 23:35
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Rabi
{
    public class DefaultEdgeConnectorListener : IEdgeConnectorListener
    {
        private readonly GraphViewChange _graphViewChange; //节点变化数据
        private readonly List<Edge> _edgesToCreate; //要创建的线
        private readonly List<GraphElement> _edgesToDelete; //要删除的线

        public DefaultEdgeConnectorListener()
        {
            _edgesToCreate = new List<Edge>();
            _edgesToDelete = new List<GraphElement>();
            _graphViewChange.edgesToCreate = _edgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }

        /// <summary>
        /// 两个端口间完成连接时回调
        /// </summary>
        /// <param name="graphView">新连接的view</param>
        /// <param name="edge">当前处理的边缘</param>
        public void OnDrop(GraphView graphView, Edge edge)
        {
            //即将创建的连接
            _edgesToCreate.Clear();
            _edgesToCreate.Add(edge);
            _edgesToDelete.Clear();
            //清理当前连线两端的全部连接
            if (edge.input.capacity == Port.Capacity.Single)
                foreach (var edgeToDelete in edge.input.connections)
                    if (edgeToDelete != edge)
                        _edgesToDelete.Add(edgeToDelete);
            if (edge.output.capacity == Port.Capacity.Single)
                foreach (var edgeToDelete in edge.output.connections)
                    if (edgeToDelete != edge)
                        _edgesToDelete.Add(edgeToDelete);
            if (_edgesToDelete.Count > 0)
                graphView.DeleteElements(_edgesToDelete);
            var edgesToCreate = _edgesToCreate;
            if (graphView.graphViewChanged != null)
            {
                edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
            }
            
            foreach (var e in edgesToCreate)
            {
                graphView.AddElement(e);
                edge.input.Connect(e);
                edge.output.Connect(e);
            }
        }
    }
}