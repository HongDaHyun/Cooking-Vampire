using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Survivor : Singleton<GameManager_Survivor>
{
    [Title("게임 관리")]
    public float MAX_GAMETIME;
    [ReadOnly] public float curGameTime;
    [ReadOnly] public int playerLvCount;
    public GameObject[] tileMaps;

    [Title("플레이어 정보")]
    public Player player;
    public int level;
    public int killCount;
    public int maxExp;

    [Title("플레이어 스탯")]
    public PlayerStat stat;

    [ReadOnly] public int exp;
    [ReadOnly] public int health;

    private void Start()
    {
        health = stat.maxHealth;
        tileMaps[(int)DataManager.Instance.curStage].gameObject.SetActive(true);
    }

    private void Update()
    {
        curGameTime += Time.deltaTime;

        if(curGameTime > MAX_GAMETIME)
        {
            curGameTime = MAX_GAMETIME;
            // 클리어
        }    
    }

    public void Player_HealHP(int amount)
    {
        health = Mathf.Min(health + amount, stat.maxHealth);
    }
    public void Player_GainExp(int amount)
    {
        exp += stat.Get_Value(StatType.EXP, amount);

        if(exp >= maxExp)
            Player_LevelUp();
    }
    private void Player_LevelUp()
    {
        UIManager um = UIManager.Instance;
        BtnManager bm = BtnManager.Instance;

        do
        {
            playerLvCount++;
            level++;
            exp -= maxExp;
            maxExp *= (int)Mathf.Pow(level, 2);
        }
        while (exp >= maxExp);

        bm.Tab(um.lvUpPannel);
        bm.Stop();
        um.Set_StatUpPannels_Ran();
    }
    public void Player_UpStat(StatType statType, int amount)
    {
        StatData statData = CSVManager.Instance.Find_StatCSV(statType);
        
        if (player.data.IsExist(statType))
            amount += Mathf.RoundToInt(player.data.Find_Bonus(statType).amount * amount);

        switch(statType)
        {
            case StatType.DMG:
                statData.Update_Stat(ref stat.dmg_p, amount);
                break;
            case StatType.DEF:
                statData.Update_Stat(ref stat.defense, amount);
                break;
            case StatType.HP:
                statData.Update_Stat(ref stat.maxHealth, amount);
                health = Mathf.Min(health + amount, stat.maxHealth);
                break;
            case StatType.SPEED:
                statData.Update_Stat(ref stat.speed_p, amount);
                break;
            case StatType.MISS:
                statData.Update_Stat(ref stat.miss_p, amount);
                break;
            case StatType.CRIT:
                statData.Update_Stat(ref stat.crit_p, amount);
                break;
            case StatType.LUCK:
                statData.Update_Stat(ref stat.luck, amount);
                break;
            case StatType.EXP:
                statData.Update_Stat(ref stat.expBonus_p, amount);
                break;
            case StatType.ACTIVE:
                statData.Update_Stat(ref stat.active_p, amount);
                break;
            case StatType.COOL:
                statData.Update_Stat(ref stat.cool_p, amount);
                break;
            case StatType.HEAL:
                statData.Update_Stat(ref stat.heal, amount);
                break;
            case StatType.DRAIN:
                statData.Update_Stat(ref stat.drain_p, amount);
                break;
            case StatType.PRO_SIZE:
                statData.Update_Stat(ref stat.proSize_p, amount);
                break;
            case StatType.PRO_SPEED:
                statData.Update_Stat(ref stat.proSpeed_p, amount);
                break;
            case StatType.COUNT:
                statData.Update_Stat(ref stat.count, amount);
                break;
            case StatType.ELE:
                statData.Update_Stat(ref stat.element, amount);
                break;
            case StatType.RANGE:
                statData.Update_Stat(ref stat.range, amount);
                break;
            case StatType.BACK:
                statData.Update_Stat(ref stat.knockBack, amount);
                break;
            case StatType.PER:
                statData.Update_Stat(ref stat.per, amount);
                break;
        }
    }

    public Tier Get_Tier()
    {
        int ran = Random.Range(0, 100 + stat.luck);

        if (ran < 3 + stat.luck)
            return Tier.Legend;
        else if (ran < 5 + stat.luck)
            return Tier.Epic;
        else if (ran < 40)
            return Tier.Rare;
        else
            return Tier.Common;
    }
}