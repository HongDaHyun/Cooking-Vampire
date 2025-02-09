using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public int BORDER = 14;
    public SpawnData[] spawnDatas;
    public int level;
    float levelTime;
    [HideInInspector] public Coroutine levelRoutine;

    SpawnManager spawnManager;
    GameManager_Survivor gm;

    private void Start()
    {
        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        // levelRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        SpawnData curData = spawnDatas[0];
        levelTime = curData.spawnTime;

        while (!gm.player.isDead || !gm.timeEnd)
        {
            if(gm.curGameTime >= levelTime && level < spawnDatas.Length - 1)
            {
                level++;
                curData = spawnDatas[level];
                levelTime += curData.spawnTime;
            }

            SpawnData_Detail detail = curData.Get_RanDetail();
            int spawnAmount = detail.isRanAmount ? Random.Range(Mathf.Max(1, detail.amount - 3), detail.amount + 3) : Mathf.Max(detail.amount, 1);
            Vector2[] poses = SpawnPoints_Ran(detail.isGether, spawnAmount);
            foreach(Vector2 pos in poses)
            {
                spawnManager.Spawn_Effect_X(detail.name, pos, 1f);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(curData.spawnCool);
        }
    }

    public Vector2 SpawnPoint_Ran(int plusBorder)
    {
        float border_f = BORDER - plusBorder;

        float xRan = Random.Range(-border_f, border_f);
        float yRan = Random.Range(-border_f, border_f);

        return new Vector2(xRan, yRan);
    }
    public Vector2[] SpawnPoints_Ran(bool isGether, int amount)
    {
        List<Vector2> ranPoses = new List<Vector2>(amount); // 미리 리스트 용량 설정
        Vector2 ranPos;

        if (isGether)
        {
            float halfAmount = amount / 2f;

            Vector2 mainPos = SpawnPoint_Ran((int)halfAmount);
            ranPoses.Add(mainPos);

            for (int i = 1; i < amount; i++) // 0부터 시작하지 않고 1부터 시작 (mainPos는 이미 추가됨)
            {
                do
                {
                    ranPos = mainPos + new Vector2(Random.Range(-halfAmount, halfAmount), Random.Range(-halfAmount, halfAmount));
                }
                while (!IsValidPosition(ranPos, ranPoses)); // 유효성 검사 함수로 중복 코드 제거

                ranPoses.Add(ranPos);
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                do
                {
                    ranPos = SpawnPoint_Ran(0);
                }
                while (!IsValidPosition(ranPos, ranPoses));

                ranPoses.Add(ranPos);
            }
        }

        return ranPoses.ToArray();
    }

    // 유효성 검사 함수로 중복 코드 제거
    private bool IsValidPosition(Vector2 newPos, List<Vector2> existingPoses)
    {
        foreach (Vector2 pos in existingPoses)
        {
            if (Vector2.Distance(newPos, pos) < 1)
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public struct SpawnData
{
    public SpawnData_Detail[] spawnData_Details;
    public float spawnCool;
    public float spawnTime;

    public SpawnData_Detail Get_RanDetail()
    {
        int length = spawnData_Details.Length;
        float fullScale = 0f;
        float curScale = 0f;

        for (int i = 0; i < length; i++)
        {
            fullScale += spawnData_Details[i].scale;
        }

        float ranID = Random.Range(0f, fullScale);

        for (int i = 0; i < length; i++)
        {
            curScale += spawnData_Details[i].scale;
            if (ranID < curScale)
                return spawnData_Details[i];
        }
        return spawnData_Details[0];
    }
}
[System.Serializable]
public struct SpawnData_Detail
{
    public string name;
    [Range(0f, 1f)]public float scale;
    public int amount;
    public bool isRanAmount, isGether;
}