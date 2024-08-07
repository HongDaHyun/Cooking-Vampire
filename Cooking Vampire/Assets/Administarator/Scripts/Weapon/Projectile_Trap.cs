using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Trap : Projectile_Animation
{
    int defOrder;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        defOrder = sr.sortingOrder;
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        sr.sortingOrder = 0;
        StartCoroutine(DelayRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            col.enabled = false;
            sr.sortingOrder = defOrder;
            animator.SetTrigger("Active");
        }
    }

    IEnumerator DelayRoutine()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    protected override void Finish()
    {
        base.Finish();
        sr.color = Color.white;
        col.enabled = true;
    }
}
