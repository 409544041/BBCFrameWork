// ******************************************************************
//       /\ /|       @file       EnumAudio.cs
//       \ V/        @brief      excel枚举(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public enum EnumAudio
    {
        None = 0,
        [EnumName("测试背景音")] TestMusic = 1,  //测试背景音
        [EnumName("测试环境音")] TestAmbient = 2,  //测试环境音
        [EnumName("测试语音")] TestVoice = 3,  //测试语音
        [EnumName("测试音效")] TestSound = 4,  //测试音效
    }
}