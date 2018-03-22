using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTest : MonoBehaviour {

    public float power = 500.0f;
    public float radius = 10.0f;
    public float upPower = 100.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    

	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                //collider.gameObject.GetComponent<MonsterManager>()
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, upPower,ForceMode.Impulse);
            }
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

}
