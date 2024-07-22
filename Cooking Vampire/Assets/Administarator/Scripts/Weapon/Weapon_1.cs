using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target_Shooting
public class Weapon_1 : Weapon
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
        yield return new WaitForSeconds(0.2f);
        for(int i = 1; i < stat.count; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Fire_Ran();
        }
    }
}
