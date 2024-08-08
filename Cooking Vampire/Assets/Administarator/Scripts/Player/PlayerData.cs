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
            case StatType.EXP: // ��
                baseAmount = 5;
                break;
            case StatType.ACTIVE: // ��
                baseAmount = 5;
                break;
            case StatType.COOL: // ��
                baseAmount = -5;
                break;
            case StatType.HEAL:
                baseAmount = 1;
                break;
            case StatType.DRAIN:
                baseAmount = 1;
                break;
            case StatType.PRO_SIZE: // ��
                baseAmount = 5;
                break;
            case StatType.PRO_SPEED: // ��
                baseAmount = 5;
                break;
            case StatType.COUNT: // ��
                baseAmount = 1;
                break;
            case StatType.ELE:
                baseAmount = 1;
                break;
            case StatType.RANGE: // ��
                baseAmount = 10;
                break;
            case StatType.BACK:
                baseAmount = 1;
                break;
            case StatType.PER: // ��
                baseAmount = 1;
                break;

        }

        if (type != StatType.COUNT || type != StatType.PER)
            baseAmount *= (int)tier;

        amount = baseAmount;
    }
}