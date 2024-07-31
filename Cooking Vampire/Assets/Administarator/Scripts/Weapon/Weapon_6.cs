using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_6 : Weapon
{
    public string petName;
    public int spawnCount;

    public override IEnumerator Active()
    {
        SpawnPet();
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    public override void LevelUp()
    {
        base.LevelUp();
        SpawnPet();
    }

    public void SpawnPet()
    {
        int count = stat.count - spawnCount;

        for(int i = 0; i < count; i++)
        {
            spawnManager.Spawn_Pet(petName);
            spawnCount++;
        }
    }
}
