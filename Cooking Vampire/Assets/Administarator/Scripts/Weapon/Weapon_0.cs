using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Surrorund
public class Weapon_0 : Weapon
{
    private void Update()
    {
        Move();
    }

    public override IEnumerator Active()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    protected override void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform projectTrans;

            if (i < transform.childCount)
                projectTrans = transform.GetChild(i);
            else
            {
                Projectile projectile = spawnManager.Spawn_Projectile(dataManager.curWeapon.weaponSprite, this, Vector3.zero);
                projectTrans = projectile.transform;
            }

            projectTrans.localPosition = Vector3.zero;
            projectTrans.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            projectTrans.Rotate(rotVec);
            projectTrans.Translate(projectTrans.up * 2f, Space.World);
        }
    }

    protected void Move()
    {
        transform.Rotate(Vector3.back * stat.speed * Time.deltaTime);
    }

    public override void LevelUp()
    {
        base.LevelUp();

        Batch();
    }
    protected override void LevelContents()
    {
        switch(lv)
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
}
