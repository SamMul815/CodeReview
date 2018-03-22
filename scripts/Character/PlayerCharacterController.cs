using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    
    enum INPUTKEY
    {
        KEY_W = 1,
        KEY_S = 2,
        KEY_A = 4,
        KEY_D = 8,
        KEY_LSHIFT = 16,
        MOUSE_LEFT = 32,
        MOUSE_RIGHT = 64
    }
    enum PLAYERSTATE
    {
        STAND,
        MOVE,
        ROLL,
        ATTACK,
        HIT,
        DEAD
    }

    public float m_TurnSpeed = 360;
    public float m_MoveSpeed = 180;


    int m_KeyBit = 0;
    float m_RotAngle = 0;
    CAction m_playerAction = new CAction();
    PLAYERSTATE m_PlayerState = PLAYERSTATE.MOVE;


 
    //플레이어 움직일 키입력값 전부 확인
    void KeyInput()
    {
        if (Input.GetKey(KeyCode.W))            { m_KeyBit |= (int)INPUTKEY.KEY_W; }
        else if (Input.GetKey(KeyCode.S))       { m_KeyBit |= (int)INPUTKEY.KEY_S; }
        if (Input.GetKey(KeyCode.A))            { m_KeyBit |= (int)INPUTKEY.KEY_A; }
        else if (Input.GetKey(KeyCode.D))       { m_KeyBit |= (int)INPUTKEY.KEY_D; }

        if (Input.GetKey(KeyCode.LeftShift))    { m_KeyBit |= (int)INPUTKEY.KEY_LSHIFT; }
        if (Input.GetMouseButton(0))            { m_KeyBit |= (int)INPUTKEY.MOUSE_LEFT; }
        if (Input.GetMouseButton(1))            { m_KeyBit |= (int)INPUTKEY.MOUSE_RIGHT; }
    }

    //키값 확인후 상태값 설정
    void CheckState()
    {
        //공격
        if ((m_KeyBit & (int)INPUTKEY.MOUSE_LEFT) == (int)INPUTKEY.MOUSE_LEFT)      { Debug.Log("마왼"); m_KeyBit -= (int)INPUTKEY.MOUSE_LEFT; }
        if ((m_KeyBit & (int)INPUTKEY.MOUSE_RIGHT) == (int)INPUTKEY.MOUSE_RIGHT)    { Debug.Log("마오"); m_KeyBit -= (int)INPUTKEY.MOUSE_RIGHT; }        
        //구르기
        if ((m_KeyBit & (int)INPUTKEY.KEY_LSHIFT) == (int)INPUTKEY.KEY_LSHIFT)      { Debug.Log("왼슆"); m_KeyBit -= (int)INPUTKEY.KEY_LSHIFT; }
        //움직임
        switch (m_KeyBit)
        {
            case (int)INPUTKEY.KEY_W: Debug.Log("위"); m_RotAngle = 0;   break;
            case (int)INPUTKEY.KEY_A: Debug.Log("왼"); m_RotAngle = -90; break;
            case (int)INPUTKEY.KEY_S: Debug.Log("아"); m_RotAngle = 180; break;
            case (int)INPUTKEY.KEY_D: Debug.Log("오"); m_RotAngle = 90;  break;

            case (int)INPUTKEY.KEY_W | (int)INPUTKEY.KEY_A: Debug.Log("위왼"); break;
            case (int)INPUTKEY.KEY_W | (int)INPUTKEY.KEY_D: Debug.Log("위오"); break;
            case (int)INPUTKEY.KEY_S | (int)INPUTKEY.KEY_A: Debug.Log("아왼"); break;
            case (int)INPUTKEY.KEY_S | (int)INPUTKEY.KEY_D: Debug.Log("아오"); break;

            default: m_PlayerState = PLAYERSTATE.STAND; break;
        }
    }

    void CheckAction()
    {
        switch(m_PlayerState)
        {
            case PLAYERSTATE.STAND:
                {

                }
                break;
            case PLAYERSTATE.MOVE:
                {
                    if (!m_playerAction.GetType().Equals(typeof(AMove)))
                    {
                        m_playerAction = new AMove();
                        m_playerAction.SetTarget(this.gameObject);                       
                    }
                    ((AMove)m_playerAction).SetRot(m_RotAngle);
                    ((AMove)m_playerAction).SetMoveSpeed(m_MoveSpeed);
                }
                break;
            case PLAYERSTATE.ROLL:
                {

                }
                break;
            case PLAYERSTATE.ATTACK:
                {

                }
                break;
            case PLAYERSTATE.HIT:
                {

                }
                break;
            case PLAYERSTATE.DEAD:
                {

                }
                break;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        KeyInput();
        CheckState();
        CheckAction();
        m_playerAction.Update();

        m_KeyBit = 0;
    }



}
