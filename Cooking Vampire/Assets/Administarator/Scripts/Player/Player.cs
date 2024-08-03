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

    [HideInInspector] public GameManager_Survivor gm;
    private DataManager dataManager;
    [ReadOnly] public PlayerData data;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        weaponController = GetComponentInChildren<WeaponController>();
        scanner = GetComponent<Scanner>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        gm = GameManager_Survivor.Instance;

        data = dataManager.Export_PlayerData();

        anim.runtimeAnimatorController = data.animator;
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

    public int Get_DMG(int def)
    {
        return def + Mathf.RoundToInt(def * dmg_p / 100f);
    }
    public float Get_SPEED(float def)
    {
        return def + def * speed_p / 100f;
    }
    public int Get_EXPBONUS(int def)
    {
        return def + Mathf.RoundToInt(def * expBonus_p / 100f);
    }
    public float Get_ACTIVE(float def)
    {
        if (def < 0)
            return -1;
        return def + def * active_p / 100f;
    }
    public float Get_COOL(float def)
    {
        if (def < 0)
            return -1;
        return def + def * cool_p / 100f;
    }
    public float Get_PRO_SIZE(float def)
    {
        return def + def * proSize_p / 100f;
    }
    public float Get_PRO_SPEED(float def)
    {
        if (def < 0)
            return -1;
        return def + def * proSpeed_p / 100f;
    }
    public int Get_COUNT(int def)
    {
        return def + count;
    }
    public float Get_RANGE()
    {
        float def = GameManager_Survivor.Instance.player.scanner.defRange;
        return def + def * range / 100f;
    }
    public float Get_RANGE(float def)
    {
        return def + def * range / 100f;
    }
    public int Get_Per(int def)
    {
        if (def < 0)
            return -1;
        return def + per;
    }
}