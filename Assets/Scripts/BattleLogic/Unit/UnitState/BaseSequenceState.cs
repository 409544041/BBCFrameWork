// ******************************************************************
//       /\ /|       @file       BaseSequenceState
//       \ V/        @brief      序列型状态
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-06-19 13:44
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using Slate;
using UnityEngine;

namespace Rabi
{
    public class BaseSequenceState : BaseUnitState
    {
        protected Rigidbody2D rigidbody2D; //刚体组件
        protected Cutscene cutscene; //序列
        private const string CasterFieldName = "Caster";

        protected BaseSequenceState(Unit parent) : base(parent)
        {
        }

        /// <summary>
        /// 获取序列回调
        /// </summary>
        protected virtual Cutscene OnGetSequence()
        {
            return null;
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            //释放技能时关闭物理效果 否则Y轴抖动会影响血条位置
            rigidbody2D = owner.GetComponent<Rigidbody2D>();
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            cutscene = OnGetSequence();
            cutscene.SetGroupActorOfName(CasterFieldName, owner.gameObject);
            cutscene.OnStop += OnFinishedCallback;
            cutscene.Play();
        }

        public override void OnExit()
        {
            if (cutscene == null)
            {
                Logger.Error($"{GetType()}的序列为空");
                return;
            }

            //先移除完成监听
            cutscene.OnStop -= OnFinishedCallback;
            //进度回退0 保留当前序列的修改
            cutscene.RewindNoUndo();
            //回收序列等待复用
            AssetUtil.Recycle(cutscene.gameObject);
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        /// <summary>
        /// 播放结束回调
        /// </summary>
        protected virtual void OnFinishedCallback()
        {
            unitStateController.ChangeState<UnitIdleState>();
        }
    }
}