using UnityEngine;
using System.Collections;
using System.Linq;
using Spine;
using Spine.Unity;
using PlaySpineAnimationClip = Slate.ActionClips.PlaySpineAnimationClip;

namespace Slate
{

    [Description("The Spine Animation Track works with the Spine's 'SkeletonAnimation' component.")]
    [Icon("SpineIcon")]
    [Category("Extensions")]
    [Attachable(typeof(ActorGroup))]
    public class SpineAnimationTrack : CutsceneTrack
    {

        [SerializeField]
        [Range(0f, 1f)]
        private float _weight = 1f;

        [Header("Base Animation")]
        [Slate.SpineAnimation]
        public string baseAnimationName;
        [Range(0.1f, 2f)]
        public float speed = 1;

        [Header("Sorting")]
        public bool changeSorting = false;
        [SortingLayer]
        public int sortingLayerID;
        public int sortingOrder;

        [System.NonSerialized]
        private SkeletonAnimation spineComponent;
        [System.NonSerialized]
        private Spine.Skeleton skeleton;
        [System.NonSerialized]
        private Spine.Animation baseAnimation;
        [System.NonSerialized]
        private Renderer myRenderer;

        [System.NonSerialized]
        private int activeClips = 0;
        [System.NonSerialized]
        private float baseWeight = 1;

        //For store value before entering
        [System.NonSerialized]
        private string wasAnimationName;
        [System.NonSerialized]
        private int wasSortingLayer;
        [System.NonSerialized]
        private int wasSortingOrder;

        //TODO: Remove this after update fix
        [System.NonSerialized]
        private int lastPoseCallFrame;
        //--

        public override string info {
            get
            {
                var baseString = string.Format("Base Clip: {0}", ( string.IsNullOrEmpty(baseAnimationName) ? "NONE" : baseAnimationName ));
                var weightString = string.Format("Weight: {0}", _weight.ToString());
                return string.Format("{0} | {1}", baseString, weightString);
            }
        }

        protected override bool OnInitialize() {
            spineComponent = actor.GetComponent<SkeletonAnimation>();
            if ( spineComponent == null ) {
                Debug.LogError("The Spine Animation Track requires the actor to have a 'SkeletonAnimation' derived Component attached", actor);
                return false;
            }

            if ( !spineComponent.valid ) {
                Debug.LogError("The SpineAnimation Track requires the SkeletonAnimation to be Valid!", actor);
                return false;
            }

            return true;
        }

        protected override void OnEnter() {
            spineComponent = actor.GetComponent<SkeletonAnimation>();
            if ( spineComponent == null ) {
                return;
            }

            spineComponent.Initialize(false);
            if ( !spineComponent.valid ) {
                spineComponent = null;
                return;
            }

            StoreSet();
        }

        protected override void OnReverseEnter() { StoreSet(); }
        protected override void OnExit() { Restore(); }
        protected override void OnReverse() { Restore(); }

        protected override void OnUpdate(float time, float previousTime) {
            if ( spineComponent == null ) {
                return;
            }


            //TODO: Add this after update fix
            // if ( layerOrder == 0 ) {
            //     skeleton.SetToSetupPose();
            // }
            //--

            //TODO: Remove this after update fix
            if ( layerOrder == 0 && lastPoseCallFrame != Time.frameCount ) {
                lastPoseCallFrame = Time.frameCount;
                skeleton.SetToSetupPose();
            }
            //--


            if ( baseAnimation != null ) {
                var scaledTime = time * speed;
                scaledTime = Mathf.Repeat(scaledTime, baseAnimation.Duration);
                baseAnimation.Apply(skeleton, 0, scaledTime, true, null, baseWeight * this._weight, MixBlend.First, MixDirection.In);

            }

            if ( activeClips == 0 && changeSorting ) {
                myRenderer.sortingLayerID = sortingLayerID;
                myRenderer.sortingOrder = sortingOrder;
            }
        }

        public void EnableClip(PlaySpineAnimationClip clip) { activeClips++; }
        public void DisableClip(PlaySpineAnimationClip clip) { activeClips--; }
        public void UpdateClip(PlaySpineAnimationClip clip, float time, float previousTime, float weight) {
            if ( spineComponent != null ) {
                baseWeight = activeClips == 2 ? 0 : 1 - weight;


                // //TODO: Remove this after update fix
                if ( layerOrder == 0 && lastPoseCallFrame != Time.frameCount ) {
                    lastPoseCallFrame = Time.frameCount;
                    skeleton.SetToSetupPose();
                }
                //--


                clip.clipAnimation.Apply(skeleton, 0, time, true, null, weight * this._weight, MixBlend.Replace, MixDirection.In);
            }
        }


        void StoreSet() {

            if ( spineComponent == null ) {
                return;
            }

            myRenderer = spineComponent.GetComponent<Renderer>();
            wasSortingLayer = myRenderer.sortingLayerID;
            wasSortingOrder = myRenderer.sortingOrder;

            skeleton = spineComponent.skeleton;
            baseWeight = 1f;
            activeClips = 0;

            if ( changeSorting ) {
                myRenderer.sortingLayerID = sortingLayerID;
                myRenderer.sortingOrder = sortingOrder;
            }


            var skeletonAnimation = (SkeletonAnimation)spineComponent;
            wasAnimationName = skeletonAnimation.AnimationName;
            skeletonAnimation.AnimationName = string.Empty;
            baseAnimation = !string.IsNullOrEmpty(baseAnimationName) ? skeletonAnimation.state.Data.SkeletonData.FindAnimation(baseAnimationName) : null;
        }

        void Restore() {

            if ( spineComponent == null ) {
                return;
            }

            myRenderer = spineComponent.GetComponent<Renderer>();
            myRenderer.sortingLayerID = wasSortingLayer;
            myRenderer.sortingOrder = wasSortingOrder;
            if ( Application.isPlaying ) {

                spineComponent.AnimationName = wasAnimationName;

            } else {

                spineComponent.skeleton.SetToSetupPose();
            }
        }
    }
}