using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Vampire;

[CreateAssetMenu(fileName = "PlayerData", menuName = "DataCreator/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Title("UI")]
    public string title;
    [TextArea] public string discription;
    public Color personalColor;
    public Sprite icon;

    [Title("������")]
    public PlayerType type;
    public RuntimeAnimatorController animator;
    public int baseWeaponID;
    public PlayerStat_Crystal[] crystals;

    public PlayerStat_Crystal GetCrystal(StatID_Player id)
    {
        return Array.Find(crystals, c => c.ID == id);
    }
}

public enum PlayerType { Knight = 0, Archer, Ninja, Magician }
public enum StatID_Player { HP, HPREG, DRA, DEF, DMG, ELE, AS, AT, CRIT, CRIT_DMG, RAN, MIS, SPE, LUK, AMT, PER, BAK, EXP }