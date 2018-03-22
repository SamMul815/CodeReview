using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamegeState : MonsterState {

    protected override void Awake()
    {
        //최대 Action 범위 설정
        _maxStateLength = 5;
        base.Awake();
    }
    

    public override void BeginState()
    {
        base.BeginState();
    }

    private void Update()
    {
        Manager.AniManager.AniTime =
            Manager.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f;

        if (1.0f - Manager.AniManager.AniTime < Manager.AniManager.EPSILON)
        {
            if (Manager.DistanceCalc(
                       transform.position, Manager.Player, Manager.Stat.AttackRange))
            {
                Manager.setState(MonsterStates.Attack);
                return;
            }
            else if (Manager.DistanceCalc(
                            transform.position, Manager.Player, Manager.Stat.ChaseSight))
            {
                Manager.setState(MonsterStates.Chase);
                return;
            }
            else
            {
                Manager.setState(MonsterStates.Patrol);
                return;
            }
        }

    }

    public override void EndState()
    {
        base.EndState();
    }

}
