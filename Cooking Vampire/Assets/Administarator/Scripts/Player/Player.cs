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
    Spawner spawner;
    Rigidbody2D rigid;

    private Coroutine hitRoutine;
    private bool isHit;
    public bool isDead;

    [HideInInspector] public GameManager_Survivor gm;
    private DataManager dataManager;
    [ReadOnly] public PlayerData data;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        weaponController = GetComponentInChildren<WeaponController>();
        scanner = GetComponent<Scanner>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spawner = GetComponentInChildren<Spawner>();
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        gm = GameManager_Survivor.Instance;

        data = dataManager.Export_PlayerData();

        anim.runtimeAnimatorController = data.animator;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isHit)
        {
            Hitted(Mathf.Max(1, (int)(gm.curGameTime / 10f)));
        }
    }

    private void Hitted(int dmg)
    {
        hitRoutine = StartCoroutine(HitRoutine());

        // 회피
        int ranMiss = Random.Range(0, 100);
        if (ranMiss < gm.stat.Get_Value(StatType.MISS, 0))
            return;

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

        spawner.gameObject.SetActive(false);
        weaponController.gameObject.SetActive(false);

        anim.SetTrigger("Dead");
        Debug.Log("게임오버");
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
}

[System.Serializable]
public struct PlayerStat
{
    public int dmg_p;
    public int defense;
    public int maxHealth;
    public int speed_p; // 완
    public int miss_p;
    public int crit_p;
    public int luck;
    public int expBonus_p; // 완
    public int active_p;
    public int cool_p;
    public int heal;
    public int drain_p;
    public int proSize_p;
    public int proSpeed_p;
    public int count;
    public int element;
    public int range; // 완
    public int knockBack; // 완
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

        def = data.isPercent ? def + def * upValue : upValue;
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