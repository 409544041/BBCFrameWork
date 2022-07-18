// ******************************************************************
//       /\ /|       @file       ThinkNodePort
//       \ V/        @brief      边缘连接器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 23:30
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rabi
{
    public class ThinkNodePort : Port
    {
        public ThinkNodePort(Direction direction, Capacity capacity) : base(Orientation.Vertical, direction, capacity,
            typeof(bool))
        {
            var connectorListener = new DefaultEdgeConnectorListener();
            m_EdgeConnector = new EdgeConnector<Edge>(connectorListener);
            this.AddManipulator(m_EdgeConnector);
            style.width = 100;
        }

        /// <summary>
        /// 检查点是否在端口顶部。 用于选择和悬停。
        /// </summary>
        /// <param name="localPoint"></param>
        /// <returns></returns>
        public override bool ContainsPoint(Vector2 localPoint)
        {
            var rect = new Rect(0, 0, layout.width, layout.height);
            return rect.Contains(localPoint);
        }
    }
}