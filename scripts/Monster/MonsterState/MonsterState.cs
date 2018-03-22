using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    //몬스터 매니져
    private MonsterManager _manager;
    public MonsterManager Manager { get { return _manager; } }

    //최대 Action 범위
    protected int _maxStateLength;
    public int MaxStateLength { get { return _maxStateLength; } }

    protected virtual void Awake()
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

    //친구 부르기
    public int CallMonster(int count)
    {
        Collider[] friend = Physics.OverlapSphere(this.transform.position, Manager.Stat.CallSight / (count + 1));

        foreach (Collider c in friend)
        {
            MonsterManager friendManager = c.gameObject.GetComponentInParent<MonsterManager>();

            //첫번째꺼 매니저 없으면 바로 foreach문 탈출
            //continue로 대체할것
            if (friendManager == null)
                continue;

            MonsterState _friendState = friendManager.GetCurrentState() as MonsterPatrolState;

            if (_friendState == null)
            {
                _friendState = friendManager.GetCurrentState() as MonsterStandState;
                if (_friendState == null)
                    continue;
            }

            if (Manager.DistanceCalc(
                    transform.position, _friendState.transform,
                    _friendState.Manager.Stat.CallSight))
            {
                _friendState.Manager.setState(MonsterStates.Chase);
            }
            return count + _friendState.CallMonster(count + 1);
        }

        return 0;

    }

    //몬스터가 데미지를 받으면 불러오는 함수
    public void TakeDamege()
    {
        MonsterDeadState monsterDeadState = Manager.GetCurrentState() as MonsterDeadState;

        if (monsterDeadState == null)
        {
            Manager.IsTargetPosReach = true;
            Manager.Stat.Hp -= 10;
            if (Manager.Stat.Hp <= 0)
            {
                Manager.setState(MonsterStates.Dead);
                Manager.setAction(MonsterActions.Dead);
            }
        }
    }


}
