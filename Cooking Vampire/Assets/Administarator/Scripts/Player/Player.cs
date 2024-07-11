using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public MoveController moveController;
    public PlayerType type;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
    }
}

public enum PlayerType { Knight = 0, Archer, Ninja, Magician }