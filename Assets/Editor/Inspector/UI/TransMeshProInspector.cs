// ******************************************************************
//       /\ /|       @file       TransMeshPro.cs
//       \ V/        @brief      meshPro支持多语言版
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 10:51:18
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using TMPro.EditorUtilities;
using UnityEditor;

namespace Rabi
{
    [CustomEditor(typeof(TransMeshPro), true), CanEditMultipleObjects]
    public class TransMeshProInspector : TMP_EditorPanelUI
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