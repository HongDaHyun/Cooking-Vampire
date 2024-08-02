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
                // 미구현
                break;
            case StatType.PRO_SPEED:
                Calculating(ref stat.speed);
                break;
            case StatType.COUNT:
                Calculating(ref stat.count);
                break;
            case StatType.ELE:
                // 미구현
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
        string output = $"{Get_Name()}이(가) ";

        if (isBase)
        {
            switch(type)
            {
                case StatType.MISS:
                case StatType.CRIT:
                    output += $"{Math.Abs(amount)}% ";
                    break;
                case StatType.COUNT:
                    output += $"{Math.Abs(amount)}개 ";
                    break;
                case StatType.ACTIVE:
                case StatType.COOL:
                    output += $"{Math.Abs(amount)}초 ";
                    break;
                default:
                    output += $"{Math.Abs(amount)}만큼 ";
                    break;
            }
        }
        else
            output += $"{Math.Abs(amount * 100)}% ";

        output += amount >= 0 ? "증가" : "감소";

        return output;
    }
    public string Get_Name()
    {
        string output = "";
        switch(type)
        {
            case StatType.DMG:
                output = "공격력";
                break;
            case StatType.DEF:
                output = "방어력";
                break;
            case StatType.HP:
                output = "체력";
                break;
            case StatType.SPEED:
                output = "스피드";
                break;
            case StatType.MISS:
                output = "회피 확률";
                break;
            case StatType.CRIT:
                output = "치명타 확률";
                break;
            case StatType.LUCK:
                output = "운";
                break;
            case StatType.EXP:
                output = "경험치 증가량";
                break;
            case StatType.ACTIVE:
                output = "무기 지속 시간";
                break;
            case StatType.COOL:
                output = "무기 대기 시간";
                break;
            case StatType.HEAL:
                output = "회복력";
                break;
            case StatType.DRAIN:
                output = "생명력 흡수";
                break;
            case StatType.SIZE:
                output = "투사체 크기";
                break;
            case StatType.PRO_SPEED:
                output = "투사체 속도";
                break;
            case StatType.COUNT:
                output = "투사체 개수";
                break;
            case StatType.ELE:
                output = "원소 데미지";
                break;
            case StatType.RANGE:
                output = "사거리";
                break;
            case StatType.BACK:
                output = "넉백";
                break;
            case StatType.PER:
                output = "관통력";
                break;
        }
        return output;
    }
}