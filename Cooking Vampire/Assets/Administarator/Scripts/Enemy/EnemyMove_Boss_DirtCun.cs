using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_DirtCun : EnemyMove_Boss
{
    BtnManager bm;

    bool isRolling;
    public int splitCount;

    protected override void Start()
    {
        base.Start();
        bm = BtnManager.Instance;
    }

    public void Split()
    {
        splitCount--;

        EnemyData copyData = enemy.data;
        copyData.defHP = Mathf.CeilToInt(copyData.defHP / 3f);
        float scale = transform.localScale.x;

        enemy.spawnManager.Spawn_Enemy(copyData, transform.position + new Vector3(scale, 0f), scale / 2f);
        enemy.spawnManager.Spawn_Enemy(copyData, transform.position + new Vector3(-scale, 0f), scale / 2f);
    }

    public IEnumerator RollingShotRoutine()
    {
        isRolling = true;
        Enemy_Projectile_Sprite sprite = enemy.spriteData.Export_Enemy_Projectile_Sprite(enemy.data.title);
        Vector3 startPos = transform.position + new Vector3(0, transform.localScale.y);

        while (isRolling)
        {
            float delay = Random.Range(0.01f, 0.04f);
            float angle = Random.Range(0, 360f);

            Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(sprite.sprite, sprite.anim, startPos);
            projectile.SetDir(angle, 5f);
            projectile.SetRotation(angle);

            yield return new WaitForSeconds(delay);
        }
    }
    public void RollingFinish()
    {
        isRolling = false;
    }

    public void PumpJump()
    {
        enemy.spawnManager.Spawn_Effect("Dirt", transform.position, 1f);
        bm.ShakeCamera();
    }
}
