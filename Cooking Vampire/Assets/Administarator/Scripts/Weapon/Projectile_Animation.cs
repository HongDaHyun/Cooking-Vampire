using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Animation : Projectile
{
    Animator animator;
    [HideInInspector] public bool isFinish;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        animator = GetComponent<Animator>();
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        isFinish = false;
    }

    public void SetAnim(RuntimeAnimatorController anim)
    {
        animator.runtimeAnimatorController = anim;
    }

    private void Finish()
    {
        isFinish = true;
        spawnManager.Destroy_Projectile(this);
    }
}
