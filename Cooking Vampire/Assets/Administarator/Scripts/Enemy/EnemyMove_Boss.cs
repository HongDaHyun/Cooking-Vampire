using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove_Boss : EnemyMove
{
    public float patternDelayT_min, patternDelayT_max;
    public int patternCount; // 0: 걷기, 달리기(기본), 1~: 특수 패턴

    public int curPatternInt;

    protected Enemy_Projectile_Sprite projectileSprite;

    public override void ReSet()
    {
        base.ReSet();
        projectileSprite = enemy.spriteData.Export_Enemy_Projectile_Sprite(enemy.data.title);
    }

    protected override IEnumerator SpecialMoveRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(SetRanDelay());

            isForceStop = true; isPattern = true;
            SetRanPattern();
            enemy.anim.SetTrigger($"Pattern_{curPatternInt}");
            
            yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName($"Pattern_{curPatternInt}"));
            yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName($"Pattern_{curPatternInt}"));

            curPatternInt = 0;
            isForceStop = false; isPattern = false;
        }
    }

    float SetRanDelay()
    {
        return Random.Range(patternDelayT_min, patternDelayT_max);
    }
    void SetRanPattern()
    {
        curPatternInt = Random.Range(1, patternCount + 1);
    }
}
