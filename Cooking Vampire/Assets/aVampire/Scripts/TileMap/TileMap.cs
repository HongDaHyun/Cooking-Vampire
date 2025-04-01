using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Vampire;

public class TileMap : MonoBehaviour
{
    public int minCount, maxCount;
    public Sprite[] objSprites;

    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = SpawnManager.Instance;

        SpawnObjs();
    }

    private void SpawnObjs()
    {
        int ranCount = Random.Range(minCount, maxCount);
        List<Vector2Int> ranPoses = new List<Vector2Int>();
        Vector2Int ranPos = Vector2Int.zero;
        int border = LevelManager.Instance.BORDER;

        for(int i = 0; i < ranCount; i++)
        {
            SpriteRenderer obj = spawnManager.Spawn_TileObj(transform);
            obj.sprite = objSprites[Random.Range(0, objSprites.Length)];

            do
            {
                ranPos = new Vector2Int(Random.Range(-border, border), Random.Range(-border, border));
            } while (ranPoses.Contains(ranPos));

            obj.transform.localPosition = new Vector3(ranPos.x, ranPos.y);

            ranPoses.Add(ranPos);
        }
    }
}
