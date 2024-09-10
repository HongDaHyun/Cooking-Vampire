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

            enemy.anim.SetTrigger("Atk");
            enemy.ShootArea();
        }
    }
}
