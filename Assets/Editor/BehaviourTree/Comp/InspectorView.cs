// ******************************************************************
//       /\ /|       @file       InspectorView.cs
//       \ V/        @brief      GUI窗口组件
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 18:35:25
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rabi
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
        {
        }

        public Action onInspectorViewChanged; //刷新回调

        public void Refresh(ThinkNodeView nodeView)
        {
            //清屏
            Clear();

            //绘制函数
            void OnGuiHandler()
            {
                var thinkNode = nodeView.node;
                var fields = thinkNode.GetType()!.GetFields(BindingFlags.Public | BindingFlags.Instance);
                if (fields.Length <= 0)
                {
                    return;
                }

                foreach (var field in fields)
                {
                    //没有绘制特性
                    if (!field.HasAttribute(typeof(DrawInInspectorAttribute), true))
                    {
                        continue;
                    }

                    DrawField(field, thinkNode);
                }

                if (!GUI.changed)
                {
                    return;
                }

                onInspectorViewChanged?.Invoke();
            }

            var container = new IMGUIContainer(OnGuiHandler);
            Add(container);
        }

        /// <summary>
        /// 绘制字段
        /// </summary>
        private static void DrawField(FieldInfo field, object thinkNode)
        {
            var fieldName = field.Name; //名称
            if (field.FieldType == typeof(Vector2))
            {
                field.SetValue(thinkNode,
                    EditorGUILayout.Vector2Field(fieldName, (Vector2) field.GetValue(thinkNode)));
                return;
            }

            if (field.FieldType == typeof(string))
            {
                field.SetValue(thinkNode,
                    EditorGUILayout.TextField(fieldName, (string) field.GetValue(thinkNode)));
                return;
            }

            if (field.FieldType == typeof(float) && field.HasAttribute(typeof(RangeAttribute), true))
            {
                var attr = field.GetCustomAttribute<RangeAttribute>();
                field.SetValue(thinkNode,
                    EditorGUILayout.Slider(fieldName, (float) field.GetValue(thinkNode), attr.min, attr.max));
                return;
            }

            if (field.FieldType == typeof(float))
            {
                field.SetValue(thinkNode,
                    EditorGUILayout.FloatField(fieldName, (float) field.GetValue(thinkNode)));
                return;
            }

            if (field.FieldType == typeof(int) && field.HasAttribute(typeof(RangeAttribute), true))
            {
                var attr = field.GetCustomAttribute<RangeAttribute>();
                field.SetValue(thinkNode,
                    EditorGUILayout.IntSlider(fieldName, (int) field.GetValue(thinkNode), (int) attr.min,
                        (int) attr.max));
                return;
            }

            if (field.FieldType == typeof(int))
            {
                field.SetValue(thinkNode,
                    EditorGUILayout.IntField(fieldName, (int) field.GetValue(thinkNode)));
                return;
            }

            if (field.FieldType == typeof(bool))
            {
                field.SetValue(thinkNode,
                    EditorGUILayout.Toggle(fieldName, (bool) field.GetValue(thinkNode)));
                return;
            }
        }
    }
}