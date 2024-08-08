using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Title("UI")]
    public string title;
    [TextArea] public string discription;

    [Title("데이터")]
    public PlayerType type;
    public RuntimeAnimatorController animator;
    public int baseWeaponID;
    public BonusStat[] bonusStats;

    public bool IsExist(StatType type)
    {
        return Array.Exists(bonusStats, stat => stat.type == type);
    }
    public BonusStat Find_Bonus(StatType type)
    {
        return Array.Find(bonusStats, stat => stat.type == type);
    }
}

public enum PlayerType { Knight = 0, Archer, Ninja, Magician }
public enum StatType { DMG = 0, DEF, HP, SPEED, MISS, CRIT, LUCK, EXP, ACTIVE, COOL, HEAL, DRAIN, PRO_SIZE, PRO_SPEED, COUNT, ELE, RANGE, BACK, PER }

[Serializable]
public struct BonusStat
{
    public StatType type;
    public float amount;

    public string Get_Name()
    {
        switch (type)
        {
            case StatType.DMG:
                return "공격력";
            case StatType.DEF:
                return "방어력";
            case StatType.HP:
                return "체력";
            case StatType.SPEED:
                return "이동속도";
            case StatType.MISS:
                return "회피 확률";
            case StatType.CRIT:
                return "치명타 확률";
            case StatType.LUCK:
                return "운";
            case StatType.EXP:
                return "경험치 배율";
            case StatType.ACTIVE:
                return "무기 지속 시간";
            case StatType.COOL:
                return "무기 쿨타임";
            case StatType.HEAL:
                return "체력 재생량";
            case StatType.DRAIN:
                return "체력 흡수 확률";
            case StatType.PRO_SIZE:
                return "투사체 크기";
            case StatType.PRO_SPEED:
                return "투사체 속도";
            case StatType.COUNT:
                return "투사체 개수";
            case StatType.ELE:
                return "원소 데미지";
            case StatType.RANGE:
                return "사거리";
            case StatType.BACK:
                return "넉백";
            case StatType.PER:
                return "관통력";
            default:
                return "";
        }
    }
    public string Get_Discription()
    {
        string output = Mathf.Abs(amount).ToString();
        switch (type)
        {
            case StatType.DMG:
            case StatType.SPEED:
            case StatType.MISS:
            case StatType.CRIT:
            case StatType.EXP:
            case StatType.ACTIVE:
            case StatType.COOL:
            case StatType.PRO_SIZE:
            case StatType.PRO_SPEED:
                output += "% ";
                break;
            case StatType.DEF:
            case StatType.HP:
            case StatType.LUCK:
            case StatType.HEAL:
            case StatType.COUNT:
            case StatType.ELE:
            case StatType.RANGE:
            case StatType.BACK:
            case StatType.PER:
                output += "만큼 ";
                break;
            default:
                output += "";
                break;
        }
        output += amount > 0 ? "증가" : "감소";
        return output;
    }

    public void Set_Amount(Tier tier)
    {
        int baseAmount = 0;

        switch(type)
        {
            case StatType.DMG:
                baseAmount = 5;
                break;
            case StatType.DEF:
                baseAmount = 1;
                break;
            case StatType.HP:
                baseAmount = 3;
                break;
            case StatType.SPEED:
                baseAmount = 5;
                break;
            case StatType.MISS:
                baseAmount = 1;
                break;
            case StatType.CRIT:
                baseAmount = 1;
                break;
            case StatType.LUCK:
                baseAmount = 1;
                break;
            case StatType.EXP: // 완
                baseAmount = 5;
                break;
            case StatType.ACTIVE: // 완
                baseAmount = 5;
                break;
            case StatType.COOL: // 완
                baseAmount = -5;
                break;
            case StatType.HEAL:
                baseAmount = 1;
                break;
            case StatType.DRAIN:
                baseAmount = 1;
                break;
            case StatType.PRO_SIZE: // 완
                baseAmount = 5;
                break;
            case StatType.PRO_SPEED: // 완
                baseAmount = 5;
                break;
            case StatType.COUNT: // 완
                baseAmount = 1;
                break;
            case StatType.ELE:
                baseAmount = 1;
                break;
            case StatType.RANGE: // 완
                baseAmount = 10;
                break;
            case StatType.BACK:
                baseAmount = 1;
                break;
            case StatType.PER: // 완
                baseAmount = 1;
                break;

        }

        if (type != StatType.COUNT || type != StatType.PER)
            baseAmount *= (int)tier;

        amount = baseAmount;
    }
}