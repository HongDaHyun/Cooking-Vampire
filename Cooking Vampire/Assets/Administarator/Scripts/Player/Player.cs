using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public MoveController moveController;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
    }
}
