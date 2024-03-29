﻿// ******************************************************************
//       /\ /|       @file       SaveUtil.cs
//       \ V/        @brief      存档工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-14 09:45:25
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System.IO;
using UnityEngine;

namespace Rabi
{
    public static class SaveUtil
    {
        #region PlayerPrefs

        /// <summary>
        /// PlayerPrefs方式保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void SaveByPlayerPrefs(string key, object data)
        {
            //将数据转为Json格式
            var json = JsonUtility.ToJson(data);
            //保存
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log("存档成功！");
#endif
        }

        /// <summary>
        /// PlayerPrefs方式读取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LoadByPlayerPrefs(string key)
        {
            return PlayerPrefs.GetString(key, null);
        }

        #endregion

        #region JSON

        /// <summary>
        /// Json方式保存
        /// </summary>
        /// <param name="saveFileName"></param>
        /// <param name="data"></param>
        public static void SaveByJson(object data, string saveFileName = null)
        {
            saveFileName ??= data.GetType().ToString();
            var json = JsonUtility.ToJson(data);
            //路径 Combine 合并路径
            var path = Path.Combine(Application.persistentDataPath, saveFileName);
            try
            {
                File.WriteAllText(path, json);

#if UNITY_EDITOR
                Debug.Log($"存档成功！存档位置 {path}");
#endif
            }
            catch (System.Exception exception)
            {
#if UNITY_EDITOR
                Debug.Log($"存档成功！存档位置 {path}. \n{exception}");
#endif
            }
        }

        /// <summary>
        /// Json方式读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveFileName"></param>
        /// <returns></returns>
        public static T LoadFromJson<T>(string saveFileName = null) where T : new()
        {
            if (saveFileName == null)
            {
                var t = typeof(T);
                saveFileName = t.ToString();
            }

            //路径 Combine 合并路径
            var path = Path.Combine(Application.persistentDataPath, saveFileName);
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                return data;
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.Log($"存档读取失败！存档位置 {path}. \n{e}");
#endif
                return default;
            }
        }

        #endregion

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="saveFileName"></param>
        public static void DeleteSaveFile(string saveFileName)
        {
            //路径 Combine 合并路径
            var path = Path.Combine(Application.persistentDataPath, saveFileName);
            try
            {
                File.Delete(path);
            }
            catch (System.Exception e)
            {
#if UNITY_EDITOR
                Debug.Log($"删除存档失败！存档位置 {path}.\n{e}");
#endif
            }
        }
    }
}