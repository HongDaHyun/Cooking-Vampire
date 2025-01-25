using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Particle : Effect
{
    protected ParticleSystem particle;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        spawnManager.Destroy_Effect(this);
    }
}