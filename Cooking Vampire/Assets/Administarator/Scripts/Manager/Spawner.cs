using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnDatas;
    int level;
    float levelTime;

    Transform[] spawnPoints;

    SpawnManager spawnManager;
    GameManager_Survivor gm;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        DataManager dataManager = DataManager.Instance;

        levelTime = spawnDatas[0].spawnTime;

        while(true)
        {
            SpawnData curData = spawnDatas[level];

            if(gm.curGameTime > levelTime && level < spawnDatas.Length - 1)
            {
                level++;
                levelTime += curData.spawnTime;
            }

            spawnManager.Spawn_Enemy(dataManager.Export_EnemyData(curData.Get_RanName()), spawnPoints[Random.Range(1, spawnPoints.Length)].position);

            yield return new WaitForSeconds(curData.spawnCool);
        }
    }
}

[System.Serializable]
public struct SpawnData
{
    public SpawnData_Detail[] spawnData_Details;
    public float spawnCool;
    public float spawnTime;

    public string Get_RanName()
    {
        int length = spawnData_Details.Length;
        float fullScale = 0f;
        float curScale = 0f;

        for(int i = 0; i < length; i++)
        {
            fullScale = spawnData_Details[i].scale;
        }

        float ranID = Random.Range(0f, fullScale);

        for(int i = 0; i < length; i++)
        {
            curScale = spawnData_Details[i].scale;
            if (ranID < curScale)
                return spawnData_Details[i].name;
        }
        return null;
    }
}
[System.Serializable]
public struct SpawnData_Detail
{
    public string name;
    [Range(0f, 1f)]public float scale;
}