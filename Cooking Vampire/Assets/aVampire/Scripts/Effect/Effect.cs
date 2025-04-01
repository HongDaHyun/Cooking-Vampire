using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Vampire;

public abstract class Effect : MonoBehaviour, IPoolObject
{
    protected SpawnManager spawnManager;
    protected LevelManager levelManager;

    public virtual void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
        levelManager = LevelManager.Instance;
    }

    public virtual void OnGettingFromPool()
    {
    }

    public virtual void SetTrans(Vector2 pos, float size)
    {
        transform.position = pos;
        transform.localScale = new Vector3(size, size, size);

        LimitBorder();
    }
    public void LimitBorder()
    {
        float border = levelManager.BORDER;
        Vector2 curPos = transform.position;

        // x축 경계 제한
        curPos.x = Mathf.Clamp(curPos.x, -border, border);

        // y축 경계 제한
        curPos.y = Mathf.Clamp(curPos.y, -border, border);

        transform.position = curPos;
    }
}
