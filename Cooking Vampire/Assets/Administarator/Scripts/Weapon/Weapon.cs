using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public PlayerType[] availPlayers;
    public bool isAvail(PlayerType type)
    {
        return System.Array.Exists(availPlayers, x => x == type);
    }
    [ReadOnly] public int lv;
    public int count;
    public float coolTime, activeTime;
    public WeaponStat stat;
    public int per; // ฐüล๋ทย
    public int GetDamage()
    {
        return stat.damage;
    }

    protected DataManager dataManager;
    protected SpawnManager spawnManager;
    protected Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        dataManager = DataManager.Instance;
        spawnManager = SpawnManager.Instance;

        SetPivot();
        SetStat();

        Batch();
    }
    private void SetPivot()
    {
        switch(dataManager.curPlayer)
        {
            case PlayerType.Knight:
                transform.localPosition = new Vector2(0.05f, 0.5f);
                break;
            case PlayerType.Archer:
                break;
            case PlayerType.Ninja:
                break;
            case PlayerType.Magician:
                break;
        }
    }
    private void SetStat()
    {
        int bonus = dataManager.curWeapon.tier;

        stat.damage += bonus;
        stat.speed += bonus;
    }

    public abstract void Active();

    protected abstract void Batch();

    public virtual void LevelUp()
    {
        LevelContents();
        lv++;
    }
    protected abstract void LevelContents();
}

[System.Serializable]
public struct WeaponStat
{
    public int damage;
    public float speed;
}