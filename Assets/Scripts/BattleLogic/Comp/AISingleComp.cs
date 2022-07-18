// ******************************************************************
//       /\ /|       @file       AISingleComp
//       \ V/        @brief      个体AI组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-07-09 20:28
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class AISingleComp : ThingComp
    {
        private BehaviourTree _behaviourTree;
        private bool _isEnable = true;

        public void CloneSingleAI(int id)
        {
            _behaviourTree = AIManager.Instance.CreateAIInstance(id, parent);
        }

        /// <summary>
        /// 设置是否启用
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnable(bool enable)
        {
            _isEnable = enable;
        }

        public override void OnUpdate()
        {
            if (!_isEnable)
            {
                return;
            }

            _behaviourTree?.Update();
        }
    }
}