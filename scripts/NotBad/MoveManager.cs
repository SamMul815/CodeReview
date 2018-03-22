using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour {

    public List<Node>    NodeList;       //노드 집합
    public List<Vector3> moveLine;       //노드 이동경로 집합
    public List<Vector3> moveDrawLine;   //그릴때 사용
    public List<Vector3> moveRot;       
    public List<float> moveSpeed;        //스피드 리스트
    public GameObject TestDragon;        //임시 드래곤 이동모델
    public float firstMoveSpeed = 100.0f;
    public bool isForwardRot = false;

    private bool isMove = false;
    private bool isEnd = false;

    Vector3     StartDragonPos = Vector3.zero;
    Quaternion  StartDragonRot = Quaternion.identity;

    //이동 관련된 변수들
    int nodeCount = 0;
    Vector3 dir;
    private void Start()
    {
        MoveType2();
    }

    private void Update()
    {

        //움직임 부탁했을때
        if(isMove)
            CenterPosCalc();

        //방향 조절
        if (isMove && isForwardRot )
            LookDir();
    }
    //베지어 곡선에 맞춰 중간 지점 생성
    //간격이 일정하지는 않음
    public void MoveType2()
    {
        moveSpeed.Add(firstMoveSpeed);
        moveRot.Add(Vector3.zero);

        for (int i = 0; i < NodeList.Count - 1; i++)
        {
            Vector3 p0 = NodeList[i].GetPosition();
            Vector3 p1 = p0 + NodeList[i].GetControlPosition();
            Vector3 p2 = NodeList[i + 1].GetPosition();

            for (int div = 0; div < NodeList[i].divCount; div++)
            {
                float t = (1.0f / (float)NodeList[i].divCount) * (float)div;
                float tt = (1.0f - t);
                float speed;
                if (i - 1 < 0)
                    speed = Mathf.Lerp(NodeList[i].speed / 2, NodeList[i].speed, t);
                else
                    speed = Mathf.Lerp(NodeList[i - 1].speed, NodeList[i].speed, t);

                Vector3 p = tt * (tt * p0 + t * p1) + t * (tt * p1 + t * p2);
                moveLine.Add(p);
                moveSpeed.Add(speed);
                moveRot.Add(NodeList[i].RotationValue);
            }
        }
        moveLine.Add(NodeList[NodeList.Count - 1].transform.position);
        moveSpeed.Add(NodeList[NodeList.Count -2].speed);
        moveRot.Add(NodeList[NodeList.Count - 2].RotationValue);
    }

    void CenterPosCalc()
    {
        if (moveLine.Count <= 0)
            return;

        if (nodeCount >= moveLine.Count)
        {
            isEnd = true;
            return;
        }

        if (TestDragon == null)
            return;

        float moveDistance;
        float pointDistance;

        //현재 이동 거리 및 
        moveDistance = moveSpeed[nodeCount] * Time.deltaTime;
        dir = (moveLine[nodeCount] - TestDragon.transform.position).normalized;
        pointDistance = Vector3.Distance(moveLine[nodeCount], TestDragon.transform.position);

        for (; moveDistance > pointDistance;)
        {
            TestDragon.transform.position += dir * pointDistance;           //점까지 남은거리 이동
            moveDistance = moveDistance - pointDistance;                    //남은 거리 저장

            nodeCount++;                                                    //다음 점까지의 방향과 거리 계산
            if (nodeCount >= moveLine.Count)
                return;
            dir = (moveLine[nodeCount] - TestDragon.transform.position).normalized;
            pointDistance = Vector3.Distance(moveLine[nodeCount], TestDragon.transform.position);

        }
        TestDragon.transform.position += dir * moveDistance;
        dir = (moveLine[nodeCount] - TestDragon.transform.position).normalized;

    }
    void LookDir()
    {
        //TestDragon.transform.LookAt(moveLine[nodeCount]);
        TestDragon.transform.rotation = Quaternion.Lerp(TestDragon.transform.rotation, Quaternion.LookRotation(dir, TestDragon.transform.up), 0.1f);
        TestDragon.transform.Rotate(moveRot[nodeCount]);
    }

    //외부에서 매니저에 들어가있는 모션 사용할때 
    public void MovePlay(Vector3 DragonPos, Quaternion rot)
    {
        StartDragonPos = DragonPos;
        StartDragonRot = rot;
        isMove = true;
    }
    //외부에서 매니저 재생중인지 확인할때
    public bool GetisEnd() { return isEnd; }

    //리셋
    public void Reset()
    {
        nodeCount = 0;
        isEnd = false;
        isMove = false;
        StartDragonPos = Vector3.zero;
        StartDragonRot = Quaternion.identity;
        TestDragon.transform.position = Vector3.zero;
        TestDragon.transform.rotation = Quaternion.identity;
    }

    //위치값 반환
    public Vector3 GetCurrentPos()
    {
        //Vector3 pos = Quaternion.AngleAxis(StartDragonRot.y,Vector3.up) * TestDragon.transform.position;
        Vector3 pos = StartDragonRot * TestDragon.transform.position;
        return pos + StartDragonPos;
    }

    public Quaternion GetCurrentRotation()
    {
        return TestDragon.transform.rotation;
    }

    public void DrawMoveLine()
    {
        for (int i = 0; i < NodeList.Count - 1; i++)
        {
            if (NodeList[i] == null) return;
            if (NodeList[i + 1] == null) return;
            Vector3 p0 = NodeList[i].GetPosition();
            Vector3 p1 = p0 + NodeList[i].GetControlPosition();
            Vector3 p2 = NodeList[i + 1].GetPosition();

            for (int div = 0; div < NodeList[i].divCount; div++)
            {
                float t = (1.0f / (float)NodeList[i].divCount) * (float)div;
                float tt = (1.0f - t);
                float speed = Mathf.Lerp(NodeList[i].speed, NodeList[i + 1].speed, t);
                Vector3 p = tt * (tt * p0 + t * p1) + t * (tt * p1 + t * p2);
                moveDrawLine.Add(p);
            }
        }
        moveDrawLine.Add(NodeList[NodeList.Count - 1].transform.position);
    }

    private void OnDrawGizmos()
    {
        if (moveDrawLine == null) return;
        if (NodeList == null) return;
        if (NodeList.Count <= 1) return;

        moveDrawLine.Clear();
        DrawMoveLine();

        Gizmos.color = Color.red;

        for (int i = 0; i < moveDrawLine.Count; i++)
        {
            Gizmos.DrawSphere(moveDrawLine[i], 0.2f);
        }

        for (int i = 0; i< moveDrawLine.Count-1; i++)
        {
            Gizmos.DrawLine(moveDrawLine[i], moveDrawLine[i + 1]);
        }
    }
}
