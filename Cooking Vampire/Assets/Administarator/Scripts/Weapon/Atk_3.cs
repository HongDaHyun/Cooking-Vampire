using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_3 : Atk
{
    [Title("애니메이션")]
    public RuntimeAnimatorController[] animators;

    public override IEnumerator Active()
    {
        yield return Slash(0);
    }

    protected override void MaxLevel()
    {
    }

    private IEnumerator Slash(int elementID)
    {
        bool playerFlip = player.sr.flipX;
        float range = gm.stat.Cal_RAN() / 3f + stat.size / 2f;

        for (int i = 0; i < gm.stat.Cal_AMT(stat.amount); i++)
        {
            Projectile_Animation projectile = spawnManager.Spawn_Projectile_Anim(GetProjectileSprite(), stat, animators[elementID], transform);

            bool isFlip = i % 2 == 0 ? playerFlip : !playerFlip;
            projectile.sr.flipX = isFlip;

            float positionOffset = isFlip ? -range : range;
            projectile.transform.position = transform.position + new Vector3(positionOffset, 0);

            yield return new WaitUntil(() => projectile.isFinish);
        }
    }
}
