// ******************************************************************
//       /\ /|       @file       TransButtonInspector.cs
//       \ V/        @brief      翻译按钮显示窗
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 09:08:13
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEditor.UI;

namespace Rabi
{
    [CustomEditor(typeof(TransButton))]
    public class TransButtonInspector : ButtonEditor
    {
        private SerializedProperty _highlightedTransId;
        private SerializedProperty _pressedTransId;
        private SerializedProperty _selectedTransId;
        private SerializedProperty _disabledTransId;

        protected override void OnEnable()
        {
            base.OnEnable();
            _highlightedTransId = serializedObject.FindProperty("highlightedTransId");
            _pressedTransId = serializedObject.FindProperty("pressedTransId");
            _selectedTransId = serializedObject.FindProperty("selectedTransId");
            _disabledTransId = serializedObject.FindProperty("disabledTransId");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_highlightedTransId);
            EditorGUILayout.PropertyField(_pressedTransId);
            EditorGUILayout.PropertyField(_selectedTransId);
            EditorGUILayout.PropertyField(_disabledTransId);
            serializedObject.ApplyModifiedProperties();
        }
    }
}