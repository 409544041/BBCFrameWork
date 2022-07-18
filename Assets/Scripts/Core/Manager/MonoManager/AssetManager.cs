// ******************************************************************
//       /\ /|       @file       AssetManager.cs
//       \ V/        @brief      资源管理器
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-15 19:53:45
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Rabi
{
    public sealed class AssetManager : BaseSingleTon<AssetManager>, IMonoManager
    {
        private bool _hasInit; //已经完成初始化

        public void OnInit()
        {
            if (_hasInit)
            {
                return;
            }

            _hasInit = true;
            Logger.Log("资源管理器初始化");
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
        }

        public void OnClear()
        {
            // //加载中的 加载完成的 都卸载掉
            // foreach (var loadType in _loadTypeToHandleDict.Keys)
            // {
            //     ReleaseByLoadType(loadType);
            // }
            //
            // _loadTypeToHandleDict.Clear();
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="assetLoadType">资源分类</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public async Task<T> LoadAssetAsync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent)
            where T : Object
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     Logger.Error($"加载资源路径为空 path:{path}");
            //     return null;
            // }
            //
            // if (!_loadTypeToHandleDict.ContainsKey(assetLoadType))
            // {
            //     _loadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
            // }
            //
            // var dict = _loadTypeToHandleDict[assetLoadType];
            // var isRepeatLoad = dict.ContainsKey(path);
            // var handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
            // if (!isRepeatLoad)
            // {
            //     dict.Add(path, handle);
            // }
            //
            // //重复加载已完成
            // if (handle.IsDone) return handle.Result;
            // await handle.Task;
            // if (handle.Status == AsyncOperationStatus.Succeeded) return handle.Result;
            // //还在的话 移除
            // if (!dict.ContainsKey(path)) return null;
            // dict.Remove(path);
            // Addressables.Release(handle);
            return null;
        }

        /// <summary>
        /// 异步加载(单个资源)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径，如果为空则不会加载</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="assetLoadType">资源分类，临时资源和长期资源</param>
        public void LoadAssetAsync<T>(string path, Action<T> callBack,
            AssetLoadType assetLoadType = AssetLoadType.Permanent) where T : Object
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     Logger.Error($"加载资源路径为空 path:{path}");
            //     return;
            // }
            //
            // if (!_loadTypeToHandleDict.ContainsKey(assetLoadType))
            // {
            //     _loadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
            // }
            //
            // var dict = _loadTypeToHandleDict[assetLoadType];
            // var isRepeatLoad = dict.ContainsKey(path);
            // var handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
            // if (!isRepeatLoad)
            // {
            //     dict.Add(path, handle);
            // }
            //
            // //重复加载已完成
            // if (handle.IsDone) callBack?.Invoke(handle.Result);
            // var handle1 = handle;
            // handle.Completed += obj =>
            // {
            //     if (obj.Status == AsyncOperationStatus.Succeeded)
            //     {
            //         callBack?.Invoke(obj.Result);
            //         return;
            //     }
            //
            //     Logger.Error($"加载失败 path:{path}");
            //     if (!dict.ContainsKey(path)) return;
            //     dict.Remove(path);
            //     Addressables.Release(handle1);
            // };
        }

        /// <summary>
        /// 同步加载(单个资源)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径，如果为空则不会加载</param>
        /// <param name="assetLoadType">资源分类，临时资源和长期资源</param>
        public T LoadAssetSync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent) where T : Object
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     Logger.Error($"加载资源路径为空 path:{path}");
            //     return null;
            // }
            //
            // if (!_loadTypeToHandleDict.ContainsKey(assetLoadType))
            // {
            //     _loadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
            // }
            //
            // var dict = _loadTypeToHandleDict[assetLoadType];
            // var isRepeatLoad = dict.ContainsKey(path);
            // var handle = isRepeatLoad ? dict[path].Convert<T>() : Addressables.LoadAssetAsync<T>(path);
            // if (!isRepeatLoad)
            // {
            //     dict.Add(path, handle);
            // }
            //
            // var asset = handle.WaitForCompletion();
            // return asset;
            return null;
        }

        /// <summary>
        /// 释放单个资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="assetLoadType">资源分类，临时资源和长期资源</param>
        public void Release<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Permanent)
        {
            // if (string.IsNullOrEmpty(path))
            // {
            //     Logger.Error($"加载资源路径为空 path:{path}");
            //     return;
            // }
            //
            // if (!_loadTypeToHandleDict.ContainsKey(assetLoadType))
            // {
            //     _loadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
            // }
            //
            // var dict = _loadTypeToHandleDict[assetLoadType];
            // //释放句柄，并将这个键名从字典离移除
            // if (!dict.ContainsKey(path)) return;
            // var handle = dict[path].Convert<T>();
            // Addressables.Release(handle);
            // dict.Remove(path);
        }

        /// <summary>
        /// 释放某种加载类型的资源
        /// </summary>
        /// <param name="assetLoadType">资源分类</param>
        public void ReleaseByLoadType(AssetLoadType assetLoadType = AssetLoadType.Permanent)
        {
            // if (!_loadTypeToHandleDict.ContainsKey(assetLoadType))
            // {
            //     _loadTypeToHandleDict.Add(assetLoadType, new Dictionary<string, AsyncOperationHandle>());
            // }
            //
            // var dict = _loadTypeToHandleDict[assetLoadType];
            // foreach (var handle in dict.Values)
            // {
            //     Addressables.Release(handle);
            // }
            //
            // dict.Clear();
        }

        // /// <summary>
        // /// 异步加载场景
        // /// </summary>
        // /// <param name="path">场景路径</param>
        // /// <param name="loadMode"></param>
        // public static async Task<SceneInstance> LoadSceneAsync(string path,
        //     LoadSceneMode loadMode = LoadSceneMode.Single)
        // {
        //     if (string.IsNullOrEmpty(path))
        //     {
        //         Logger.Error($"加载资源路径为空 path:{path}");
        //         return default;
        //     }
        //
        //     var handle = Addressables.LoadSceneAsync(path, loadMode);
        //     await handle.Task;
        //     if (handle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         return handle.Result;
        //     }
        //
        //     Logger.Error($"场景加载失败 path:{path}");
        //     Addressables.Release(handle);
        //     return default;
        // }
        //
        // /// <summary>
        // /// 异步卸载场景
        // /// </summary>
        // /// <param name="scene"> 场景Instance引用 </param>
        // public static async Task UnloadSceneAsync(SceneInstance scene)
        // {
        //     var handle = Addressables.UnloadSceneAsync(scene);
        //     await handle.Task;
        //     if (handle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         Addressables.Release(handle);
        //         return;
        //     }
        //
        //     Logger.Error($"场景卸载失败 scene:{scene}");
        // }

        /// <summary>
        /// 异步实例化 优先考虑使用对象池实例化物体 而不是此函数
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="assetLoadType">资源分类</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public async Task<T> InstantiateAsync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Temp)
            where T : Object
        {
            var asset = await LoadAssetAsync<T>(path, assetLoadType);
            return asset == null ? null : Object.Instantiate(asset);
        }

        /// <summary>
        /// 同步实例化 优先考虑使用对象池实例化物体 而不是此函数
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="assetLoadType">资源分类</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public T InstantiateSync<T>(string path, AssetLoadType assetLoadType = AssetLoadType.Temp)
            where T : Object
        {
            var asset = LoadAssetSync<T>(path, assetLoadType);
            return asset == null ? null : Object.Instantiate(asset);
        }

        /// <summary>
        /// 释放未在使用的资源
        /// </summary>
        public static void ClearUnused()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}