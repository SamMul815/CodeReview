using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 이 코드는 플레이어 내부컴퍼넌트로 들어가는 코드임
 * 플레이어쪽에서 카메라를 지정해 움직이는 방법으로 적용됨
 * 추후 변경 가능
 * 최종 수정일 20180316
 */

public class CameraMove : MonoBehaviour
{

    //카메라 관련
    public float CameraMaxDistance;     //카메라 위치 고정 거리값
    public float CameraAngularSpeed;    //카메라 회전 스피드
    public float CameraMaxAngleX;       //최대 X축 회전값
    public float CameraMinangleX;       //최소 X축 회전값
    public Vector3 AddPos;              //카메라 위치 조정값
    public Vector3 FocusPos;           //카메라 포커스 조정값
    public bool isEnable = false;

    public float shakingRadius;
    public float shakingWaitTime;
    public float shakingPlayTime;
    public bool isShaking;

    GameObject playerCamera;            //카메라 오브젝트
    GameObject EnemyTarget;             //화면 고정될 몬스터 타겟
    GameObject player;                  //플레이어 오브젝트
    Vector3 CameraRot;                  //카메라 회전값 X, Y만 사용
    Vector3 CameraPos;                  //카메라 위치 계산을 위한 변수
    


    void Awake()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = this.gameObject;
        CameraPos = playerCamera.transform.position;
    }

    void LateUpdate()
    {

        MouseCursor();

        //카메라 회전 적용
        CameraSetRot();

        //카메라 위치 적용
        CameraSetPos();

        //카메라 방향 적용
        CameraSetDir();
    }

    //카메라 정면방향 반환 Y축값 제외
    public Vector3 GetCameraFowardDir()
    {
        Vector3 dir = player.transform.position - playerCamera.transform.position;
        dir.y = 0;
        return dir.normalized;
    }

    //카메라 회전
    void CameraSetRot()
    {
        //if (Input.GetMouseButton(1))
        {

            float x = Input.GetAxis("Mouse Y");
            float y = Input.GetAxis("Mouse X");

            CameraRot.x -= x * CameraAngularSpeed * Time.deltaTime;
            CameraRot.y += y * CameraAngularSpeed * Time.deltaTime;

            if (CameraRot.x > CameraMaxAngleX)
                CameraRot.x = CameraMaxAngleX;

            if (CameraRot.x < CameraMinangleX)
                CameraRot.x = CameraMinangleX;
        }

    }

    //카메라 위치
    void CameraSetPos()
    {
        Quaternion rot =  Quaternion.Euler(CameraRot);
        //카메라 위치값 = (플레이어 위치 정보 + 보정값) - (회전값 * 정면 * 최대 거리값) 
        playerCamera.transform.position = (player.transform.position + AddPos) - (rot * Vector3.forward * CameraMaxDistance);
        //playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, (player.transform.position + AddPos) - (rot * Vector3.forward * CameraMaxDistance), 0.1f);


        //float distance = Vector3.Distance(player.transform.position, playerCamera.transform.position);
        //playerCamera.transform.position = (player.transform.position + AddPos) - (rot * Vector3.forward * Mathf.Lerp(distance, CameraMaxDistance, 0.1f));

        //Vector3 pos = player.transform.position + AddPos - rot * Vector3.forward * CameraMaxDistance;
        //playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, pos, 0.05f);

        //Vector3 pos = player.transform.position + AddPos - player.transform.forward * CameraMaxDistance;
        //CameraPos = Vector3.Lerp(playerCamera.transform.position, CameraPos, 0.1f);

        //playerCamera.transform.position = rot * CameraPos;
    }

    //카메라 방향
    void CameraSetDir()
    {

        Vector3 CameraDir = ((player.transform.position + FocusPos) - playerCamera.transform.position).normalized;
        playerCamera.transform.rotation = Quaternion.LookRotation(CameraDir, Vector3.up);

        //타겟 생성시 추가 코드
        if(EnemyTarget != null)
        {

        }
    }

    void MouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Vector3 GetCameraRot()
    {
        return CameraRot;
    }


    IEnumerator corShaking()
    {
        float time = shakingPlayTime;
        while(time > 0)
        {
            Vector3 shakingPos = Random.insideUnitSphere * shakingRadius;
            time -= shakingWaitTime;
            yield return new WaitForSeconds(shakingWaitTime);
        }
        yield return null;
    }

}
