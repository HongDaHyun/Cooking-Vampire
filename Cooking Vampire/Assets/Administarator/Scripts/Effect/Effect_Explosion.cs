using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Explosion : Effect
{
    private CircleCollider2D col;
    private ParticleSystem particle;
    private AtkStat stat;

    GameManager_Survivor gm;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        col = GetComponent<CircleCollider2D>();
        particle = GetComponent<ParticleSystem>();
        gm = GameManager_Survivor.Instance;
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        stat.dmg = gm.stat.Cal_DMG(5);
        StartCoroutine(LifeRoutine());
    }

    public override void SetTrans(Vector2 pos, float size)
    {
        base.SetTrans(pos, size);
        col.radius = particle.shape.radius;
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(particle.main.duration);
        spawnManager.Destroy_Effect(this);
    }
}
