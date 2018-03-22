using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAction
{

    protected Animator anim;
    protected GameObject target;

    public void SetAnimator(Animator _anim)     { anim = _anim; }
    public void SetTarget(GameObject _target)   { target = _target; }

    public virtual void Update(){ }

}
