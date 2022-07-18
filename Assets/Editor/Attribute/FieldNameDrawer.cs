// ******************************************************************
//       /\ /|       @file       FieldLabelDrawer.cs
//       \ V/        @brief      Inspector字段命名 绘制
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-31 09:01:01
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEngine;

namespace Rabi
{
    [CustomPropertyDrawer(typeof(FieldNameAttribute))]
    public class FieldNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (FieldNameAttribute) attribute;
            var color = ColorUtil.GetColor(attr.htmlColor);
            //在这里重新绘制
            var cacheColor = GUI.color;
            GUI.color = color;
            EditorGUI.PropertyField(position, property, new GUIContent(attr.label), true);
            GUI.color = cacheColor;
        }
    }
}