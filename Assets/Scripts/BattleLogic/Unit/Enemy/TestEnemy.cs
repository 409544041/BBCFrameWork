// ******************************************************************
//       /\ /|       @file       TestEnemy
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-26 15:06
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

namespace Rabi
{
    public class TestEnemy : Unit
    {
        public override void OnInit()
        {
            base.OnInit();
            var movingState = new UnitMovingState(this);
            var idleState = new UnitIdleState(this);
            var onHitState = new UnitOnHitState(this);
            var onDieState = new UnitOnDieState(this);
            unitStateController.ResetStateList(new BaseUnitState[] {movingState, idleState, onHitState, onDieState});
            unitStateController.ChangeState<UnitIdleState>();
        }

        protected override void OnCreateComps()
        {
            base.OnCreateComps();
            var aiSingleComp = new AISingleComp();
            aiSingleComp.SetData(this);
            aiSingleComp.CloneSingleAI(EnumAI.TestAI.GetHashCode());
            compDict.Add(typeof(AISingleComp), aiSingleComp); //AI
        }
    }
}