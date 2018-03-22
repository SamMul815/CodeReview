using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRun : MonsterAction {

    public override void BeginState()
    {

        //이동위치에 도착을 하지 않았으니
        Manager.IsTargetPosReach = false;

        //moveSpeed 설정
        Manager.Agent.speed = Manager.Stat.RunSpeed;

        //이동위치 설정
        if (Manager.currentState == MonsterStates.Chase)
            Manager.Stat.MoveDir = Manager.Player.position;

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
