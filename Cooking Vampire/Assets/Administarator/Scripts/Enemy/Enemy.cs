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

    private void SetStat(EnemyData _data)
    {
        data = _data;

        difficult = Mathf.Max(1, Mathf.RoundToInt(gm.curGameTime / 10));
        stat.maxHp = (10 + difficult) * tier;
        stat.curHp = stat.maxHp;
        stat.speed = _data.speed;
        isDead = false;
    }

    public void Damaged(int dmg)
    {
        // 크리티컬
        bool isCrit = dataManager.Get_Ran(gm.stat.Get_Value(StatType.CRIT));
        if (isCrit)
            dmg *= 2;

        // 체력 흡수
        if (dataManager.Get_Ran(gm.stat.Get_Value(StatType.DRAIN)))
            gm.Player_HealHP(1);

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