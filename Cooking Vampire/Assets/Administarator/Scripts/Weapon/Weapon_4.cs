using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_4 : Weapon
{
    [Title("애니메이션")]
    public RuntimeAnimatorController animator;

    public override IEnumerator Active()
    {
        for (int i = 1; i <= stat.count; i++)
            yield return Slash();
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    private IEnumerator Slash()
    {
        Projectile_Animation projectile = spawnManager.Spawn_Projectile_Anim(GetProjectileSprite(), stat, animator, 2f, null);

        Transform target = player.scanner.Export_RanTarget();
        projectile.transform.position = target ? target.position : RanPos();

        yield return new WaitUntil(() => projectile.isFinish);
    }
}
