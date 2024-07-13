using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{

    public SpriteRenderer Spawn_TileObj(Transform parentTrans)
    {
        SpriteRenderer obj = PoolManager.Instance.GetFromPool<SpriteRenderer>("TileObj");

        obj.transform.SetParent(parentTrans);

        return obj;
    }
    public void Destroy_TileObj(SpriteRenderer sr)
    {
        PoolManager.Instance.TakeToPool<SpriteRenderer>(sr);
    }

    public Enemy Spawn_Enemy(int[] spawnTier, Vector3 position)
    {
        Enemy enemy = PoolManager.Instance.GetFromPool<Enemy>("Enemy");

        enemy.transform.position = position;
        enemy.SetEnemy(spawnTier[Random.Range(0, spawnTier.Length)]);

        return enemy;
    }
    public void Destroy_Enemy(Enemy enemy)
    {
        PoolManager.Instance.TakeToPool<Enemy>(enemy);
    }

    public Projectile Spawn_Projectile(Sprite sprite, Weapon weapon, Vector3 dir)
    {
        Projectile projectile = PoolManager.Instance.GetFromPool<Projectile>(dir == Vector3.zero ? "Projectile" : "Projectile_Rigid");

        projectile.SetProjectile(sprite, weapon, dir);

        return projectile;
    }
    public void Destroy_Projectile(Projectile projectile)
    {
        PoolManager.Instance.TakeToPool<Projectile>(projectile);
    }
}
