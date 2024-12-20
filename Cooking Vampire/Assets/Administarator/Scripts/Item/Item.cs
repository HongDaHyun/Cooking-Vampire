using Redcode.Pools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IPoolObject
{
    protected SpriteRenderer sr;

    protected GameManager_Survivor gm;
    protected DataManager dm;
    protected SpriteData spriteData;
    protected Player player;
    protected SpawnManager spawnManager;

    [ReadOnly] public bool isActive, isDrain;

    public virtual void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sr = GetComponent<SpriteRenderer>();
        gm = GameManager_Survivor.Instance;
        dm = DataManager.Instance;
        spriteData = SpriteData.Instance;
        player = GameManager_Survivor.Instance.player;
        spawnManager = SpawnManager.Instance;
    }

    public virtual void OnGettingFromPool()
    {
        isActive = false;
        isDrain = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!isActive || !isDrain)
            return;
        Drain();
    }

    protected virtual void Drain()
    {
        transform.position = 
            Vector3.MoveTowards(transform.position, player.transform.position, player. gm.stat.Cal_SPE() * 2f * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) <= 0.1f)
            Destroy();
    }

    protected abstract void Destroy();
    protected abstract void Drop(Vector2 pos);
}
