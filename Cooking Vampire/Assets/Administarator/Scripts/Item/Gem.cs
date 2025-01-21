using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Gem : Item
{
    DataManager dataManager;

    [ReadOnly] public int amount;

    public override void OnCreatedInPool()
    {
        dataManager = DataManager.Instance;
        base.OnCreatedInPool();
    }

    public void SetGem(int unit, Vector2 pos)
    {
        amount = unit;

        Drop(pos);
    }
    protected override void Drain()
    {
        sr.sprite = spriteData.Export_GemSprite(amount).moveSprite;
        base.Drain();
    }
    protected override void Drop(Vector2 pos)
    {
        DropRanPos(pos, spriteData.Export_GemSprite(amount).moveSprite, spriteData.Export_GemSprite(amount).idleSprite);
    }

    protected override void Destroy()
    {
        gm.stat.curExp += gm.stat.Cal_EXP(amount);

        if(rm.IsHave(43) && dataManager.Get_Ran(8))
        {
            RelicData relicData = dataManager.Export_RelicData(43);

            SpecialContent special = relicData.specialContent.FindSpecialContent(StatID_Player.LUK);

            int dmg = Mathf.Max(1, special.CalAmount(gm.stat.GetStat(special.statID, false)));
            Enemy ranEnemy = spawnManager.Find_EnemyList_Ran();

            if (ranEnemy != null)
                ranEnemy.Damaged(dmg);
        }

        spawnManager.Destroy_Gem(this);
    }
}