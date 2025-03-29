using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Effect_Dirt : Effect
{
    public Sprite[] dirtSprites;
    SpriteData spriteData;

    SpriteRenderer sr;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        sr = GetComponent<SpriteRenderer>();

        spriteData = SpriteData.Instance;
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();

        sr.sprite = dirtSprites[Random.Range(0, dirtSprites.Length)];

        Color redColor = spriteData.Export_Pallate("Red");

        sr.color = Color.white;

        Sequence colorSeq = DOTween.Sequence();
        colorSeq.Append(sr.DOColor(redColor, 0.5f))
            .Append(sr.DOColor(Color.white, 1f))
            .Append(sr.DOColor(redColor, 0.5f))
            .Append(sr.DOColor(Color.white, 1f))
            .Append(sr.DOColor(redColor, 0.5f))
            .Append(sr.DOColor(Color.white, 1f))
            .Append(sr.DOFade(0f, 1f))
            .OnComplete(() => spawnManager.Destroy_Effect(this));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Player>().GetSlow(0.2f, 3f);
    }
}
