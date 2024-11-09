using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVManager : Singleton<CSVManager>
{
    public const int NULL = -99999;

    public TextAsset[] textAssets;
    public CSVList csvList = new CSVList();

    protected override void Awake()
    {
        base.Awake();

        CSV_Stat();
        Import_StatData_Player();
    }

    #region Import
    public void CSV_Stat()
    {
        int order = 0; int size = 5;
        string[] data = textAssets[order].text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / size - 1;
        csvList.statDatas = new StatData[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int k = i + 1;
            csvList.statDatas[i] = new StatData
            {
                type = (StatType)Enum.Parse(typeof(StatType), data[size * k]),
                baseAmount = Filtering_int(data[size * k + 1]),
                minAmount = Filtering_int(data[size * k + 2]),
                maxAmount = Filtering_int(data[size * k + 3]),
                isPercent = bool.Parse(data[size * k + 4])
            };
        }
    }

    public void Import_StatData_Player()
    {
        int order = 1; int size = 6;
        string[] data = textAssets[order].text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / size - 1;
        csvList.statDatas_Player = new StatData_Player[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int k = i + 1;
            csvList.statDatas_Player[i] = new StatData_Player
            {
                ID = (StatID_Player)Enum.Parse(typeof(StatID_Player), data[size * k]),
                name = data[size * k + 1],
                explanation = data[size * k + 2],
                min = Filtering_int(data[size * k + 3]),
                max = Filtering_int(data[size * k + 4]),
                isPercent = bool.Parse(data[size * k + 5])
            };
        }
    }

    private int Filtering_int(string s)
    {
        if (s == "")
            return NULL;
        return int.Parse(s);
    }
    #endregion

    #region Export
    public StatData Find_StatCSV(StatType type)
    {
        return Array.Find(csvList.statDatas, data => data.type == type);
    }
    #endregion
}

[Serializable]
public class CSVList
{
    public StatData[] statDatas;
    public StatData_Player[] statDatas_Player;
}

[Serializable]
public struct StatData
{
    public StatType type;
    public int baseAmount;
    public int minAmount;
    public int maxAmount;
    public bool isPercent;

    public void Update_Stat(ref int defStat, int amount)
    {
        defStat += amount;
    }
}

[Serializable]
public struct StatData_Player
{
    public StatID_Player ID;
    public string name;
    public string explanation;
    public int min, max;
    public bool isPercent;
}