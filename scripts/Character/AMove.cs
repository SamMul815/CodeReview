using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMove : CAction {

    float m_RotAngle;
    float m_MoveSpeed;
	
	// Update is called once per frame
	public override void Update ()
    {
        Quaternion angle = Quaternion.AngleAxis(m_RotAngle, Vector3.up);
        target.transform.position += angle * Vector3.forward * m_MoveSpeed * Time.deltaTime;
	}

    public void SetRot(float rot) { m_RotAngle = rot; }
    public void SetMoveSpeed(float speed) { m_MoveSpeed = speed; }

}
