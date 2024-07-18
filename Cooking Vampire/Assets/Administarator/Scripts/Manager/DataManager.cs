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

    [Title("정적 데이터")]
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public WeaponData[] weaponDatas;

    public PlayerData Export_PlayerData()
    {
        return Array.Find(playerDatas, data => data.type == curPlayer);
    }

    public EnemyData Export_EnemyData(int tier)
    {
        EnemyData[] tierDatas = Array.FindAll(enemyDatas, data => data.tier == tier && data.stage == curStage);
        return tierDatas[UnityEngine.Random.Range(0, tierDatas.Length)];
    }

    public WeaponData Export_WeaponData(PlayerType type, int tier)
    {
        return Array.Find(weaponDatas, data => data.weaponType == type && data.tier == tier);
    }
}

public enum StageType { Grass = 0, Cave, Swarm, Forest }