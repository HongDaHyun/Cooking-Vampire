using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rigid : Projectile
{
    protected Rigidbody2D rigid;
    int curPer;
    protected int border2;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        rigid = GetComponent<Rigidbody2D>();
        border2 = LevelManager.Instance.BORDER * 2;
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        StartCoroutine(DistanceRoutine());
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Item_Box"))
            return;

        curPer--;
        spawnManager.Spawn_Effect(spriteData.effects[1], transform.position, 0.8f);

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
    public void SetRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
    public void SetGravity(float scale)
    {
        rigid.gravityScale = scale;
    }

    protected IEnumerator DistanceRoutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(5f);

            Vector2 pos = transform.position;

            if (pos.x > border2 || pos.x < -border2 || pos.y > border2 || pos.y < -border2)
            {
                spawnManager.Destroy_Projectile(this);
            }
        }
    }
}
