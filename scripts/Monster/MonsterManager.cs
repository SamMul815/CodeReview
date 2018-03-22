using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* 몬스터 상태 */
public enum MonsterStates
{
    Stand   = 0,    //0~1(Idle, Stand)
    Patrol  = 2,    //2(Walk)
    Chase   = 3,    //1~3(Walk, Run)
    Attack  = 4,    //4(Attack)
    Damege  = 5,    //5~6(Damege, KnockBack)
    Dead    = 6     //7(Dead)
}

/* 몬스터 행동 */
public enum MonsterActions
{
    Idle    = 0,
    Stand,
    Walk,
    Run,
    Attack,
    Damage,
    Dead
}

[RequireComponent(typeof(MonsterStat))]
public class MonsterManager : MonoBehaviour
{
    /* 몬스터 상태 변수 */
    public MonsterStates startState;
    public MonsterStates currentState;

    /* 몬스터 행동 변수 */
    public MonsterActions startAction;
    public MonsterActions currentAction;

    /* State Coroutine */
    private IEnumerator _actionChangeCoroutine;
    public IEnumerator ActionChangeCoroutine { set { _actionChangeCoroutine = value; } get { return _actionChangeCoroutine; } }

    /* Move Coroutine */
    private IEnumerator _moveCoroutine;
    public IEnumerator MoveCoroutine { set { _moveCoroutine = value; } get { return _moveCoroutine; } }
    
    /* 몬스터 행동 */
    Dictionary<MonsterActions, MonsterAction> _actions = new Dictionary<MonsterActions, MonsterAction>();
    /* 몬스터 상태 */
    Dictionary<MonsterStates, MonsterState> _states = new Dictionary<MonsterStates, MonsterState>();


    /* 플레이어 트렌스폼 정보 */
    private Transform _player;
    public Transform Player { set { _player = value;  } get { return _player; } }

    /* 몬스터 스텟정보 */
    private MonsterStat _stat;
	public MonsterStat Stat { get { return _stat; } }
    
    public bool IsTargetPosReach = true; // 몬스터가 TargetPos에 도착하였는지
    
    /* 애니메이션 */
    Animator _ani;
    public Animator Ani { get { return _ani; } }
    int _aniParamID;
    private MonsterAniManager _aniManager;
    public MonsterAniManager AniManager { get { return _aniManager; } }

    /* NavMeshAgent */
    NavMeshAgent _Agent;
    public NavMeshAgent Agent { get { return _Agent; } }

    /* 초기화 변수 */
    bool _init = false; 

    private void Awake()
    {
        // 플레이어 정보 가져오기
        _player = GameObject.FindWithTag("Player").transform;

        //몬스터 스텟 정보 가져오기
		_stat = GetComponent<MonsterStat>();
        
        // 애니메이션 정보 가져오기
        _ani = GetComponentInChildren<Animator>();
        _aniParamID = Animator.StringToHash("CurrentState");
        _aniManager = GetComponentInChildren<MonsterAniManager>();

        //몬스터 상태, 행동들의 정보 가져오기
        MonsterStates[] statesValues = (MonsterStates[])System.Enum.GetValues(typeof(MonsterStates));
        MonsterActions[] actionValues = (MonsterActions[])System.Enum.GetValues(typeof(MonsterActions));

        //몬스터 상태 스크립트
        foreach(MonsterStates s in statesValues)
        {
            System.Type StateType = System.Type.GetType("Monster" + s.ToString("G") + "State");
            MonsterState state = (MonsterState)GetComponentInChildren(StateType);

            if (state == null)
                state = (MonsterState)transform.Find("StateScript").gameObject.AddComponent(StateType);

            state.enabled = false;
            _states.Add(s, state);

        }
        //몬스터 행동 스크립트
        foreach (MonsterActions a in actionValues)
        {
            System.Type ActionType = System.Type.GetType("Monster" + a.ToString("G"));
            MonsterAction action = (MonsterAction)GetComponent(ActionType);

            if (action == null)
                action = (MonsterAction)transform.Find("ActionScript").gameObject.AddComponent(ActionType);
 
            action.enabled = false;
            _actions.Add(a, action);
        }


        //행동 바꾸는 코루틴
        _actionChangeCoroutine = this.ChangeState();
        //몬스터 이동 코루틴
        _moveCoroutine = this.monsterMove();

        /* 네브메쉬 셋팅 */
        _Agent = GetComponent<NavMeshAgent>();
    }


    // Use this for initialization

    void Start()
    {
        if (Application.isPlaying)
        {
			CoroutineManager.DoCoroutine(_actionChangeCoroutine); //행동체인지 코루틴 시작
            CoroutineManager.DoCoroutine(_moveCoroutine);//이동 코루틴 시작
            startState = MonsterStates.Stand;//처음 상태
            startAction = this.GetRandomMonsterAction<MonsterActions>();//랜덤행동 가져오기
            setState(startState);//상태 초기화
            setAction(startAction);//행동 초기화
            _init = true;
        }
    }

    void Update()
    {

    }
    
    //상태 설정
    public void setState(MonsterStates newState)
    {
        if (_init)
        {
            _states[currentState].enabled = false;
            _states[currentState].EndState();
        }

        currentState = newState;
        IsTargetPosReach = true;
        _states[currentState].enabled = true;
        _states[currentState].BeginState();
    }

    //행동 설정
    public void setAction(MonsterActions newAction)
    {
        if (_init)
        {
            _actions[currentAction].enabled = false;
            _actions[currentAction].EndState();
        }

        currentAction = newAction;
        _actions[currentAction].enabled = true;
        _actions[currentAction].BeginState();
        _ani.SetInteger(_aniParamID, (int)currentAction);
    }

    //상태 가져오기
    public MonsterState GetCurrentState()
    {
        return _states[currentState];
    }
    
    //행동 가져오기
    public MonsterAction GetCurrentAction()
    { 
        return _actions[currentAction];
    }

    //거리비교
    public bool DistanceCalc(Vector3 _NowPos, Transform _target, float _Range)
    {
        float _Distance = Vector3.Distance(_NowPos, _target.position);

        if (_player == null)
            return false;

        return (_Distance <= _Range);
    }


    void OnDrawGizmos()
    {
        if(Stat != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, Stat.Sight);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, Stat.AttackRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, Stat.ChaseSight);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(this. transform.position, Stat.CallSight);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            MonsterAttack attackAction = GetCurrentAction() as MonsterAttack;
            if (attackAction != null)
            {
                attackAction.OnHit();
            }
        }

        if (other.transform.tag == "Weapon")
        {
            MonsterState monsterDamegeState = GetCurrentState() as MonsterDamegeState;

            if (monsterDamegeState == null)
            {
                setState(MonsterStates.Damege);
                setAction(MonsterActions.Damage);
                monsterDamegeState = GetCurrentState() as MonsterDamegeState;
                monsterDamegeState.TakeDamege();
            }

        }
    }


}
