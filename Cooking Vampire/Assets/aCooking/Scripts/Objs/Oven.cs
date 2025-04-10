using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Oven : IObj_Slider
{
    [Title("���")]
    public int MAXWOOD;
    public int MAXINGREDIENT;
    [Title("����")]
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
        // ����ִ� �������� ������ ���
        if(chef.IsItem(8))
        {
            woodCount = MAXWOOD;
            chef.UseItem();
            return;
        }

        // ������ �� ������ ���
        if (woodCount <= 0)
            return;

        // ��� �غ� ����
        if (!isDoing)
        {
            // ������ �տ� ��ᰡ ������
            if (!chef.IsItem())
            {
                // �־��� ��ᵵ ���ٸ�
                if (ingredients.Count <= 0)
                    return;
                else
                    TakeIngredient();
            }

            // ������ �տ� �ٸ� ��ᰡ ������
            else
                GainIngredient();
        }

        // �丮 ����
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
