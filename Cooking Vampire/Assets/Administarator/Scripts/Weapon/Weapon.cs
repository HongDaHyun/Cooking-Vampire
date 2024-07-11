using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [ReadOnly] public int lv;
    public int count;
    public WeaponStat stat;
    public int GetDamage()
    {
        return stat.damage;
    }

    protected DataManager dataManager;
    protected SpawnManager spawnManager;

    private void Start()
    {
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

    private void Update()
    {
        Move();
    }
    protected abstract void Move();

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
    public int per; // ฐüล๋ทย
    public float speed;
}