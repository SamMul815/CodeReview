using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttackCheck : MonoBehaviour {
    public Animator anim;
    private AnimatorStateInfo animAttackLayer;
    // Update is called once per frame
    void Update ()
    {
        anim = this.GetComponentInParent<Animator>();
        animAttackLayer = anim.GetCurrentAnimatorStateInfo(1);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Monster")
    //    {
    //        if (animAttackLayer.IsName("Attack1") ||
    //             animAttackLayer.IsName("Attack2") ||
    //             animAttackLayer.IsName("Attack3"))
    //            other.SendMessage("TakeDamege");
    //            //other.gameObject.GetComponent<MonsterManager>().GetCurrentState().TakeDamege();
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (animAttackLayer.IsName("Attack1") ||
                 animAttackLayer.IsName("Attack2") ||
                 animAttackLayer.IsName("Attack3"))
                collision.gameObject.GetComponent<MonsterManager>().GetCurrentState().TakeDamege();
        }
    }


}
