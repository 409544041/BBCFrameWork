// ******************************************************************
//       /\ /|       @file       CfgFontSettingPro.cs
//       \ V/        @brief      excel数据解析(由python自动生成)
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |
//      /  \\        @Modified   2022-04-25 13:25:11
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Rabi
{
    public class RowCfgFontSettingPro
    {
        public int id; //id
        public string desc; //描述
        public MeshProSetting[] meshProSettings; //MeshProSetting
    }

    public struct MeshProSetting
    {
        public float fontSizeScale; //中文MeshPro字号缩放比
        public float lineSpacingScale; //MeshPro行间距缩放比
        public float characterSpacingScale; //MeshPro字间距缩放比
    }

    public class CfgFontSettingPro
    {
        private readonly Dictionary<int, RowCfgFontSettingPro> _configs = new Dictionary<int, RowCfgFontSettingPro>(); //cfgId映射row
        public RowCfgFontSettingPro this[Enum cid] => _configs.ContainsKey(cid.GetHashCode()) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public RowCfgFontSettingPro this[int cid] => _configs.ContainsKey(cid) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public List<RowCfgFontSettingPro> AllConfigs => _configs.Values.ToList();

        /// <summary>
        /// 获取行数据
        /// </summary>
        public RowCfgFontSettingPro Find(int i)
        {
            return this[i];
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        public void Load()
        {
            var reader = new CsvReader();
            reader.LoadText("Assets/AddressableAssets/Config/CfgFontSettingPro.txt", 3);
            var rows = reader.GetRowCount();
            for (var i = 0; i < rows; ++i)
            {
                var row = reader.GetColValueArray(i);
                var data = ParseRow(row);
                if (!_configs.ContainsKey(data.id))
                {
                    _configs.Add(data.id, data);
                }
            }
        }

        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private RowCfgFontSettingPro ParseRow(string[] col)
        {
            //列越界
            if (col.Length < 11)
            {
                Debug.LogError($"配置表字段行数越界:{GetType()}");
                return null;
            }

            var data = new RowCfgFontSettingPro();
            var rowHelper = new RowHelper(col);
            data.id = CsvUtility.ToInt(rowHelper.ReadNextCol()); //id
            data.desc = CsvUtility.ToString(rowHelper.ReadNextCol()); //描述
            data.meshProSettings = new MeshProSetting[3];
            data.meshProSettings[0] = new MeshProSetting();
            data.meshProSettings[0].fontSizeScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //中文MeshPro字号缩放比
            data.meshProSettings[0].lineSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro行间距缩放比
            data.meshProSettings[0].characterSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro字间距缩放比
            data.meshProSettings[1] = new MeshProSetting();
            data.meshProSettings[1].fontSizeScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //中文MeshPro字号缩放比
            data.meshProSettings[1].lineSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro行间距缩放比
            data.meshProSettings[1].characterSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro字间距缩放比
            data.meshProSettings[2] = new MeshProSetting();
            data.meshProSettings[2].fontSizeScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //中文MeshPro字号缩放比
            data.meshProSettings[2].lineSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro行间距缩放比
            data.meshProSettings[2].characterSpacingScale = CsvUtility.ToFloat(rowHelper.ReadNextCol()); //MeshPro字间距缩放比
            return data;
        }
    }
}