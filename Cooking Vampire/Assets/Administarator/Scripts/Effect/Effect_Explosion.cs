using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Explosion : Effect_Particle
{
    private CircleCollider2D col;
    private Projectile projectile;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        col = GetComponent<CircleCollider2D>();
        particle = GetComponent<ParticleSystem>();
        projectile = GetComponent<Projectile>();

        projectile.stat = new AtkStat();
        projectile.stat.dmg = 5;
    }

    public override void SetTrans(Vector2 pos, float size)
    {
        base.SetTrans(pos, size);
        col.radius = particle.shape.radius;
        col.enabled = true;
    }
}
