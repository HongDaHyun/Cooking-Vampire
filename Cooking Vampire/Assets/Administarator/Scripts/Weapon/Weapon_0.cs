using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Surrorund
public class Weapon_0 : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        Batch();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override IEnumerator Active()
    {
        ResetRanSprite();
        gameObject.SetActive(true);
        yield return null;
    }

    private void Batch()
    {
        for (int i = 0; i < stat.count; i++)
        {
            Transform projectTrans;

            if (i < transform.childCount)
                projectTrans = transform.GetChild(i);

            else
            {
                Projectile projectile = spawnManager.Spawn_Projectile(GetProjectileSprite(), this);
                projectTrans = projectile.transform;
            }

            projectTrans.localPosition = Vector3.zero;
            projectTrans.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / stat.count;
            projectTrans.Rotate(rotVec);
            projectTrans.Translate(projectTrans.up * 2f, Space.World);
        }
    }

    protected override void MaxLevel()
    {
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

    private void ResetRanSprite()
    {
        Projectile[] projectiles = GetComponentsInChildren<Projectile>(true);
        foreach (Projectile pro in projectiles)
            pro.SetSprite(GetProjectileSprite());
    }
}
