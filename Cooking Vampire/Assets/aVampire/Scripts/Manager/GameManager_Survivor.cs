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
    public bool timeEnd;

    [Title("플레이어 정보")]
    public Player player;
    public int killCount;

    [Title("플레이어 스탯")]
    public PlayerStat stat;

    private void Start()
    {
        stat.curHP = stat.HP;
        stat.curSpeed = stat.defSpeed;
        tileMaps[(int)DataManager.Instance.curStage].gameObject.SetActive(true);

        SetCamZoom();
    }

    private void Update()
    {
        curGameTime += Time.deltaTime;

        if(curGameTime > MAX_GAMETIME && !timeEnd)
        {
            curGameTime = MAX_GAMETIME;
            timeEnd = true;
            StartCoroutine(UIManager.Instance.bossPannel.CinematicSequence());
        }    
    }

    public Vector2 Get_Player_RoundPos(float noise)
    {
        Vector2 playerPos = player.transform.position;

        return new Vector2(playerPos.x + UnityEngine.Random.Range(-noise, noise), playerPos.y + UnityEngine.Random.Range(-noise, noise));
    }
    public int Get_TimeDifficult()
    {
        return Mathf.Max(0, Mathf.RoundToInt(curGameTime / 30f));
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
    [ReadOnly] public float curSpeed;
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

    public PlayerStat()
    {
        statActions = new Dictionary<StatID_Player, Action<int>>
        {
            {StatID_Player.HP, x => hp += x },
            {StatID_Player.HPREG, x => hpReg += x },
            {StatID_Player.DRA, x => drain += x },
            {StatID_Player.DEF, x => def += x },
            {StatID_Player.DMG, x => dmg += x },
            {StatID_Player.ELE, x => ele += x },
            {StatID_Player.AS, x => atkSpeed += x },
            {StatID_Player.AT, x => activeT += x },
            {StatID_Player.CRIT, x => crit += x },
            {StatID_Player.CRIT_DMG, x => critDmg += x },
            {StatID_Player.RAN, x => ran += x },
            {StatID_Player.MIS, x => miss += x },
            {StatID_Player.SPE, x => speed += x },
            {StatID_Player.LUK, x => luk += x},
            {StatID_Player.AMT, x => amount += x },
            {StatID_Player.PER, x => per += x },
            {StatID_Player.BAK, x => back += x},
            {StatID_Player.EXP, x => exp += x }
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

        level = 1;
    }
    public int HP
    {
        get
        {
            return hp;
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
            return hpReg;
        }
        set { hpReg = value; }
    }
    public int DRA
    {
        get
        {
            return drain;
        }
        set { drain = value; }
    }
    public int DEF
    {
        get
        {
            return def;
        }
        set { def = value; }
    }
    public int DMG
    {
        get
        {
            RelicManager rm = RelicManager.Instance;

            int value = dmg;

            if (rm.IsHave(41))
                value += GetStat(StatID_Player.BAK, true);
            if (rm.IsHave(51))
                value += Mathf.RoundToInt(DataManager.Instance.coin / 20f);

            return value;
        }
        set { dmg = value; }
    }
    public int ELE
    {
        get
        {
            return ele;
        }
        set { ele = value; }
    }
    public int AS
    {
        get
        {
            return atkSpeed;
        }
        set { atkSpeed = value; }
    }
    public int AT
    {
        get
        {
            return activeT;
        }
        set { activeT = value; }
    }
    public int CRIT
    {
        get
        {
            int value = crit;

            if (RelicManager.Instance.IsHave(20))
                value += Mathf.RoundToInt(GetStat(StatID_Player.LUK, true) / 10f);

            return value;
        }
        set { crit = value; }
    }
    public int CRIT_DMG
    {
        get
        {
            return critDmg;
        }
        set { critDmg = value; }
    }
    public int RAN
    {
        get
        {
            return ran;
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
            return miss;
        }
        set { miss = value; }
    }
    public int SPE
    {
        get
        {
            return speed;
        }
        set { speed = value; } // Def MoveController 스크립트에 존재
    }
    public int LUK
    {
        get
        {
            return luk;
        }
        set { luk = value; }
    }
    public int AMT
    {
        get
        {
            return amount;
        }
        set { amount = value; }
    }
    public int PER
    {
        get
        {
            return per;
        }
        set { per = value; }
    }
    public int BAK
    {
        get
        {
            return back;
        }
        set { back = value; }
    }
    public int EXP
    {
        get
        {
            return exp;
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
        }
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }
    public int GetStat(StatID_Player id, bool isDefault)
    {
        if (statGetters.TryGetValue(id, out Func<int> getter))
        {
            if (!isDefault)
            {
                StatData_Player statData = CSVManager.Instance.Find_StatData_Player(id);
                return Mathf.Clamp(getter(), statData.min, statData.max);
            }
            else
                return getter();
        }
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }

    public void HealHP(int amount)
    {
        if (curHP >= HP)
            return;

        int healedAmount = Mathf.Min(amount, HP - curHP);
        curHP += healedAmount;

        SpawnManager sm = SpawnManager.Instance;
        Vector2 pos = GameManager_Survivor.Instance.player.transform.position;
        sm.Spawn_PopUpTxt(healedAmount.ToString(), PopUpType.Heal, pos);
        sm.Spawn_Effect("Heal", pos, 1f);
    }
    public void LevelUp()
    {
        playerLvCount++;
        level++;
        SetStat(StatID_Player.HP, 1);
        if (RelicManager.Instance.IsHave(54) && DataManager.Instance.Get_Ran(20))
            SetStat(StatID_Player.HPREG, 1);

        curExp -= maxExp;
        maxExp = Mathf.RoundToInt(maxExp * 1.5f);

        UIManager.Instance.lvUpPannel.Tab(BtnManager.Instance);
    }

    public TierType Get_Tier()
    {
        int weightedLuck = UnityEngine.Random.Range(0, 100 + GetStat(StatID_Player.LUK, false)) + GameManager_Survivor.Instance.Get_TimeDifficult();

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
    public float Cal_HPREG_Cool()
    {
        return 5f / (1 + (GetStat(StatID_Player.HPREG, true) - 1) / 2.25f);
    }
    public bool Cal_DRA_Percent()
    {
        return DataManager.Instance.Get_Ran(GetStat(StatID_Player.DRA, true));
    }
    public float Cal_DEF()
    {
        int defend = GetStat(StatID_Player.DEF, true);
        return (float)defend / (Mathf.Abs(defend) + 15);
    }
    public int Cal_DMG(int defDmg)
    {
        return defDmg + Mathf.RoundToInt(defDmg * GetStat(StatID_Player.DMG, true) / 100f);
    }
    public float Cal_Ele(int defAmount, EleType type)
    {
        float returnAmount = 0;

        switch(type)
        {
            case EleType.Fire: // 단순 덧셈
            case EleType.Poison:
                returnAmount = defAmount + GetStat(StatID_Player.ELE, true);
                break;
            case EleType.Ice: // 최솟값이 2, x값이 커질수록 8에 수렴
                returnAmount = 8 - 6 * Mathf.Exp(-0.05f * defAmount);
                break;
            case EleType.Thunder:
                returnAmount = Mathf.Min(defAmount + GetStat(StatID_Player.ELE, true) / 10f, 6);
                break;
        }

        return Mathf.Max(1, returnAmount);
    }
    public float Cal_AS(float defAtkSpeed)
    {
        return defAtkSpeed - defAtkSpeed * GetStat(StatID_Player.AS, true) / 100f;
    }
    public float Cal_AT(float defActiveT)
    {
        return defActiveT + defActiveT * GetStat(StatID_Player.AT, true) / 100f;
    }
    public bool Cal_CRIT_Percent()
    {
        return DataManager.Instance.Get_Ran(GetStat(StatID_Player.CRIT, true));
    }
    public int Cal_CRIT_DMG(int defDmg)
    {
        return defDmg + Mathf.RoundToInt(defDmg * GetStat(StatID_Player.CRIT_DMG, true) / 100f);
    }
    public float Cal_RAN()
    {
        return defRan + defRan * GetStat(StatID_Player.RAN, true) / 100f;
    }
    public bool Cal_MIS_PERCENT()
    {
        return DataManager.Instance.Get_Ran(GetStat(StatID_Player.MIS, true));
    }
    public float Cal_SPE()
    {
        return curSpeed + curSpeed * GetStat(StatID_Player.SPE, true) / 100f;
    }
    public int Cal_AMT(int defAmount)
    {
        return defAmount + GetStat(StatID_Player.AMT, true);
    }
    public int Cal_PER(int defPer)
    {
        return defPer + GetStat(StatID_Player.PER, true);
    }
    public float Cal_BAK() // 기본값 1
    {
        return defBack + GetStat(StatID_Player.BAK, true) / 100f;
    }
    public int Cal_EXP(int defExp)
    {
        return defExp + Mathf.RoundToInt(defExp * GetStat(StatID_Player.EXP, true) / 100f);
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