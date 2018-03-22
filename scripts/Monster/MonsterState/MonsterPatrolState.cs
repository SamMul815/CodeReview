using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrolState : MonsterState {

    protected override void Awake()
    {
        //최대 Action 범위 설정
        _maxStateLength = 2;
        base.Awake();
    }

    public override void BeginState()
    {
        base.BeginState();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //플레이어가 정찰 범위에 있는가?
        if (Manager.DistanceCalc(
                transform.position, Manager.Player, Manager.Stat.Sight))
        {
            Manager.setState(MonsterStates.Chase);
            return;
        }
        //플레이어가 어택 거리에 있는가?
        else if (Manager.DistanceCalc(
                    transform.position, Manager.Player, Manager.Stat.AttackRange))
        {
            Manager.setState(MonsterStates.Attack);
            Manager.setAction(MonsterActions.Attack);
            return;
        }
		
	}

    public override void EndState()
    {
        base.EndState();
    }

}
