using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRigid : MonoBehaviour {

    Rigidbody[] cellRigids;

	// Use this for initialization
	void Start ()
    {
        //cellRigids = this.GetComponentsInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    public void CellFire(Vector3 pos, float power, float radius)
    {
        cellRigids = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigid in cellRigids)
        {
            rigid.AddExplosionForce(power, pos, radius, power / 100, ForceMode.Impulse);
        }
    }



}
