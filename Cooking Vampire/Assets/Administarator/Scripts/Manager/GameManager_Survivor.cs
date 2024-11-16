using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
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
        health = stat.HP;
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
        int healedAmount = Mathf.Min(amount, stat.HP - health);
        health += healedAmount;
        SpawnManager.Instance.Spawn_PopUpTxt(healedAmount.ToString(), PopUpType.Heal, player.transform.position);
    }
    public void Player_GainExp(int amount)
    {
        exp += stat.Cal_EXP(amount);
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
    public TierType Get_Tier()
    {
        int weightedLuck = UnityEngine.Random.Range(0, 100 + stat.LUK) + Get_TimeDifficult();

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
        float randomValue = UnityEngine.Random.Range(0f, totalChance);

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

[Serializable]
public class PlayerStat
{
    [Title("증가폭")]
    public int hp;
    public int hpReg;
    public int drain;
    public int def;
    public int dmg;
    public int ele;
    public int atkSpeed;
    public int activeT;
    public int crit;
    public int critDmg;
    public int ran;
    public int miss;
    public int speed;
    public int luk;
    public int amount;
    public int per;
    public int back;
    public int exp;
    private Dictionary<StatID_Player, Action<int>> statActions;

    public PlayerStat()
    {
        statActions = new Dictionary<StatID_Player, Action<int>>
        {
            {StatID_Player.HP, x => HP += x },
            {StatID_Player.HPREG, x => HPREG += x },
            {StatID_Player.DRA, x => DRA += x },
            {StatID_Player.DEF, x => DEF += x },
            {StatID_Player.DMG, x => DMG += x },
            {StatID_Player.ELE, x => ELE += x },
            {StatID_Player.AS, x => AS += x },
            {StatID_Player.AT, x => AT += x },
            {StatID_Player.CRIT, x => CRIT += x },
            {StatID_Player.CRIT_DMG, x => CRIT_DMG += x },
            {StatID_Player.RAN, x => RAN += x },
            {StatID_Player.MIS, x => MIS += x },
            {StatID_Player.SPE, x => SPE += x },
            {StatID_Player.LUK, x => LUK += x },
            {StatID_Player.AMT, x => AMT += x },
            {StatID_Player.PER, x => PER += x },
            {StatID_Player.BAK, x => BAK += x },
            {StatID_Player.EXP, x => EXP += x }
        };
    }
    public int HP
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.HP);
            return Mathf.Clamp(hp, statData.max, statData.min);
        }
        set { hp = value; }
    }
    public int HPREG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.HPREG);
            return Mathf.Clamp(hpReg, statData.max, statData.min);
        }
        set { hpReg = value; }
    }
    public int DRA
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DRA);
            return Mathf.Clamp(drain, statData.max, statData.min);
        }
        set { drain = value; }
    }
    public int DEF
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DEF);
            return Mathf.Clamp(def, statData.max, statData.min);
        }
        set { def = value; }
    }
    public int DMG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DMG);
            return Mathf.Clamp(dmg, statData.max, statData.min);
        }
        set { dmg = value; }
    }
    public int ELE
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.ELE);
            return Mathf.Clamp(ele, statData.max, statData.min);
        }
        set { ele = value; }
    }
    public int AS
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AS);
            return Mathf.Clamp(atkSpeed, statData.max, statData.min);
        }
        set { atkSpeed = value; }
    }
    public int AT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AT);
            return Mathf.Clamp(activeT, statData.max, statData.min);
        }
        set { activeT = value; }
    }
    public int CRIT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.CRIT);
            return Mathf.Clamp(crit, statData.max, statData.min);
        }
        set { crit = value; }
    }
    public int CRIT_DMG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.CRIT_DMG);
            return Mathf.Clamp(critDmg, statData.max, statData.min);
        }
        set { critDmg = value; }
    }
    public int RAN
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.RAN);
            return Mathf.Clamp(ran, statData.max, statData.min);
        }
        set { ran = value; }
    }
    public int MIS
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.MIS);
            return Mathf.Clamp(miss, statData.max, statData.min);
        }
        set { miss = value; }
    }
    public int SPE
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.SPE);
            return Mathf.Clamp(speed, statData.max, statData.min);
        }
        set { speed = value; } // Def MoveController 스크립트에 존재
    }
    public int LUK
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.LUK);
            return Mathf.Clamp(luk, statData.max, statData.min);
        }
        set { luk = value; }
    }
    public int AMT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AMT);
            return Mathf.Clamp(amount, statData.max, statData.min);
        }
        set { amount = value; }
    }
    public int PER
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.PER);
            return Mathf.Clamp(per, statData.max, statData.min);
        }
        set { per = value; }
    }
    public int BAK
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.BAK);
            return Mathf.Clamp(back, statData.max, statData.min);
        }
        set { back = value; }
    }
    public int EXP
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.EXP);
            return Mathf.Clamp(exp, statData.max, statData.min);
        }
        set { exp = value; }
    }

    public void SetStat(StatID_Player id, int amount)
    {
        if (statActions.TryGetValue(id, out Action<int> action))
            action(amount);
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }

    public float Cal_HPREG_Cool()
    {
        return 5f / (1 + (amount - 1) / 2.25f);
    }
    public bool Cal_DRA_Percent()
    {
        return DataManager.Instance.Get_Ran(DRA);
    }
    public float Cal_DEF()
    {
        return (float)DEF / (Mathf.Abs(DEF) + 15);
    }
    public int Cal_DMG(int defDmg)
    {
        return defDmg + Mathf.RoundToInt(defDmg * DMG / 100f);
    }
    public int Cal_Ele(int defDmg)
    {
        return defDmg + Mathf.RoundToInt(defDmg * ELE / 100f);
    }
    public float Cal_AS(float defAtkSpeed)
    {
        return defAtkSpeed - defAtkSpeed * AS / 100f;
    }
    public float Cal_AT(float defActiveT)
    {
        return defActiveT + defActiveT * AT / 100f;
    }
    public bool Cal_CRIT_Percent()
    {
        return DataManager.Instance.Get_Ran(CRIT);
    }
    public int Cal_CRIT_DMG(int defDmg)
    {
        return defDmg + Mathf.RoundToInt(defDmg * CRIT_DMG / 100f);
    }
    public float Cal_RAN() // 기본값 5
    {
        return 5 + 5 * RAN / 100f;
    }
    public bool Cal_MIS_PERCENT()
    {
        return DataManager.Instance.Get_Ran(MIS);
    }
    public float Cal_SPE() // 기본값 2.5
    {
        return 2.5f + 2.5f * SPE / 100f;
    }
    public int Cal_AMT(int defAmount)
    {
        return defAmount + AMT;
    }
    public int Cal_PER(int defPer)
    {
        return defPer + PER;
    }
    public float Cal_BAK() // 기본값 1
    {
        return 1f + BAK / 100f;
    }
    public int Cal_EXP(int defExp)
    {
        return defExp + Mathf.RoundToInt(defExp * EXP / 100f);
    }
}