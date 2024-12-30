using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Relic : Item, IPoolObject
{
    public RelicData data;

    public void SetRelic(RelicData _Data, Vector2 pos)
    {
        data = _Data;

        Drop(pos);
    }

    protected override void Drain()
    {
        sr.sprite = data.sprites[1];
        base.Drain();
    }

    protected override void Drop(Vector2 pos)
    {
        DropRanPos(pos, data.sprites[1], data.sprites[0]);
    }

    protected override void Destroy()
    {
        data.Collect();
        spawnManager.Destroy_Relic(this);
    }
}
