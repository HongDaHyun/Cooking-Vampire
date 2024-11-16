using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Title("UI")]
    public string title;
    [TextArea] public string discription;

    [Title("ตฅภฬลอ")]
    public PlayerType type;
    public RuntimeAnimatorController animator;
    public int baseWeaponID;
}

public enum PlayerType { Knight = 0, Archer, Ninja, Magician }
public enum StatID_Player { HP, HPREG, DRA, DEF, DMG, ELE, AS, AT, CRIT, CRIT_DMG, RAN, MIS, SPE, LUK, AMT, PER, BAK, EXP }