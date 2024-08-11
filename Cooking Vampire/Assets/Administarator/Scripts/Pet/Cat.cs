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
        float speed = gm.stat.Get_Value(StatType.PRO_SPEED, player.weaponController.Find_Weapon_Pet().stat.speed);
        float range = gm.stat.Get_Value(StatType.RANGE, player.scanner.defRange) / 3f;

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
        WeaponStat stat = player.weaponController.Find_Weapon_Pet().stat;

        Projectile_Animation projectile =
            spawnManager.Spawn_Projectile_Anim(projectileSprite, stat, projectileAnim, transform);
        float range = gm.stat.Get_Value(StatType.RANGE, player.scanner.defRange);
        float proSize = gm.stat.Get_Value(StatType.PRO_SIZE, stat.size);

        if(spriteRenderer.flipX)
        {
            projectile.transform.localPosition = new Vector2(-(range + proSize) / 3f, 0);
            projectile.sr.flipX = true;
        }
        else
        {
            projectile.transform.localPosition = new Vector2((range + proSize) / 3f, 0);
            projectile.sr.flipX = false;
        }

        yield return new WaitUntil(() => projectile.isFinish);
        SetAnim(PetState.Idle);
        yield return new WaitForSeconds(gm.stat.Get_Value(StatType.COOL, stat.coolTime));
    }

    protected override IEnumerator DangerRoutine()
    {
        Transform ranTarget = player.scanner.Export_RanTarget();

        yield return WalkRoutine(ranTarget);

        if (ranTarget == null)
            yield break;

        SetAnim(PetState.Danger);
        SetAnim(PetState.Idle);
        yield return new WaitForSeconds(gm.stat.Get_Value(StatType.COOL, player.weaponController.Find_Weapon_Pet().stat.coolTime));
    }
}
