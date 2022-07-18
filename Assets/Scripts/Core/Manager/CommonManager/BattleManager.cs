// ******************************************************************
//       /\ /|       @file       BattleManager
//       \ V/        @brief      战场管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-26 11:07
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Rabi
{
    public class BattleManager : BaseSingleTon<BattleManager>
    {
        public int? currentLevelId; //当前关卡id
        public Unit player; //玩家
        public List<Unit> enemyList = new List<Unit>(); //敌人

        public async Task OnEnter(int? levelId)
        {
            Logger.Log($"战场管理器初始化 levelId:{levelId}");
            currentLevelId = levelId;
            //当前进入的不是战斗关卡
            if (currentLevelId == null)
            {
                return;
            }

            //预加载战场
            await Preload(levelId);
        }

        public void OnExit()
        {
            //当前切换的不是战斗关卡 战斗关卡已经卸载过了
            if (currentLevelId == null)
            {
                return;
            }

            Release();
            currentLevelId = null;
        }

        /// <summary>
        /// 预加载
        /// </summary>
        private async Task Preload(int? levelId)
        {
            //没有需要预加载的配置
            if (levelId == null)
            {
                return;
            }

            // //主角
            // player = await AssetUtil.InstantiateAsync<TestPlayer>(
            //     "Assets/AddressableAssets/Mix/Character/Player/TestPlayer.prefab");
            // var testEnemy1 = await AssetUtil.InstantiateAsync<TestEnemy>(
            //     "Assets/AddressableAssets/Mix/Character/Enemy1/TestEnemy.prefab");
            // testEnemy1.transform.position = Vector3.right;
            // var testEnemy2 = await AssetUtil.InstantiateAsync<TestEnemy>(
            //     "Assets/AddressableAssets/Mix/Character/Enemy1/TestEnemy.prefab");
            // testEnemy2.transform.position = Vector3.left;
            // enemyList.Add(testEnemy1);
            // enemyList.Add(testEnemy2);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        private void Release()
        {
            AssetUtil.Recycle(player);
            foreach (var enemy in enemyList)
            {
                AssetUtil.Recycle(enemy);
            }

            enemyList.Clear();
        }
    }
}