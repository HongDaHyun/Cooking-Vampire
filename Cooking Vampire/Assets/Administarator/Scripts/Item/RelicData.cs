using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "RelicData")]
public class RelicData : ScriptableObject
{
    public int ID;
    public TierType tierType;
    public string relicName;
    public RelicContent[] statContent;
    public SpecialContent_Relic specialContent;
    [TextArea] public string explain;
    public Sprite[] sprites; // 0: Idle, 1: Drain, 2: Icon

    public void Collect()
    {
        GameManager_Survivor gm = GameManager_Survivor.Instance;
        SpawnManager sm = SpawnManager.Instance;
        CSVManager cm = CSVManager.Instance;

        DataManager.Instance.relicCollectors.Add(ID);
        sm.Spawn_RelicUI(this);
        string popup = "";
        foreach (RelicContent content in statContent)
        {
            StatData_Player playerData = cm.Find_StatData_Player(content.ID);

            popup += cm.Find_StatData_ContentText(content.amount, playerData.name, playerData.isPercent) + "\n";
            gm.stat.SetStat(content.ID, content.amount);
        }

        sm.Spawn_PopUpTxt(popup, PopUpType.StatUP, gm.player.transform.position);
    }
}

[System.Serializable]
public struct RelicContent
{
    public StatID_Player ID;
    public int amount;
}
[System.Serializable]
public struct SpecialContent_Relic
{
    [TextArea] public string explain;
    public SpecialContent[] specialContents;
    public int amount;
}
[System.Serializable]
public struct SpecialContent
{
    public StatID_Player statID;
    public int percent;
}