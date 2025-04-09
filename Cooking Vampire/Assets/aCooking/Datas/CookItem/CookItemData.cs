using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CookItemData", menuName = "DataCreator/CookItemData")]
public class CookItemData : ScriptableObject
{
    [Title("UI")]
    public Sprite sprite;

    [Title("º¯¼ö")]
    public int ID;

    public void Init(int id, Sprite icon)
    {
        ID = id;
        sprite = icon;
    }
}
