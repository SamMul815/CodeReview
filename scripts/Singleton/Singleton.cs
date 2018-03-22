using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제네릭 싱글톤~~~
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    
    //싱글톤

    protected static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject GameObject = new GameObject();
                    GameObject.name = "@" + typeof(T).ToString();
                    _instance = GameObject.AddComponent<T>();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this as T;
    }


}
