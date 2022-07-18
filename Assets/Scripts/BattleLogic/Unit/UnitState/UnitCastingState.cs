// ******************************************************************
//       /\ /|       @file       UnitCastingState
//       \ V/        @brief      单位施法状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-17 10:57
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using Slate;

namespace Rabi
{
    public class UnitCastingState : BaseSequenceState
    {
        public UnitCastingState(Unit parent) : base(parent)
        {
        }

        protected override Cutscene OnGetSequence()
        {
            return AssetUtil.InstantiateSync<Cutscene>("Assets/AddressableAssets/Mix/Skill/TestUnitSkill1.prefab");
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            Logger.Log($"{owner.name}进入技能状态");
        }
    }
}