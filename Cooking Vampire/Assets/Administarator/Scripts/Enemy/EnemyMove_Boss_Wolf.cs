using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Wolf : EnemyMove_Boss
{
    bool rangeFlip;

    void SpawnBabyWolf()
    {
        int ranCount = Random.Range(3, 6); // 3~5 새끼늑대 스폰

        for(int i = 0; i < ranCount; i++)
        {
            enemy.spawnManager.Spawn_Effect_X("새끼늑대", GetRanPos(), 1f);
        }
    }

    Vector2 GetRanPos()
    {
        Vector2 curTrans = transform.position;
        Vector2 ranPos = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        return curTrans + ranPos;
    }

    void ShootRange()
    {
        int count = 5;

        Enemy_Projectile_Sprite sprite = enemy.spriteData.Export_Enemy_Projectile_Sprite(enemy.data.title);

        for (int i = 0; i < count; i++)
        {
            Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(sprite.sprite,sprite.anim, transform.position);
            // 각도를 이용하여 방향을 설정 (2D 환경, Z축 기준 회전)
            float angle = i * (360f / count) + 90f; // 90도에서 시작
            if (rangeFlip) angle = -angle;

            projectile.SetDir(angle, 5f);
            projectile.SetRotation(angle);
        }

        rangeFlip = !rangeFlip;
    }
}
