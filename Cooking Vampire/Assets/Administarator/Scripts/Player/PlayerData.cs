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
public enum StatID_Player { HP, HPREG, DRA, DEF, DMG, ELE, AS, AT, CRIT, CRIT_DMG, RAN, MIS, SPE, LUK, AMT, PER, BAK, EXP }

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
        SpriteData sd = SpriteData.Instance;

        bool isPlus = amount > 0;
        string output = isPlus ? $"<color=#{ColorUtility.ToHtmlStringRGB(sd.Export_Pallate("Red"))}>" : $"<color=#{ColorUtility.ToHtmlStringRGB(sd.Export_Pallate("Blue"))}>";
        output += Mathf.Abs(amount).ToString();
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
            case StatType.BACK:
                output += "%";
                break;
            case StatType.DEF:
            case StatType.HP:
            case StatType.LUCK:
            case StatType.HEAL:
            case StatType.COUNT:
            case StatType.ELE:
            case StatType.RANGE:
            case StatType.PER:
            default:
                break;
        }
        output += isPlus ? "증가" : "감소";
        output += "</color>";
        return output;
    }

    public void Set_Amount(TierType tier)
    {
        int baseAmount = CSVManager.Instance.Find_StatCSV(type).baseAmount;

        if (type != StatType.COUNT && type != StatType.PER)
            baseAmount *= (int)tier;

        amount = baseAmount;
    }
}