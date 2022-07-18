// ******************************************************************
//       /\ /|       @file       AssetUtil.cs
//       \ V/        @brief      资源工具
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-18 22:52:37
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rabi
{
    public static class AssetUtil
    {
        /// <summary>
        /// 异步实例化 回调型
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="callback"></param>
        /// <param name="assetLoadType">资源分类</param>
        /// <returns></returns>
        public static async void InstantiateAsync(string assetPath, Action<GameObject> callback,
            AssetLoadType assetLoadType = AssetLoadType.Temp)
        {
            //尝试从池中获取
            var objName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn(objName);
            if (ins != null)
            {
                callback(ins);
                return;
            }

            //异步加载并实例化
            var obj = await AssetManager.Instance.InstantiateAsync<GameObject>(assetPath, assetLoadType);
            obj.name = objName;
            callback(obj);
        }

        /// <summary>
        /// 异步实例化 阻塞型
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <returns></returns>
        public static async Task<GameObject> InstantiateAsync(string assetPath)
        {
            //尝试从池中获取
            var objName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn(objName);
            if (ins != null) return ins;
            //异步加载并实例化
            var obj = await AssetManager.Instance.InstantiateAsync<GameObject>(assetPath);
            obj.name = objName;
            return ins;
        }

        /// <summary>
        /// 同步实例化
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetLoadType">资源分类</param>
        /// <returns></returns>
        public static GameObject InstantiateSync(string assetPath, AssetLoadType assetLoadType = AssetLoadType.Temp)
        {
            //尝试从池中获取
            var objName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn(objName);
            if (ins != null)
            {
                return ins;
            }

            //异步加载并实例化
            ins = AssetManager.Instance.InstantiateSync<GameObject>(assetPath, assetLoadType);
            ins.name = objName;
            return ins;
        }

        /// <summary>
        /// 根据原型体同步实例化
        /// </summary>
        /// <param name="proto">原型体</param>
        /// <param name="assetLoadType">资源分类</param>
        /// <returns></returns>
        public static GameObject InstantiateSync(GameObject proto, AssetLoadType assetLoadType = AssetLoadType.Temp)
        {
            //尝试从池中获取
            var ins = ObjectPoolManager.Instance.Spawn(proto.name);
            if (ins != null)
            {
                return ins;
            }

            //异步加载并实例化
            ins = Object.Instantiate(proto);
            return ins;
        }

        /// <summary>
        /// 同步获取实例
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public static T InstantiateSync<T>(string assetPath)
        {
            //尝试从池中获取
            var objName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn<T>(objName);
            if (ins != null) return ins;
            //同步加载并实例化
            var obj = AssetManager.Instance.InstantiateSync<GameObject>(assetPath);
            obj.name = objName;
            ins = obj.GetComponent<T>();
            if (ins is Thing thing)
            {
                thing.OnInit();
                thing.OnSpawn();
            }

            if (ins == null) Logger.Error($"找不到组件obj:{obj.name} comp:{typeof(T)}");
            return ins;
        }

        /// <summary>
        /// 异步获取实例
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public static async Task<T> InstantiateAsync<T>(string assetPath)
        {
            //尝试从池中获取
            var objName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn<T>(objName);
            if (ins != null) return ins;
            //异步加载并实例化
            var obj = await AssetManager.Instance.InstantiateAsync<GameObject>(assetPath);
            obj.name = objName;
            ins = obj.GetComponent<T>();
            if (ins is Thing thing)
            {
                thing.OnInit();
                thing.OnSpawn();
            }

            if (ins == null) Logger.Error($"找不到组件obj:{obj.name} comp:{typeof(T)}");
            return ins;
        }

        /// <summary>
        /// 回收实例
        /// </summary>
        /// <param name="poolObj">待回收对象</param>
        public static void Recycle(GameObject poolObj)
        {
            ObjectPoolManager.Instance.Recycle(poolObj);
        }

        /// <summary>
        /// 回收实例
        /// </summary>
        /// <param name="thing"></param>
        public static void Recycle(Thing thing)
        {
            ObjectPoolManager.Instance.Recycle(thing);
        }

        /// <summary>
        /// 根据路径异步加载sp后返回
        /// </summary>
        /// <param name="spritePath"></param>
        /// <returns></returns>
        public static async Task<Sprite> LoadSpriteAsync(string spritePath)
        {
            var realSpritePath = spritePath.GetTransSpritePath();
            var isAtlasSprite = realSpritePath.IndexOf(AtlasDef.AtlasRootFolder, StringComparison.Ordinal) != -1;
            var sp = isAtlasSprite
                ? await AssetManager.Instance.LoadAssetAsync<Sprite>(realSpritePath)
                : await AtlasManager.GetSpriteFromAtlas(realSpritePath);
            return sp;
        }
    }
}