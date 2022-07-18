// ******************************************************************
//       /\ /|       @file       LanguageUtil.cs
//       \ V/        @brief      多语言工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-20 12:17:09
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;

namespace Rabi
{
    public static class LanguageUtil
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TransContent CreateTransContent(int transId, params object[] args)
        {
            var transContent = TransContent.Default;
            transContent.transId = transId;
            transContent.args = args;
            return transContent;
        }

        /// <summary>
        /// 获取key对应的翻译
        /// </summary>
        /// <param name="transTextId"></param>
        /// <returns></returns>
        public static string GetTransStr(int transTextId)
        {
            try
            {
                var cfgLanguage = ConfigManager.Instance.cfgText[transTextId];
                //从全局数据找到当前的国家country 根据key和country去配置表找到翻译
                return SettingData.Instance.currentLanguageId switch
                {
                    (int)LanguageId.English => cfgLanguage.en,
                    (int)LanguageId.Chinese => cfgLanguage.cn,
                    (int)LanguageId.Japanese => cfgLanguage.jp,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            catch (Exception e)
            {
                Logger.Error($"翻译缺失: {e}");
                return "missing translation";
            }
        }

        //TODO：封装一个获取对应ID 对应语言 配置数据结构体的函数 如果没做设置则采用默认数据或者是汉语的数据
    }
}