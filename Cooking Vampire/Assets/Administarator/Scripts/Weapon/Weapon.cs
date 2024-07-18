using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int ID;

    [Title("UI")]
    public Sprite icon;
    public string title;
    [TextArea] public string discription;

    [Title("스탯")]
    [ReadOnly] public int lv;
    [ReadOnly] public bool isMax;
    public WeaponStat stat;

    [Title("레벨")]
    public WeaponLevel[] weaponPerLevels;

    protected UIManager uiManager;
    protected DataManager dataManager;
    protected SpawnManager spawnManager;
    protected Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        dataManager = DataManager.Instance;
        spawnManager = SpawnManager.Instance;
        uiManager = UIManager.Instance;

        isMax = false;

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

    public abstract IEnumerator Active();

    protected abstract void Batch();

    public virtual void LevelUp()
    {
        if(lv >=  weaponPerLevels.Length + 1)
        {
            isMax = true;
            MaxLevel();
            return;
        }

        foreach (BonusStat update in weaponPerLevels[lv - 1].updates)
            update.Update_Stat(ref stat);

        lv++;
    }
    
    protected abstract void MaxLevel();

    public string Export_LevelDiscription()
    {
        if (lv == 0)
            return discription;
        else if(lv < weaponPerLevels.Length + 1)
        {
            string sum = "";

            BonusStat[] updates = weaponPerLevels[lv - 1].updates;

            foreach(BonusStat update in updates)
            {
                string element = "";

                element += update.Get_Discription();

                if (update.type != updates[updates.Length - 1].type)
                    element += "\n";

                sum += element;
            }

            return sum;
        }
        else
        {
            //Max
            return "MAX";
        }
    }
}

[System.Serializable]
public struct WeaponStat
{
    public int count;
    public float coolTime, activeTime;
    public int damage;
    public float speed;
    public int per; // 관통력
}

[System.Serializable]
public struct WeaponLevel
{
    public BonusStat[] updates;
}