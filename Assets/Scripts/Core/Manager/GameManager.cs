// ******************************************************************
//       /\ /|       @file       GameManager.cs
//       \ V/        @brief      游戏控制器
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-03-17 20:08:55
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace Rabi
{
    public class GameManager : BaseMonoSingleTon<GameManager>
    {
        [SerializeField] [FieldName("正式流程")] private bool autoStart;
        private readonly List<IMonoManager> _managerList = new List<IMonoManager>(); //管理器列表
        [HideInInspector] public Camera uiCamera; //UI相机
        private static GamePrucedureController _fsm; //游戏流程状态机

        private void Awake()
        {
            var uiCameraObj = GameObject.Find("UICamera") ?? new GameObject("UICamera");
            uiCamera = uiCameraObj.GetOrAddComponent<Camera>();
            _managerList.Add(AssetManager.Instance);
            _managerList.Add(SceneManager.Instance);
            _managerList.Add(ObjectPoolManager.Instance);
            _managerList.Add(FsmManager.Instance);
            _managerList.Add(AudioManager.Instance);
            _managerList.Add(InputManager.Instance);
            foreach (var manager in _managerList)
            {
                manager.OnInit();
            }

            //加载AI配置
            AIManager.Instance.LoadAIProto();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            InitPrucedure();
            if (autoStart)
            {
                _fsm.ChangeState<StartPrucedure>();
            }
        }

        private void Update()
        {
            foreach (var manager in _managerList)
            {
                manager.Update();
            }
        }

        private void FixedUpdate()
        {
            foreach (var manager in _managerList)
            {
                manager.FixedUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach (var manager in _managerList)
            {
                manager.LateUpdate();
            }
        }

        private void OnDestroy()
        {
            for (var i = _managerList.Count - 1; i >= 0; i--)
            {
                _managerList[i].OnClear();
            }
        }

        /// <summary>
        /// 初始化游戏流程
        /// </summary>
        private static void InitPrucedure()
        {
            _fsm = FsmManager.Instance.GetFsm<GamePrucedureController>(FsmDef.GamePrucedure);
            var startPrucedure = new StartPrucedure(); //启动游戏流程
            var mainMenuPrucedure = new MainMenuPrucedure(); //主菜单流程
            var changeScenePrucedure = new ChangeScenePrucedure(); //切换场景流程
            _fsm.ResetStateList(
                new List<IFsmState>
                {
                    startPrucedure, mainMenuPrucedure, changeScenePrucedure
                });
        }
    }
}