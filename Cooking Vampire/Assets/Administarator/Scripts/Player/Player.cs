using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public MoveController moveController;
    [HideInInspector] public WeaponController weaponController;
    [HideInInspector] public Scanner scanner;
    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer sr;
    public GameObject shield_Effect;
    Rigidbody2D rigid;

    private Coroutine hitRoutine;
    private bool isHit;
    private int shieldCount;
    public bool isDead;

    [HideInInspector] public GameManager_Survivor gm;
    private DataManager dataManager;
    private SpawnManager spawnManager;
    [ReadOnly] public PlayerData data;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        weaponController = GetComponentInChildren<WeaponController>();
        scanner = GetComponent<Scanner>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;

        data = dataManager.Export_PlayerData();

        anim.runtimeAnimatorController = data.animator;

        StartCoroutine(HealRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy_Projectile"))
            return;

        Hitted(gm.Get_TimeDifficult());
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy_Projectile"))
            return;

        Hitted(gm.Get_TimeDifficult());
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        Hitted(gm.Get_TimeDifficult());
    }

    public void Hitted(int dmg)
    {
        if (isHit)
            return;

        hitRoutine = StartCoroutine(HitRoutine());

        // È¸ÇÇ
        if (dataManager.Get_Ran(gm.stat.Get_Value(StatType.MISS)))
            return;

        if(shieldCount > 0)
        {
            ReduceShield();
            return;
        }

        int defendDmg = Mathf.RoundToInt(dmg * (1 - gm.stat.Cal_Defense()));
        gm.health -= Mathf.Min(gm.health, defendDmg);

        if(gm.health <= 0)
        {
            Dead();
        }
        else
        {
            anim.SetTrigger("Damaged");
        }
    }
    private void Dead()
    {
        isDead = true;
        rigid.simulated = false;

        weaponController.gameObject.SetActive(false);

        anim.SetTrigger("Dead");
    }

    private IEnumerator HitRoutine()
    {
        if (hitRoutine != null)
            yield break;

        isHit = true;
        yield return new WaitForSeconds(0.2f);

        isHit = false;
        hitRoutine = null;
    }
    private IEnumerator HealRoutine()
    {
        while(!isDead)
        {
            int amount = gm.stat.Get_Value(StatType.HEAL);

            if (amount <= 0)
                yield return new WaitForSeconds(1f);
            else
            {
                gm.Player_HealHP(1);
                yield return new WaitForSeconds(5f / (1 + (amount - 1) / 2.25f));
            }
        }
    }

    public void GetShield(int amount)
    {
        shieldCount += amount;

        if (!shield_Effect.activeSelf)
            shield_Effect.SetActive(true);
    }
    private void ReduceShield()
    {
        shieldCount--;
        spawnManager.Spawn_PopUpTxt("BLOCK", PopUpType.Block, transform.position);

        if(shieldCount <= 0)
        {
            shieldCount = 0;
            shield_Effect.SetActive(false);
        }
    }
}

[System.Serializable]
public struct PlayerStat
{
    public int dmg_p;
    public int defense;
    public int maxHealth;
    public int speed_p;
    public int miss_p;
    public int crit_p;
    public int luck;
    public int expBonus_p;
    public int active_p;
    public int cool_p;
    public int heal;
    public int drain_p;
    public int proSize_p;
    public int proSpeed_p;
    public int count;
    public int element;
    public int range;
    public int knockBack;
    public int per;

