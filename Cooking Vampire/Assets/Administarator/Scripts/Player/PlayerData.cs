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

    [Title("������")]
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
                return "���ݷ�";
            case StatType.DEF:
                return "����";
            case StatType.HP:
                return "ü��";
            case StatType.SPEED:
                return "�̵��ӵ�";
            case StatType.MISS:
                return "ȸ�� Ȯ��";
            case StatType.CRIT:
                return "ġ��Ÿ Ȯ��";
            case StatType.LUCK:
                return "��";
            case StatType.EXP:
                return "����ġ ����";
            case StatType.ACTIVE:
                return "���� ���� �ð�";
            case StatType.COOL:
                return "���� ��Ÿ��";
            case StatType.HEAL:
                return "ü�� �����";
            case StatType.DRAIN:
                return "ü�� ��� Ȯ��";
            case StatType.PRO_SIZE:
                return "����ü ũ��";
            case StatType.PRO_SPEED:
                return "����ü �ӵ�";
            case StatType.COUNT:
                return "����ü ����";
            case StatType.ELE:
                return "���� ������";
            case StatType.RANGE:
                return "��Ÿ�";
            case StatType.BACK:
                return "�˹�";
            case StatType.PER:
                return "�����";
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
            case StatType.BACK:
                output += "% ";
                break;
            case StatType.DEF:
            case StatType.HP:
            case StatType.LUCK:
            case StatType.HEAL:
            case StatType.COUNT:
            case StatType.ELE:
            case StatType.RANGE:
            case StatType.PER:
                output += "��ŭ ";
                break;
            default:
                output += "";
                break;
        }
        output += amount > 0 ? "����" : "����";
        return output;
    }

    public void Set_Amount(Tier tier)
    {
        int baseAmount = CSVManager.Instance.Find_StatCSV(type).baseAmount;

        if (type != StatType.COUNT && type != StatType.PER)
            baseAmount *= (int)tier;

        amount = baseAmount;
    }
}