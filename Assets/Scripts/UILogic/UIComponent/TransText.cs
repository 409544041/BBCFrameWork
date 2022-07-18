// ******************************************************************
//       /\ /|       @file       TransText.cs
//       \ V/        @brief      翻译文本组件
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-21 12:49:49
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Rabi
{
    public class TransText : Text
    {
        [FieldName("翻译id(为0不翻译)", HtmlColorDef.AliceBlue)] [SerializeField]
        private int transId; //默认的静态翻译id

        [FieldName("字体配置id(为0采用默认)", HtmlColorDef.AliceBlue)] [SerializeField]
        private int fontSettingId; //字体配置id

        private readonly List<TransContent> _transContentList = new List<TransContent>(4); //翻译文本列表
        private readonly StringBuilder _resultBuilder = new StringBuilder(); //用于拼接结果
        private bool _isDirty; //脏标记
        private int _cacheFontSize; //初始字体大小
        private float _cacheLineSpacing; //初始行间距
        private int _fontId; //字体id

        protected override void Awake()
        {
            base.Awake();
            //初始化 添加翻译文本
            _transContentList.Clear();
            //无视0
            if (transId != 0)
            {
                _transContentList.Add(LanguageUtil.CreateTransContent(transId));
            }

            //缓存当前的字体大小 间距
            _cacheFontSize = fontSize;
            _cacheLineSpacing = lineSpacing;
            //计算当前字体id
            _fontId = LanguageDef.FindFontId(font.name);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnLanguageChanged();
            EventManager.Instance.AddListener(EventId.OnLanguageChanged, OnLanguageChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.Instance.RemoveListener(EventId.OnLanguageChanged, OnLanguageChanged);
        }

        protected void Update()
        {
            UpdateText();
        }

        /// <summary>
        /// 更新翻译id
        /// </summary>
        /// <param name="id"></param>
        public void UpdateTrans(int id)
        {
            _transContentList.Clear();
            _transContentList.Add(LanguageUtil.CreateTransContent(id));
            _isDirty = true;
        }

        /// <summary>
        /// 更新翻译id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paramArray"></param>
        public void UpdateTrans(int id, params object[] paramArray)
        {
            _transContentList.Clear();
            _transContentList.Add(LanguageUtil.CreateTransContent(id, paramArray));
            _isDirty = true;
        }

        /// <summary>
        /// 更新翻译id
        /// </summary>
        /// <param name="transContentList"></param>
        public void UpdateTrans(params TransContent[] transContentList)
        {
            _transContentList.Clear();
            _transContentList.AddRange(transContentList);
            _isDirty = true;
        }

        /// <summary>
        /// 更新文本
        /// </summary>
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
            _resultBuilder.Clear();
            if (_transContentList.Count <= 0)
            {
                return;
            }

            foreach (var transContent in _transContentList)
            {
                _resultBuilder.Append(transContent.ToString());
            }

            text = _resultBuilder.ToString().RecoverEnter();
        }

        /// <summary>
        /// 语言更变回调
        /// </summary>
        private void OnLanguageChanged()
        {
            //Text组件估计有编辑模式运行的特性 每次关掉都会走awake
            if (!Application.isPlaying)
            {
                return;
            }

            //更新字体
            var currentLanguageHash = SettingData.Instance.currentLanguageId.GetHashCode();
            var fontAssetId = ConfigManager.Instance.cfgFontMap[_fontId].fontIds[currentLanguageHash];
            var fontPath = ConfigManager.Instance.cfgFont[fontAssetId].path;
            AssetManager.Instance.LoadAssetAsync<Font>(fontPath, OnFontAssetLoadComplete);
        }

        /// <summary>
        /// 字体资源加载完成回调
        /// </summary>
        /// <param name="f"></param>
        private void OnFontAssetLoadComplete(Font f)
        {
            //设置字体
            font = f;
            //如果缺少配置 则使用默认字体配置
            if (fontSettingId == 0)
            {
                fontSettingId = 1;
            }

            var rowCfgFontSetting = ConfigManager.Instance.cfgFontSetting[fontSettingId];
            var index = SettingData.Instance.currentLanguageId.GetHashCode();
            //越界
            if (index >= rowCfgFontSetting.textSettings.Length)
            {
                Logger.Error($"当前语言缺少字体配置:{SettingData.Instance.currentLanguageId}");
                return;
            }

            //调整新字体参数
            fontSize = (int)(_cacheFontSize * rowCfgFontSetting.textSettings[index].fontSizeScale);
            lineSpacing = _cacheLineSpacing * rowCfgFontSetting.textSettings[index].lineSpacingScale;
            //标记当前帧更新
            _isDirty = true;
        }
    }
}