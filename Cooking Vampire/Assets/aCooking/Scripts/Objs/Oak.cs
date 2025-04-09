using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Oak : IObj
{
    [Title("고유 변수")]
    public int ingredientID;
    public int amount;

    protected override void Start()
    {
        base.Start();
        sr.sprite = spriteData.Export_OakSprites(false);
    }

    public override void Interact()
    {
        if (amount <= 0)
            return;

        StartCoroutine(SpriteRoutine());

        if (!chef.IsItem())
        {
            chef.GainItem(ingredientID);
            amount--;
        }
        else if (chef.IsItem(ingredientID))
        {
            chef.UseItem();
            amount++;
        }
    }

    IEnumerator SpriteRoutine()
    {
        if (sr.sprite == spriteData.Export_OakSprites(true))
            yield break;

        sr.sprite = spriteData.Export_OakSprites(true);
        yield return new WaitForSeconds(2f);
        sr.sprite = spriteData.Export_OakSprites(false);
    }
}