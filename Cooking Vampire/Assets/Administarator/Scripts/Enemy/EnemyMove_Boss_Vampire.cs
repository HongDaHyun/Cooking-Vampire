using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_Vampire : EnemyMove_Boss
{
    bool isShotting;

    public void VampireDead()
    {
        enemy.spawnManager.Spawn_Enemy("¹ì¹ÚÁã", transform.position, 2f);
        enemy.Destroy();
    }

    IEnumerator BloodShot_Start()
    {
        isShotting = true;
        List<Projectile_Enemy> projectiles = new List<Projectile_Enemy>();
        float ranAngle;
        float ranSize;
        Vector2 direction = enemy.sr.flipX ? Vector2.left : Vector2.right;
        Vector3 startPos = transform.position + new Vector3(0, 0.8f);

        while (isShotting)
        {
            ranAngle = enemy.sr.flipX ? Random.Range(-80f, -30f) : Random.Range(30f, 80f);
            ranSize = Random.Range(0.5f, 1.3f);
            Vector2 dir = Quaternion.Euler(0, 0, ranAngle) * direction;

            Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(projectileSprite.sprite, projectileSprite.anim, startPos, ranSize);
            projectiles.Add(projectile);
            projectile.SetDir(dir.normalized, 8f);
            projectile.SetRotation(Vector2.up);
            yield return new WaitForFixedUpdate();
        }

        foreach (Projectile_Enemy p in projectiles)
            p.SetGravity(1f);
    }
    void BloodShot_End()
    {
        isShotting = false;
    }

    void BatSpawn()
    {
        int ranCount = Random.Range(12, 20); // 3~5 ¹ÚÁã ½ºÆù

        for (int i = 0; i < ranCount; i++)
        {
            enemy.spawnManager.Spawn_Effect_X("¹ÚÁã", GetRanPos(), 1f);
        }
    }
    Vector2 GetRanPos()
    {
        Vector2 curTrans = transform.position;
        Vector2 ranPos = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        return curTrans + ranPos;
    }
}
