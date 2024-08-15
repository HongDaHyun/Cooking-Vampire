using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Enemy : Projectile_Rigid
{
    protected Animator animator;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        animator = GetComponent<Animator>();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        rigid.velocity = Vector2.zero;
        spawnManager.Destroy_Projectile(this);
    }

    public void SetAnim(RuntimeAnimatorController anim)
    {
        animator.runtimeAnimatorController = anim;
    }
}