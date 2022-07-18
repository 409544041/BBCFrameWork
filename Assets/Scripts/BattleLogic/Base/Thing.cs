// ******************************************************************
//       /\ /|       @file       Thing.cs
//       \ V/        @brief      所有场景中的物体基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-04-02 08:58:20
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Rabi
{
    public class Thing : MonoBehaviour
    {
        public virtual bool IsDie => false; //物体是否存活
        public EnumDirection dir = EnumDirection.East; //朝向

        /// <summary>
        /// 初始化回调
        /// </summary>
        public virtual void OnInit()
        {
        }

        /// <summary>
        /// 启用回调
        /// </summary>
        public virtual void OnSpawn()
        {
            gameObject.SetActive(true);
            transform.SetParent(null);
        }

        /// <summary>
        /// 禁用回调
        /// </summary>
        public virtual void OnRecycle()
        {
        }
    }
}