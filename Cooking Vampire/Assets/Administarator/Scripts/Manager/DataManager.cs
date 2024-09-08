using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataManager : Singleton<DataManager>
{
    [Title("���� ������")]
    public StageType curStage;
    public PlayerType curPlayer;
    public WeaponData curWeapon;
    public int coin;

    [Title("���� ������")]
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public WeaponData[] weaponDatas;
    public DroptemData[] droptemDatas;

    public void EarnCoin(int amount)
    {
        coin += amount;
    }

    public PlayerData Export_PlayerData()
    {
        return Array.Find(playerDatas, data => data.type == curPlayer);
    }
    public EnemyData Export_EnemyData(string enemyName)
    {
        EnemyData[] datas = Array.FindAll(enemyDatas, data => data.title == enemyName);
        return datas[UnityEngine.Random.Range(0, datas.Length)];
    }
    public WeaponData Export_WeaponData(PlayerType type, int tier)
    {
        return Array.Find(weaponDatas, data => data.weaponType == type && data.tier == tier);
    }
    public DroptemData Export_DroptemData(string _name)
    {
        return Array.Find(droptemDatas, data => data.droptemName == _name);
    }
    public DroptemData Export_DroptemData_Ran()
    {
        Tier ranTier = GameManager_Survivor.Instance.Get_Tier();

        DroptemData[] dropArray = Array.FindAll(droptemDatas, data => data.tier == ranTier);
        
        // ���� ó��
        if (dropArray.Length == 0 || dropArray == null)
            return droptemDatas[0];

        return dropArray[UnityEngine.Random.Range(0, dropArray.Length)];
    }

    public string Get_Tier_Name(Tier tier)
    {
        switch(tier)
        {
            case Tier.Common:
                return "�Ϲ�";
            case Tier.Rare:
                return "����";
            case Tier.Epic:
                return "����";
            case Tier.Legend:
                return "������";
            default:
                return "";
        }
    }
    public bool Get_Ran(int percent)
    {
        int ranID = UnityEngine.Random.Range(1, 101);

        if (ranID <= percent)
            return true;
        else
            return false;
    }
}

public enum Tier { Common = 1, Rare = 2, Epic = 4, Legend = 8 }
public enum StageType { Grass = 0, Cave, Swarm, Forest }