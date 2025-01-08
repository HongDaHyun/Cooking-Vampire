using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class GameManager_Survivor : Singleton<GameManager_Survivor>
{
    [Title("게임 관리")]
    public float MAX_GAMETIME;
    [ReadOnly] public float curGameTime;
    public GameObject[] tileMaps;

    [Title("플레이어 정보")]
    public Player player;
    public int killCount;

    [Title("플레이어 스탯")]
    public PlayerStat stat;

    private void Start()
    {
        stat.curHP = stat.HP;
        tileMaps[(int)DataManager.Instance.curStage].gameObject.SetActive(true);

        SetCamZoom();
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

    public Vector2 Get_Player_RoundPos(float noise)
    {
        Vector2 playerPos = player.transform.position;

        return new Vector2(playerPos.x + UnityEngine.Random.Range(-noise, noise), playerPos.y + UnityEngine.Random.Range(-noise, noise));
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
    public void SetCamZoom()
    {
        ProCamera2DPixelPerfect cam = Camera.main.GetComponent<ProCamera2DPixelPerfect>();

        cam.Zoom = Mathf.RoundToInt(Mathf.Lerp(4f, 1f, Mathf.Clamp01(stat.RAN / 300f)));
    }
}

[Serializable]
public class PlayerStat
{
    [Title("정보")]
    [ReadOnly] public int curHP;
    [ReadOnly] public int curExp;
    [ReadOnly] public int playerLvCount;
    [ReadOnly] public int level = 1;
    public int maxExp;
    public float defRan;
    public float defSpeed;
    public float defBack;

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
    private Dictionary<StatID_Player, Func<int>> statGetters;
    private Dictionary<StatID_Player, Func<int>> statGetters_def;

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
        statGetters = new Dictionary<StatID_Player, Func<int>>
        {
            {StatID_Player.HP, () => HP },
            {StatID_Player.HPREG, () => HPREG },
            {StatID_Player.DRA, () => DRA },
            {StatID_Player.DEF, () => DEF },
            {StatID_Player.DMG, () => DMG },
            {StatID_Player.ELE, () => ELE },
            {StatID_Player.AS, () => AS },
            {StatID_Player.AT, () => AT },
            {StatID_Player.CRIT, () => CRIT },
            {StatID_Player.CRIT_DMG, () => CRIT_DMG },
            {StatID_Player.RAN, () => RAN },
            {StatID_Player.MIS, () => MIS },
            {StatID_Player.SPE, () => SPE },
            {StatID_Player.LUK, () => LUK },
            {StatID_Player.AMT, () => AMT },
            {StatID_Player.PER, () => PER },
            {StatID_Player.BAK, () => BAK },
            {StatID_Player.EXP, () => EXP }
        };
        statGetters_def = new Dictionary<StatID_Player, Func<int>>
        {
            {StatID_Player.HP, () => hp },
            {StatID_Player.HPREG, () => hpReg },
            {StatID_Player.DRA, () => drain },
            {StatID_Player.DEF, () => def },
            {StatID_Player.DMG, () => dmg },
            {StatID_Player.ELE, () => ele },
            {StatID_Player.AS, () => atkSpeed },
            {StatID_Player.AT, () => activeT },
            {StatID_Player.CRIT, () => crit },
            {StatID_Player.CRIT_DMG, () => critDmg },
            {StatID_Player.RAN, () => ran },
            {StatID_Player.MIS, () => miss },
            {StatID_Player.SPE, () => speed },
            {StatID_Player.LUK, () => luk },
            {StatID_Player.AMT, () => amount },
            {StatID_Player.PER, () => per },
            {StatID_Player.BAK, () => back },
            {StatID_Player.EXP, () => exp }
        };

        level = 1;
    }
    public int HP
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.HP);
            
            return Mathf.Clamp(hp, statData.min, statData.max);
        }
        set 
        {
            hp = value;
        }
    }
    public int HPREG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.HPREG);
            return Mathf.Clamp(hpReg, statData.min, statData.max);
        }
        set { hpReg = value; }
    }
    public int DRA
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DRA);
            return Mathf.Clamp(drain, statData.min, statData.max);
        }
        set { drain = value; }
    }
    public int DEF
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DEF);
            return Mathf.Clamp(def, statData.min, statData.max);
        }
        set { def = value; }
    }
    public int DMG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.DMG);
            return Mathf.Clamp(dmg, statData.min, statData.max);
        }
        set { dmg = value; }
    }
    public int ELE
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.ELE);
            return Mathf.Clamp(ele, statData.min, statData.max);
        }
        set { ele = value; }
    }
    public int AS
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AS);
            return Mathf.Clamp(atkSpeed, statData.min, statData.max);
        }
        set { atkSpeed = value; }
    }
    public int AT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AT);
            return Mathf.Clamp(activeT, statData.min, statData.max);
        }
        set { activeT = value; }
    }
    public int CRIT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.CRIT);
            return Mathf.Clamp(crit, statData.min, statData.max);
        }
        set { crit = value; }
    }
    public int CRIT_DMG
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.CRIT_DMG);
            return Mathf.Clamp(critDmg, statData.min, statData.max);
        }
        set { critDmg = value; }
    }
    public int RAN
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.RAN);
            return Mathf.Clamp(ran, statData.min, statData.max);
        }
        set 
        { 
            ran = value;
        }
    }
    public int MIS
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.MIS);
            return Mathf.Clamp(miss, statData.min, statData.max);
        }
        set { miss = value; }
    }
    public int SPE
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.SPE);
            return Mathf.Clamp(speed, statData.min, statData.max);
        }
        set { speed = value; } // Def MoveController 스크립트에 존재
    }
    public int LUK
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.LUK);
            return Mathf.Clamp(luk, statData.min, statData.max);
        }
        set { luk = value; }
    }
    public int AMT
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.AMT);
            return Mathf.Clamp(amount, statData.min, statData.max);
        }
        set { amount = value; }
    }
    public int PER
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.PER);
            return Mathf.Clamp(per, statData.min, statData.max);
        }
        set { per = value; }
    }
    public int BAK
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.BAK);
            return Mathf.Clamp(back, statData.min, statData.max);
        }
        set { back = value; }
    }
    public int EXP
    {
        get
        {
            StatData_Player statData = CSVManager.Instance.Find_StatData_Player(StatID_Player.EXP);
            return Mathf.Clamp(exp, statData.min, statData.max);
        }
        set { exp = value; }
    }

    public void SetStat(StatID_Player id, int amount)
    {
        if (statActions.TryGetValue(id, out Action<int> action))
        {
            float crystalIncrease = GameManager_Survivor.Instance.player.data.GetCrystal(id).GetAmount();
            amount = crystalIncrease != 0f ? amount + Mathf.RoundToInt(amount * crystalIncrease) : amount;

            action(amount);
            UIManager.Instance.lvUpPannel.Adjust_StatUI_Player(id);
        }
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }
    public int GetStat(StatID_Player id)
    {
        if (statGetters.TryGetValue(id, out Func<int> getter))
            return getter();
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }
    public int GetStat_Def(StatID_Player id)
    {
        if (statGetters_def.TryGetValue(id, out Func<int> getter))
            return getter();
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }

    public void HealHP(int amount)
    {
        if (curHP >= HP)
            return;

        int healedAmount = Mathf.Min(amount, HP - curHP);
        curHP += healedAmount;
        SpawnManager.Instance.Spawn_PopUpTxt(healedAmount.ToString(), PopUpType.Heal, GameManager_Survivor.Instance.player.transform.position);
    }
    public void LevelUp()
    {
        playerLvCount++;
        level++;
        SetStat(StatID_Player.HP, 1);
        curExp -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.5f);

        UIManager.Instance.lvUpPannel.Tab(BtnManager.Instance);
    }

    public float Cal_HPREG_Cool()
    {
        return 5f / (1 + (HPREG - 1) / 2.25f);
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
    public float Cal_RAN()
    {
        return defRan + defRan * RAN / 100f;
    }
    public bool Cal_MIS_PERCENT()
    {
        return DataManager.Instance.Get_Ran(MIS);
    }
    public float Cal_SPE()
    {
        return defSpeed + defSpeed * SPE / 100f;
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
        return defBack + BAK / 100f;
    }
    public int Cal_EXP(int defExp)
    {
        return defExp + Mathf.RoundToInt(defExp * EXP / 100f);
    }
}

[Serializable]
public struct PlayerStat_Crystal
{
    public StatID_Player ID;
    public int increaseAmount;

    public string Export_Explain()
    {
        StatData_Player statData = CSVManager.Instance.Find_StatData_Player(ID);
        SpriteData spriteData = SpriteData.Instance;
        bool isPlus = increaseAmount >= 0;

        return $"{statData.name} 수정 <color={spriteData.Export_ColorTag(spriteData.Export_SignColor(increaseAmount))}>{(isPlus ? "+" : "-")}{Mathf.Abs(increaseAmount)}%</color>";
    }
    public float GetAmount()
    {
        return increaseAmount / 100f;
    }
}