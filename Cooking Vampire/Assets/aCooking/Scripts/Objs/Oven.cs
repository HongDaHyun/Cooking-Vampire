using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Oven : IObj_Slider
{
    [Title("상수")]
    public int MAXWOOD;
    public int MAXINGREDIENT;
    [Title("변수")]
    public int woodCount;
    public List<int> ingredients;

    protected override void Start()
    {
        base.Start();
        woodCount = MAXWOOD;

        sr.sprite = spriteData.Export_OvenSprites(woodCount, false);
    }

    public override void Interact()
    {
        // 들고있는 아이템이 나무인 경우
        if(chef.IsItem(8))
        {
            woodCount = MAXWOOD;
            chef.UseItem();
            return;
        }

        // 나무가 다 떨어진 경우
        if (woodCount <= 0)
            return;

        // 재료 준비 상태
        if (!isDoing)
        {
            // 셰프의 손에 재료가 없으면
            if (!chef.IsItem())
            {
                // 넣어진 재료도 없다면
                if (ingredients.Count <= 0)
                    return;
                else
                    TakeIngredient();
            }

            // 셰프의 손에 다른 재료가 있으면
            else
                GainIngredient();
        }

        // 요리 상태
        else
        {
            sr.sprite = spriteData.Export_OvenSprites(woodCount, true);

            StartCoroutine(SliderRoutine());
        }
    }

    protected override void SliderFinish()
    {
        woodCount--;
        sr.sprite = spriteData.Export_OvenSprites(woodCount, false);

        ingredients.Clear();
        isDoing = false;
    }

    protected void TakeIngredient()
    {
        if (ingredients.Count <= 0)
            return;
        else
        {
            chef.GainItem(ingredients[0]);
            ingredients.RemoveAt(0);
        }
    }
    protected void GainIngredient()
    {
        if (!chef.IsItem() || ingredients.Count >= MAXINGREDIENT)
            return;

        ingredients.Add(chef.ingredientInven);
        chef.UseItem();

        if (ingredients.Count >= MAXINGREDIENT)
            isDoing = true;
    }
}
