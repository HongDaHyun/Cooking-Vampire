using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Chef : MonoBehaviour
{
    [HideInInspector] public ChefMove chefMove;

    private void Awake()
    {
        chefMove = GetComponent<ChefMove>();
    }
}
