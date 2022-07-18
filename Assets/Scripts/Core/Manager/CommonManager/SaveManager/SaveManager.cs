// ******************************************************************
//       /\ /|       @file       SaveManager.cs
//       \ V/        @brief      存档管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-14 09:45:04
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class SaveManager : BaseSingleTon<SaveManager>
    {
        public void SavePlayerData()
        {
            //SaveUtil.SaveByJson("LevelData", levelData);
            //SaveUtil.SaveByJson("SettingData", settingData);
        }

        public void LoadPlayerData()
        {
            // levelData = SaveUtil.LoadFromJson<LevelData>("LevelData") ?? new LevelData();
            // settingData = SaveUtil.LoadFromJson<SettingData>("SettingData") ?? new SettingData();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        public void SaveSetting()
        {
            SaveUtil.SaveByJson(SettingData.Instance);
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        public void LoadSetting()
        {
            SettingData.Instance.SetData(SaveUtil.LoadFromJson<SettingData>());
        }
    }
}