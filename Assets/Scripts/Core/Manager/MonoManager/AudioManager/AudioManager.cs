// ******************************************************************
//       /\ /|       @file       AudioManager
//       \ V/        @brief      音频管理器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-24 20:25
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rabi
{
    public sealed class AudioManager : BaseSingleTon<AudioManager>, IMonoManager
    {
        //使用中的音效 按类别分组 每组k是id v是音效资源
        private readonly Dictionary<int, Dictionary<int, AudioSource>>
            _audioSourceMap = new Dictionary<int, Dictionary<int, AudioSource>>(); //当前使用中的音频

        private readonly Dictionary<int, Transform> _enumAudioType2Layer =
            new Dictionary<int, Transform>(); //层级挂点

        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
            var root = new GameObject("AudioManager");
            root.transform.SetParent(GameManager.Instance.transform, false);
            foreach (int value in System.Enum.GetValues(typeof(EnumAudioLayer)))
            {
                var enumValue = (EnumAudioLayer)value;
                if (enumValue == EnumAudioLayer.None)
                {
                    continue;
                }

                var layer = new GameObject(enumValue.ToString());
                layer.transform.SetParent(root.transform, false);
                _enumAudioType2Layer.Add(value, layer.transform);
                _audioSourceMap.Add(value, new Dictionary<int, AudioSource>());
            }

            Logger.Log("音频管理器初始化");
        }

        public void Update()
        {
            //遍历音效层级
            foreach (var audioSources in _audioSourceMap.Values)
            {
                //遍历音效
                for (var i = 0; i < audioSources.Count;)
                {
                    var (key, audioSource) = audioSources.ElementAt(i);
                    //播放完成的clip 回收
                    if (!audioSource.loop && audioSource.clip != null &&
                        audioSource.timeSamples >= audioSource.clip.samples)
                    {
                        audioSource.Stop();
                        AssetUtil.Recycle(audioSource.gameObject);
                        audioSources.Remove(key);
                        continue;
                    }

                    i++;
                }
            }
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
        }

        public void OnClear()
        {
            foreach (var layer in _audioSourceMap.Keys)
            {
                ClearLayer(layer);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="layer"></param>
        public void Pause(int layer)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Error($"音效资源找不到层级 layer:{layer}");
                return;
            }

            foreach (var audioSource in _audioSourceMap[layer].Values)
            {
                audioSource.Pause();
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="layer"></param>
        public void UnPause(int layer)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Error($"音效资源找不到层级 layer:{layer}");
                return;
            }

            foreach (var audioSource in _audioSourceMap[layer].Values)
            {
                audioSource.UnPause();
            }
        }

        /// <summary>
        /// 设置某个层级静音状态
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="isMute"></param>
        public void SetMute(int layer, bool isMute)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Error($"音效资源找不到层级 layer:{layer}");
                return;
            }

            foreach (var audioSource in _audioSourceMap[layer].Values)
            {
                audioSource.mute = isMute;
            }
        }

        /// <summary>
        /// 设置全部层级静音状态
        /// </summary>
        /// <param name="isMute"></param>
        public void SetAllMute(bool isMute)
        {
            foreach (var audioSource in _audioSourceMap.Values.SelectMany(audioSources => audioSources.Values))
            {
                audioSource.mute = isMute;
            }
        }

        /// <summary>
        /// 设置某个层级音量
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="volume"></param>
        public void SetVolume(int layer, float volume)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Error($"音效资源找不到层级 layer:{layer}");
                return;
            }

            foreach (var audioSource in _audioSourceMap[layer].Values)
            {
                audioSource.volume = volume;
            }
        }

        /// <summary>
        /// 设置全部层级音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetAllVolume(float volume)
        {
            foreach (var audioSource in _audioSourceMap.Values.SelectMany(audioSources => audioSources.Values))
            {
                audioSource.volume = volume;
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioId"></param>
        public async void PlayAudio(int audioId)
        {
            var rowCfgAudio = ConfigManager.Instance.cfgAudio[audioId];
            var rowCfgAudioLayer = ConfigManager.Instance.cfgAudioLayer[rowCfgAudio.layer];
            if (string.IsNullOrEmpty(rowCfgAudio.path))
            {
                return;
            }

            //不叠加
            if (!rowCfgAudioLayer.needOverlay)
            {
                ClearLayer(rowCfgAudio.layer);
            }

            //音效资源
            var audioClip = await AssetManager.Instance.LoadAssetAsync<AudioClip>(rowCfgAudio.path, AssetLoadType.Temp);
            //音效载体
            var audioSource = SpawnAudioSourceObj(rowCfgAudio.layer, rowCfgAudio.path);
            audioSource.clip = audioClip;
            audioSource.loop = rowCfgAudioLayer.needLoop;
            //设置音量
            audioSource.volume = rowCfgAudio.layer == EnumAudioLayer.Music.GetHashCode()
                ? SettingData.Instance.currentMusicVolume
                : SettingData.Instance.currentSoundVolume;
            audioSource.Play();
        }

        /// <summary>
        /// 生成音效物体
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="assetPath"></param>
        private AudioSource SpawnAudioSourceObj(int layer, string assetPath)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Error($"音效资源找不到层级 layer:{layer}");
                return default;
            }

            if (!_enumAudioType2Layer.ContainsKey(layer))
            {
                Logger.Error($"音效节点找不到层级 layer:{layer}");
                return default;
            }

            var clipName = assetPath.GetAssetName();
            var ins = ObjectPoolManager.Instance.Spawn(clipName);
            //对象池中找不到
            if (ins == null)
            {
                ins = new GameObject(clipName);
            }

            ins.transform.SetParent(_enumAudioType2Layer[layer]);
            var audioSource = ins.GetOrAddComponent<AudioSource>();
            audioSource.volume = 1.0f;
            audioSource.pitch = 1.0f;
            audioSource.mute = false;
            var id = ins.GetInstanceID();
            if (_audioSourceMap[layer].ContainsKey(id))
            {
                Logger.Error($"音效物体可能忘记回收 layer:{layer} assetPath:{assetPath}");
                return audioSource;
            }

            _audioSourceMap[layer].Add(id, audioSource);
            return audioSource;
        }

        /// <summary>
        /// 清理某个层级
        /// </summary>
        /// <param name="layer"></param>
        private void ClearLayer(int layer)
        {
            if (!_audioSourceMap.ContainsKey(layer))
            {
                Logger.Warning($"音效资源找不到层级 layer:{layer}");
                return;
            }

            foreach (var audioSource in _audioSourceMap[layer].Values)
            {
                audioSource.Stop();
                AssetUtil.Recycle(audioSource.gameObject);
            }

            _audioSourceMap[layer].Clear();
        }
    }
}