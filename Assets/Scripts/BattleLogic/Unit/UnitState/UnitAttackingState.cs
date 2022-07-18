// ******************************************************************
//       /\ /|       @file       UnitAttackingState
//       \ V/        @brief      单位攻击状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:55
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using Slate;

namespace Rabi
{
    public class UnitAttackingState : BaseSequenceState
    {
        public UnitAttackingState(Unit parent) : base(parent)
        {
        }

        protected override Cutscene OnGetSequence()
        {
            return AssetUtil.InstantiateSync<Cutscene>("Assets/AddressableAssets/Mix/Skill/TestUnitAttack1.prefab");
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入攻击状态");
        }
    }
}