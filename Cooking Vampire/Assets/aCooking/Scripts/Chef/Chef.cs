using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Cooking;

public class Chef : MonoBehaviour
{
    [HideInInspector] public GameManager_Cooking gm;

    [HideInInspector] public ChefMove chefMove;
    [HideInInspector] public ChefScan chefScan;

    public int ingredientInven; // 0 일때는 아무것도 소지하지 않은 상태

    private void Awake()
    {
        gm = GameManager_Cooking.Instance;

        chefMove = GetComponent<ChefMove>();
        chefScan = GetComponent<ChefScan>();
    }
}
