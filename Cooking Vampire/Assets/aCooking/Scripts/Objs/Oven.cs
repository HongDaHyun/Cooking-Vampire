using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Oven : IObj
{
    [Title("고유 변수")]
    private int MAXWOOD = 6;
    public int woodCount;
    public List<int> ingredients;

    private void Start()
    {
        woodCount = MAXWOOD;

        sr.sprite = spriteData.Export_OvenSprites(woodCount, false);
    }

    protected override IEnumerator SliderRoutine()
    {
        if (chef.ingredientInven == 8)
        {
            woodCount = MAXWOOD;
            chef.ingredientInven = 0;
            yield break;
        }

        if (woodCount <= 0)
            yield break;

        if(isPickable)
        {
            if(chef.ingredientInven == 0)
            {
                if (ingredients.Count <= 0)
                    yield break;
                else
                {
                    chef.ingredientInven = ingredients[0];
                    ingredients.RemoveAt(0);
                }
            }

            ingredients.Add(chef.ingredientInven);
            chef.ingredientInven = 0;

            if (ingredients.Count >= 4)
                isPickable = false;
        }
        else
        {
            if (!chef.chefMove.isInteract)
                yield break;
            sr.sprite = spriteData.Export_OvenSprites(woodCount, true);
            sliderObj.gameObject.SetActive(true);

            while (chef.chefMove.isInteract)
            {
                slider.value += Time.deltaTime;

                // 완료
                if (slider.value >= 1f)
                {
                    slider.value = 0;

                    woodCount--;
                    sr.sprite = spriteData.Export_OvenSprites(woodCount, false);
                    sliderObj.gameObject.SetActive(false);
                    DoingFinish();
                    yield break;
                }
                yield return null;
            }
        }
    }

    protected override void DoingFinish()
    {
        Debug.Log("요리 완료!");
    }
}
