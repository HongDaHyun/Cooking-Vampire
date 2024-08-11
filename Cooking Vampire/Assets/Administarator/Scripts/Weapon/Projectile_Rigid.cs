using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rigid : Projectile
{
    Rigidbody2D rigid;
    int curPer;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        curPer--;

        if (curPer == -1)
        {
            rigid.velocity = Vector2.zero;
            spawnManager.Destroy_Projectile(this);
        }
    }

    public override void SetProjectile(Sprite sprite, WeaponStat stat, Transform parent)
    {
        base.SetProjectile(sprite, stat, parent);
        curPer = gm.stat.Get_Value(StatType.PER, stat.per);
    }

    public void SetDir(Vector3 dir)
    {
        rigid.velocity = dir * gm.stat.Get_Value(StatType.PRO_SPEED, stat.speed);
    }
    public void SetDir(Vector3 dir, float speed)
    {
        rigid.velocity = dir * speed;
    }
}
