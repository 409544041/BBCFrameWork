// ******************************************************************
//       /\ /|       @file       DoubleClickSelection
//       \ V/        @brief      双击操作扩展 双击选中当前节点的所有子节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 23:24
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEngine.UIElements;

namespace Rabi
{
    public class DoubleClickSelection : MouseManipulator
    {
        private double _time;
        private const double DoubleClickDuration = 0.3;

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        public DoubleClickSelection()
        {
            _time = EditorApplication.timeSinceStartup;
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (target is not ThinkTreeView)
                return;

            var duration = EditorApplication.timeSinceStartup - _time;
            if (duration < DoubleClickDuration)
            {
                SelectChildren(evt);
            }

            _time = EditorApplication.timeSinceStartup;
        }

        private void SelectChildren(MouseDownEvent evt)
        {
            if (target is not ThinkTreeView thinkTreeView)
                return;

            if (!CanStopManipulation(evt))
                return;

            if (evt.target is not ThinkNodeView clickedElement)
            {
                var ve = evt.target as VisualElement;
                clickedElement = ve?.GetFirstAncestorOfType<ThinkNodeView>();
                if (clickedElement == null)
                    return;
            }

            BehaviourTree.Traverse(clickedElement.node, node => { thinkTreeView.AddToSelection(node); });
        }
    }
}