using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float speed = 5.0f;         //다음 노드까지 속도
    public Vector3 ControlPosition;    //곡선 보정값
    public Vector3 RotationValue;      //회전값
    public int divCount = 10;
    public bool isUp;


    public Vector3 GetPosition() { return this.transform.position;  }
    public Vector3 GetControlPosition() { return ControlPosition; }
    public Vector3 GetRotationValue() { return RotationValue; }
}
