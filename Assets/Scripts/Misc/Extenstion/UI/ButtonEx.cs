// ******************************************************************
//       /\ /|       @file       ButtonEx.cs
//       \ V/        @brief      按钮扩展
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-17 09:16:28
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine.UI;

namespace Rabi
{
    public static class ButtonEx
    {
        /// <summary>
        /// 根据多语言 更新按钮状态
        /// </summary>
        /// <param name="transButton"></param>
        public static async void UpdateSpriteState(this TransButton transButton)
        {
            //原状态
            var highlightedSprite = transButton.spriteState.highlightedSprite;
            var pressedSprite = transButton.spriteState.pressedSprite;
            var selectedSprite = transButton.spriteState.selectedSprite;
            var disabledSprite = transButton.spriteState.disabledSprite;
            //根据id和语言 替换状态
            if (transButton.highlightedTransId != 0)
            {
                var highlightedSpritePath = ConfigManager.Instance.cfgSprite[transButton.highlightedTransId].path;
                highlightedSprite = await AssetUtil.LoadSpriteAsync(highlightedSpritePath);
            }

            if (transButton.pressedTransId != 0)
            {
                var pressedSpritePath = ConfigManager.Instance.cfgSprite[transButton.pressedTransId].path;
                pressedSprite = await AssetUtil.LoadSpriteAsync(pressedSpritePath);
            }

            if (transButton.selectedTransId != 0)
            {
                var selectedSpritePath = ConfigManager.Instance.cfgSprite[transButton.selectedTransId].path;
                selectedSprite = await AssetUtil.LoadSpriteAsync(selectedSpritePath);
            }

            if (transButton.disabledTransId != 0)
            {
                var disabledSpritePath = ConfigManager.Instance.cfgSprite[transButton.disabledTransId].path;
                disabledSprite = await AssetUtil.LoadSpriteAsync(disabledSpritePath);
            }

            transButton.spriteState = new SpriteState
            {
                highlightedSprite = highlightedSprite,
                pressedSprite = pressedSprite,
                selectedSprite = selectedSprite,
                disabledSprite = disabledSprite
            };
        }
    }
}