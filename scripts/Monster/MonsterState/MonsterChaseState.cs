using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : MonsterState {

    protected override void Awake()
    {
        //최대 Action 범위 설정
        _maxStateLength = 3;
        base.Awake();
    }

    public override void BeginState()
    {
        int friendCallNumber = CallMonster(0);

        base.BeginState();
    }

    private void FixedUpdate()
    {

        //플레이어가 공격범위에 있는가?
        if (Manager.DistanceCalc(
                transform.position, Manager.Player,Manager.Stat.AttackRange))
        {
            Manager.setAction(MonsterActions.Attack);
            Manager.setState(MonsterStates.Attack);
            return;
        }
        //플레이어가 추적범위에 있는가?
        else if(!Manager.DistanceCalc(
                    transform.position, Manager.Player, Manager.Stat.ChaseSight))
        {
            Manager.setState(MonsterStates.Patrol);
            return;
        }
    }


    public override void EndState()
    {
        base.EndState();
    }
}
