using UnityEngine;

namespace Slate.ActionClips
{
    [Attachable(typeof(ActorActionTrack))]
    public abstract class ActorActionClip : ActionClip
    {
    }

    [Attachable(typeof(ActorActionTrack))]
    //The .actor property of the generic ActorActionClip version, returns the T argument component directly
    public abstract class ActorActionClip<T> : ActionClip where T : Component
    {
        private T _actorComponent;
        public new T actor
        {
            get
            {
                if (_actorComponent != null && _actorComponent.gameObject == base.actor)
                {
                    return _actorComponent;
                }

                return _actorComponent = base.actor != null ? base.actor.GetComponent<T>() : null;
                /*
                                if (_actorComponent != null && base.actor != null && _actorComponent.transform.IsChildOf(base.actor.transform)){
                                    return _actorComponent;
                                }
                                return _actorComponent = base.actor != null? base.actor.GetComponentInChildren<T>() : null;
                */
            }
        }

        public override bool isValid => actor != null;
    }
}