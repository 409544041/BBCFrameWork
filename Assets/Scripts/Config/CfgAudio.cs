// ******************************************************************
//       /\ /|       @file       CfgAudio.cs
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
    public class RowCfgAudio
    {
        public int id; //id
        public string annotate; //注释
        public string enumName; //枚举名称
        public string path; //资源路径
        public int layer; //音效层级
    }

    public class CfgAudio
    {
        private readonly Dictionary<int, RowCfgAudio> _configs = new Dictionary<int, RowCfgAudio>(); //cfgId映射row
        private readonly Dictionary<int, List<RowCfgAudio>> _layerConfigGroup = new Dictionary<int, List<RowCfgAudio>>();
        public RowCfgAudio this[Enum cid] => _configs.ContainsKey(cid.GetHashCode()) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public RowCfgAudio this[int cid] => _configs.ContainsKey(cid) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public List<RowCfgAudio> AllConfigs => _configs.Values.ToList();

        /// <summary>
        /// 获取行数据
        /// </summary>
        public RowCfgAudio Find(int i)
        {
            return this[i];
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        public void Load()
        {
            var reader = new CsvReader();
            reader.LoadText("Assets/AddressableAssets/Config/CfgAudio.txt", 3);
            var rows = reader.GetRowCount();
            for (var i = 0; i < rows; ++i)
            {
                var row = reader.GetColValueArray(i);
                var data = ParseRow(row);
                if (!_configs.ContainsKey(data.id))
                {
                    _configs.Add(data.id, data);
                }

                if (!_layerConfigGroup.ContainsKey(data.layer))
                {
                     _layerConfigGroup.Add(data.layer, new List<RowCfgAudio>());
                }

                _layerConfigGroup[data.layer].Add(data);
            }
        }

        /// <summary>
        /// 根据layer值获取分组
        /// </summary>
        public List<RowCfgAudio> GetListByLayer(int groupValue)
        {
                return _layerConfigGroup.ContainsKey(groupValue) ? _layerConfigGroup[groupValue] : throw new Exception($"找不到组 Cfg:{GetType()} groupId:{groupValue}");
        }

        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private RowCfgAudio ParseRow(string[] col)
        {
            //列越界
            if (col.Length < 5)
            {
                Debug.LogError($"配置表字段行数越界:{GetType()}");
                return null;
            }

            var data = new RowCfgAudio();
            var rowHelper = new RowHelper(col);
            data.id = CsvUtility.ToInt(rowHelper.ReadNextCol()); //id
            data.annotate = CsvUtility.ToString(rowHelper.ReadNextCol()); //注释
            data.enumName = CsvUtility.ToString(rowHelper.ReadNextCol()); //枚举名称
            data.path = CsvUtility.ToString(rowHelper.ReadNextCol()); //资源路径
            data.layer = CsvUtility.ToInt(rowHelper.ReadNextCol()); //音效层级
            return data;
        }
    }
}