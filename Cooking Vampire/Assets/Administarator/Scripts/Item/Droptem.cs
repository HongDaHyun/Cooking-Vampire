using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droptem : Item
{
    public DroptemData temData;

    Animator anim;
    DataManager dataManager;

    public override void OnCreatedInPool()
    {
        anim = GetComponent<Animator>();
        dataManager = DataManager.Instance;
        base.OnCreatedInPool();
    }

    public void SetDropItem(Vector2 pos, DroptemData data)
    {
        temData = data;

        anim.runtimeAnimatorController = temData.anim;

        Drop(pos);
    }

    protected override void Destroy()
    {
        switch(temData.droptemName)
        {
            case "코인":
                dataManager.EarnCoin(50);
                break;
            case "포션":
                gm.stat.HealHP(5);
                break;
            case "보호막":
                player.GetShield(1);
                break;
        }

        if (rm.IsHave(10))
            gm.stat.SetStat(StatID_Player.DMG, 1);
        spawnManager.Destroy_Item(this);
    }

    protected override void Drop(Vector2 pos)
    {
        anim.SetBool("IsMove", false);

        transform.position = pos;

        isActive = true;
    }

    protected override void Drain()
    {
        anim.SetBool("IsMove", true);

        base.Drain();
    }
}