// ******************************************************************
//       /\ /|       @file       BehaviourTreeEditor.cs
//       \ V/        @brief      行为树主窗口
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-07-06 18:08:10
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rabi
{
    public class BehaviourTreeEditorController : EditorWindow
    {
        private readonly BehaviourTreeEditorModel _model = new BehaviourTreeEditorModel(); //数据模型
        private ThinkTreeView _treeView; //行为树窗口
        private InspectorView _inspectorView; //节点参数窗口
        private ToolbarMenu _toolbarMenu; //工具栏

        [MenuItem("Rabi/AI编辑器", false, -1)]
        public static void OpenWindow()
        {
            var window = GetWindow<BehaviourTreeEditorController>();
            window.titleContent = new GUIContent("行为树编辑器");
            window.minSize = new Vector2(800, 600);
            window.CreateNewTree();
        }

        /// <summary>
        /// 创建基础布局
        /// </summary>
        private void CreateGUI()
        {
            var layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AIDef.AIMainLayoutPath);
            if (layout == null)
            {
                Debug.Log("AI编辑器 布局文件不存在");
                return;
            }

            layout.CloneTree(rootVisualElement);
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(AIDef.AIMainStylePath);
            if (style == null)
            {
                Debug.Log("AI编辑器 样式文件不存在");
                return;
            }

            rootVisualElement.styleSheets.Add(style);
            //标题工具栏
            _toolbarMenu = rootVisualElement.Q<ToolbarMenu>();
            var treeConfigs = _model.GetAllTreeConfigs();
            treeConfigs.ForEach(treePath =>
            {
                var fileNameWithoutExtend = treePath.GetFileNameWithoutExtend();
                _toolbarMenu.menu.AppendAction(fileNameWithoutExtend,
                    _ =>
                    {
                        _model.LoadTree(fileNameWithoutExtend);
                        _treeView.RefreshTree(_model.GetSelectedTree());
                    });
            });
            _toolbarMenu.menu.AppendSeparator();
            _toolbarMenu.menu.AppendAction("新建", _ => CreateNewTree());
            //节点参数窗口
            _inspectorView = rootVisualElement.Q<InspectorView>();
            _inspectorView.onInspectorViewChanged = SaveTree;
            //树构建
            _treeView = rootVisualElement.Q<ThinkTreeView>();
            _treeView.onNodeSelected = OnNodeSelectionChanged;
            _treeView.onGraphViewChanged = SaveTree;
        }

        /// <summary>
        /// 新建行为树
        /// </summary>
        private void CreateNewTree()
        {
            _model.CreateNewTree();
            _treeView.RefreshTree(_model.GetSelectedTree());
        }

        /// <summary>
        /// 节点选择更变回调
        /// </summary>
        /// <param name="nodeView"></param>
        private void OnNodeSelectionChanged(ThinkNodeView nodeView)
        {
            _inspectorView.Refresh(nodeView);
        }

        /// <summary>
        /// 保存树
        /// </summary>
        private void SaveTree()
        {
            _model.SaveTree();
        }
    }
}