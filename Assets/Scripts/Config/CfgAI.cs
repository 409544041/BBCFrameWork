// ******************************************************************
//       /\ /|       @file       CfgAI.cs
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
    public class RowCfgAI
    {
        public int id; //id
        public string annotate; //注释
        public string enumName; //枚举名称
        public string path; //资源路径
    }

    public class CfgAI
    {
        private readonly Dictionary<int, RowCfgAI> _configs = new Dictionary<int, RowCfgAI>(); //cfgId映射row
        public RowCfgAI this[Enum cid] => _configs.ContainsKey(cid.GetHashCode()) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public RowCfgAI this[int cid] => _configs.ContainsKey(cid) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public List<RowCfgAI> AllConfigs => _configs.Values.ToList();

        /// <summary>
        /// 获取行数据
        /// </summary>
        public RowCfgAI Find(int i)
        {
            return this[i];
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        public void Load()
        {
            var reader = new CsvReader();
            reader.LoadText("Assets/AddressableAssets/Config/CfgAI.txt", 3);
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
        private RowCfgAI ParseRow(string[] col)
        {
            //列越界
            if (col.Length < 4)
            {
                Debug.LogError($"配置表字段行数越界:{GetType()}");
                return null;
            }

            var data = new RowCfgAI();
            var rowHelper = new RowHelper(col);
            data.id = CsvUtility.ToInt(rowHelper.ReadNextCol()); //id
            data.annotate = CsvUtility.ToString(rowHelper.ReadNextCol()); //注释
            data.enumName = CsvUtility.ToString(rowHelper.ReadNextCol()); //枚举名称
            data.path = CsvUtility.ToString(rowHelper.ReadNextCol()); //资源路径
            return data;
        }
    }
}