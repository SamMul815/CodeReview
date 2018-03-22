using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAniManager : MonoBehaviour {

    //애니메이션 현재 시간
    private float _aniTime = 1.0f;
    public float AniTime { set { _aniTime = value; }  get { return _aniTime; } }

    //애니메이션 재생 시간 오차값
    private const float _EPSILON = 0.01f;
    public float EPSILON { get { return _EPSILON; } }

    private MonsterManager _manager;
    
    private void Awake()
    {
        _manager = transform.root.GetComponent<MonsterManager>();
    }

}
