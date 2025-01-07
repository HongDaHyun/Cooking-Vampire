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
        if(statContent.Length != 0)
        {
            string popup = "";
            foreach (RelicContent content in statContent)
            {
                StatData_Player playerData = cm.Find_StatData_Player(content.ID);

                popup += cm.Find_StatData_ContentText(content.amount, playerData.name, playerData.isPercent) + "\n";
                gm.stat.SetStat(content.ID, content.amount);
            }

            sm.Spawn_PopUpTxt(popup, PopUpType.StatUP, gm.player.transform.position);
        }

        if(specialContent.explain != "")
        {
            sm.Spawn_PopUpTxt(string.Format(specialContent.explain, cm.Find_StatData_SpecialRelic_ContentText(specialContent.specialContents)),
                PopUpType.StatUP, gm.player.transform.position);
            switch(ID)
            {
                case 2:
                    gm.player.relic2.SetActive(true);
                    break;
            }
        }
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
    public int dealAmount;
}
[System.Serializable]
public struct SpecialContent
{
    public StatID_Player statID;
    public int def, percent;
}