using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour {

    //몬스터 매니져
    private MonsterManager _manager;
    public MonsterManager Manager { get { return _manager; } }


    private void Awake()
    {
        _manager = GetComponent<MonsterManager>();
        if (_manager == null)
        {
            _manager = GetComponentInParent<MonsterManager>();
            if (_manager == null)
                _manager = GetComponentInChildren<MonsterManager>();
            if (_manager == null)
                Debug.Log("Error");
        }
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }

}
