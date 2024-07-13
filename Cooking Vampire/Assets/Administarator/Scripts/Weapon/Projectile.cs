using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour, IPoolObject
{
    [HideInInspector] public Weapon weapon;
    int curPer;

    Player player;
    SpawnManager spawnManager;
    Rigidbody2D rigid;
    SpriteRenderer sr;
    BoxCollider2D col;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        player = GameManager_Survivor.Instance.player;
        spawnManager = SpawnManager.Instance;
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public void OnGettingFromPool()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || curPer == -1)
            return;

        curPer--;

        if(curPer == -1)
        {
            rigid.velocity = Vector2.zero;
            spawnManager.Destroy_Projectile(this);
        }
    }

    void Update()
    {
        Dead();
    }

    public void SetProjectile(Sprite sprite, Weapon weapon, Vector3 dir)
    {
        this.weapon = weapon;
        curPer = weapon.per;
        transform.SetParent(weapon.transform);

        sr.sprite = sprite;
        ReSetCollider();

        if (dir != Vector3.zero)
            rigid.velocity = dir * weapon.stat.speed;
    }

    void Dead()
    {
        Transform target = player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 20f)
            spawnManager.Destroy_Projectile(this);
    }

    private void ReSetCollider()
    {
        Vector2 spriteSize = sr.sprite.bounds.size;

        col.size = spriteSize;
        col.offset = sr.sprite.bounds.center;
    }
}
