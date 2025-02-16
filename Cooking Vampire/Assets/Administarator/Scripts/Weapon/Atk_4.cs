using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_4 : Atk
{
    [Title("애니메이션")]
    public RuntimeAnimatorController animator;

    public override IEnumerator Active()
    {
        for (int i = 1; i <= gm.stat.Cal_AMT(stat.amount); i++)
            yield return Slash();
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    private IEnumerator Slash()
    {
        Projectile_Animation projectile = spawnManager.Spawn_Projectile_Anim(GetProjectileSprite(), stat, animator, null);

        Transform target = player.scanner.Export_RanTarget();
        projectile.transform.position = target ? target.position : RanPos();

        yield return new WaitUntil(() => projectile.isFinish);
    }
}
