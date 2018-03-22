using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager> {
    
    //코루틴 딜레이 설정
	private WaitForSeconds _delay = new WaitForSeconds(0.02f);
	public WaitForSeconds Delay {  get { return _delay; } }

    private WaitForSeconds _longDelay = new WaitForSeconds(0.08f);
	public WaitForSeconds LongDelay { get { return _longDelay; } }

	private WaitForSeconds _shortDelay = new WaitForSeconds(0.005f);
	public WaitForSeconds ShortDelay { get { return _shortDelay; } }

	private WaitForFixedUpdate _fiexdUpdate = new WaitForFixedUpdate();
	public WaitForFixedUpdate FiexdUpdate { get { return _fiexdUpdate; } }

	private WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
	public WaitForEndOfFrame EndOfFrame { get { return _endOfFrame; } }
    
    //코루틴이 종료가 될 때까지...(실제로 코루틴 부분)
    IEnumerator Preform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
    }

    //코루틴 시작
    public static void DoCoroutine(IEnumerator coroutine)
    {
        Instance.StartCoroutine(Instance.Preform(coroutine));
    }

    //코루틴 죽이기
    public void Die()
    {
        _instance = null;
        Destroy(gameObject);
    }

    //종료가 되면 _instance에 Null값
    public void OnApplicationQuit()
    {
        _instance = null;
        Die();
    }

}
