using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionRock : MonoBehaviour {

    public GameObject destructibleMesh;
    public GameObject _particle;

    //public Particle explosionParticle;

	
	// Update is called once per frame
	void Update ()
    {

	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Boss")
        {
            Instantiate(destructibleMesh, this.transform.position, Quaternion.identity);
            Instantiate(_particle, collision.contacts[0].point, Quaternion.identity);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 50.0f);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(5000.0f, explosionPos, 1000.0f, -100.0f);
            }

            DestroyObject(this.gameObject);

            //collision.contacts[0].point
        }
    }

}
