using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_6 : Atk
{
    public string petName;
    public List<Pet> pets;

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

        foreach (Pet pet in pets)
            pet.SetAtkStat(stat);
    }

    public void SpawnPet()
    {
        int count = stat.amount - pets.Count;

        for(int i = 0; i < count; i++)
        {
            pets.Add(spawnManager.Spawn_Pet(petName));
        }
    }
}
