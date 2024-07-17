using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Framework
{
    /// <summary>
    /// 配置表管理
    /// </summary>
    public static class ConfigManager
    {
        static Dictionary<Type, IConfig> mConfigs = new Dictionary<Type, IConfig>();

        public static T Get<T>() where T : ScriptableObject, IConfig, new()
        {
            var type = typeof(T);

            if (mConfigs.TryGetValue(type, out var result))
            {
                return (T)result;
            }

            result = Load<T>();
            return (T)result;
        }

        public static T Load<T>() where T : ScriptableObject, IConfig, new()
        {
            var type = typeof(T);
            if (mConfigs.TryGetValue(type, out var result))
            {
                return (T)result;
            }

            T obj = null;
            obj = ScriptableObject.CreateInstance<T>();
            var path = obj.Path;
            obj = Resources.Load<T>(path);
            if (obj)
            {
                mConfigs.Add(obj.GetType(), obj);
            }

            return obj;
        }

        public static void UnLoad<T>() where T : ScriptableObject, IConfig
        {
            IConfig result = null;
            var type = typeof(T);
            if (mConfigs.TryGetValue(type, out result))
            {
                mConfigs.Remove(type);
                Resources.UnloadAsset((T)result);
            }
        }

        public static void CleanAll()
        {
            foreach (var kvp in mConfigs)
            {
                var configObj = (UnityEngine.Object)kvp.Value;
                Resources.UnloadAsset(configObj);
            }

            mConfigs.Clear();
        }
    }
}