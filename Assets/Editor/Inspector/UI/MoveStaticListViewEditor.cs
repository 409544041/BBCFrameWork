// ******************************************************************
//       /\ /|       @file       MoveStaticListViewEditor.cs
//       \ V/        @brief      可移动静态列表编辑器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-02-09 10:56:17
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rabi
{
    [CustomEditor(typeof(MoveStaticListView))]
    public class MoveStaticListViewEditor : Editor
    {
        private MethodInfo _editorClearItems; //清理items
        private MethodInfo _editorShowItems; //预览布局效果
        private MethodInfo _editorMoveToFirst; //移动到目标位置
        private MethodInfo _editorMoveToOrigin; //移动到原位置

        private void OnEnable()
        {
            //反射获取实例私有方法
            _editorClearItems =
                target.GetType().GetMethod("EditorClearItems", BindingFlags.Instance | BindingFlags.NonPublic);
            _editorShowItems = target.GetType()
                .GetMethod("EditorDrawItems", BindingFlags.Instance | BindingFlags.NonPublic);
            _editorMoveToFirst = target.GetType()
                .GetMethod("EditorMoveToFirst", BindingFlags.Instance | BindingFlags.NonPublic);
            _editorMoveToOrigin = target.GetType()
                .GetMethod("EditorMoveToOrigin", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void OnDisable()
        {
            _editorClearItems = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (GUILayout.Button("清空子节点"))
            {
                _editorClearItems?.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("预览"))
            {
                _editorShowItems?.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }

            GUILayout.Label("********运行中限定********");
            if (GUILayout.Button("移动"))
            {
                _editorMoveToFirst?.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("还原"))
            {
                _editorMoveToOrigin?.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}