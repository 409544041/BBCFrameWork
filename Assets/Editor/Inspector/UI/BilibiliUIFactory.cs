// ******************************************************************
//       /\ /|       @file       BilibiliUIFactory.cs
//       \ V/        @brief      UI创建工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-01-14 18:54:26
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using UnityEditor;
using UnityEngine;

namespace Rabi
{
    [Serializable]
    public class BilibiliUIFactory
    {
        //无射线翻译图片
        private static string _prefabTransImageName = "_TransImg";
        private static string _prefabTransImagePath = "Assets/AddressableAssets/Mix/UI/Example/UITransImage.prefab";

        //无射线图片
        private static string _prefabImageName = "_Img";
        private static string _prefabImagePath = "Assets/AddressableAssets/Mix/UI/Example/UIImage.prefab";

        //有射线btn
        private static string _prefabButtonName = "_Btn";
        private static string _prefabButtonPath = "Assets/AddressableAssets/Mix/UI/Example/UIButton.prefab";

        //滑动垂直布局
        private static string _prefabVerticalListViewName = "_Lv";

        private static string _prefabVerticalListViewPath =
            "Assets/AddressableAssets/Mix/UI/Example/UIVerticalListView.prefab";

        //滑动水平布局
        private static string _prefabHorizontalListViewName = "_Lv";

        private static string _prefabHorizontalListViewPath =
            "Assets/AddressableAssets/Mix/UI/Example/UIHorizontalListView.prefab";

        //空按钮
        private static string _prefabEmptyButtonName = "_Btn";
        private static string _prefabEmptyButtonPath = "Assets/AddressableAssets/Mix/UI/Example/UIEmptyButton.prefab";

        //翻译文本
        private static string _prefabTextBoldName = "_TransTxt";
        private static string _prefabTextBoldPath = "Assets/AddressableAssets/Mix/UI/Example/UITransText.prefab";

        //状态按钮
        private static string _prefabStateButtonName = "_Sbt";
        private static string _prefabStateButtonPath = "Assets/AddressableAssets/Mix/UI/Example/UIStateButton.prefab";

        //翻译按钮
        private static string _prefabTransButtonName = "_TransBtn";
        private static string _prefabTransButtonPath = "Assets/AddressableAssets/Mix/UI/Example/TransButton.prefab";

        //静态垂直布局
        private static string _prefabVerticalStaticListViewName = "_Slv";

        private static string _prefabVerticalStaticListViewPath =
            "Assets/AddressableAssets/Mix/UI/Example/UIVerticalStaticListView.prefab";

        //静态水平布局
        private static string _prefabHorizontalStaticListViewName = "_Slv";

        private static string _prefabHorizontalStaticListViewPath =
            "Assets/AddressableAssets/Mix/UI/Example/UIHorizontalStaticListView.prefab";

        [MenuItem("GameObject/bilibili UI/垂直滑动布局", false, priority = -1001)]
        private static void CreateVerticalListView()
        {
            CreatePrefab(_prefabVerticalListViewPath, _prefabVerticalListViewName);
        }

        [MenuItem("GameObject/bilibili UI/水平滑动布局", false, priority = -1002)]
        private static void CreateHorizontalListView()
        {
            CreatePrefab(_prefabHorizontalListViewPath, _prefabHorizontalListViewName);
        }

        [MenuItem("GameObject/bilibili UI/按钮", false, priority = -1003)]
        private static void CreateButton()
        {
            CreatePrefab(_prefabButtonPath, _prefabButtonName);
        }

        [MenuItem("GameObject/bilibili UI/空按钮", false, priority = -1004)]
        private static void CreateEmptyButton()
        {
            CreatePrefab(_prefabEmptyButtonPath, _prefabEmptyButtonName);
        }

        [MenuItem("GameObject/bilibili UI/翻译文本", false, priority = -1005)]
        private static void CreateTextBold()
        {
            CreatePrefab(_prefabTextBoldPath, _prefabTextBoldName);
        }

        [MenuItem("GameObject/bilibili UI/无射线翻译图片", false, priority = -1007)]
        private static void CreateTransImage()
        {
            CreatePrefab(_prefabTransImagePath, _prefabTransImageName);
        }

        [MenuItem("GameObject/bilibili UI/无射线图片", false, priority = -1008)]
        private static void CreateImage()
        {
            CreatePrefab(_prefabImagePath, _prefabImageName);
        }

        [MenuItem("GameObject/bilibili UI/状态按钮", false, priority = -1009)]
        private static void CreateStateButton()
        {
            CreatePrefab(_prefabStateButtonPath, _prefabStateButtonName);
        }

        [MenuItem("GameObject/bilibili UI/翻译按钮", false, priority = -1010)]
        private static void CreateTransButton()
        {
            CreatePrefab(_prefabTransButtonPath, _prefabTransButtonName);
        }

        [MenuItem("GameObject/bilibili UI/静态垂直布局", false, priority = -1011)]
        private static void CreateVerticalStaticListView()
        {
            CreatePrefab(_prefabVerticalStaticListViewPath, _prefabVerticalStaticListViewName);
        }

        [MenuItem("GameObject/bilibili UI/静态水平布局", false, priority = -1012)]
        private static void CreateHorizontalStaticListView()
        {
            CreatePrefab(_prefabHorizontalStaticListViewPath, _prefabHorizontalStaticListViewName);
        }

        /// <summary>
        /// 创建预制体 并挂载到当前选中物体上
        /// </summary>
        /// <param name="prefabPath"></param>
        /// <param name="name"></param>
        private static void CreatePrefab(string prefabPath, string name)
        {
            var selectedObjArray = Selection.gameObjects;
            foreach (var selectedObj in selectedObjArray)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                var prefabObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (prefabObj == null)
                {
                    continue;
                }

                prefabObj.transform.SetParent(selectedObj.transform, false);
                prefabObj.name = name;
                PrefabUtility.UnpackPrefabInstance(prefabObj, PrefabUnpackMode.Completely,
                    InteractionMode.AutomatedAction);
                Selection.activeObject = prefabObj;
            }
        }
    }
}