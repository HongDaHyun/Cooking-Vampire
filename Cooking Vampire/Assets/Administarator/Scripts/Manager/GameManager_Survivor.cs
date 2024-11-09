using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager_Survivor : Singleton<GameManager_Survivor>
{
    [Title("���� ����")]
    public float MAX_GAMETIME;
    [ReadOnly] public float curGameTime;
    [ReadOnly] public int playerLvCount;
    public GameObject[] tileMaps;

    [Title("�÷��̾� ����")]
    public Player player;
    public int level;
    public int killCount;
    public int maxExp;

    [Title("�÷��̾� ����")]
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
            // Ŭ����
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

        // Ȯ�� ������� Ƽ�� ���� (weightedLuck�� Ŭ���� ���� Ƽ�� Ȯ�� ����)
        float epicChance = Mathf.Clamp((weightedLuck - 100) * 0.3f, 0, 30);      // epic�� ������ ���� (�ִ� 30%)
        float legendChance = Mathf.Clamp((weightedLuck - 100) * 0.2f, 0, 30);    // legend�� epic���� �ణ �� ���� (�ִ� 30%)

        // rareChance�� epicChance�� legendChance�� ���������� �پ�鵵�� ����
        float rareChanceBase = Mathf.Clamp(40 + (weightedLuck * 0.1f), 40, 60);  // �⺻ rare Ȯ��
        float rareChance = Mathf.Clamp(rareChanceBase - (epicChance + legendChance) * 0.5f, 10, rareChanceBase);  // epic, legend ������ ���� rare ����

        // commonChance�� ������ Ȯ���� �����Ǹ�, rareChance�� epicChance, legendChance�� �����Կ� ���� �پ��
        float commonChance = Mathf.Clamp(100 - rareChance - epicChance - legendChance, 10, 60);  // common�� ������

        // ���� ���
        float totalChance = legendChance + epicChance + rareChance + commonChance;

        // ���� ���� �� Ȯ���� ���� Ƽ�� ����
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