using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBodyHit : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pillar")
            other.gameObject.GetComponent<TowerHitCheck>().OnHit(this.transform.position);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Piller")
    //    {
    //        collision.gameObject.GetComponent<TowerHitCheck>().OnHit(collision.contacts[0].point);
    //    }

    //}


}
