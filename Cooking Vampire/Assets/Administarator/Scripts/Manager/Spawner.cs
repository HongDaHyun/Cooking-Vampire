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
        levelTime = spawnDatas[0].spawnTime;

        while(true)
        {
            SpawnData curData = spawnDatas[level];

            if(gm.curGameTime > levelTime && level < spawnDatas.Length - 1)
            {
                level++;
                levelTime += curData.spawnTime;
            }
            spawnManager.SpawnEnemy(curData.spawnTier, spawnPoints[Random.Range(1, spawnPoints.Length)].position);

            yield return new WaitForSeconds(curData.spawnCool);
        }
    }
}

[System.Serializable]
public struct SpawnData
{
    public int[] spawnTier;
    public float spawnCool;
    public float spawnTime;
}
