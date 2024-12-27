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
        sr.sprite = spriteData.Export_GemSprite(amount).moveSprite;
        base.Drain();
    }
    protected override void Drop(Vector2 pos)
    {
        DropRanPos(pos, spriteData.Export_GemSprite(amount).moveSprite, spriteData.Export_GemSprite(amount).idleSprite);
    }

    protected override void Destroy()
    {
        gm.stat.curExp += gm.stat.Cal_EXP(amount);
        spawnManager.Destroy_Gem(this);
    }
}