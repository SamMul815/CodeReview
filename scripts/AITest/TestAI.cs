using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour {
    //상태값 정의
    enum STATE
    {
        STATE_IDLE,     //대기
        STATE_FIND,     //이동
        STATE_ATTACK,   //공격
        STATE_DEAD      //죽음
    }

    //ai관련 변수
    public float moveSpeed = 5.0f;
    public float findRadius = 5.0f;
    public float attackDistance = 1.5f;

    //변수 
    STATE currentState;
    Animator anim;
    Vector3 BasePoint;
    GameObject player;
    NavMeshAgent navi;

    //초기설정
	void Awake ()
    {
        //초기 대기상태 설정
        currentState = STATE.STATE_IDLE;
        anim = this.GetComponent<Animator>();
        BasePoint = this.transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        navi = this.GetComponent<NavMeshAgent>();       

	}
	void Update ()
    {
        //발견 못했을시 탈출
        if (player == null) return;

        //현재 상태값에 따른 행동 정의
        switch (currentState)
        {
            case STATE.STATE_IDLE:
                {
                    //플레이어 발견시
                    if(Vector3.Distance(player.transform.position,this.transform.position) <= findRadius)
                    {
                        currentState = STATE.STATE_FIND;
                    }
                }
                break;
            case STATE.STATE_FIND:
                {
                    //사정거리 이내
                    if(Vector3.Distance(player.transform.position,this.transform.position) <= attackDistance)
                    {
                        currentState = STATE.STATE_ATTACK;
                        navi.SetDestination(this.transform.position);
                    }
                    else
                    {
                        navi.SetDestination(player.transform.position);
                    }
                }
                break;
            case STATE.STATE_ATTACK:
                {
                    if (Vector3.Distance(player.transform.position, this.transform.position) > attackDistance)
                    {
                        currentState = STATE.STATE_FIND;
                    }
                }
                break;
            case STATE.STATE_DEAD:
                {

                }
                break;
        }
        //상태값에 따른 애니메이션 정의
        anim.SetInteger("State", (int)currentState);
    }

    //다른 몬스터에서의 호출
    public void CallMonsterHelp()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, findRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, attackDistance);

    }

}


