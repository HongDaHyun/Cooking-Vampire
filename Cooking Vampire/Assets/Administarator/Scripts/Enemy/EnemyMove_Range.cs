using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Range : EnemyMove
{
    protected override IEnumerator SpecialMoveRoutine()
    {
        float atkCool = 0f;

        yield return new WaitUntil(() => target);

        while (true)
        {
            atkCool += Time.fixedDeltaTime;

            if (Vector2.Distance(target.transform.position, transform.position) < 5f)
            {
                if (atkCool > 4f)
                {
                    enemy.anim.SetTrigger("Atk");
                    enemy.ShootRange(target.transform.position);
                    atkCool = 0f;
                    yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));
                }
                else
                {
                    isStop = true;
                    enemy.anim.SetBool("IsStop", true);
                    yield return new WaitForSeconds(0.5f);
                    atkCool += 0.5f;
                }
            }
            else
            {
                isStop = false;
                enemy.anim.SetBool("IsStop", false);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
