using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Cooking;

public class Chef : MonoBehaviour
{
    [HideInInspector] public GameManager_Cooking gm;
    [HideInInspector] public SpriteData spriteData;

    [HideInInspector] public ChefMove chefMove;
    [HideInInspector] public ChefScan chefScan;

    public SpriteRenderer itemSr;
    public int ingredientInven; // 0 �϶��� �ƹ��͵� �������� ���� ����

    private void Awake()
    {
        gm = GameManager_Cooking.Instance;

        chefMove = GetComponent<ChefMove>();
        chefScan = GetComponent<ChefScan>();
    }

    private void Start()
    {
        spriteData = SpriteData.Instance;
    }

    public void GainItem(int ID)
    {
        // �������� �����ϰ� ���� ���� ���� ����
        if (ID == 0 || IsItem())
            return;

        chefMove.CarryAnim(true);
        itemSr.sprite = spriteData.Export_CookItemSprites(ID);
        
        ingredientInven = ID;
    }
    public void UseItem()
    {
        chefMove.CarryAnim(false);
        itemSr.sprite = null;

        ingredientInven = 0;
    }
    public bool IsItem()
    {
        return ingredientInven != 0;
    }
    public bool IsItem(int ID)
    {
        return ingredientInven == ID;
    }
}
