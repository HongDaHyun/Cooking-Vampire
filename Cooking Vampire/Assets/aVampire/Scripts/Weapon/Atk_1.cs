using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target_Shooting
public class Atk_1 : Atk
{
    public override IEnumerator Active()
    {
        yield return FireRoutine();
    }

    protected override void MaxLevel()
    {
    }

    private IEnumerator FireRoutine()
    {
        Fire_Nearest();
        for(int i = 1; i < gm.stat.Cal_AMT(stat.amount); i++)
        {
            yield return new WaitForSeconds(0.2f);
            Fire_Ran();
        }
    }
}
