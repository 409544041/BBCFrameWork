// ******************************************************************
//       /\ /|       @file       ListViewEditor.cs
//       \ V/        @brief      listview扩展编辑器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-01-03 14:56:32
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rabi
{
    [CustomEditor(typeof(ListView))]
    public class ListViewEditor : Editor
    {
        private MethodInfo _clearItems; //清理items方法
        private MethodInfo _init; //初始化方法

        private void OnEnable()
        {
            //反射获取实例私有方法
            _clearItems = target.GetType()
                .GetMethod("EditorClearItems", BindingFlags.Instance | BindingFlags.NonPublic);
            _init = target.GetType().GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
            //初始化组件
            _init?.Invoke(target, null);
        }

        private void OnDisable()
        {
            _clearItems = null;
            _init = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            GUILayout.Label("*****预览*****");
            if (GUILayout.Button("清空"))
            {
                _clearItems?.Invoke(target, null);
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}