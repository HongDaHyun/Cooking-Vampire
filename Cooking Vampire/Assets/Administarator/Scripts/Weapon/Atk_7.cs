using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_7 : Atk
{
    public RuntimeAnimatorController trapAnim;
    SpriteData spriteData;

    protected override void Awake()
    {
        base.Awake();
        spriteData = SpriteData.Instance;
    }

    public override IEnumerator Active()
    {
        int count = gm.stat.Get_Value(StatType.COUNT, stat.count);
        for (int i = 0; i < count; i++)
        {
            Trap();

            if(i != count - 1)
                yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    protected override void MaxLevel()
    {
    }

    private void Trap()
    {
        spawnManager.Spawn_Effect(spriteData.effects[0], player.transform.position, 1f);
        Projectile projectile = spawnManager.Spawn_Projectile_Trap(GetProjectileSprite(), stat, trapAnim, null);
        projectile.transform.position = player.transform.position;
    }
}
