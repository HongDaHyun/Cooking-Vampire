using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public int tier; // 1���� ����
    [ReadOnly] public bool isDead;
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
        stat.curHp -= dmg;
        spawnManager.Spawn_PopUpTxt(dmg, transform.position);
        StartCoroutine(enemyMove.KnockBack());

        // ����
        if (stat.curHp > 0)
        {
            anim.SetTrigger("Damaged");
        }

        // ����
        else
        {
            isDead = true;
            col.enabled = false;
            rigid.simulated = false;
            anim.SetTrigger("Dead");

            // ������ ó��
            gm.killCount++;
            spawnManager.Spawn_Gems(difficult * tier, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile") || isDead)
            return;

        int trueDmg = Mathf.Min(stat.curHp , collision.GetComponent<Projectile>().stat.damage);
        Damaged(trueDmg);
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