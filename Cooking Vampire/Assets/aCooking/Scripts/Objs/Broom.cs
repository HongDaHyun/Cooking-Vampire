using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : IObj
{
    public override void Interact()
    {
        if (!chef.IsItem())
            chef.GainItem(100);
        else if (chef.IsItem(100))
            chef.UseItem();
    }
}
