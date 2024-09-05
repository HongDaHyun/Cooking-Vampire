using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataManager : Singleton<DataManager>
{
    [Title("저장 데이터")]
    public StageType curStage;
    public PlayerType curPlayer;
    public WeaponData curWeapon;
    public int coin;

    [Title("정적 데이터")]
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public WeaponData[] weaponDatas;

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
        EnemyData[] datas = Array.FindAll(enemyDatas, data => data.title == enemyName && data.stage == curStage);
        return datas[UnityEngine.Random.Range(0, datas.Length)];
    }

    public WeaponData Export_WeaponData(PlayerType type, int tier)
    {
        return Array.Find(weaponDatas, data => data.weaponType == type && data.tier == tier);
    }

    public string Get_Tier_Name(Tier tier)
    {
        switch(tier)
        {
            case Tier.Common:
                return "일반";
            case Tier.Rare:
                return "레어";
            case Tier.Epic:
                return "에픽";
            case Tier.Legend:
                return "레전드";
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