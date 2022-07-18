// ******************************************************************
//       /\ /|       @file       AtlasManager.cs
//       \ V/        @brief      图集管理器
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-15 20:09:30
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace Rabi
{
    public sealed class AtlasManager : BaseSingleTon<AtlasManager>
    {
        /// <summary>
        /// 获取图集中的sp
        /// </summary>
        /// <param name="spritePath">sp的路径</param>
        /// <returns></returns>
        public static async Task<Sprite> GetSpriteFromAtlas(string spritePath)
        {
            var atlasPath = AtlasUtil.GetAtlasPath(spritePath);
            var spriteName = AtlasUtil.GetSpriteName(spritePath);
            var atlas = await AssetManager.Instance.LoadAssetAsync<SpriteAtlas>(atlasPath);
            if (atlas != null)
            {
                return atlas.GetSprite(spriteName);
            }

            Logger.Error($"图集不存在 spritePath:{atlasPath}");
            return null;
        }
    }
}