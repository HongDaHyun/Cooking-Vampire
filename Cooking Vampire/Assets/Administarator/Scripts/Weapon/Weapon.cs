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

    [Title("½ºÅÈ")]
    [ReadOnly] public int lv;
    [ReadOnly] public bool isMax;
    public int count;
    public float coolTime, activeTime;
    public int damage;
    public float speed;
    public int per; // °üÅë·Â

    [Title("·¹º§")]
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

        damage += bonus;
        speed += bonus;
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

        foreach (Update update in weaponPerLevels[lv - 1].updates)
            StatUp(update.type, update.amount);

        lv++;
    }
    public void StatUp(UpdateType type, float amount)
    {
        switch(type)
        {
            case UpdateType.Count:
                count += (int)amount;
                break;
            case UpdateType.CoolTime:
                coolTime -= amount;
                break;
            case UpdateType.ActiveTime:
                activeTime += amount;
                break;
            case UpdateType.Damage:
                damage += (int)amount;
                break;
            case UpdateType.Speed: // % »ó½Â (10% »ó½Â)
                speed += speed * amount / 100f;
                break;
            case UpdateType.Per:
                per += (int)amount;
                break;
        }
    }
    protected abstract void MaxLevel();

    public string Export_LevelDiscription()
    {
        if (lv == 0)
            return discription;
        else if(lv < weaponPerLevels.Length + 1)
        {
            string sum = "";

            Update[] updates = weaponPerLevels[lv - 1].updates;

            foreach(Update update in updates)
            {
                string element = "";

                element += uiManager.Export_UpdateString(update.type, update.amount);

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
public struct WeaponLevel
{
    public Update[] updates;
}
[System.Serializable]
public struct Update
{
    public UpdateType type;
    public float amount;
}
public enum UpdateType { Count = 0, CoolTime, ActiveTime, Damage, Speed, Per }