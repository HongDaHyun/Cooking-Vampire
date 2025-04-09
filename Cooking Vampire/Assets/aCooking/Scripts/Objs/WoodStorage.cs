using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStorage : IObj
{
    public override void Interact()
    {
        if (!chef.IsItem())
            chef.GainItem(8);
        else if (chef.IsItem(8))
            chef.UseItem();
    }
}
