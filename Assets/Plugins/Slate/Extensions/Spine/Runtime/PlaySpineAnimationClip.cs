using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;

namespace Slate.ActionClips
{

    [Name("Spine Animation Clip")]
    [Description("Spine Animation Clip can play a specific spine animation.")]
    [Attachable(typeof(SpineAnimationTrack))]
    public class PlaySpineAnimationClip : ActorActionClip<SkeletonAnimation>, ISubClipContainable
    {

        [Header("Animation")]
        [Slate.SpineAnimation]
        public string animationName;
        [Range(0.1f, 2)]
        public float speed = 1;
        public float clipOffset;

        [Header("Sorting")]
        public bool changeSorting = false;
        [SortingLayer]
        public int sortingLayerID;
        public int sortingOrder;

        [System.NonSerialized]
        private Renderer myRenderer;
        [System.NonSerialized]
        private int wasSortingLayer;
        [System.NonSerialized]
        private int wasSortingOrder;


        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.2f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.2f;

        float ISubClipContainable.subClipOffset {
            get { return clipOffset; }
            set { clipOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get
            {
                try { return actor.skeleton.Data.FindAnimation(animationName).Duration; }
                catch { return this.length; }
            }
        }

        float ISubClipContainable.subClipSpeed {
            get { return speed; }
        }

        public override string info {
            get { return !string.IsNullOrEmpty(animationName) ? animationName : "NONE"; }
        }

        public override bool isValid {
            get { return base.isValid && !string.IsNullOrEmpty(animationName); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        public override bool canCrossBlend {
            get { return true; }
        }

        public Spine.Animation clipAnimation { get; private set; }
        private SpineAnimationTrack track { get { return (SpineAnimationTrack)parent; } }

        protected override void OnEnter() { StoreSet(); track.EnableClip(this); }
        protected override void OnReverseEnter() { StoreSet(); track.EnableClip(this); }

        protected override void OnUpdate(float time, float previousTime) {
            if ( clipAnimation != null ) {
                var scaledTime = ( time - clipOffset ) * speed;
                scaledTime = Mathf.Repeat(scaledTime, clipAnimation.Duration);
                track.UpdateClip(this, scaledTime, previousTime, GetClipWeight(time));
                if ( changeSorting ) {
                    myRenderer.sortingLayerID = sortingLayerID;
                    myRenderer.sortingOrder = sortingOrder;
                }
            }
        }

        protected override void OnReverse() { Restore(); track.DisableClip(this); }
        protected override void OnExit() { Restore(); track.DisableClip(this); }

        void StoreSet() {
            clipAnimation = actor.state.Data.SkeletonData.FindAnimation(animationName);
            myRenderer = actor.GetComponent<Renderer>();
            wasSortingLayer = myRenderer.sortingLayerID;
            wasSortingOrder = myRenderer.sortingOrder;
        }

        void Restore() {
            myRenderer = actor.GetComponent<Renderer>();
            myRenderer.sortingLayerID = wasSortingLayer;
            myRenderer.sortingOrder = wasSortingOrder;
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            if ( isValid ) {
                actor.Initialize(false);
                var clipAnimation = actor.skeleton.Data.FindAnimation(animationName);
                if ( clipAnimation != null ) {
                    EditorTools.DrawLoopedLines(rect, clipAnimation.Duration / speed, this.length, clipOffset);
                }
            }
        }

#endif
    }
}