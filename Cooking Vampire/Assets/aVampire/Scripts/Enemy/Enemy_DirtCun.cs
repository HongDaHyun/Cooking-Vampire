using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DirtCun : Enemy
{
    EnemyMove_Boss_DirtCun enemyMove_Boss;
    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        enemyMove_Boss = GetComponent<EnemyMove_Boss_DirtCun>();
    }

    public override void Dead(bool isForce)
    {
        isDead = true;
        col.enabled = false;
        rigid.simulated = false;

        if(enemyMove_Boss.splitCount > 0)
        {
            anim.SetTrigger("Split");
            return;
        }

        anim.SetTrigger("Dead");

        if (!isForce)
        {
            gm.killCount++;

            if (data.ingredientID != -1)
            {
                TierType tier = gm.stat.Get_Tier();

                if (data.atkType == AtkType.Boss || tier == TierType.Epic)
                    spawnManager.Spawn_IngredientDrop(data.ingredientID, transform.position);
                else if (tier == TierType.Legend)
                    spawnManager.Spawn_Relic_Ran(transform.position);
            }

            spawnManager.Spawn_Gems(Random.Range(data.gemAmount, data.gemAmount + gm.stat.LUK / 10), transform.position);
        }
    }
}
