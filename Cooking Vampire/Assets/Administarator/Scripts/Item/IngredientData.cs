using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "DataCreator/IngredientData")]
public class IngredientData : ScriptableObject
{
    [Title("UI")]
    public string title;
    public Sprite[] sprites;
    // type

    [Title("ตฅภฬลอ")]
    public int ID;
    public int amount;
}
