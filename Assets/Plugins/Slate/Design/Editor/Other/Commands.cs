#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Slate
{
    public static class Commands
    {
        [MenuItem("Rabi/Slate技能编辑 %&M", priority = -9999)]
        public static void OpenSlateSkill()
        {
            //测试场景
            EditorSceneManager.OpenScene($"Assets/Scenes/SkillEditor.unity");
            //布局
            EditorUtility.LoadWindowLayout($"Assets/Plugins/Slate/Asset/SlateSkill.wlt");
            //技能用的序列
            var cutscene = Cutscene.Create();
            cutscene.name = "SkillSeq";
            //虚拟施法者时间轴
            var actorGroup = cutscene.AddGroup<ActorGroup>();
            actorGroup.name = "Caster";
            actorGroup.AddTrack<ActorActionTrack>();
            actorGroup.AddTrack<AnimatorTrack>();
            actorGroup.AddTrack<ActionTrack>();
            //清除全局组
            cutscene.directorGroup.ClearTrack();
            CutsceneEditor.ShowWindow(null);
        }

        [MenuItem("Rabi/Slate/Open SLATE Editor", false, 0)]
        public static void OpenDirectorWindow()
        {
            CutsceneEditor.ShowWindow(null);
        }

        [MenuItem("Rabi/Slate/Create New Cutscene", false, 0)]
        public static Cutscene CreateCutscene()
        {
            var cutscene = Cutscene.Create();
            CutsceneEditor.ShowWindow(cutscene);
            Selection.activeObject = cutscene;
            return cutscene;
        }

        [MenuItem("Rabi/Slate/Visit Website")]
        public static void VisitWebsite()
        {
            Help.BrowseURL("http://slate.paradoxnotion.com");
        }

        [MenuItem("Rabi/Slate/Extra/Create Shot Camera")]
        public static ShotCamera CreateShot()
        {
            var shot = ShotCamera.Create();
            Selection.activeObject = shot;
            return shot;
        }

        [MenuItem("Rabi/Slate/Extra/Create Bezier Path")]
        public static Path CreateBezierPath()
        {
            var path = BezierPath.Create();
            Selection.activeObject = path;
            return path;
        }

        [MenuItem("Rabi/Slate/Extra/Create Cutscene Starter")]
        public static GameObject CreateCutsceneStartPlayer()
        {
            var go = PlayCutsceneOnStart.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Rabi/Slate/Extra/Create Cutscene Zone Trigger")]
        public static GameObject CreateCutsceneTriggerPlayer()
        {
            var go = PlayCutsceneOnTrigger.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Rabi/Slate/Extra/Create Cutscene Click Trigger")]
        public static GameObject CreateCutsceneClickPlayer()
        {
            var go = PlayCutsceneOnClick.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Rabi/Slate/Extra/Create Cutscenes Sequence Player")]
        public static GameObject CreateCutscenesSequencePlayer()
        {
            var go = CutsceneSequencePlayer.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }
    }
}

#endif