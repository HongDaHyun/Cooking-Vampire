using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStorage : IObj
{
    protected override void DoingFinish()
    {
        if (chef.ingredientInven == 0)
            chef.ingredientInven = 8;
        else if (chef.ingredientInven == 8)
            chef.ingredientInven = 0;
    }
}
