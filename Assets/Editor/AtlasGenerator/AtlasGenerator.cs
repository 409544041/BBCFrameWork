// ******************************************************************
//       /\ /|       @file       AtlasGenerator.cs
//       \ V/        @brief      图集生成器
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified    2021-11-09 12:37:42
//    *(__\_\        @Copyright  Copyright (c)  2021, Shadowrabbit
// ******************************************************************

using System;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Rabi
{
    public static class AtlasGenerator
    {
        [MenuItem("Assets/Create/AutoSpriteAtlas", false, priority = -1003)]
        public static void CreateAutoSpriteAtlas()
        {
            //获取选中目录
            var folderName = SelectObjectPathInfo(out var folderDir);
            AutoCreateAtlas(folderName, folderDir);
        }

        [MenuItem("Rabi/AutoSpriteAtlas/自动构建图集")]
        public static void AutoAtlas()
        {
            var folderList = RabiFileUtil.GetAllFolders(AtlasDef.AtlasRootFolder);
            foreach (var folder in folderList)
            {
                //不算自身
                if (folder.Equals(AtlasDef.AtlasRootFolder))
                {
                    continue;
                }

                var folderName = Path.GetFileName(folder);
                AutoCreateAtlas(folderName, folder);
            }
        }

        /// <summary>
        /// 获取选中目录
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns>filename</returns>
        private static string SelectObjectPathInfo(out string dirName)
        {
            dirName = string.Empty;
            if (Selection.activeInstanceID < 0)
            {
                return dirName;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            dirName = path;
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 自动创建图集文件 引用路径下sp
        /// </summary>
        /// <param name="folderName">目录名称</param>
        /// <param name="folderDir">目录路径</param>
        private static void AutoCreateAtlas(string folderName, string folderDir)
        {
            if (folderName == null || folderDir == null || folderName.Equals(string.Empty) ||
                folderDir.Equals(string.Empty))
            {
                Debug.LogError($"操作对象应为目录,当前未知");
                return;
            }

            //非法操作 在文件右键点了生成
            if (folderName.IndexOf(".", StringComparison.Ordinal) != -1)
            {
                Debug.LogError($"操作对象应为目录,当前为文件{folderName}");
                return;
            }

            var fixFolderDir = folderDir.Replace("\\", "/");
            //atlas所在完整目录
            var fullFolder = $"{fixFolderDir}";
            //不在预期路径下
            if (fullFolder.IndexOf(AtlasDef.AtlasRootFolder, StringComparison.Ordinal) == -1)
            {
                Debug.LogError($"操作目标应在{AtlasDef.AtlasRootFolder}下,当前为{fullFolder}");
                return;
            }

            //子目录数组
            var folderArray = Directory.GetDirectories(fullFolder);
            if (folderArray.Length > 0)
            {
                Debug.LogWarning($"{fullFolder}子目录的数量大于0,超出预期");
            }

            //资源所在目录
            var atlasPath = $"{fullFolder}/{folderName}.spriteatlas";
            //删除原图集
            if (File.Exists(atlasPath))
            {
                AssetDatabase.DeleteAsset(atlasPath);   
            }

            //创建新图集
            var atlas = CreateAtlas(atlasPath);
            //引用当前目录下png
            var dir = new DirectoryInfo(fullFolder);
            var files = dir.GetFiles("*.png");
            //GetFiles无序 会导致guid更变
            Array.Sort(files, (a, b) => a.CreationTime.Millisecond - b.CreationTime.Millisecond);
            foreach (var file in files)
            {
                atlas.Add(new Object[] {AssetDatabase.LoadAssetAtPath<Sprite>($"{fullFolder}/{file.Name}")});
            }

            //引用子目录
            foreach (var folder in folderArray)
            {
                var obj = AssetDatabase.LoadAssetAtPath(folder, typeof(Object));
                atlas.Add(new[] {obj});
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"atlas生成成功: {atlasPath}");
        }

        /// <summary>
        /// 创建图集
        /// </summary>
        /// <param name="atlasPath"></param>
        /// <returns></returns>
        private static SpriteAtlas CreateAtlas(string atlasPath)
        {
            var atlas = new SpriteAtlas();
            // 设置参数 可根据项目具体情况进行设置
            var packSetting = new SpriteAtlasPackingSettings
            {
                enableTightPacking = false,
                padding = 4,
                enableRotation = true
            };
            atlas.SetPackingSettings(packSetting);
            var textureSetting = new SpriteAtlasTextureSettings
            {
                readable = false,
                generateMipMaps = false,
                sRGB = true,
                filterMode = FilterMode.Bilinear
            };
            atlas.SetTextureSettings(textureSetting);
            //安卓设置
            var androidSetting = new TextureImporterPlatformSettings
            {
                name = "Android",
                maxTextureSize = 2048,
                format = TextureImporterFormat.ASTC_4x4,
                compressionQuality = 50,
                overridden = true
            };
            //ios设置
            var iosSetting = new TextureImporterPlatformSettings
            {
                name = "iPhone",
                maxTextureSize = 2048,
                format = TextureImporterFormat.ASTC_4x4,
                compressionQuality = 50,
                overridden = true
            };
            //PC
            var pcSetting = new TextureImporterPlatformSettings
            {
                name = "Standalone",
                maxTextureSize = 2048,
                format = TextureImporterFormat.DXT5,
                compressionQuality = 50,
                overridden = true
            };
            atlas.SetPlatformSettings(androidSetting);
            atlas.SetPlatformSettings(iosSetting);
            atlas.SetPlatformSettings(pcSetting);
            AssetDatabase.CreateAsset(atlas, atlasPath);
            return atlas;
        }
    }
}