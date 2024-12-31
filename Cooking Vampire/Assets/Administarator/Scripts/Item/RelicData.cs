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
    [TextArea] public string contents;
    [TextArea] public string explain;
    public Sprite[] sprites; // 0: Idle, 1: Drain, 2: Icon

    public void Collect()
    {
        GameManager_Survivor gm = GameManager_Survivor.Instance;
        SpawnManager sm = SpawnManager.Instance;

        DataManager.Instance.relicCollectors.Add(ID);
        sm.Spawn_RelicUI(this);
        foreach (RelicContent content in statContent)
        {
            gm.stat.SetStat(content.ID, content.amount);
        }
    }
}

[System.Serializable]
public struct RelicContent
{
    public StatID_Player ID;
    public int amount;
}