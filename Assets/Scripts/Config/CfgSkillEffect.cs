// ******************************************************************
//       /\ /|       @file       CfgSkillEffect.cs
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
    public class RowCfgSkillEffect
    {
        public int id; //id
        public string annotate; //注释
        public string enumName; //枚举名称
        public int skillEffectType; //技能效果类型
        public int skillEffectLogicType; //技能效果逻辑类型
        public Dictionary<string,int> skillEffectParams; //技能效果参数
        public List<int> conflictSkillEffectList; //冲突技能效果列表
        public int priority; //冲突保留优先级
        public string condition; //生效条件/释放条件
    }

    public class CfgSkillEffect
    {
        private readonly Dictionary<int, RowCfgSkillEffect> _configs = new Dictionary<int, RowCfgSkillEffect>(); //cfgId映射row
        public RowCfgSkillEffect this[Enum cid] => _configs.ContainsKey(cid.GetHashCode()) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public RowCfgSkillEffect this[int cid] => _configs.ContainsKey(cid) ? _configs[cid.GetHashCode()] : throw new Exception($"找不到配置 Cfg:{GetType()} configId:{cid}");
        public List<RowCfgSkillEffect> AllConfigs => _configs.Values.ToList();

        /// <summary>
        /// 获取行数据
        /// </summary>
        public RowCfgSkillEffect Find(int i)
        {
            return this[i];
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        public void Load()
        {
            var reader = new CsvReader();
            reader.LoadText("Assets/AddressableAssets/Config/CfgSkillEffect.txt", 3);
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
        private RowCfgSkillEffect ParseRow(string[] col)
        {
            //列越界
            if (col.Length < 9)
            {
                Debug.LogError($"配置表字段行数越界:{GetType()}");
                return null;
            }

            var data = new RowCfgSkillEffect();
            var rowHelper = new RowHelper(col);
            data.id = CsvUtility.ToInt(rowHelper.ReadNextCol()); //id
            data.annotate = CsvUtility.ToString(rowHelper.ReadNextCol()); //注释
            data.enumName = CsvUtility.ToString(rowHelper.ReadNextCol()); //枚举名称
            data.skillEffectType = CsvUtility.ToInt(rowHelper.ReadNextCol()); //技能效果类型
            data.skillEffectLogicType = CsvUtility.ToInt(rowHelper.ReadNextCol()); //技能效果逻辑类型
            data.skillEffectParams = CsvUtility.ToDictionary<string,int>(rowHelper.ReadNextCol()); //技能效果参数
            data.conflictSkillEffectList = CsvUtility.ToList<int>(rowHelper.ReadNextCol()); //冲突技能效果列表
            data.priority = CsvUtility.ToInt(rowHelper.ReadNextCol()); //冲突保留优先级
            data.condition = CsvUtility.ToString(rowHelper.ReadNextCol()); //生效条件/释放条件
            return data;
        }
    }
}