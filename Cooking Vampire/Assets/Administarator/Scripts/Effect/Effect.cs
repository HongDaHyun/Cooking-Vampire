using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public abstract class Effect : MonoBehaviour, IPoolObject
{
    protected SpawnManager spawnManager;

    public virtual void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
    }

    public virtual void OnGettingFromPool()
    {
    }

    public void SetTrans(Vector2 pos, float size)
    {
        transform.position = pos;
        transform.localScale = new Vector2(size, size);
    }

    protected void Destroy()
    {
        spawnManager.Destroy_Effect(this);
    }
}
