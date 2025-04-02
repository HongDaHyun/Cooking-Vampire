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

    private void Awake()
    {
        gm = GameManager_Cooking.Instance;

        chefMove = GetComponent<ChefMove>();
        chefScan = GetComponent<ChefScan>();
    }
}
