// ******************************************************************
//       /\ /|       @file       StateButtonEditor.cs
//       \ V/        @brief      状态按钮编辑器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-01-05 17:21:40
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEditor;

namespace Rabi
{
    [CustomEditor(typeof(StateButton))]
    public class StateButtonEditor : Editor
    {
        private SerializedProperty _isSelect; //是否处于选中状态
        private SerializedProperty _isSelectObj; //按钮选中状态下显示的物体
        private SerializedProperty _isNotSelectObj; //按钮非选中状态下显示的物体
        private SerializedProperty _selectedObjGraphic; //选中时使用的图形
        private SerializedProperty _notSelectedObjGraphic; //未选中时使用的图形

        private void OnEnable()
        {
            _isSelect = serializedObject.FindProperty("isSelect");
            _isSelectObj = serializedObject.FindProperty("isSelectObj");
            _isNotSelectObj = serializedObject.FindProperty("isNotSelectObj");
            _selectedObjGraphic = serializedObject.FindProperty("selectedObjGraphic");
            _notSelectedObjGraphic = serializedObject.FindProperty("notSelectedObjGraphic");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_isSelect);
            EditorGUILayout.PropertyField(_isSelectObj);
            EditorGUILayout.PropertyField(_isNotSelectObj);
            EditorGUILayout.PropertyField(_selectedObjGraphic);
            EditorGUILayout.PropertyField(_notSelectedObjGraphic);
            serializedObject.ApplyModifiedProperties();
        }
    }
}