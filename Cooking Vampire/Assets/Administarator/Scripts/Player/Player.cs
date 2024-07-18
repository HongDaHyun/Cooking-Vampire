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
    public int defense;
    public int maxHealth;
    public float speed; // 완
    public int missPercent;
    public int critPercent;
    public int luck;
    public float expBonus; // 완
    public int heal;
    public int drainPercent;
    public float range; // 완
    public float knockBack; // 완
}