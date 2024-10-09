using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Shrew : EnemyMove_Boss
{
    float distance = 5f;

    IEnumerator ShotTooth()
    {
        Vector2 pos = player.transform.position;

        // 방향과 오프셋을 정의
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2[] offsets = { new Vector2(0, distance), new Vector2(0, -distance), new Vector2(-distance, 0), new Vector2(distance, 0) };

        // 반복문을 통해 적 투사체를 생성
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

        // 생성된 투사체의 방향과 회전 설정
        for (int i = 0; i < directions.Length; i++)
        {
            projectiles[i].SetDir(directions[i] * -1, distance); // 반대 방향으로 설정
            projectiles[i].SetRotation(directions[i] * -1);
        }
    }
}
