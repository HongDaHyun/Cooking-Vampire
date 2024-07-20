using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Gem : Item
{
    [ReadOnly] public int amount;

    public void SetGem(int unit, Vector2 pos)
    {
        amount = unit;

        Drop(pos);
    }
    protected override void Drain()
    {
        sr.sprite = spriteData.Export_GemSprites(amount).moveSprite;
        base.Drain();
    }
    protected override void Drop(Vector2 pos)
    {
        sr.sortingOrder = 1;
        sr.sprite = spriteData.Export_GemSprites(amount).moveSprite;
        transform.position = pos;

        Vector2 startPos = pos;
        Vector2 targetPos = new Vector2(pos.x + Random.Range(-0.5f, 0.5f), pos.y + Random.Range(-0.5f, 0.5f));
        Vector2 controlPos = startPos + (targetPos - startPos) / 2f + Vector2.up * 0.5f;

        Vector3[] path = new Vector3[3];
        path[0] = startPos;
        path[1] = controlPos;
        path[2] = targetPos;

        transform.DOPath(path, 0.5f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() =>
        {
            sr.sprite = spriteData.Export_GemSprites(amount).idleSprite;
            sr.sortingOrder = 0;
            isActive = true;
        });
    }

    protected override void Destroy()
    {
        gm.Player_GainExp(amount);
        spawnManager.Destroy_Gem(this);
    }
}