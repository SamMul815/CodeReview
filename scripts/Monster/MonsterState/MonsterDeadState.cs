using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : MonsterState {


    protected override void Awake()
    {
        //최대 Action 범위 설정
        _maxStateLength = 6;
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
            this.StopCoroutine(Manager.ActionChangeCoroutine);
            this.StopCoroutine(Manager.MoveCoroutine);
            this.Manager.Player = null;
            Manager.gameObject.SetActive(false);
            Destroy(this.Manager.transform.gameObject);
        }

    }


    public override void EndState()
    {
        base.EndState();
    }

}
