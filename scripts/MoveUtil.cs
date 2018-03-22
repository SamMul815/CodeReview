using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveUtil {

    /*
    //플레이어 이동
    public static void Move(this PlayerManager player, Vector3 Dir, float moveSpeed, float turnSpeed)
    {

        Transform t = player.transform;

        Dir = Dir.normalized;
        
        if (Dir != Vector3.zero)
        {
            t.rotation =
                Quaternion.Slerp(t.rotation,
                Quaternion.LookRotation(Dir),
                Time.smoothDeltaTime * turnSpeed * 0.3f);
        }

        t.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);

    }
    */
	//몬스터 이동
	public static IEnumerator monsterMove(this MonsterManager monster)
	{
        while (monster.Player != null)
        {
            Transform p = monster.Player;
            
			while (monster.currentAction == MonsterActions.Walk || 
                monster.currentAction == MonsterActions.Run)
            {
                Vector3 moveDir = monster.Stat.MoveDir;

                if (monster.currentState == MonsterStates.Chase)
                    moveDir = p.position;

                if (monster.currentState == MonsterStates.Dead)
                    break;

                monster.Agent.SetDestination(moveDir); // 목표지점 설정
                
                if (monster.Agent.remainingDistance < monster.Agent.stoppingDistance) // 남은거리
                    monster.IsTargetPosReach = true;

                yield return CoroutineManager.Instance.FiexdUpdate;
            }

			yield return CoroutineManager.Instance.FiexdUpdate;
		}

		yield return null;
	}
}
