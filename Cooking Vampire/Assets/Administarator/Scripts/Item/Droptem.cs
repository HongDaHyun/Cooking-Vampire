using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droptem : Item
{
    public ItemType type;

    Animator anim;
    DataManager dataManager;

    public override void OnCreatedInPool()
    {
        anim = GetComponent<Animator>();
        dataManager = DataManager.Instance;
        base.OnCreatedInPool();
    }

    public void SetDropItem(Vector2 pos, ItemType _type)
    {
        type = _type;

        anim.runtimeAnimatorController = spriteData.Export_DroptemSprite(type);

        Drop(pos);
    }

    protected override void Destroy()
    {
        switch(type)
        {
            case ItemType.Coin:
                dataManager.EarnCoin(50);
                break;
            case ItemType.Potion:
                break;
            case ItemType.Shield:
                break;
        }
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

public enum ItemType { Coin, Potion, Shield }