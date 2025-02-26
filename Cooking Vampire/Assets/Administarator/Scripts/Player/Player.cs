using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public MoveController moveController;
    [HideInInspector] public AtkController atkController;
    [HideInInspector] public Scanner scanner;
    [HideInInspector] public Animator anim;
    [HideInInspector] public SpriteRenderer sr;
    public GameObject shield_Effect;
    public GameObject relic2;
    Rigidbody2D rigid;

    private Coroutine hitRoutine;
    private bool isHit;
    private bool isSlow;
    private int shieldCount;
    [HideInInspector] public int rebornCount;
    public bool isDead;

    [HideInInspector] public GameManager_Survivor gm;
    private DataManager dataManager;
    private SpawnManager spawnManager;
    [ReadOnly] public PlayerData data;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        atkController = GetComponentInChildren<AtkController>();
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

        Hitted(Mathf.Max(1, gm.Get_TimeDifficult()));
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy_Projectile"))
            return;

        Hitted(Mathf.Max(1, gm.Get_TimeDifficult()));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        Hitted(Mathf.Max(1, gm.Get_TimeDifficult()));
    }

    public void Hitted(int dmg)
    {
        if (isHit)
            return;

        hitRoutine = StartCoroutine(HitRoutine());

        // È¸ÇÇ
        if (gm.stat.Cal_MIS_PERCENT())
            return;

        if(shieldCount > 0)
        {
            ReduceShield();
            return;
        }

        int defendDmg = Mathf.RoundToInt(dmg * (1 - gm.stat.Cal_DEF()));
        gm.stat.curHP -= Mathf.Min(gm.stat.curHP, defendDmg);

        if(gm.stat.curHP <= 0)
            Dead();
        else
            anim.SetTrigger("Damaged");
    }
    private void Dead()
    {
        if (rebornCount > 0)
        {
            rebornCount--;
            spawnManager.Spawn_Effect("Reborn", transform.position, 1f);
            gm.stat.HealHP(gm.stat.GetStat(StatID_Player.HP, true));
            return;
        }

        isDead = true;
        rigid.simulated = false;
        sr.sortingOrder += 1;

        atkController.gameObject.SetActive(false);

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
            if (gm.stat.HPREG > 0)
            {
                gm.stat.HealHP(1);
                yield return new WaitForSeconds(gm.stat.Cal_HPREG_Cool());
            }
            else
                yield return new WaitForSeconds(1f);
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

    public void GetSlow(float amount, float time)
    {
        if (isSlow || amount > 1f)
            return;

        StartCoroutine(SlowRoutine(amount, time));
    }
    private IEnumerator SlowRoutine(float amount, float time)
    {
        isSlow = true;
        gm.stat.curSpeed *= (1 - amount);

        yield return new WaitForSeconds(time);

        isSlow = false;
        gm.stat.curSpeed = gm.stat.defSpeed;
    }

    public Vector2 Get_Player_RoundPos(float noise)
    {
        Vector2 playerPos = transform.position;

        return new Vector2(playerPos.x + Random.Range(-noise, noise), playerPos.y + Random.Range(-noise, noise));
    }
}
