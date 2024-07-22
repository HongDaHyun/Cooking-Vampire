using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_3 : Weapon
{
    [Title("애니메이션")]
    public RuntimeAnimatorController[] animators;

    public override IEnumerator Active()
    {
        for (int i = 1; i <= stat.count; i++)
            yield return Slash(0, i % 2 == 0);
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    private IEnumerator Slash(int elementID, bool isEven)
    {
        Projectile_Animation projectile = spawnManager.Spawn_Projectile_Anim(GetProjectileSprite(), this, animators[elementID]);
        Vector2 slashPos = transform.position;
        projectile.sr.flipX = isEven;
        slashPos += isEven ? new Vector2(-3, 0) : new Vector2(3, 0);

        projectile.transform.position = slashPos;
        yield return new WaitUntil(() => projectile.isFinish);
    }
}
