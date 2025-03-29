using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_2 : Atk
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
        StartCoroutine(MoveToTargetAndBack(Fire_Nearest()));
        yield return new WaitForSeconds(0.2f);
        for (int i = 1; i < gm.stat.Cal_AMT(stat.amount); i++)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(MoveToTargetAndBack(Fire_Ran()));
        }
    }

    private IEnumerator MoveToTargetAndBack(Projectile_Rigid projectile)
    {
        yield return new WaitUntil(() => Vector2.Distance(projectile.transform.position, transform.position) > gm.stat.Cal_RAN());
        // 정지
        projectile.SetDir(Vector3.zero);

        // 회전
        Vector3 direction = (player.transform.position - projectile.transform.position).normalized;
        direction.z = 0;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        yield return projectile.Spin(targetRotation, stat.speed / 45f, 1);

        projectile.SetDir(-direction, stat.speed / 2f);
        // 되돌아오기
        yield return new WaitForSeconds(0.1f);
        projectile.SetDir(direction);
    }
}
