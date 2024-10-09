using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Shrew : EnemyMove_Boss
{
    static float distance = 5f;
    static float diagonalDistance = distance / Mathf.Sqrt(2);
    static Vector2[] diagonalDir = new Vector2[] { new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized, new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized };
    static Vector2[] udlrDir = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    protected override IEnumerator SpecialMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(SetRanDelay());

            isForceStop = true; isPattern = true;
            SetRanPattern();
            enemy.anim.SetTrigger($"Pattern_{curPatternInt}");

            yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName($"Pattern_{curPatternInt}"));
            yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName($"Pattern_{curPatternInt}"));
            if(curPatternInt == 2)
            {
                yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern_3"));
                yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern_3"));
            }

            curPatternInt = 0;
            isForceStop = false; isPattern = false;
        }
    }

    IEnumerator ShotTooth()
    {
        Vector2 pos = player.transform.position;

        // 방향과 오프셋을 정의
        bool isDiagonal = Random.Range(0, 2) == 1;
        Vector2[] directions = isDiagonal ? diagonalDir : udlrDir;
        Vector2[] offsets = new Vector2[directions.Length];

        // 오프셋 계산
        for (int i = 0; i < directions.Length; i++)
        {
            offsets[i] = isDiagonal ? directions[i] * diagonalDistance : directions[i] * distance;
        }

        Projectile_Enemy[] projectiles = new Projectile_Enemy[directions.Length];

        // 투사체 생성 및 설정
        for (int i = 0; i < directions.Length; i++)
        {
            projectiles[i] = enemy.spawnManager.Spawn_Projectile_Enemy(projectileSprite.sprite, 0.8f, projectileSprite.anim, pos + offsets[i], 0f);
            projectiles[i].SetRotation(directions[i] * -1);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < directions.Length; i++)
            projectiles[i].SetDir(directions[i] * -1, distance);
    }

    void Teleport()
    {
        transform.position = player.transform.position;
    }
}
