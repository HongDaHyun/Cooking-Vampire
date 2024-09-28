using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Anim : Effect
{
    Animator anim;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        anim = GetComponent<Animator>();
    }

    public void SetEffect(RuntimeAnimatorController animatorController, Vector2 pos, float size)
    {
        anim.runtimeAnimatorController = animatorController;
        SetTrans(pos, size);
    }
}
