using UnityEngine;

namespace Slate.ActionClips
{
    [Category("Transform")]
    [Description("将物体附加到演员上,中断或结束时回收物体")]
    public class AttachObject : ActorActionClip
    {
        [SerializeField] [HideInInspector] private float _length = 1f;

        [Required] public Transform targetObject;
        public string childTransformName;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale = Vector3.one;

        private TransformSnapshot _snapshot;
        private bool _temporary;

        public override bool isValid => targetObject != null;

        public override float length
        {
            get => _length;
            set => _length = value;
        }

        protected override void OnEnter()
        {
            _temporary = length > 0;
            Do();
        }

        protected override void OnReverseEnter()
        {
            if (_temporary)
            {
                Do();
            }
        }

        protected override void OnExit()
        {
            if (_temporary)
            {
                UnDo();
            }
        }

        protected override void OnReverse()
        {
            UnDo();
        }

        private void Do()
        {
            _snapshot = new TransformSnapshot(targetObject.gameObject, TransformSnapshot.StoreMode.RootOnly);
            var finalTransform = actor.transform.FindInChildren(childTransformName, true);
            if (finalTransform == null)
            {
                Debug.LogError(
                    $"Child Transform with name '{childTransformName}', can't be found on actor '{actor.name}' hierarchy",
                    actor.gameObject);
                return;
            }

            targetObject.SetParent(finalTransform);
            targetObject.localPosition = localPosition;
            targetObject.localEulerAngles = localRotation;
            targetObject.localScale = localScale;
        }

        private void UnDo()
        {
            _snapshot.Restore();
        }
    }
}