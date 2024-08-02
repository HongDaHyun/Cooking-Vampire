using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Pet
{
    protected override IEnumerator WalkRoutine(Transform target)
    {
        isMove = true;
        if (target == null)
            target = player.weaponController.Find_Weapon_Pet().transform;

        SetFlip(target);
        SetAnim(PetState.Walk);
        float speed = player.weaponController.Find_Weapon_Pet().stat.speed;
        float range = gm.stat.range / 3f;

        while (Vector2.Distance(transform.position, target.position) > range && isMove)
        {
            if (target == null)
            {
                yield return WalkRoutine(target);
                yield break;
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * speed);
            yield return new WaitForFixedUpdate();
        }

        if(target == player.weaponController.Find_Weapon_Pet().transform)
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
            spawnManager.Spawn_Projectile_Anim(projectileSprite, player.weaponController.Find_Weapon_Pet().stat, projectileAnim, 1, transform);
        projectile.transform.localPosition = new Vector2(spriteRenderer.flipX ? -gm.stat.range / 3f : gm.stat.range / 3f, 0);

        yield return new WaitUntil(() => projectile.isFinish);
        SetAnim(PetState.Idle);
        yield return new WaitForSeconds(player.weaponController.Find_Weapon_Pet().stat.coolTime);
    }

    protected override IEnumerator DangerRoutine()
    {
        Transform ranTarget = player.scanner.Export_RanTarget();

        yield return WalkRoutine(ranTarget);

        if (ranTarget == null)
            yield break;

        SetAnim(PetState.Danger);
        SetAnim(PetState.Idle);
        yield return new WaitForSeconds(player.weaponController.Find_Weapon_Pet().stat.coolTime);
    }
}
