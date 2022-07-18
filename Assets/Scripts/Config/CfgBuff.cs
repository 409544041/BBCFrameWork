// ******************************************************************
//       /\ /|       @file       CfgBuff.cs
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
    public class RowCfgBuff
    {
        public int id; //id
        public string annotate; //注释
        public string enumName; //枚举名称
        public int buffType; //buff类型
        public Dictionary<string,int> buffParams; //buff参数
        public List<int> conflictBuffList; //冲突buff列表
        public int priority; //冲突保留优先级
        public int duration; //持续时间s
    }

    public class CfgBuff
    {
        private readonly Dictionary<int, RowCfgBuff> _configs = new Dictionary<int, RowCfgBuff>(); //cfgId映射row
        public RowCfgBuff this[Enum cid] => _configs.ContainsKey(cid.GetHashCode()) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public RowCfgBuff this[int cid] => _configs.ContainsKey(cid) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public List<RowCfgBuff> AllConfigs => _configs.Values.ToList();

        /// <summary>
        /// 获取行数据
        /// </summary>
        public RowCfgBuff Find(int i)
        {
            return this[i];
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        public void Load()
        {
            var reader = new CsvReader();
            reader.LoadText("Assets/AddressableAssets/Config/CfgBuff.txt", 3);
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
        private RowCfgBuff ParseRow(string[] col)
        {
            //列越界
            if (col.Length < 8)
            {
                Debug.LogError($"配置表字段行数越界:{GetType()}");
                return null;
            }

            var data = new RowCfgBuff();
            var rowHelper = new RowHelper(col);
            data.id = CsvUtility.ToInt(rowHelper.ReadNextCol()); //id
            data.annotate = CsvUtility.ToString(rowHelper.ReadNextCol()); //注释
            data.enumName = CsvUtility.ToString(rowHelper.ReadNextCol()); //枚举名称
            data.buffType = CsvUtility.ToInt(rowHelper.ReadNextCol()); //buff类型
            data.buffParams = CsvUtility.ToDictionary<string,int>(rowHelper.ReadNextCol()); //buff参数
            data.conflictBuffList = CsvUtility.ToList<int>(rowHelper.ReadNextCol()); //冲突buff列表
            data.priority = CsvUtility.ToInt(rowHelper.ReadNextCol()); //冲突保留优先级
            data.duration = CsvUtility.ToInt(rowHelper.ReadNextCol()); //持续时间s
            return data;
        }
    }
}