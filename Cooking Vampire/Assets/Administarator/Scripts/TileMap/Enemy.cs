using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public int tier; // 1부터 시작
    [ReadOnly] public bool isDead;
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
        col.enabled = true;
        enemyMove.ReSet();
    }

    private void SetStat(EnemyData data)
    {
        stat.maxHp = 10 + Mathf.RoundToInt(gm.curGameTime * tier);
        stat.curHp = stat.maxHp;
        stat.speed = data.speed;
        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile") || isDead)
            return;

        stat.curHp -= collision.GetComponent<Projectile>().weapon.damage;
        StartCoroutine(enemyMove.KnockBack());

        // 생존
        if(stat.curHp > 0)
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
            gm.Player_GainExp(1); // 후에 젤리로 바꿈
        }
    }

    private IEnumerator DeadRoutine()
    {
        yield return new WaitForSeconds(1f);

        spawnManager.Destroy_Enemy(this);
    }
}

[System.Serializable]
public struct EnemyStat
{
    public int curHp;
    public int maxHp;
    public float speed;
}