using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVManager : Singleton<CSVManager>
{
    public const int NULL = 9999999;

    public TextAsset[] textAssets;
    public CSVList csvList = new CSVList();

    protected override void Awake()
    {
        base.Awake();

        Import_StatData_Player();
        Import_StatData_PlayerLvUp();
        Import_StatData_Atk();
    }

    #region Import
    public void Import_StatData_Player()
    {
        int order = 0; int size = 6;
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
                min = -Filtering_int(data[size * k + 3]),
                max = Filtering_int(data[size * k + 4]),
                isPercent = bool.Parse(data[size * k + 5])
            };
        }
    }
    public void Import_StatData_PlayerLvUp()
    {
        int order = 1; int size = 6;
        string[] data = textAssets[order].text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / size - 1;
        csvList.statDatas_PlayerLvUp = new StatData_PlayerLvUp[tableSize];

        for(int i = 0; i< tableSize; i++)
        {
            int k = i + 1;
            csvList.statDatas_PlayerLvUp[i] = new StatData_PlayerLvUp
            {
                ID = (StatID_Player)Enum.Parse(typeof(StatID_Player), data[size * k]),
                tierPer = new int[4],
                isLegend = bool.Parse(data[size * k + 5])
            };

            for (int j = 0; j < 4; i++)
                csvList.statDatas_PlayerLvUp[i].tierPer[j] = int.Parse(data[size * k + 1 + j]);
        }
    }
    public void Import_StatData_Atk()
    {
        int order = 2; int size = 3;
        string[] data = textAssets[order].text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / size - 1;
        csvList.statDatas_Atk = new StatData_Atk[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int k = i + 1;
            csvList.statDatas_Atk[i] = new StatData_Atk
            {
                ID = (StatID_Atk)Enum.Parse(typeof(StatID_Atk), data[size * k]),
                name = data[size * k + 1],
                isPercent = bool.Parse(data[size * k + 2])
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
    public StatData_Player Find_StatData_Player(StatID_Player statID)
    {
        return Array.Find(csvList.statDatas_Player, data => data.ID == statID);
    }
    public int Find_StatData_PlayerLvUp(StatID_Player statID, TierType tier)
    {
        return Array.Find(csvList.statDatas_PlayerLvUp, data => data.ID == statID).tierPer[(int)tier];
    }
    public StatData_Atk Find_StatData_Atk(StatID_Atk statID)
    {
        return Array.Find(csvList.statDatas_Atk, data => data.ID == statID);
    }
    #endregion
}

[Serializable]
public class CSVList
{
    public StatData_Player[] statDatas_Player;
    public StatData_PlayerLvUp[] statDatas_PlayerLvUp;
    public StatData_Atk[] statDatas_Atk;
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
[Serializable]
public struct StatData_PlayerLvUp
{
    public StatID_Player ID;
    public int[] tierPer;
    public bool isLegend;
}
[Serializable]
public struct StatData_Atk
{
    public StatID_Atk ID;
    public string name;
    public bool isPercent;
}