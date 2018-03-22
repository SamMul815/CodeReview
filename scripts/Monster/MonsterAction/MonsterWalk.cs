using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalk : MonsterAction
{
    public override void BeginState()
    {
        //이동위치에 도착을 하지 않았으니
        Manager.IsTargetPosReach = false;

        //moveSpeed 설정
        Manager.Agent.speed = Manager.Stat.WalkSpeed;

        //이동위치 설정
        if (Manager.currentState == MonsterStates.Patrol)
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);

            Manager.Stat.MoveDir = new Vector3(x, transform.position.y, z);

            return;
        }

        base.BeginState();
    }

    private void Update()
    {
    }

    public override void EndState()
    {
        Manager.IsTargetPosReach = true;
        base.EndState();
    }

}
