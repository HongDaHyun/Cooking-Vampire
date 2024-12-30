using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "RelicData")]
public class RelicData : ScriptableObject
{
    public int ID;
    public TierType tierType;
    public string relicName;
    [TextArea] public string contents;
    [TextArea] public string explain;
    public Sprite[] sprites; // 0: Idle, 1: Drain, 2: Icon

    public void Collect()
    {
        DataManager.Instance.relicCollectors.Add(ID);
        SpawnManager.Instance.Spawn_RelicUI(this);
    }
}
