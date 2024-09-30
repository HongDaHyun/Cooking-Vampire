using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public int BORDER = 14;
    public SpawnData[] spawnDatas;
    public int level;
    float levelTime;

    SpawnManager spawnManager;
    GameManager_Survivor gm;

    private void Start()
    {
        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        // StartCoroutine(SpawnRoutine());
        spawnManager.Spawn_Effect_X("½½¶óÀÓ", SpawnPoint_Ran(), 2f);
    }

    IEnumerator SpawnRoutine()
    {
        SpawnData curData = spawnDatas[0];
        levelTime = curData.spawnTime;

        while (!gm.player.isDead)
        {
            if(gm.curGameTime >= levelTime && level < spawnDatas.Length - 1)
            {
                level++;
                curData = spawnDatas[level];
                levelTime += curData.spawnTime;
            }

            spawnManager.Spawn_Effect_X(curData.Get_RanName(), SpawnPoint_Ran(), 1f);

            yield return new WaitForSeconds(curData.spawnCool);
        }
    }

    private Vector2 SpawnPoint_Ran()
    {
        float border_f = BORDER;

        float xRan = Random.Range(-border_f, border_f);
        float yRan = Random.Range(-border_f, border_f);

        return new Vector2(xRan, yRan);
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
            fullScale += spawnData_Details[i].scale;
        }

        float ranID = Random.Range(0f, fullScale);

        for(int i = 0; i < length; i++)
        {
            curScale += spawnData_Details[i].scale;
            if (ranID < curScale)
                return spawnData_Details[i].name;
        }
        return spawnData_Details[0].name;
    }
}
[System.Serializable]
public struct SpawnData_Detail
{
    public string name;
    [Range(0f, 1f)]public float scale;
}