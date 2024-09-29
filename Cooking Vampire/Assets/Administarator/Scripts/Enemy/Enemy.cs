using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public bool isDead, isDamaged;
    private int difficult;
    public EnemyStat stat;
    public EnemyData data;
    private Coroutine hitRoutine;

    EnemyMove enemyMove;
    [HideInInspector] public Animator anim;
    Collider2D col;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer sr;

    [HideInInspector] public GameManager_Survivor gm;
    DataManager dataManager;
    SpawnManager spawnManager;
    SpriteData spriteData;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        dataManager = DataManager.Instance;
        spriteData = SpriteData.Instance;

        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEnemy(EnemyData data)
    {
        anim.runtimeAnimatorController = data.Export_RanAnim();

        SetStat(data);
        ReSet();
    }

    private void ReSet()
    {
        anim.Rebind();
        anim.ResetTrigger("Dead");
        isDamaged = false;

        col.enabled = true;
        enemyMove.ReSet();
    }

    private void SetStat(EnemyData _data)
    {
        data = _data;

        difficult = gm.Get_TimeDifficult();
        stat.maxHp = Mathf.Max(1, Mathf.RoundToInt((10 + difficult) * data.hpScale));
        stat.curHp = stat.maxHp;
        stat.speed = _data.speed;
        isDead = false;
    }

    public void Damaged(int dmg)
    {
        if (isDamaged)
            return;

        // 크리티컬
        bool isCrit = dataManager.Get_Ran(gm.stat.Get_Value(StatType.CRIT));
        if (isCrit)
            dmg *= 2;

        // 체력 흡수
        if (dataManager.Get_Ran(gm.stat.Get_Value(StatType.DRAIN)))
            gm.Player_HealHP(1);

        stat.curHp -= dmg;
        spawnManager.Spawn_PopUpTxt(dmg.ToString(), isCrit ? PopUpType.Deal_Crit : PopUpType.Deal, transform.position);
        hitRoutine = StartCoroutine(DamagedRoutine());
        StartCoroutine(enemyMove.KnockBack());

        // 생존
        if (stat.curHp > 0)
        {
            anim.SetTrigger("Damaged");
        }

        // 죽음
        else
        {
            isDead = true;
            col.enabled = false;
            rigid.simulated = false;
            anim.SetTrigger("Dead");

            // 데이터 처리
            gm.killCount++;
            spawnManager.Spawn_Gems(difficult, transform.position);
        }
    }

    public void ShootArea()
    {
        Area_Sprite sprite = spriteData.Export_Area_Sprite(data.eleType);
        spawnManager.Spawn_Projectile_Area(1f, sprite.anim, transform);
    }
    public void ShootRange(Vector3 pos)
    {
        Enemy_Projectile_Sprite sprite = spriteData.Export_Enemy_Projectile_Sprite(data.title);
        Projectile_Enemy projectile = spawnManager.Spawn_Projectile_Enemy(sprite.sprite, 1f, sprite.anim, transform);

        Vector3 dir = (pos - transform.position).normalized;
        projectile.SetDir(dir, 5f);
        projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile") || isDead || isDamaged)
            return;

        int trueDmg = Mathf.Min(stat.curHp , gm.stat.Get_Value(StatType.DMG, collision.GetComponent<Projectile>().stat.damage));
        Damaged(trueDmg);
    }

    private void Destroy()
    {
        if (data.atkType == AtkType.Box)
            spawnManager.Spawn_Box(transform.position);

        spawnManager.Destroy_Enemy(this);
    }
    private IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(1f);

        Destroy();
    }
    private IEnumerator DamagedRoutine()
    {
        if (hitRoutine != null)
            yield break;

        isDamaged = true;
        yield return new WaitForSeconds(0.1f);

        isDamaged = false;
        hitRoutine = null;
    }
}

[System.Serializable]
public struct EnemyStat
{
    public int curHp;
    public int maxHp;
    public float speed;
}