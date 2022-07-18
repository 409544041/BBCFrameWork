// ******************************************************************
//       /\ /|       @file       BehaviourTreeEditorModel.cs
//       \ V/        @brief      行为树编辑器模型层
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-07 12:11:16
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class BehaviourTreeEditorModel
    {
        private BehaviourTree _selectedTree = new BehaviourTree(); //当前选中的行为树配置

        /// <summary>
        /// 创建新的行为树
        /// </summary>
        public void CreateNewTree()
        {
            _selectedTree = new BehaviourTree("TempTree");
        }

        /// <summary>
        /// 获取当前选中的树配置
        /// </summary>
        /// <returns></returns>
        public BehaviourTree GetSelectedTree()
        {
            return _selectedTree;
        }

        /// <summary>
        /// 获取全部行为树配置文件名
        /// </summary>
        /// <returns></returns>
        public string[] GetAllTreeConfigs()
        {
            return RabiFileUtil.GetSpecifyFilesInFolder(AIDef.AIDataFolder, new[] { ".json" });
        }

        /// <summary>
        /// 加载当前树的配置
        /// </summary>
        /// <param name="fileNameWithoutExtend"></param>
        public void LoadTree(string fileNameWithoutExtend)
        {
            if (fileNameWithoutExtend == null)
            {
                return;
            }

            _selectedTree?.Deserialize($"{AIDef.AIDataFolder}/{fileNameWithoutExtend}.json");
        }

        /// <summary>
        /// 保存树
        /// </summary>
        public void SaveTree()
        {
            _selectedTree?.Serialize();
        }
    }
}