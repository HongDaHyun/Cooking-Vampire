using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public bool isDead, isDamaged;
    [ReadOnly] public EnemyEleHit[] enemyEleHits;
    private int difficult;
    public EnemyStat stat;
    public EnemyData data;
    private Coroutine hitRoutine;

    [HideInInspector] public EnemyMove enemyMove;
    [HideInInspector] public Animator anim;
    CapsuleCollider2D col;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer sr;

    [HideInInspector] public GameManager_Survivor gm;
    [HideInInspector] public DataManager dataManager;
    [HideInInspector] public SpawnManager spawnManager;
    [HideInInspector] public SpriteData spriteData;
    RelicManager relicManager;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        dataManager = DataManager.Instance;
        spriteData = SpriteData.Instance;
        relicManager = RelicManager.Instance;

        col = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }

    public void OnGettingFromPool()
    {
        enemyEleHits = new EnemyEleHit[4]
        {
            new EnemyEleHit(EleType.Fire),
            new EnemyEleHit(EleType.Ice),
            new EnemyEleHit(EleType.Poison),
            new EnemyEleHit(EleType.Thunder),
        };
    }

    public void SetEnemy(EnemyData data, float size)
    {
        anim.runtimeAnimatorController = data.Export_RanAnim();

        SetStat(data, size);
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

    private void SetStat(EnemyData _data, float size)
    {
        data = _data;

        SetCol(data.colSetting, size);

        difficult = gm.Get_TimeDifficult();
        stat.maxHp = Mathf.Max(1, Mathf.RoundToInt((10 + difficult) * data.hpScale));
        stat.curHp = stat.maxHp;
        stat.speed = _data.speed;
        isDead = false;
    }
    private void SetCol(ColliderSetting setting, float size)
    {
        col.offset = setting.colOff;
        col.size = setting.colSize;
        transform.localScale = new Vector3(size, size, size);
    }

    public void Damaged(int dmg)
    {
        if (isDamaged || isDead)
            return;

        dmg = gm.stat.Cal_DMG(dmg);

        // 유물 2
        if(relicManager.IsHave(2))
        {
            RelicData relicData = dataManager.Export_RelicData(2);

            if (Vector2.Distance(transform.position, gm.player.transform.position) <= relicData.specialContent.FindSpecialContent(StatID_Player.RAN).CalDef())
                dmg = relicData.specialContent.FindSpecialContent(StatID_Player.DMG).CalAmount(dmg);
        }
        if (relicManager.IsHave(32))
        {
            EleRoutine(EleType.Thunder, 1);
            spawnManager.Spawn_ChainThunder(3, transform.position);
        }
        EleRoutine(EleType.Poison, 5);

        // 크리티컬
        bool isCrit = gm.stat.Cal_CRIT_Percent();
        if (isCrit)
            dmg = gm.stat.Cal_CRIT_DMG(dmg);

        // 체력 흡수
        if (gm.stat.Cal_DRA_Percent())
            gm.stat.HealHP(1);

        stat.curHp -= Mathf.Min(stat.curHp, dmg);
        spawnManager.Spawn_PopUpTxt(dmg.ToString(), isCrit ? PopUpType.Deal_Crit : PopUpType.Deal, transform.position);
        hitRoutine = StartCoroutine(DamagedRoutine());
        StartCoroutine(enemyMove.KnockBack());

        // 생존
        if (stat.curHp > 0)
        {
            if (!enemyMove.isPattern)
                anim.SetTrigger("Damaged");
        }

        // 죽음
        else
        {
            if (isCrit && relicManager.IsHave(39) && dataManager.Get_Ran(3))
                gm.stat.HealHP(1);

            Dead();
        }
    }
    public void DamagedEle(int dmg)
    {
        if (isDead)
            return;

        stat.curHp -= Mathf.Min(stat.curHp, dmg);
        spawnManager.Spawn_PopUpTxt(dmg.ToString(), PopUpType.Deal, transform.position);
        StartCoroutine(enemyMove.KnockBack());

        // 죽음
        if (stat.curHp <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        isDead = true;
        col.enabled = false;
        rigid.simulated = false;
        anim.SetTrigger("Dead");

        // 데이터 처리
        gm.killCount++;
        spawnManager.Spawn_Gems(Random.Range(data.gemAmount, data.gemAmount + gm.stat.LUK / 10), transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile") || isDead || isDamaged)
            return;

        Damaged(collision.GetComponent<Projectile>().stat.dmg);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌시 물리 영향 방지
        if(collision.gameObject.CompareTag("Enemy"))
            rigid.velocity = Vector3.zero;
    }

    public void Destroy()
    {
        if (data.atkType == AtkType.Box)
            spawnManager.Spawn_Box(transform.position);

        Effect[] effects = GetComponentsInChildren<Effect>();
        foreach (Effect effect in effects)
            spawnManager.Destroy_Effect(effect);

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
    public void EleRoutine(EleType type, int amount)
    {
        EnemyEleHit eleHit = Find_EleHit(type);
        if (amount >= eleHit.amount)
        {
            if (eleHit.eleRoutine != null)
                StopCoroutine(eleHit.eleRoutine);

            eleHit.amount = amount;
            eleHit.eleRoutine = StartCoroutine(eleHit.EleRoutine(this));
        }
    }
    public EnemyEleHit Find_EleHit(EleType type)
    {
        return System.Array.Find(enemyEleHits, ele => ele.type == type);
    }
}

[System.Serializable]
public struct EnemyStat
{
    public int curHp;
    public int maxHp;
    public float speed;
}
[System.Serializable]
public class EnemyEleHit
{
    public EleType type;
    public int amount;
    public Coroutine eleRoutine;

    public EnemyEleHit(EleType _type)
    {
        type = _type;
        amount = 0;
        eleRoutine = null;
    }

    public IEnumerator EleRoutine(Enemy enemy)
    {
        GameManager_Survivor gm = GameManager_Survivor.Instance;
        SpawnManager sm = SpawnManager.Instance;
        float calAmount = gm.stat.Cal_Ele(amount, type);

        switch(type)
        {
            case EleType.Fire:
                sm.Spawn_Effect_Loop("Fire", enemy.transform, 1f, Mathf.Min(calAmount, 5f));

                for (int i = 0; i < 5; i++)
                {
                    if (calAmount <= 0)
                        break;

                    enemy.DamagedEle((int)calAmount);
                    calAmount--;

                    yield return new WaitForSeconds(1f);
                }
                break;
            case EleType.Ice:
                sm.Spawn_Effect_Loop("Ice", enemy.transform, 1f, calAmount);
                enemy.enemyMove.curSpeed = enemy.stat.speed * 0.8f;
                yield return new WaitForSeconds(calAmount);
                enemy.enemyMove.curSpeed = enemy.stat.speed;
                break;
            case EleType.Poison:
                sm.Spawn_Effect_Loop("Poison", enemy.transform, 1f, Mathf.Min(calAmount, 4.8f));
                for (int i = 0; i < 4; i++)
                {
                    if (calAmount <= 0)
                        break;

                    enemy.DamagedEle((int)calAmount);

                    yield return new WaitForSeconds(1.2f);
                }
                break;
            case EleType.Thunder:
                enemy.DamagedEle((int)calAmount);
                yield return new WaitForSeconds(1f);
                break;
        }

        amount = 0;
        eleRoutine = null;
        yield break;
    }
}