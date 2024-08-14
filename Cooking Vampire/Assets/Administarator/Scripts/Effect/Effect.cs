using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Effect : MonoBehaviour, IPoolObject
{
    SpawnManager spawnManager;
    Animator anim;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        spawnManager = SpawnManager.Instance;
        anim = GetComponent<Animator>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEffect(RuntimeAnimatorController animatorController, Vector2 pos, float size)
    {
        anim.runtimeAnimatorController = animatorController;
        transform.position = pos;
        transform.localScale = new Vector2(size, size);
    }

    private void Destroy()
    {
        spawnManager.Destroy_Effect(this);
    }
}
