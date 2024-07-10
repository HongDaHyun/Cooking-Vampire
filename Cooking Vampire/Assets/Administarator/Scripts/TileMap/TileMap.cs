using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TileMap : MonoBehaviour
{
    [ReadOnly]
    public List<SpriteRenderer> objList;

    private SpawnManager spawnManager;
    private Sprite[] objSprites;

    private void Start()
    {
        spawnManager = SpawnManager.Instance;
        objSprites = SpriteData.Instance.Export_StageSprites(DataManager.Instance.curStage);

        SpawnObjs();
    }

    public void ArrangeObj()
    {
        DestroyObjs();
        SpawnObjs();
    }

    private void DestroyObjs()
    {
        foreach (SpriteRenderer sr in objList)
            spawnManager.Destroy_TileObj(sr);
        objList.Clear();
    }

    private void SpawnObjs()
    {
        int ranCount = Random.Range(10, 50);
        List<Vector2Int> ranPoses = new List<Vector2Int>();
        Vector2Int ranPos = Vector2Int.zero;

        for(int i = 0; i < ranCount; i++)
        {
            SpriteRenderer obj = spawnManager.Spawn_TileObj(transform);
            obj.sprite = objSprites[Random.Range(0, objSprites.Length)];

            do
            {
                ranPos = new Vector2Int(Random.Range(-9, 9), Random.Range(-9, 9));
            } while (ranPoses.Contains(ranPos));

            obj.transform.localPosition = new Vector3(ranPos.x, ranPos.y);

            ranPoses.Add(ranPos);
            objList.Add(obj);
        }
    }
}
