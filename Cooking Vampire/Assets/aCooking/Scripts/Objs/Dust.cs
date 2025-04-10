using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : IObj_Slider
{
    public override void Interact()
    {
        if (!isDoing && chef.IsItem(100))
        {
            isDoing = true;

            chef.UseItem();
        }
            

        if(isDoing)
            StartCoroutine(SliderRoutine());
    }

    protected override void SliderFinish()
    {
        sm.Destroy_Dust(this);
        isDoing = false;
    }
}
