// ******************************************************************
//       /\ /|       @file       UnitOnHitState
//       \ V/        @brief      单位受击状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-18 11:45
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using Slate;

namespace Rabi
{
    public class UnitOnHitState : BaseSequenceState
    {
        private AISingleComp _aiSingleComp; //个体AI组件

        public UnitOnHitState(Unit parent) : base(parent)
        {
        }

        protected override Cutscene OnGetSequence()
        {
            return AssetUtil.InstantiateSync<Cutscene>("Assets/AddressableAssets/Mix/Skill/TestUnitHurt.prefab");
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入受击状态");
            _aiSingleComp = owner.TryGetComp<AISingleComp>();
            _aiSingleComp?.SetEnable(false);
        }

        public override void OnExit()
        {
            _aiSingleComp?.SetEnable(true);
            base.OnExit();
        }
    }
}