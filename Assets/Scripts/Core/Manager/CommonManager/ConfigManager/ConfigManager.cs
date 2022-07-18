// ******************************************************************
//       /\ /|       @file       ConfigManager.cs
//       \ V/        @brief      配置表管理器(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-23 22:40:15
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public sealed class ConfigManager : BaseSingleTon<ConfigManager>
    {
        public readonly CfgAI cfgAI = new CfgAI();
        public readonly CfgAudio cfgAudio = new CfgAudio();
        public readonly CfgAudioLayer cfgAudioLayer = new CfgAudioLayer();
        public readonly CfgBuff cfgBuff = new CfgBuff();
        public readonly CfgBuffType cfgBuffType = new CfgBuffType();
        public readonly CfgDirection cfgDirection = new CfgDirection();
        public readonly CfgEffect cfgEffect = new CfgEffect();
        public readonly CfgFont cfgFont = new CfgFont();
        public readonly CfgFontMap cfgFontMap = new CfgFontMap();
        public readonly CfgFontSetting cfgFontSetting = new CfgFontSetting();
        public readonly CfgFontSettingPro cfgFontSettingPro = new CfgFontSettingPro();
        public readonly CfgProperty cfgProperty = new CfgProperty();
        public readonly CfgPropertyType cfgPropertyType = new CfgPropertyType();
        public readonly CfgSkill cfgSkill = new CfgSkill();
        public readonly CfgSkillEffect cfgSkillEffect = new CfgSkillEffect();
        public readonly CfgSkillEffectLogicType cfgSkillEffectLogicType = new CfgSkillEffectLogicType();
        public readonly CfgSkillEffectType cfgSkillEffectType = new CfgSkillEffectType();
        public readonly CfgSkillType cfgSkillType = new CfgSkillType();
        public readonly CfgSprite cfgSprite = new CfgSprite();
        public readonly CfgText cfgText = new CfgText();
        public readonly CfgValueType cfgValueType = new CfgValueType();

        public ConfigManager()
        {
            //初始场景有Text的情况 查找翻译文本需要加载资源 因为同为Awake回调 加载顺序可能优于AssetManager 故补充加载
            AssetManager.Instance.OnInit();
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            cfgAI.Load();
            cfgAudio.Load();
            cfgAudioLayer.Load();
            cfgBuff.Load();
            cfgBuffType.Load();
            cfgDirection.Load();
            cfgEffect.Load();
            cfgFont.Load();
            cfgFontMap.Load();
            cfgFontSetting.Load();
            cfgFontSettingPro.Load();
            cfgProperty.Load();
            cfgPropertyType.Load();
            cfgSkill.Load();
            cfgSkillEffect.Load();
            cfgSkillEffectLogicType.Load();
            cfgSkillEffectType.Load();
            cfgSkillType.Load();
            cfgSprite.Load();
            cfgText.Load();
            cfgValueType.Load();
        }
    }}