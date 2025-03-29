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

            isForceStop = true; isPattern = true;

            enemy.anim.SetTrigger("Atk");

            yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));
            yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));

            isForceStop = false; isPattern = false;
        }
    }

    public void ShootArea()
    {
        Area_Sprite sprite = enemy.spriteData.Export_Area_Sprite(enemy.data.title);
        enemy.spawnManager.Spawn_Projectile_Area(1f, sprite.anim, transform);
    }
}
