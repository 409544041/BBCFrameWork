// ******************************************************************
//       /\ /|       @file       UnitOnDieState.cs
//       \ V/        @brief      单位死亡状态
//       | "")       @author     Shadowrabbit, yue.wang04@mihoyo.com
//       /  |                    
//      /  \\        @Modified   2022-06-28 10:20:27
//    *(__\_\        @Copyright  Copyright (c)  2022, Shadowrabbit
// ******************************************************************

using Slate;

namespace Rabi
{
    public class UnitOnDieState : BaseSequenceState
    {
        private AISingleComp _aiSingleComp; //个体AI组件

        public UnitOnDieState(Unit parent) : base(parent)
        {
        }

        protected override Cutscene OnGetSequence()
        {
            return AssetUtil.InstantiateSync<Cutscene>("Assets/AddressableAssets/Mix/Skill/TestUnitOnDie.prefab");
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入死亡状态");
            _aiSingleComp = owner.TryGetComp<AISingleComp>();
            _aiSingleComp?.SetEnable(false);
            //禁用碰撞效果
            rigidbody2D.simulated = false;
        }

        public override void OnExit()
        {
            //恢复碰撞效果
            rigidbody2D.simulated = true;
            _aiSingleComp?.SetEnable(true);
            base.OnExit();
        }

        protected override void OnFinishedCallback()
        {
            //通知战场管理器清理尸体和血条
            EventManager.Instance.Dispatch(EventId.OnUnitDie, owner);
        }
    }
}