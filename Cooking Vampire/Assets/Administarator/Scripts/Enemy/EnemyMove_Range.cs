using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove_Range : EnemyMove
{
    Enemy_Projectile_Sprite projectileSprite;

    public override void ReSet()
    {
        base.ReSet();
        projectileSprite = enemy.spriteData.Export_Enemy_Projectile_Sprite(enemy.data.title);
    }

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
                    isForceStop = true; isPattern = true;

                    enemy.anim.SetTrigger("Atk");
                    atkCool = 0f;

                    yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));
                    yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));

                    isForceStop = false; isPattern = false;
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

    public void ShootRange()
    {
        Projectile_Enemy projectile = enemy.spawnManager.Spawn_Projectile_Enemy(projectileSprite.sprite, projectileSprite.anim, transform.position);

        Vector3 tarPos = player.Get_Player_RoundPos(1f);
        Vector3 dir = (tarPos - transform.position).normalized;
        projectile.SetDir(dir, 5f);
        projectile.SetRotation(dir);
    }
}
