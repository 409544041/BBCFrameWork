// ******************************************************************
//       /\ /|       @file       UnitAttackingState2
//       \ V/        @brief      攻击状态 连段2
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 14:52
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using Slate;

namespace Rabi
{
    public class UnitAttackingState2 : BaseSequenceState
    {
        public UnitAttackingState2(Unit parent) : base(parent)
        {
        }

        protected override Cutscene OnGetSequence()
        {
            return AssetUtil.InstantiateSync<Cutscene>("Assets/AddressableAssets/Mix/Skill/TestUnitAttack2.prefab");
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入攻击状态2");
        }
    }
}