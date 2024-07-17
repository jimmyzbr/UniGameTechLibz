using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Framework
{
    /// <summary>
    /// Mono singleton Class. Extend this class to make singleton component.
    /// Example: 
    /// <code>
    /// public class Foo : MonoSingleton<Foo>
    /// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
    /// Override <code>Init()</code> method instead of using <code>Awake()</code>
    /// from this class.
    /// </summary>
    /// 
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T _Instance = null;
        private static bool _IsApplicationQuiting = false;

        public static T Instance
        {
            get
            {
                // Instance requiered for the first time, we look for it
                if (_Instance == null && !_IsApplicationQuiting)
                {
                    _Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    // Object not found, we create a temporary one
                    if (_Instance == null)
                    {
                        _Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        // Problem during the creation, this should not happen
                        if (!_Instance)
                        {
                            Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                        }
                    }
                }
                return _Instance;
            }
            protected set
            {
                _Instance = value;
            }
        }

        private bool _isInitialized = false;

        public bool IsInitialized { get { return _isInitialized; } }

        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
            if (_Instance == null)
            {
                _Instance = this as T;
            }
            else if (_Instance != this)
            {
                Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
                DestroyImmediate(this);
                return;
            }

            DontDestroyOnLoad(gameObject);
            if (!IsInitialized)
            {
                Init();
            }
        }

        private void OnDestroy()
        {
            if (_Instance == this)
            {
                if (IsInitialized)
                {
                    Uninit();
                }
                _Instance = null;
            }   
        }

        private void OnApplicationQuit()
        {
            _IsApplicationQuiting = true;
        }

        /// <summary>
        /// This function is called when the instance is used the first time
        /// Put all the initializations you need here, as you would do in Awake
        /// </summary>
        public virtual bool Init()
        {
            _isInitialized = true;
            return true;
        }       

        public virtual void Uninit()
        {
            _isInitialized = false;
        }

    }

}
