using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_7 : Weapon
{
    public RuntimeAnimatorController trapAnim;

    public override IEnumerator Active()
    {
        int count = gm.stat.Get_COUNT(stat.count);
        for (int i = 0; i < count; i++)
        {
            Trap();

            if(i != count - 1)
                yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    private void Trap()
    {
        Projectile projectile = spawnManager.Spawn_Projectile_Trap(GetProjectileSprite(), stat, trapAnim, null);
        projectile.transform.position = player.transform.position;
    }
}
