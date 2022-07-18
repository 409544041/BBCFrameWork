using UnityEngine;

namespace Slate
{
    [Description(
        "The DirectorGroup is the master group of the Cutscene. There is always one and can't be removed. The target actor of this group is always the 'Director Camera', thus it's possible to add a PropertiesTrack to animate any of the components attached on the DirectorCamera game object. Usualy Image Effects.")]
    public class DirectorGroup : CutsceneGroup
    {
        public override string name
        {
            get => "★ DIRECTOR";
            set { }
        }

        public override GameObject actor
        {
            get => DirectorCamera.current.gameObject;
            set { }
        }

        public override ActorReferenceMode referenceMode
        {
            get => ActorReferenceMode.UseOriginal;
            set { }
        }

        public override ActorInitialTransformation initialTransformation
        {
            get => ActorInitialTransformation.UseOriginal;
            set { }
        }

        public override Vector3 initialLocalPosition
        {
            get => Vector3.zero;
            set { }
        }

        public override Vector3 initialLocalRotation
        {
            get => Vector3.zero;
            set { }
        }

        public override bool displayVirtualMeshGizmo
        {
            get => false;
            set { }
        }
    }
}