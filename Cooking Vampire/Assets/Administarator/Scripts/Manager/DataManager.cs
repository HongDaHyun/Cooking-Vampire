using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataManager : Singleton<DataManager>
{
    [Title("���� ������")]
    public StageType curStage;

    [Title("���� ������")]
    public EnemyData[] enemyDatas;

    public EnemyData Export_EnemyData(int tier)
    {
        EnemyData[] tierDatas = Array.FindAll(enemyDatas, data => data.tier == tier && data.stage == curStage);
        return tierDatas[UnityEngine.Random.Range(0, tierDatas.Length)];
    }
}

public enum StageType { Grass = 0, Cave, Swarm, Forest }