// ******************************************************************
//       /\ /|       @file       TransMeshPro.cs
//       \ V/        @brief      支持多语言MeshPro
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-18 11:37:30
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Rabi
{
    public class TransMeshPro : TextMeshProUGUI
    {
        [FieldName("翻译id(为0不翻译)", HtmlColorDef.AliceBlue)] [SerializeField]
        private int transId; //默认的静态翻译id

        [FieldName("字体配置id(为0采用默认)", HtmlColorDef.AliceBlue)] [SerializeField]
        private int fontSettingId; //字体配置id

        private readonly List<TransContent> _transContentList = new List<TransContent>(2); //翻译文本列表
        private readonly StringBuilder _resultBuilder = new StringBuilder(); //用于拼接结果
        private bool _isDirty; //脏标记
        private float _cacheFontSize; //初始字体大小
        private float _cacheLineSpacing; //初始行间距
        private float _cacheCharacterSpacing; //初始字间距
        private int _fontId; //字体id

        protected override void Awake()
        {
            base.Awake();
            //初始化 添加翻译文本
            _transContentList.Clear();
            if (transId != 0)
            {
                _transContentList.Add(LanguageUtil.CreateTransContent(transId));
            }

            //缓存当前的字体大小 间距
            _cacheFontSize = fontSize;
            _cacheLineSpacing = lineSpacing;
            _cacheCharacterSpacing = characterSpacing;
            //计算当前字体id
            _fontId = LanguageDef.FindFontId(font.name, 2);
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
            AssetManager.Instance.LoadAssetAsync<TMP_FontAsset>(fontPath, OnFontAssetLoadComplete);
        }

        /// <summary>
        /// 字体资源加载完成回调
        /// </summary>
        /// <param name="f"></param>
        private void OnFontAssetLoadComplete(TMP_FontAsset f)
        {
            //设置字体
            font = f;
            //如果缺少配置 则使用默认字体配置
            if (fontSettingId == 0)
            {
                fontSettingId = 1;
            }

            var rowCfgFontSetting = ConfigManager.Instance.cfgFontSettingPro[fontSettingId];
            var index = SettingData.Instance.currentLanguageId.GetHashCode();
            //越界
            if (index >= rowCfgFontSetting.meshProSettings.Length)
            {
                Logger.Error($"当前语言缺少字体配置:{SettingData.Instance.currentLanguageId}");
                return;
            }

            //调整新字体参数
            fontSize = _cacheFontSize * rowCfgFontSetting.meshProSettings[index].fontSizeScale;
            lineSpacing = _cacheLineSpacing * rowCfgFontSetting.meshProSettings[index].lineSpacingScale;
            characterSpacing = _cacheCharacterSpacing * rowCfgFontSetting.meshProSettings[index].characterSpacingScale;
            _isDirty = true;
        }
    }
}