using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour {

    enum MOVEARROW
    {
        MOVE_UP = 1,
        MOVE_DOWN = 2,
        MOVE_LEFT = 4,
        MOVE_UL = 5,
        MOVE_DL = 6,
        MOVE_RIGHT = 8,
        MOVE_UR = 9,
        MOVE_DR = 10
    }

    //플레이어
    GameObject player;

    //캐릭어 이동관련 
    public float moveSpeed;
    public float angularSpeed;

    //캐릭터 공격 관련
    public float AttackDamage1;
    public float AttackDamage2;
    public float AttackDamage3;
    public float KickDamage;

    //카메라 컴퍼넌트
    CameraMove cameraMove;

    //캐릭터 에니메이션 관련
    Animator anim;
    AnimatorStateInfo animAttackLayer;
    bool isAttack;
    float angleAxis = 0;

    void Start ()
    {
        player = this.gameObject;
        cameraMove = this.GetComponent<CameraMove>();
        anim = this.GetComponent<Animator>();
        animAttackLayer = anim.GetCurrentAnimatorStateInfo(1);

    }
	
    //플레이어 관련 전체 업데이트
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
            anim.SetTrigger("test");
            //anim.enabled = !anim.enabled;

        //애니메이션 정보 업데이트
        animAttackLayer = anim.GetCurrentAnimatorStateInfo(1);
        int keyBit = KeyInput();
        Debug.Log(keyBit);
        SetAttack(keyBit);
        Move(keyBit);
        SetPlayerRot(keyBit);

        Debug.Log(anim.transform);

	}

    //플레이어 입력 저장
    int KeyInput()
    {
        int KeyBit = 0;

        if (Input.GetKey(KeyCode.W)) KeyBit += 1;
        else if (Input.GetKey(KeyCode.S)) KeyBit += 2;

        if (Input.GetKey(KeyCode.A)) KeyBit += 4;
        else if (Input.GetKey(KeyCode.D)) KeyBit += 8;

        if (Input.GetMouseButtonDown(0)) KeyBit += 16;

        return KeyBit;
    }

    //플레이어 캐릭터 회전
    void SetPlayerRot(int KeyCode)
    {
        if (KeyCode >= 16) KeyCode -= 16;
        switch ((MOVEARROW)KeyCode)
        {
            case MOVEARROW.MOVE_UP:     angleAxis = 0; break;
            case MOVEARROW.MOVE_DOWN:   angleAxis = 180; break;
            case MOVEARROW.MOVE_LEFT:   angleAxis = -90; break;
            case MOVEARROW.MOVE_UL:     angleAxis = -45; break;
            case MOVEARROW.MOVE_DL:     angleAxis = -135; break;
            case MOVEARROW.MOVE_RIGHT:  angleAxis = 90; break;
            case MOVEARROW.MOVE_UR:     angleAxis = 45; break;
            case MOVEARROW.MOVE_DR:     angleAxis = 135; break;
            default: break;
        }
        if (!animAttackLayer.IsName("Empty")) return;
        if (KeyCode != 0)
            player.transform.rotation = Quaternion.Lerp(
                player.transform.rotation,
                Quaternion.AngleAxis(angleAxis, Vector3.up) * Quaternion.AngleAxis(cameraMove.GetCameraRot().y, Vector3.up),
                angularSpeed * Time.deltaTime);
    }

    //플레이어 캐릭터 이동
    void Move(int KeyCode)
    {
        Vector3 pos;
        if (KeyCode >= 16) KeyCode -= 16;
        if (!animAttackLayer.IsName("Empty")) return;

        switch ((MOVEARROW)KeyCode)
        {
            case MOVEARROW.MOVE_UP: pos =       (Quaternion.AngleAxis(0, Vector3.up) * cameraMove.GetCameraFowardDir()).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_DOWN: pos =     (Quaternion.AngleAxis(180, Vector3.up) * cameraMove.GetCameraFowardDir()).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_LEFT: pos =     (Quaternion.AngleAxis(-90, Vector3.up) * cameraMove.GetCameraFowardDir()).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_UL: pos =       (Quaternion.AngleAxis(-45, Vector3.up) * cameraMove.GetCameraFowardDir() ).normalized     * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_DL: pos =       (Quaternion.AngleAxis(-135, Vector3.up) * cameraMove.GetCameraFowardDir()).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_RIGHT: pos =    (Quaternion.AngleAxis(90, Vector3.up) * cameraMove.GetCameraFowardDir()  ).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_UR: pos =       (Quaternion.AngleAxis(45, Vector3.up) * cameraMove.GetCameraFowardDir()  ).normalized * moveSpeed * Time.deltaTime; break;
            case MOVEARROW.MOVE_DR: pos =       (Quaternion.AngleAxis(135, Vector3.up) * cameraMove.GetCameraFowardDir() ).normalized * moveSpeed * Time.deltaTime; break;
            default: pos = Vector3.zero; break;
        }

        if (KeyCode != 0)
            anim.SetBool("IsMove", true);
        else
            anim.SetBool("IsMove", false);
        
        player.transform.position += pos;
    }

    void SetAttack(int Keycode)
    {
        if (Keycode >= 16)
        {
            if (animAttackLayer.IsName("Empty"))
            {



                anim.SetTrigger("Attack1");
            }
            else
                isAttack = true;
        }
    }

    void AnimAttackCheck()
    {
        if(isAttack)
        {
            if(animAttackLayer.IsName("Attack1"))
            {
                anim.ResetTrigger("Attack1");
                anim.SetTrigger("Attack2");
            }
            else if (animAttackLayer.IsName("Attack2"))
            {
                anim.ResetTrigger("Attack2");
                anim.SetTrigger("Attack3");
            }
            else if (animAttackLayer.IsName("Attack3"))
            {
                anim.ResetTrigger("Attack3");
                anim.SetTrigger("Attack4");
            }
            StartCoroutine("CorAttackFowardChange");
            isAttack = false;
        }
    }
    
    IEnumerator CorAttackFowardChange()
    {
        float angle = angleAxis;

        for(int  i = 0; i<10; i++)
        {
            player.transform.rotation = 
                Quaternion.Lerp(
                player.transform.rotation,
                Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.AngleAxis(cameraMove.GetCameraRot().y, Vector3.up),
                angularSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;

    }
}
