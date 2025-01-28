using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Effect_X : Effect
{
    string spawnName;

    SpriteRenderer sr;

    bool isTrigger;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();

        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isTrigger = true;
    }

    public void SetEffect(Vector2 pos, float size, string s_name)
    {
        spawnName = s_name;
        SetTrans(pos, size);
        DOEffect();
    }

    public void DOEffect()
    {
        Sequence effectSeq = DOTween.Sequence().SetUpdate(false);
        effectSeq.OnStart(() => sr.color = new Vector4(1, 1, 1, 0))
            .Append(sr.DOFade(1, 0.2f))
            .AppendInterval(0.5f)
            .Append(sr.DOFade(0, 0.2f))
            .OnComplete(() =>
            {
                if (!isTrigger)
                {
                    spawnManager.Spawn_Enemy(spawnName, transform.position, transform.localScale.x);
                    spawnManager.Destroy_Effect(this);
                }
                else
                    spawnManager.Spawn_Effect_X(spawnName, levelManager.SpawnPoint_Ran(0), 1f);
            });
    }
}
