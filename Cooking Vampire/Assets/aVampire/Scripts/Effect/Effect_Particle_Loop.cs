using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Particle_Loop : Effect
{
    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        StopAllCoroutines();
    }

    public void SetEffect(Transform parent, float size, float lifeTime)
    {
        transform.SetParent(parent);
        SetTrans(parent.transform.position, size);

        StartCoroutine(LifeRoutine(lifeTime));
    }
    public override void SetTrans(Vector2 pos, float size)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(size, size, size);

        LimitBorder();
    }

    private IEnumerator LifeRoutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        spawnManager.Destroy_Effect(this);
    }
}
