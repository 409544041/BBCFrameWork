// ******************************************************************
//       /\ /|       @file       ChangeScenePrucedure.cs
//       \ V/        @brief      切换场景流程 Loading
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-27 01:31:01
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Rabi
{
    public class ChangeScenePrucedure : BaseGamePrucedure
    {
        private const float MinLoadingTime = 2f; //最小加载时间

        public override async void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            if (args.Length < 2)
            {
                Logger.Error($"场景切换参数数量缺失 需要scenePath nextPrucedureType");
                return;
            }

            if (args[0] is not string scenePath)
            {
                Logger.Error($"scenePath参数错误 期望:string 实际:{args[0].GetType()}");
                return;
            }

            if (args[1] is not Type nextPrucedureType)
            {
                Logger.Error($"nextPrucedureType参数错误 期望:Type 实际:{args[1].GetType()}");
                return;
            }

            //当前场景的战斗关卡id
            int? levelId = null;
            if (args.Length >= 3)
            {
                levelId = Convert.ToInt32(args[2]);
            }

            AudioManager.Instance.OnClear();
            //UIManager.Instance.CloseAllWindows();
            //UIManager.Instance.OpenWindow("Loading");
            //最低加载时间
            var startTime = Time.realtimeSinceStartup;
            //退出当前战场
            BattleManager.Instance.OnExit();
            //存档
            SaveManager.Instance.SavePlayerData();
            //切换场景
            await SceneManager.Instance.ChangeSceneAsync(scenePath);
            var endTime = Time.realtimeSinceStartup;
            var costTime = endTime - startTime;
            //低于最小耗时 补一点时间
            if (costTime < MinLoadingTime)
            {
                await Task.Delay((int)(MinLoadingTime - costTime) * 1000);
            }

            OnSceneUnloadComplete();
            //把UI相机叠加到当前场景的主相机上
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(GameManager.Instance.uiCamera);
            //关卡场景的情况 需要预加载和初始化
            await BattleManager.Instance.OnEnter(levelId);
            OnSceneLoadComplete(nextPrucedureType);
        }

        /// <summary>
        /// 场景加载完成
        /// </summary>
        /// <param name="nextPrucedureType"></param>
        private static void OnSceneLoadComplete(Type nextPrucedureType)
        {
            Logger.Log($"场景加载完成");
            FsmManager.Instance.ChangeState(FsmDef.GamePrucedure, nextPrucedureType);
        }

        /// <summary>
        /// 场景加载完成
        /// </summary>
        private static void OnSceneUnloadComplete()
        {
            Logger.Log($"全部场景卸载完成");
            AssetManager.ClearUnused();
        }
    }
}