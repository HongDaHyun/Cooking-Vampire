using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Surrorund
public class Atk_0 : Atk
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
        Batch();
        gameObject.SetActive(true);
        yield return null;
    }

    private void Batch()
    {
        int count = gm.stat.Cal_AMT(stat.amount);
        float range = (stat.size + gm.stat.Cal_RAN()) / 4f;
        for (int i = 0; i < count; i++)
        {
            Transform projectTrans;

            if (i < transform.childCount)
            {
                projectTrans = transform.GetChild(i);
            }

            else
            {
                Projectile projectile = spawnManager.Spawn_Projectile(GetProjectileSprite(), stat, transform);
                projectTrans = projectile.transform;
            }

            projectTrans.localPosition = Vector3.zero;
            projectTrans.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            projectTrans.Rotate(rotVec);
            projectTrans.Translate(projectTrans.up * range, Space.World);
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
            pro.SetProjectile(GetProjectileSprite(), stat, transform);
    }
}
