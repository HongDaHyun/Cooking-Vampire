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
public enum StatType { DMG = 0, DEF, HP, SPEED, MISS, CRIT, LUCK, EXP, ACTIVE, COOL, HEAL, DRAIN, SIZE, PRO_SPEED, COUNT, ELE, RANGE, BACK, PER }

[Serializable]
public struct BonusStat
{
    public StatType type;
    public float amount;
    public bool isBase;

    private void Calculating(ref float i)
    {
        if (isBase)
            i += amount;
        else
        {
            if (i == 0)
                i = 1f;
            i += Mathf.Round(i * amount * 100) / 100;
        }
    }
    private void Calculating(ref int i)
    {
        if (isBase)
            i += (int)amount;
        else
        {
            if (i == 0)
                i = 1;
            i += Mathf.RoundToInt(i * amount);
        }
    }

    public void Update_Stat(ref WeaponStat stat)
    {
        Player player = GameManager_Survivor.Instance.player;

        if(player.data.IsExist(type))
        {
            BonusStat playerBonus = player.data.Find_Bonus(type);
            playerBonus.Calculating(ref amount);
        }

        switch (type)
        {
            case StatType.DMG:
                Calculating(ref stat.damage);
                break;
            case StatType.ACTIVE:
                if (stat.activeTime == -1)
                    return;
                Calculating(ref stat.activeTime);
                break;
            case StatType.COOL:
                Calculating(ref stat.coolTime);
                break;
            case StatType.SIZE:
                // �̱���
                break;
            case StatType.PRO_SPEED:
                Calculating(ref stat.speed);
                break;
            case StatType.COUNT:
                Calculating(ref stat.count);
                break;
            case StatType.ELE:
                // �̱���
                break;
            case StatType.PER:
                if (stat.per == -1)
                    break;
                Calculating(ref stat.per);
                break;
            default:
                break;
        }
    }
    public void Update_Stat(ref PlayerStat stat)
    {
        Player player = GameManager_Survivor.Instance.player;

        if (player.data.IsExist(type))
        {
            BonusStat playerBonus = player.data.Find_Bonus(type);
            playerBonus.Calculating(ref amount);
        }

        switch (type)
        {
            case StatType.DEF:
                Calculating(ref stat.defense);
                break;
            case StatType.HP:
                Calculating(ref stat.maxHealth);
                break;
            case StatType.SPEED:
                Calculating(ref stat.speed);
                break;
            case StatType.MISS:
                Calculating(ref stat.missPercent);
                break;
            case StatType.CRIT:
                Calculating(ref stat.critPercent);
                break;
            case StatType.LUCK:
                Calculating(ref stat.luck);
                break;
            case StatType.EXP:
                Calculating(ref stat.expBonus);
                break;
            case StatType.HEAL:
                Calculating(ref stat.heal);
                break;
            case StatType.DRAIN:
                Calculating(ref stat.drainPercent);
                break;
            case StatType.RANGE:
                Calculating(ref stat.range);
                break;
            case StatType.BACK:
                Calculating(ref stat.knockBack);
                break;
            default:
                break;
        }
    }

    public string Get_Discription()
    {
        string output = $"{Get_Name()}��(��) ";

        if (isBase)
        {
            switch(type)
            {
                case StatType.MISS:
                case StatType.CRIT:
                    output += $"{Math.Abs(amount)}% ";
                    break;
                case StatType.COUNT:
                    output += $"{Math.Abs(amount)}�� ";
                    break;
                case StatType.ACTIVE:
                case StatType.COOL:
                    output += $"{Math.Abs(amount)}�� ";
                    break;
                default:
                    output += $"{Math.Abs(amount)}��ŭ ";
                    break;
            }
        }
        else
            output += $"{Math.Abs(amount * 100)}% ";

        output += amount >= 0 ? "����" : "����";

        return output;
    }
    public string Get_Name()
    {
        string output = "";
        switch(type)
        {
            case StatType.DMG:
                output = "���ݷ�";
                break;
            case StatType.DEF:
                output = "����";
                break;
            case StatType.HP:
                output = "ü��";
                break;
            case StatType.SPEED:
                output = "���ǵ�";
                break;
            case StatType.MISS:
                output = "ȸ�� Ȯ��";
                break;
            case StatType.CRIT:
                output = "ġ��Ÿ Ȯ��";
                break;
            case StatType.LUCK:
                output = "��";
                break;
            case StatType.EXP:
                output = "����ġ ������";
                break;
            case StatType.ACTIVE:
                output = "���� ���� �ð�";
                break;
            case StatType.COOL:
                output = "���� ��� �ð�";
                break;
            case StatType.HEAL:
                output = "ȸ����";
                break;
            case StatType.DRAIN:
                output = "����� ���";
                break;
            case StatType.SIZE:
                output = "����ü ũ��";
                break;
            case StatType.PRO_SPEED:
                output = "����ü �ӵ�";
                break;
            case StatType.COUNT:
                output = "����ü ����";
                break;
            case StatType.ELE:
                output = "���� ������";
                break;
            case StatType.RANGE:
                output = "��Ÿ�";
                break;
            case StatType.BACK:
                output = "�˹�";
                break;
            case StatType.PER:
                output = "�����";
                break;
        }
        return output;
    }
}