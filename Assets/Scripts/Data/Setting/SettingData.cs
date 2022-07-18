// ******************************************************************
//       /\ /|       @file       SettingData
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-25 21:51
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class SettingData : BaseSingleTon<SettingData>
    {
        public int currentLanguageId = 2; //当前选中的语言
        public float currentMusicVolume = 1f; //音乐音量
        public float currentSoundVolume = 1f; //音效音量

        public void SetData(SettingData data)
        {
            if (data == null)
            {
                return;
            }

            currentLanguageId = data.currentLanguageId;
            currentMusicVolume = data.currentMusicVolume;
            currentSoundVolume = data.currentSoundVolume;
        }
    }
}