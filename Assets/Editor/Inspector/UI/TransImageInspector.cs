// ******************************************************************
//       /\ /|       @file       TransImageInspector.cs
//       \ V/        @brief      翻译图片显示窗
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 08:26:32
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEditor.UI;

namespace Rabi
{
    [CustomEditor(typeof(TransImage))]
    public class TransImageInspector : ImageEditor
    {
        private SerializedProperty _transId;

        protected override void OnEnable()
        {
            base.OnEnable();
            _transId = serializedObject.FindProperty("transId");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_transId);
            serializedObject.ApplyModifiedProperties();
        }
    }
}