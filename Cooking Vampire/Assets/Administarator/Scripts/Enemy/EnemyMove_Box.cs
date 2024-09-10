using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Box : EnemyMove
{
    Vector2 dir;

    protected override void LateUpdate()
    {
    }

    protected override void Track()
    {
        Vector2 nextVec = dir * enemy.stat.speed * Time.fixedDeltaTime;
        enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
        enemy.rigid.velocity = Vector2.zero;
    }

    protected override IEnumerator SpecialMoveRoutine()
    {
        yield return new WaitUntil(() => target);

        while (true)
        {
            SetDir();
            yield return new WaitForSeconds(8f);
        }
    }

    private void SetDir()
    {
        dir = (target.position - enemy.rigid.position).normalized;

        enemy.sr.flipX = dir.x < 0;
    }
}
