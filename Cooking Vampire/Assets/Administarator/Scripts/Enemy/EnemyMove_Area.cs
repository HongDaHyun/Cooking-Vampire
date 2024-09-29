using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Area : EnemyMove
{
    protected override IEnumerator SpecialMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            isForceStop = true;

            enemy.anim.SetTrigger("Atk");
            enemy.ShootArea();

            yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));
            yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));

            isForceStop = false;
        }
    }
}
