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

    public void Player_GainExp(int amount)
    {
        exp += stat.Get_EXPBONUS(amount);

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
    private void Player_UpStat(StatType statType, int amount)
    {
        if (player.data.IsExist(statType))
            amount += Mathf.RoundToInt(player.data.Find_Bonus(statType).amount * amount);

        switch(statType)
        {
            case StatType.DMG:
                stat.dmg_p += amount;
                break;
            case StatType.DEF:
                stat.defense += amount;
                break;
            case StatType.HP:
                stat.maxHealth += amount;
                break;
            case StatType.SPEED:
                stat.speed_p += amount;
                break;
            case StatType.MISS:
                stat.miss_p += amount;
                break;
            case StatType.CRIT:
                stat.crit_p += amount;
                break;
            case StatType.LUCK:
                stat.luck += amount;
                break;
            case StatType.EXP:
                stat.expBonus_p += amount;
                break;
            case StatType.ACTIVE:
                stat.active_p += amount;
                break;
            case StatType.COOL:
                stat.cool_p += amount;
                break;
            case StatType.HEAL:
                stat.heal += amount;
                break;
            case StatType.DRAIN:
                stat.drain_p += amount;
                break;
            case StatType.PRO_SIZE:
                stat.proSize_p += amount;
                break;
            case StatType.PRO_SPEED:
                stat.proSpeed_p += amount;
                break;
            case StatType.COUNT:
                stat.count += amount;
                break;
            case StatType.ELE:
                stat.element += amount;
                break;
            case StatType.RANGE:
                stat.range += amount;
                break;
            case StatType.BACK:
                stat.knockBack += amount;
                break;
            case StatType.PER:
                stat.per += amount;
                break;
        }
    }
}