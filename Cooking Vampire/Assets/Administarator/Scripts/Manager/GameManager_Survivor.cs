using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int healedAmount = Mathf.Min(amount, stat.maxHealth - health);
        health += healedAmount;
        SpawnManager.Instance.Spawn_PopUpTxt(healedAmount.ToString(), PopUpType.Heal, player.transform.position);
    }
    public void Player_GainExp(int amount)
    {
        exp += stat.Get_Value(StatType.EXP, amount);
    }
    public void Player_LevelUp()
    {
        UIManager um = UIManager.Instance;
        BtnManager bm = BtnManager.Instance;

        playerLvCount++;
        level++;
        exp -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.5f);

        bm.Tab(um.lvUpPannel.transform);
        bm.Stop();
        um.lvUpPannel.Set_StatUpPannels_Ran();
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
    public TierType Get_Tier()
    {
        int weightedLuck = Random.Range(0, 100 + stat.Get_Value(StatType.LUCK)) + Get_TimeDifficult();

        // 확률 기반으로 티어 결정 (weightedLuck이 클수록 높은 티어 확률 증가)
        float epicChance = Mathf.Clamp((weightedLuck - 100) * 0.3f, 0, 30);      // epic은 빠르게 증가 (최대 30%)
        float legendChance = Mathf.Clamp((weightedLuck - 100) * 0.2f, 0, 30);    // legend는 epic보다 약간 덜 증가 (최대 30%)

        // rareChance는 epicChance와 legendChance가 높아질수록 줄어들도록 설정
        float rareChanceBase = Mathf.Clamp(40 + (weightedLuck * 0.1f), 40, 60);  // 기본 rare 확률
        float rareChance = Mathf.Clamp(rareChanceBase - (epicChance + legendChance) * 0.5f, 10, rareChanceBase);  // epic, legend 증가에 따라 rare 감소

        // commonChance는 나머지 확률로 설정되며, rareChance와 epicChance, legendChance가 증가함에 따라 줄어듦
        float commonChance = Mathf.Clamp(100 - rareChance - epicChance - legendChance, 10, 60);  // common은 나머지

        // 총합 계산
        float totalChance = legendChance + epicChance + rareChance + commonChance;

        // 난수 생성 및 확률에 따라 티어 선택
        float randomValue = Random.Range(0f, totalChance);

        if (randomValue <= legendChance)
            return TierType.Legend;
        else if (randomValue <= legendChance + epicChance)
            return TierType.Epic;
        else if (randomValue <= legendChance + epicChance + rareChance)
            return TierType.Rare;
        else
            return TierType.Common;
    }
    public int Get_TimeDifficult()
    {
        return Mathf.Max(1, Mathf.RoundToInt(curGameTime / 10));
    }
}