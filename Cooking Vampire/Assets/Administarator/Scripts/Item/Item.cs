using Redcode.Pools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if (isActive && isDrain)
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

    protected void DropRanPos(Vector2 startPos, Sprite move, Sprite idle)
    {
        sr.sortingOrder = 1;
        sr.sprite = move;
        transform.position = startPos;

        Vector2 targetPos = new Vector2(startPos.x + Random.Range(-0.5f, 0.5f), startPos.y + Random.Range(-0.5f, 0.5f));
        Vector2 controlPos = startPos + (targetPos - startPos) / 2f + Vector2.up * 0.5f;

        Vector3[] path = new Vector3[3];
        path[0] = startPos;
        path[1] = controlPos;
        path[2] = targetPos;

        transform.DOPath(path, 0.5f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() =>
        {
            sr.sprite = idle;
            sr.sortingOrder = 0;
            isActive = true;
        });
    }
}
