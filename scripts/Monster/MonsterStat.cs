using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour {

    /* 걷는 속도 */
    [SerializeField]
    private float _walkSpeed = 1.0f;
    public float WalkSpeed { set { _walkSpeed = value; } get { return _walkSpeed; } }
    
    /* 뛰는 속도 */
    [SerializeField]
    private float _runSpeed = 3.0f;
    public float RunSpeed { set { _runSpeed = value; } get { return _runSpeed; } }

    /* 이동속도(걷기, 뛰기 속도) */
    private float _moveSpeed = 0.0f;
    public float MoveSpeed { set { _moveSpeed = value; }  get { return _moveSpeed; } } 

    /* 몬스터 회전 속도 */
    [SerializeField]
    private float _turnSpeed = 360.0f;
    public float TurnSpeed { set { _turnSpeed = value; } get { return _turnSpeed; } }

    /* 몬스터 데미지 */
    [SerializeField]
    private float _attackDamage = 20.0f;
    public float AttackDamage { set { _attackDamage = value; } get { return _attackDamage; } }

    /* 공격범위 */
    [SerializeField]
    private float _attackRange = 3.0f;
    public float AttackRange { set { _attackRange = value; } get { return _attackRange; } }

    /* 공격 딜레이 */
    [SerializeField]
    private float _attackDelay = 1.0f;
    public float AttackDelay { set { _attackDelay = value; }  get { return _attackDelay; } }

    /* 방어력 */
    [SerializeField]
    private float _defense = 10.0f;
    public float Defense { set { _defense = value; } get { return _defense; } }

    /* Patrol 범위 */
    [SerializeField]
    private float _sight = 5.0f;
    public float Sight{ set { _sight = value; }  get { return _sight; } }

    /* 추적 범위 */
    [SerializeField]
    private float _chaseSight = 20.0f;
    public float ChaseSight { set { _chaseSight = value; } get { return _chaseSight; } }

    /* 친구 부르는 범위 */
    [SerializeField]
    private float _callSIght = 5.0f;
    public float CallSight { set { _callSIght = value; }  get { return _callSIght; } }

    /* 몬스터 HP */
    [SerializeField]
    private int _hp = 100;
    public int Hp { set { _hp = value; } get { return _hp; } }

    /* 몬스터 이동 포인트 */
    private Vector3 _moveDir;
    public Vector3 MoveDir { set { _moveDir = value; } get { return _moveDir; } }

    /* 몬스터 매니져 */
    private MonsterManager _manager;
    public MonsterManager Manager { get{ return _manager; }}

    private void Awake()
    {
        _manager = GetComponent<MonsterManager>();
        _moveSpeed = _walkSpeed;
    }

}
