#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Linq;
using Spine.Unity;

namespace Slate{

	///Used for a sorting layer popup
	public class SpineAnimationAttribute : PropertyAttribute { }

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(SpineAnimationAttribute))]
	public class SpineAnimationAttributeDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label){ return -2; }
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent content){
			
			if (prop.propertyType != SerializedPropertyType.String){
				GUILayout.Label("Use [SpineAnimation] attribute with a string");
				return;
			}

			var directable = prop.serializedObject.targetObject as IDirectable;
			if (directable != null && directable.actor != null){
				var iSkel = directable.actor.GetComponent(typeof(ISkeletonComponent)) as ISkeletonComponent;
				if (iSkel != null){
					var skeletonDataAsset = iSkel.SkeletonDataAsset;
					if (skeletonDataAsset != null){
						var animations = skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations;
						var animNames = animations.Items.Select(i => i.Name).ToList();
						prop.stringValue = EditorTools.Popup<string>(content.text, prop.stringValue, animNames);
						return;
					}
				}
			}

			prop.stringValue = EditorGUILayout.TextField(content.text, prop.stringValue);
		}

	}
#endif

}