// ******************************************************************
//       /\ /|       @file       ThinkNodeView.cs
//       \ V/        @brief      行为树节点视图
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 20:24:31
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rabi
{
    public class ThinkNodeView : Node
    {
        public Action<ThinkNodeView> onNodeSelected; //节点选中回调
        public BaseThinkNode node; //储存的行为节点
        public Port input; //输入端口
        public Port output; //输出端口

        public ThinkNodeView(BaseThinkNode node) : base(
            "Assets/Editor/BehaviourTree/Layout/NodeView.uxml")
        {
            this.node = node;
            style.left = node.position.x;
            style.top = node.position.y;
            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();
            RefreshTitle();
        }

        /// <summary>
        /// 刷新标题
        /// </summary>
        private void RefreshTitle()
        {
            title = node.GetType().Name;
        }

        /// <summary>
        /// 根据节点类别添加css样式
        /// </summary>
        private void SetupClasses()
        {
            switch (node)
            {
                case ThinkNodeAction:
                    AddToClassList("action");
                    break;
                case ThinkNodeComposite:
                    AddToClassList("composite");
                    break;
                case ThinkNodeDecorator:
                    AddToClassList("decorator");
                    break;
                case ThinkNodeRoot:
                    AddToClassList("root");
                    break;
            }
        }

        /// <summary>
        /// 创建输入端口
        /// </summary>
        private void CreateInputPorts()
        {
            switch (node)
            {
                case ThinkNodeAction:
                case ThinkNodeComposite:
                case ThinkNodeDecorator:
                    input = new ThinkNodePort(Direction.Input, Port.Capacity.Single);
                    break;
                case ThinkNodeRoot:
                    break;
            }

            if (input == null) return;
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }

        /// <summary>
        /// 创建输出端口
        /// </summary>
        private void CreateOutputPorts()
        {
            switch (node)
            {
                case ThinkNodeAction:
                    break;
                case ThinkNodeComposite:
                    output = new ThinkNodePort(Direction.Output, Port.Capacity.Multi);
                    break;
                case ThinkNodeDecorator:
                case ThinkNodeRoot:
                    output = new ThinkNodePort(Direction.Output, Port.Capacity.Single);
                    break;
            }

            if (output == null) return;
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }

        /// <summary>
        /// 设置节点位置
        /// </summary>
        /// <param name="newPos"></param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.position = new Vector2(newPos.xMin, newPos.yMin);
        }

        /// <summary>
        /// 节点选中回调
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            onNodeSelected?.Invoke(this);
        }

        /// <summary>
        /// 组合节点排序
        /// </summary>
        public void SortChildren()
        {
            if (node is ThinkNodeComposite composite)
            {
                composite.children.Sort(SortByHorizontalPosition);
            }
        }

        /// <summary>
        /// 组合节点下的排序 根据编辑器内x坐标
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int SortByHorizontalPosition(BaseThinkNode left, BaseThinkNode right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }
    }
}