using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient_Drop : Item
{
    public IngredientData data;

    public void SetIngredient(int ID, Vector2 pos)
    {
        data = dm.Export_IngredientData(ID);

        Drop(pos);
    }

    protected override void Destroy()
    {
        dm.Gain_Ingredient(data.ID, 1);
        spawnManager.Destroy_Item(this);
    }

    protected override void Drain()
    {
        sr.sprite = data.sprites[1];
        base.Drain();
    }

    protected override void Drop(Vector2 pos)
    {
        DropRanPos(pos, data.sprites[1], data.sprites[0]);
    }
}
