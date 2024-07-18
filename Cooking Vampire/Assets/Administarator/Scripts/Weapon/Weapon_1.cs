using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target_Shooting
public class Weapon_1 : Weapon
{
    public override IEnumerator Active()
    {
        yield return FireRoutine();
    }

    protected override void Batch()
    {

    }

    protected override void MaxLevel()
    {
    }

    private void Fire(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Projectile projectile = spawnManager.Spawn_Projectile(dataManager.curWeapon.weaponSprite, this, dir);
        Transform projectTrans = projectile.transform;
        projectTrans.localPosition = Vector2.zero;
        projectTrans.localRotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
    private void Fire_Nearest()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Fire(targetPos);
    }
    private void Fire_Ran()
    {
        Transform target = player.scanner.Export_RanTarget();
        if (!target)
            return;

        Fire(target.position);
    }

    private IEnumerator FireRoutine()
    {
        Fire_Nearest();
        yield return new WaitForSeconds(0.2f);
        for(int i = 1; i < stat.count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Fire_Ran();
        }
    }
}
