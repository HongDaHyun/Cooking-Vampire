using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Shrew : EnemyMove_Boss
{
    float distance = 5f;

    IEnumerator ShotTooth()
    {
        Vector2 pos = player.transform.position;

        // ����� �������� ����
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2[] offsets = { new Vector2(0, distance), new Vector2(0, -distance), new Vector2(-distance, 0), new Vector2(distance, 0) };

        // �ݺ����� ���� �� ����ü�� ����
        Projectile_Enemy[] projectiles = new Projectile_Enemy[directions.Length];
        for (int i = 0; i < directions.Length; i++)
        {
            projectiles[i] = enemy.spawnManager.Spawn_Projectile_Enemy(
                projectileSprite.sprite,
                1f,
                projectileSprite.anim,
                pos + offsets[i],
                0f
            );
        }

        yield return new WaitForSeconds(0.2f);

        // ������ ����ü�� ����� ȸ�� ����
        for (int i = 0; i < directions.Length; i++)
        {
            projectiles[i].SetDir(directions[i] * -1, distance); // �ݴ� �������� ����
            projectiles[i].SetRotation(directions[i] * -1);
        }
    }
}
