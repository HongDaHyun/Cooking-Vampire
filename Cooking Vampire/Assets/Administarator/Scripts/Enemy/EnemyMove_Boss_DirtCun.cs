using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Boss_DirtCun : EnemyMove_Boss
{
    BtnManager bm;

    bool isRolling;
    public int splitCount;
    public float scale;

    protected override void Start()
    {
        base.Start();
        bm = BtnManager.Instance;
        scale = transform.localScale.x;
    }

    public void SetSplit(int count)
    {
        splitCount = count;
        scale = Mathf.Max(1f, scale / 2f);
    }

    public void Split()
    {
        splitCount--;

        EnemyData copyData = new EnemyData(enemy.data);
        copyData.defHP = Mathf.CeilToInt(copyData.defHP / 3f);

        enemy.spawnManager.Spawn_Enemy(copyData, transform.position + new Vector3(scale, 0f), scale / 2f).GetComponent<EnemyMove_Boss_DirtCun>().SetSplit(splitCount);
        enemy.spawnManager.Spawn_Enemy(copyData, transform.position + new Vector3(-scale, 0f), scale / 2f).GetComponent<EnemyMove_Boss_DirtCun>().SetSplit(splitCount);
        enemy.spawnManager.Destroy_Enemy(enemy);
    }

    public IEnumerator RollingShotRoutine()
    {
        isRolling = true;
        Enemy_Projectile_Sprite sprite = enemy.spriteData.Export_Enemy_Projectile_Sprite(enemy.data.title);
        Vector3 startPos = transform.position + new Vector3(0, scale);

        while (isRolling)
        {
            float delay = Random.Range(0.01f, 0.04f);
            float angle = Random.Range(0, 360f);
            float ranSize = Random.Range(scale / 2f, scale);

            Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(sprite.sprite, sprite.anim, startPos, ranSize);
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
        int dirtCount = Mathf.RoundToInt(15 * scale);

        for(int i = 0; i < dirtCount; i++)
        {
            Vector2 ranPos = enemy.Get_Enemy_RoundPos(2.5f * scale);
            float ranSize = Random.Range(scale / 2f, scale);

            enemy.spawnManager.Spawn_Effect("Dirt", ranPos, ranSize);
        }
        bm.ShakeCamera();
    }
}
