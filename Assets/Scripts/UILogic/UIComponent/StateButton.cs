// ******************************************************************
//       /\ /|       @file       StateButton.cs
//       \ V/        @brief      状态按钮组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-01-05 16:32:15
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace Rabi
{
    public class StateButton : Button
    {
        public bool IsSelect
        {
            get => isSelect;
            set
            {
                isSelect = value;
                targetGraphic = isSelect ? selectedObjGraphic : notSelectedObjGraphic;
                Refresh();
            }
        }

        [SerializeField] private bool isSelect; //按钮是否被选中
        public GameObject isSelectObj; //按钮选中状态下显示的物体
        public GameObject isNotSelectObj; //未选中状态下显示的物体
        public Graphic selectedObjGraphic; //选中时使用的图形
        public Graphic notSelectedObjGraphic; //未选中时使用的图形

        protected override void Awake()
        {
            base.Awake();
            if (selectedObjGraphic == null)
            {
                selectedObjGraphic = targetGraphic;
            }

            if (notSelectedObjGraphic == null)
            {
                notSelectedObjGraphic = targetGraphic;
            }

            targetGraphic = isSelect ? selectedObjGraphic : notSelectedObjGraphic;
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="str"></param>
        public void SetTitle(string str)
        {
            var compTexts = GetComponentsInChildren<Text>();
            foreach (var compText in compTexts)
            {
                compText.text = str;
            }
        }

        /// <summary>
        /// 刷新按钮状态
        /// </summary>
        private void Refresh()
        {
            if (isSelectObj != null)
            {
                isSelectObj.SetActive(isSelect);
            }

            if (isNotSelectObj != null)
            {
                isNotSelectObj.SetActive(!isSelect);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 参数改变回调
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();
            Refresh();
        }
#endif
    }
}