using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class ChainThunder : MonoBehaviour, IPoolObject
{
    SpawnManager sm;

    public int amountToChain;

    CircleCollider2D col;
    Animator anim;
    ParticleSystem particle;

    public void OnCreatedInPool()
    {
        sm = SpawnManager.Instance;
        col = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();
    }

    public void OnGettingFromPool()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy nearEnemy = collision.GetComponent<Enemy>();

            amountToChain--;
            sm.Spawn_ChainThunder(amountToChain, nearEnemy.transform.position);

            anim.StopPlayback();
            col.enabled = false;

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = transform.position;
            particle.Emit(emitParams, 1);
            emitParams.position = nearEnemy.transform.position;
            particle.Emit(emitParams, 1);
        }
    }

    public void SetChainThunder(int _amountToChain, Vector2 pos)
    {
        amountToChain = _amountToChain;
        transform.position = pos;
    }
}
