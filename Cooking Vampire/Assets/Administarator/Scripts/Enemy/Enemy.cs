using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public int tier; // 1부터 시작
    [ReadOnly] public bool isDead, isDamaged;
    private int difficult;
    public EnemyStat stat;

    EnemyMove enemyMove;
    [HideInInspector] public Animator anim;
    Collider2D col;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer sr;

    [HideInInspector] public GameManager_Survivor gm;
    DataManager dataManager;
    SpawnManager spawnManager;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        dataManager = DataManager.Instance;
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEnemy(int _tier)
    {
        tier = _tier;

        EnemyData data = dataManager.Export_EnemyData(tier);
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

    private void SetStat(EnemyData data)
    {
        difficult = Mathf.Max(1, Mathf.RoundToInt(gm.curGameTime / 10));
        stat.maxHp = (10 + difficult) * tier;
        stat.curHp = stat.maxHp;
        stat.speed = data.speed;
        isDead = false;
    }

    public void Damaged(int dmg)
    {
        int ranCrit = Random.Range(0, 100);
        bool isCrit = false;
        if (ranCrit < gm.stat.Get_Value(StatType.CRIT, 0))
        {
            dmg *= 2;
            isCrit = true;
        }

        stat.curHp -= dmg;
        spawnManager.Spawn_PopUpTxt(dmg, transform.position, isCrit);
        StartCoroutine(enemyMove.KnockBack());

        // 생존
        if (stat.curHp > 0)
        {
            isDamaged = true;
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
            spawnManager.Spawn_Gems(difficult * tier, transform.position);
        }
    }
    private void Crit(ref int dmg)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile") || isDead || isDamaged)
            return;

        int trueDmg = Mathf.Min(stat.curHp , gm.stat.Get_Value(StatType.DMG, collision.GetComponent<Projectile>().stat.damage));
        Damaged(trueDmg);
    }

    private IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(1f);

        spawnManager.Destroy_Enemy(this);
    }

    private IEnumerator DamagedRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        isDamaged = false;
    }
}

[System.Serializable]
public struct EnemyStat
{
    public int curHp;
    public int maxHp;
    public float speed;
}