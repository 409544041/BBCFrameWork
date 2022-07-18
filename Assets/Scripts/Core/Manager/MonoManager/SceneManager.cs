// ******************************************************************
//       /\ /|       @file       SceneManager.cs
//       \ V/        @brief      场景管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-29 09:36:59
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Rabi
{
    public sealed class SceneManager : BaseSingleTon<SceneManager>, IMonoManager
    {
        private readonly HashSet<string> _loadingHashSet = new HashSet<string>(); //正在加载的场景
        private readonly HashSet<string> _unloadingHashSet = new HashSet<string>(); //正在卸载的场景

        public void OnInit()
        {
            Logger.Log("场景管理器初始化");
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

        public async void OnClear()
        {
            // foreach (var loadedScene in _loadedSceneDic.Where(loadedScene =>
            //     !_unloadingHashSet.Contains(loadedScene.Key)))
            // {
            //     await UnloadScene(loadedScene.Key);
            // }
            //
            // _loadingHashSet.Clear();
            // _unloadingHashSet.Clear();
            // _loadedSceneDic.Clear();
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        public async Task ChangeSceneAsync(string scenePath)
        {
            // //single模式切换场景
            // var sceneInstance = await AssetManager.LoadSceneAsync(scenePath);
            // _loadingHashSet.Clear();
            // _unloadingHashSet.Clear();
            // _loadedSceneDic.Clear();
            // _loadedSceneDic.Add(scenePath, sceneInstance);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="scenePath"> 场景名称 </param>
        public async Task LoadSceneAsync(string scenePath)
        {
            // if (string.IsNullOrEmpty(scenePath))
            // {
            //     Logger.Error($"无效路径:{scenePath}");
            //     return;
            // }
            //
            // if (_unloadingHashSet.Contains(scenePath))
            // {
            //     Logger.Error($"场景正在卸载:{scenePath}");
            //     return;
            // }
            //
            // if (_loadingHashSet.Contains(scenePath))
            // {
            //     Logger.Error($"场景正在加载:{scenePath}");
            //     return;
            // }
            //
            // if (_loadedSceneDic.ContainsKey(scenePath))
            // {
            //     Logger.Error($"场景已经加载:{scenePath}");
            //     return;
            // }
            //
            // _loadingHashSet.Add(scenePath);
            // //叠加模式添加场景
            // var sceneInstance = await AssetManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            // _loadingHashSet.Remove(scenePath);
            // _loadedSceneDic.Add(scenePath, sceneInstance);
        }

        /// <summary>
        /// 卸载全部场景
        /// </summary>
        public async Task UnloadAllScenes()
        {
            // foreach (var sceneInstance in _loadedSceneDic.Values)
            // {
            //     await AssetManager.UnloadSceneAsync(sceneInstance);
            // }
            //
            // _loadedSceneDic.Clear();
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="scenePath"> 场景名称 </param>
        private async Task UnloadScene(string scenePath)
        {
            // if (string.IsNullOrEmpty(scenePath))
            // {
            //     Logger.Error($"无效路径：{scenePath}");
            //     return;
            // }
            //
            // if (_unloadingHashSet.Contains(scenePath))
            // {
            //     Logger.Error($"场景正在卸载:{scenePath}");
            //
            //     return;
            // }
            //
            // if (_loadingHashSet.Contains(scenePath))
            // {
            //     Logger.Error($"场景正在加载:{scenePath}");
            //     return;
            // }
            //
            // if (!_loadedSceneDic.ContainsKey(scenePath))
            // {
            //     Logger.Error($"场景没有被加载:{scenePath}");
            //     return;
            // }
            //
            // //标记卸载中
            // _unloadingHashSet.Add(scenePath);
            // //卸载场景
            // await AssetManager.UnloadSceneAsync(_loadedSceneDic[scenePath]);
            // if (!_unloadingHashSet.Contains(scenePath) || !_loadedSceneDic.ContainsKey(scenePath)) return;
            // _unloadingHashSet.Remove(scenePath);
            // _loadedSceneDic.Remove(scenePath);
        }
    }
}