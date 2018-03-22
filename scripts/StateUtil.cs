using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateUtil{

    public static int MonsterCount = 0;

    public static T GetRandomMonsterAction<T>(this MonsterManager monster)
    {
        int min = (int)monster.currentState;
        int max = monster.GetCurrentState().MaxStateLength + 1;
        
        System.Array RandomStates = System.Enum.GetValues(typeof(T));
        T State = (T)RandomStates.GetValue(Random.Range(min, max));

        return State;
    }
	 
	//RandomStateChange
	public static IEnumerator ChangeState(this MonsterManager monster)
	{
        MonsterActions RandomChangeAction = monster.GetRandomMonsterAction<MonsterActions>();

		while (monster.Player != null)
		{
            //while (monster.Ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
			//{
				if (RandomChangeAction == monster.currentAction)
                    RandomChangeAction = monster.GetRandomMonsterAction<MonsterActions>();
                yield return null;
			//}

            if (!monster.IsTargetPosReach)
            {
				yield return null;
				continue;
            }

            if (monster.currentState == MonsterStates.Dead)
                break;

            monster.setAction(RandomChangeAction); 
			yield return CoroutineManager.Instance.Delay;
		}
		yield return null;
	}

}
