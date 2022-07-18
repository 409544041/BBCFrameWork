// ******************************************************************
//       /\ /|       @file       ImageEx.cs
//       \ V/        @brief      Image扩展
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-18 12:50:23
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rabi
{
    public static class ImageEx
    {
        // /// <summary>
        // /// 异步加载sp 通过枚举名称
        // /// </summary>
        // /// <param name="image"></param>
        // /// <param name="enumName"></param>
        // public static void SetSpriteAsyncByEnumName(this Image image, string enumName)
        // {
        //     if (!Enum.TryParse<SpriteId>(enumName, out var enumSpriteId))
        //     {
        //         Logger.Error($"Sprite枚举解析失败spriteId:{enumName}");
        //         return;
        //     }
        //
        //     //通过枚举的方式加载图片
        //     image.SetSpriteAsync(enumSpriteId);
        // }
        //
        // /// <summary>
        // /// 异步加载sp 通过枚举
        // /// </summary>
        // /// <param name="image"></param>
        // /// <param name="spriteId"></param>
        // public static void SetSpriteAsync(this Image image, SpriteId spriteId)
        // {
        //     if (spriteId == SpriteId.None)
        //     {
        //         return;
        //     }
        //     
        //     var spritePath = ConfigManager.Instance.cfgSprite[spriteId].path;
        //     image.SetSpriteAsync(spritePath);
        // }

        /// <summary>
        /// 异步加载sp 通过id
        /// </summary>
        /// <param name="image"></param>
        /// <param name="spriteId"></param>
        public static void SetSpriteAsync(this Image image, int spriteId)
        {
            if (spriteId == 0)
            {
                return;
            }

            var spritePath = ConfigManager.Instance.cfgSprite[spriteId].path;
            image.SetSpriteAsync(spritePath);
        }

        /// <summary>
        /// 异步加载sp 通过完整路径
        /// </summary>
        /// <param name="image"></param>
        /// <param name="spritePath"></param>
        public static async void SetSpriteAsync(this Image image, string spritePath)
        {
            var sp = await AssetUtil.LoadSpriteAsync(spritePath);
            image.sprite = sp;
            //普通类别 设置原尺寸
            if (image.type == Image.Type.Simple)
            {
                image.SetNativeSize();
            }
        }
    }
}