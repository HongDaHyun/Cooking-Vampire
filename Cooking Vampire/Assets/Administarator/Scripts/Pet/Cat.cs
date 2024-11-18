using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Pet
{
    protected override IEnumerator WalkRoutine(Transform target)
    {
        isMove = true;
        if (target == null)
            target = player.transform;

        SetFlip(target);
        SetAnim(PetState.Walk);
        
        float range = gm.stat.Cal_RAN() / 3f;

        while (Vector2.Distance(transform.position, target.position) > range && isMove)
        {
            if (target == null)
            {
                yield return WalkRoutine(target);
                yield break;
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * stat.speed);
            yield return new WaitForFixedUpdate();
        }

        if(target == player.transform)
        {
            SetAnim(PetState.Idle);
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected override IEnumerator AtkRoutine()
    {
        Transform ranTarget = player.scanner.Export_RanTarget();

        yield return WalkRoutine(ranTarget);

        if (ranTarget == null)
            yield break;

        SetAnim(PetState.Atk);
        Projectile_Animation projectile =
            spawnManager.Spawn_Projectile_Anim(projectileSprite, stat, projectileAnim, transform);
        float range = gm.stat.Cal_RAN();
        float proSize = stat.size;

        if (spriteRenderer.flipX)
        {
            projectile.transform.localPosition = new Vector2(-(range + proSize) / 3f, 0);
            projectile.sr.flipX = true;
        }
        else
        {
            projectile.transform.localPosition = new Vector2((range + proSize) / 3f, 0);
            projectile.sr.flipX = false;
        }

        SetAnim(PetState.Idle);
        yield return new WaitUntil(() => projectile.isFinish);

        yield return new WaitForSeconds(gm.stat.Cal_AS(stat.atkSpeed));
    }

    protected override IEnumerator DangerRoutine()
    {
        Transform ranTarget = player.scanner.Export_RanTarget();

        yield return WalkRoutine(ranTarget);

        if (ranTarget == null)
            yield break;

        SetAnim(PetState.Danger);
        SetAnim(PetState.Idle);
        yield return new WaitForSeconds(gm.stat.Cal_AS(stat.atkSpeed));
    }
}
