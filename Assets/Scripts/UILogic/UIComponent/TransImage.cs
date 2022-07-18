// ******************************************************************
//       /\ /|       @file       TransImage.cs
//       \ V/        @brief      翻译图片组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 08:23:52
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace Rabi
{
    public class TransImage : Image
    {
        [FieldName("翻译id", HtmlColorDef.AliceBlue)]
        public int transId; //翻译id

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
        /// 更新翻译id
        /// </summary>
        /// <param name="id"></param>
        public void UpdateTrans(int id)
        {
            transId = id;
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
            this.SetSpriteAsync(transId);
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