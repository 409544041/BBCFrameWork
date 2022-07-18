// ******************************************************************
//       /\ /|       @file       EnumAudioLayer.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumAudioLayer
    {
        None = 0,
        [EnumName("背景音")] Music = 1,  //背景音
        [EnumName("环境音")] Ambient = 2,  //环境音
        [EnumName("语音")] Voice = 3,  //语音
        [EnumName("音效")] Sound = 4,  //音效
    }
}