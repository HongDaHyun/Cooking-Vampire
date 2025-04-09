using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Oak : IObj
{
    [Title("고유 변수")]
    public int ingredientID;
    public int amount;

    private void Start()
    {
        sr.sprite = spriteData.Export_OakSprites(false);
    }

    protected override void DoingFinish()
    {
        if (amount <= 0)
            return;

        StartCoroutine(SpriteRoutine());

        if (chef.ingredientInven == 0)
        {
            chef.ingredientInven = ingredientID;
            amount--;
        }
        else if (chef.ingredientInven == ingredientID)
        {
            chef.ingredientInven = 0;
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