    public int Get_Value(StatType type, int def)
    {
        StatData data = CSVManager.Instance.Find_StatCSV(type);

        int upValue = 0;

        switch(type)
        {
            case StatType.DMG:
                upValue = dmg_p;
                break;
            case StatType.DEF:
                upValue = defense;
                break;
            case StatType.HP:
                upValue = maxHealth;
                break;
            case StatType.SPEED:
                upValue = speed_p;
                break;
            case StatType.MISS:
                upValue = miss_p;
                break;
            case StatType.CRIT:
                upValue = crit_p;
                break;
            case StatType.LUCK:
                upValue = luck;
                break;
            case StatType.EXP:
                upValue = expBonus_p;
                break;
            case StatType.ACTIVE:
                upValue = active_p;
                break;
            case StatType.COOL:
                upValue = cool_p;
                break;
            case StatType.HEAL:
                upValue = heal;
                break;
            case StatType.DRAIN:
                upValue = drain_p;
                break;
            case StatType.PRO_SIZE:
                upValue = proSize_p;
                break;
            case StatType.PRO_SPEED:
                upValue = proSpeed_p;
                break;
            case StatType.COUNT:
                upValue = count;
                break;
            case StatType.ELE:
                upValue = element;
                break;
            case StatType.RANGE:
                upValue = range;
                break;
            case StatType.BACK:
                upValue = knockBack;
                break;
            case StatType.PER:
                upValue = per;
                break;
        }

        def = data.isPercent ? def + Mathf.RoundToInt(def * upValue / 100f) : def + upValue;
        Limit_Value(data, ref def);
        return def;
    }
    public float Get_Value(StatType type, float def)
    {
        StatData data = CSVManager.Instance.Find_StatCSV(type);

        int upValue = 0;

        switch (type)
        {
            case StatType.DMG:
                upValue = dmg_p;
                break;
            case StatType.DEF:
                upValue = defense;
                break;
            case StatType.HP:
                upValue = maxHealth;
                break;
            case StatType.SPEED:
                upValue = speed_p;
                break;
            case StatType.MISS:
                upValue = miss_p;
                break;
            case StatType.CRIT:
                upValue = crit_p;
                break;
            case StatType.LUCK:
                upValue = luck;
                break;
            case StatType.EXP:
                upValue = expBonus_p;
                break;
            case StatType.ACTIVE:
                upValue = active_p;
                break;
            case StatType.COOL:
                upValue = cool_p;
                break;
            case StatType.HEAL:
                upValue = heal;
                break;
            case StatType.DRAIN:
                upValue = drain_p;
                break;
            case StatType.PRO_SIZE:
                upValue = proSize_p;
                break;
            case StatType.PRO_SPEED:
                upValue = proSpeed_p;
                break;
            case StatType.COUNT:
                upValue = count;
                break;
            case StatType.ELE:
                upValue = element;
                break;
            case StatType.RANGE:
                upValue = range;
                break;
            case StatType.BACK:
                upValue = knockBack;
                break;
            case StatType.PER:
                upValue = per;
                break;
        }

        def = data.isPercent ? def + def * upValue / 100f : upValue;
        Limit_Value(data, ref def);

        return def;
    }
    public int Get_Value(StatType type)
    {
        StatData data = CSVManager.Instance.Find_StatCSV(type);

        int def = 0;

        switch (type)
        {
            case StatType.DMG:
                def = dmg_p;
                break;
            case StatType.DEF:
                def = defense;
                break;
            case StatType.HP:
                def = maxHealth;
                break;
            case StatType.SPEED:
                def = speed_p;
                break;
            case StatType.MISS:
                def = miss_p;
                break;
            case StatType.CRIT:
                def = crit_p;
                break;
            case StatType.LUCK:
                def = luck;
                break;
            case StatType.EXP:
                def = expBonus_p;
                break;
            case StatType.ACTIVE:
                def = active_p;
                break;
            case StatType.COOL:
                def = cool_p;
                break;
            case StatType.HEAL:
                def = heal;
                break;
            case StatType.DRAIN:
                def = drain_p;
                break;
            case StatType.PRO_SIZE:
                def = proSize_p;
                break;
            case StatType.PRO_SPEED:
                def = proSpeed_p;
                break;
            case StatType.COUNT:
                def = count;
                break;
            case StatType.ELE:
                def = element;
                break;
            case StatType.RANGE:
                def = range;
                break;
            case StatType.BACK:
                def = knockBack;
                break;
            case StatType.PER:
                def = per;
                break;
        }

        Limit_Value(data, ref def);
        return def;
    }
    private void Limit_Value(StatData data, ref int def)
    {
        int NULL = CSVManager.NULL;
        if (data.maxAmount != NULL)
            Mathf.Min(def, data.maxAmount);
        if (data.minAmount != NULL)
            Mathf.Max(def, data.minAmount);
    }
    private void Limit_Value(StatData data, ref float def)
    {
        int NULL = CSVManager.NULL;
        if (data.maxAmount != NULL)
            Mathf.Min(def, data.maxAmount);
        if (data.minAmount != NULL)
            Mathf.Max(def, data.minAmount);
    }

    public float Cal_Defense()
    {
        return (float)defense / (Mathf.Abs(defense) + 15);
    }
}