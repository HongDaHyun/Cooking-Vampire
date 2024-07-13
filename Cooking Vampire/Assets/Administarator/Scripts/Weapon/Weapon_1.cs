using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target_Shooting
public class Weapon_1 : Weapon
{
    public override void Active()
    {
        Fire();
    }

    protected override void Batch()
    {

    }

    protected override void LevelContents()
    {
        switch (lv)
        {
            case 0:
            case 1:
            case 2:
                count++;
                break;
            case 3:
            case 4:
                count += 2;
                break;
            case 5:
                // MAX
                break;
            default:
                break;
        }
    }

    private void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Projectile projectile = spawnManager.Spawn_Projectile(dataManager.curWeapon.weaponSprite, this, dir);
        Transform projectTrans = projectile.transform;
        projectTrans.localPosition = Vector2.zero;
        projectTrans.localRotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}
