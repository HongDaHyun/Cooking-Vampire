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
    }

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
    public StatData Find_StatCSV(StatType type)
    {
        return Array.Find(csvList.statDatas, data => data.type == type);
    }

    private int Filtering_int(string s)
    {
        if (s == "")
            return NULL;
        return int.Parse(s);
    }
}

[Serializable]
public class CSVList
{
    public StatData[] statDatas;
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