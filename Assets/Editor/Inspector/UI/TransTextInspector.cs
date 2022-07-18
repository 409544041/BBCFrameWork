// ******************************************************************
//       /\ /|       @file       TransTextInspector.cs
//       \ V/        @brief      翻译文本显示窗
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-21 14:26:45
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEditor.UI;

namespace Rabi
{
    [CustomEditor(typeof(TransText))]
    public class TransTextInspector : TextEditor
    {
        private SerializedProperty _transId;
        private SerializedProperty _fontSettingId;

        protected override void OnEnable()
        {
            base.OnEnable();
            _transId = serializedObject.FindProperty("transId");
            _fontSettingId = serializedObject.FindProperty("fontSettingId");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_transId);
            EditorGUILayout.PropertyField(_fontSettingId);
            serializedObject.ApplyModifiedProperties();
        }
    }
}