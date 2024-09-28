using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Area : Projectile_Animation
{
    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();

        col.enabled = false;
        StartCoroutine(LifeRoutine());
    }

    public override void SetProjectile(Sprite sprite, WeaponStat stat, Transform parent)
    {
        this.stat = stat;
        transform.SetParent(parent);
        transform.localScale = new Vector2(stat.size, stat.size);

        SetCol();
    }

    private void SetCol()
    {
        // 현재 정적으로 콜라이더 지정, 후에 필요 시 동적으로 변경
        col.isTrigger = true;
        col.offset = new Vector2(0, 0.35f);
        col.size = new Vector2(0.7f, 0.7f);
    }

    IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(5f);

        animator.SetTrigger("Stop");
        col.enabled = false;
    }

    void FireStart()
    {
        col.enabled = true;
    }
}
