// ******************************************************************
//       /\ /|       @file       AnimatorPreviewInspector
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-04 21:15
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Rabi
{
    [CustomEditor(typeof(AnimatorPreview))]
    public class AnimatorPreviewInspector : Editor
    {
        private AnimationClip[] _animationClips;
        private AnimatorPreview _script;
        private int _selectedIndex; // 当前选择的动画下标
        private float _sliderValue; // 当前滚动的位置值

        private void OnEnable()
        {
            _script = target as AnimatorPreview;
            var animator = _script!.gameObject.GetComponent<Animator>();
            // 控制动画器的 AnimatorController 的运行时表示
            var animatorController = (AnimatorController) animator!.runtimeAnimatorController;
            // 获取动画列表
            _animationClips = animatorController.animationClips;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // 启动代码块以检查GUI更改
            EditorGUI.BeginChangeCheck();
            // 获取动画名称列表
            var displayedOptions = _animationClips.Select(clip => clip.name).ToArray();
            _selectedIndex = EditorGUILayout.Popup("动画", _selectedIndex, displayedOptions);
            // 滑动条
            _sliderValue = EditorGUILayout.Slider("进度", _sliderValue, 0.0f, 1.0f);
            // 没发生改变
            if (!EditorGUI.EndChangeCheck()) return;
            // 当前使用的动画
            var animationClip = _animationClips[_selectedIndex];
            // 将总时间均匀分布在滚动条上
            var time = animationClip.length * _sliderValue;
            // 在给定时间针对任何动画属性对动画进行采样
            animationClip.SampleAnimation(_script.gameObject, time);
        }
    }
}