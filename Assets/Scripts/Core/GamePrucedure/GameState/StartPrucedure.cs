// ******************************************************************
//       /\ /|       @file       StartPrucedure.cs
//       \ V/        @brief      启动游戏流程
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-25 12:27:05
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class StartPrucedure : BaseGamePrucedure
    {
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            EventManager.Instance.AddListener(EventId.OnLogoComplete, OnLogoComplete);
            //logo展示
            //UIManager.Instance.OpenWindow("Logo");
        }

        public override void OnExit()
        {
            EventManager.Instance.RemoveListener(EventId.OnLogoComplete, OnLogoComplete);
            base.OnExit();
        }

        /// <summary>
        /// lua层页面传来
        /// </summary>
        private static void OnLogoComplete()
        {
            //读取设置
            SaveManager.Instance.LoadSetting();
            EventManager.Instance.Dispatch(EventId.OnLanguageChanged);
            //todo 游戏存档读取
            //todo 输入设备识别
            //todo 键位配置映射
            //todo 预加载
            //进入下一个状态
            FsmManager.Instance.ChangeState<MainMenuPrucedure>(FsmDef.GamePrucedure);
        }
    }
}