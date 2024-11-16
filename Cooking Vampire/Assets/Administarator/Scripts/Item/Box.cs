using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Item
{
    int maxHp, curHp;
    Animator anim;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        anim = GetComponent<Animator>();
    }

    public void SetBox(Vector2 pos)
    {
        Drop(pos);

        maxHp = 50 * Mathf.Max(1, Mathf.RoundToInt(gm.curGameTime / 10));
        curHp = maxHp;
    }

    protected override void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Projectile"))
            return;

        Hit(collision.GetComponent<Projectile>().stat.dmg);
    }

    protected override void Destroy()
    {
        spawnManager.Spawn_Droptem_Ran(transform.position);
        spawnManager.Destroy_Item(this);
    }

    protected override void Drop(Vector2 pos)
    {
        transform.position = pos;
    }

    public void Hit(int dmg)
    {
        anim.SetTrigger("Hit");

        dmg = gm.stat.Cal_DMG(dmg);

        // 크리티컬
        bool isCrit = gm.stat.Cal_CRIT_Percent();
        if (isCrit)
            gm.stat.Cal_CRIT_DMG(dmg);

        curHp -= Mathf.Min(curHp, dmg);
        spawnManager.Spawn_PopUpTxt(dmg.ToString(), isCrit ? PopUpType.Deal_Crit : PopUpType.Deal, transform.position);

        if (curHp > 0)
        {
            anim.SetTrigger("Hit");
        }

        else
        {
            Destroy();
        }
    }
}
