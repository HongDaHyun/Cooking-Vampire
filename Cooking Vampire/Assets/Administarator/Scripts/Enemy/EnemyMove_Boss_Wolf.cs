using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Wolf : EnemyMove_Boss
{
    bool rangeFlip;

    void SpawnBabyWolf()
    {
        int ranCount = Random.Range(3, 6); // 3~5 �������� ����

        for(int i = 0; i < ranCount; i++)
        {
            enemy.spawnManager.Spawn_Effect_X("��������", GetRanPos(), 1f);
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
            rangeFlip = !rangeFlip;
            Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(sprite.sprite, 1f, sprite.anim, transform);
            // ������ �̿��Ͽ� ������ ���� (2D ȯ��, Z�� ���� ȸ��)
            int plusAngle = rangeFlip ? 90 : 0;
            float angle = i * (360f / count) + plusAngle;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0); // 2D ���� ����

            projectile.SetDir(dir, 5f);
            projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }
    }
}
