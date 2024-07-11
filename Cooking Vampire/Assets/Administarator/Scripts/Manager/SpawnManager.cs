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

    public Projectile Spawn_Projectile(Sprite sprite, Weapon weapon)
    {
        Projectile projectile = PoolManager.Instance.GetFromPool<Projectile>("Projectile");

        projectile.SetProjectile(sprite, weapon);

        return projectile;
    }
}
