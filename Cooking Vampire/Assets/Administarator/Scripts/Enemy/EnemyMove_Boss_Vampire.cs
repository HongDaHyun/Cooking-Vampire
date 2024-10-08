using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Vampire : EnemyMove_Boss
{
    public void VampireDead()
    {
        enemy.spawnManager.Spawn_Enemy("¹ì¹ÚÁã", transform.position, 2f);
        enemy.Destroy();
    }
}
