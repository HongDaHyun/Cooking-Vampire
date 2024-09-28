using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Effect_X : Effect
{
    string spawnName;

    SpriteRenderer sr;
    Sequence effectSeq;

    bool isTrigger;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();

        sr = GetComponent<SpriteRenderer>();

        effectSeq = DOTween.Sequence().SetUpdate(false).SetAutoKill(false);
        effectSeq.OnStart(() => sr.color = new Vector4(1, 1, 1, 0))
            .Append(sr.DOFade(1, 0.2f))
            .AppendInterval(0.5f)
            .Append(sr.DOFade(0, 0.2f))
            .OnComplete(() =>
            {
                if (!isTrigger)
                    spawnManager.Spawn_Enemy(spawnName, transform.position);
            });
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
        effectSeq.Restart();
    }
}