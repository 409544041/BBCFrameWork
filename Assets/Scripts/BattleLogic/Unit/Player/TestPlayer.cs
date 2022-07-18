// ******************************************************************
//       /\ /|       @file       TestPlayer
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-28 22:09
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class TestPlayer : Unit
    {
        public override void OnInit()
        {
            base.OnInit();
            var movingState = new UnitMovingState(this);
            var attackingState = new UnitAttackingState(this);
            var attackingState2 = new UnitAttackingState2(this);
            var castingState = new UnitCastingState(this);
            var idleState = new UnitIdleState(this);
            var jumpingState = new UnitJumpingState(this);
            var onHitState = new UnitOnHitState(this);
            var onDieState = new UnitOnDieState(this);
            unitStateController.ResetStateList(new BaseUnitState[]
            {
                movingState, attackingState, attackingState2, castingState, idleState, jumpingState, onHitState,
                onDieState
            });
        }

        protected override void OnCreateComps()
        {
            base.OnCreateComps();
            var moveControlComp = new MoveControlComp();
            moveControlComp.SetData(this);
            compDict.Add(typeof(MoveControlComp), moveControlComp); //控制
            var attackControlComp = new AttackControlComp();
            attackControlComp.SetData(this);
            compDict.Add(typeof(AttackControlComp), attackControlComp); //攻击
            var skillControlComp = new SkillControlComp();
            skillControlComp.SetData(this);
            compDict.Add(typeof(SkillControlComp), skillControlComp); //技能
        }
    }
}