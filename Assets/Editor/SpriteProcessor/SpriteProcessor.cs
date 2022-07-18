// ******************************************************************
//       /\ /|       @file       SpriteProcessor.cs
//       \ V/        @brief      自动设置某个路径下的导入纹理格式为sprite
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-02-24 11:47:30
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using UnityEditor;

namespace Rabi
{
    public class SpriteProcessor : AssetPostprocessor
    {
        private const string AtlasFolder = "Assets/AddressableAssets/Atlas"; //图集目录

        private void OnPreprocessTexture()
        {
            if (assetPath.IndexOf(AtlasFolder, StringComparison.Ordinal) == -1) return;
            //图集目录下的图片导入时自动转精灵格式
            var textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.alphaIsTransparency = true;
            textureImporter.mipmapEnabled = false;
        }
    }
}