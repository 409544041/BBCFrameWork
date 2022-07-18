// ******************************************************************
//       /\ /|       @file       TransButton.cs
//       \ V/        @brief      翻译按钮
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 09:04:09
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace Rabi
{
    public class TransButton : Button
    {
        [FieldName("高亮图id", HtmlColorDef.AliceBlue)]
        public int highlightedTransId; //高亮图id

        [FieldName("按下图id", HtmlColorDef.AliceBlue)]
        public int pressedTransId; //按下图id

        [FieldName("选中图id", HtmlColorDef.AliceBlue)]
        public int selectedTransId; //选中图id

        [FieldName("禁用图id", HtmlColorDef.AliceBlue)]
        public int disabledTransId; //禁用图id

        private bool _isDirty; //脏标记

        protected override void Start()
        {
            base.Start();
            OnLanguageChanged();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Instance.AddListener(EventId.OnLanguageChanged, OnLanguageChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.Instance.RemoveListener(EventId.OnLanguageChanged, OnLanguageChanged);
        }

        /// <summary>
        /// 更新翻译图id
        /// </summary>
        /// <param name="highlightedId"></param>
        /// <param name="pressedId"></param>
        /// <param name="selectedId"></param>
        /// <param name="disabledId"></param>
        public void UpdateTrans(int highlightedId, int pressedId, int selectedId, int disabledId)
        {
            highlightedTransId = highlightedId;
            pressedTransId = pressedId;
            selectedTransId = selectedId;
            disabledTransId = disabledId;
            _isDirty = true;
        }

        private void Update()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (!_isDirty)
            {
                return;
            }

            if (!Application.isPlaying)
            {
                return;
            }

            _isDirty = false;
            this.UpdateSpriteState();
        }

        /// <summary>
        /// 语言更变回调
        /// </summary>
        private void OnLanguageChanged()
        {
            _isDirty = true;
        }
    }
}