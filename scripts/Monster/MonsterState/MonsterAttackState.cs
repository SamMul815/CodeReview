using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterState {

    protected override void Awake()
    {
        //최대 Action 범위 설정
        _maxStateLength = 4;
        base.Awake();
    }

    public override void BeginState()
    {
        base.BeginState();
    }

    private void FixedUpdate()
    {
        //플레이어가 공격범위에 없으면
        if(!Manager.DistanceCalc(
                    transform.position, Manager.Player, Manager.Stat.AttackRange))
        {
            //추적범위에 있으면
            if (Manager.DistanceCalc(
                       transform.position, Manager.Player, Manager.Stat.ChaseSight))
            {
                Debug.Log("test");

                Manager.setState(MonsterStates.Chase);
                return;
            }
            //추적범위에 없으면
            else
            {
                Manager.setState(MonsterStates.Patrol);
                return;
            }
        }

        /* 플레이어를 향해서 어택 */
        Vector3 Dir = (Manager.Player.position - transform.position);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float Dot = Vector3.Dot(Dir, forward);

        if (Dot > 0.0f)
        {
            Manager.transform.rotation = 
                Quaternion.Slerp(
                    Manager.transform.rotation, 
                    Quaternion.LookRotation(Dir),
                    180 * Time.deltaTime);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

}
