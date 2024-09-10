using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Charge : EnemyMove
{
    public bool isCharge;
    protected void IsCharge_Stop()
    {
        IsMove();
        isCharge = false;
    }

    public override void ReSet()
    {
        isCharge = false;
        base.ReSet();
    }

    protected override void FixedUpdate()
    {
        if (isCharge)
            return;
        base.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        if (isCharge)
            return;
        base.LateUpdate();
    }

    protected override IEnumerator SpecialMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            enemy.anim.SetTrigger("Charge");
        }
    }

    private IEnumerator Charging()
    {
        isCharge = true;
        Vector2 dirVec = (target.position - enemy.rigid.position).normalized;

        while (isCharge)
        {
            Vector2 nextVec = dirVec.normalized * enemy.stat.speed * 7f * Time.fixedDeltaTime;
            enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
            enemy.rigid.velocity = Vector2.zero;
            yield return new WaitForFixedUpdate();
        }
    }
}
