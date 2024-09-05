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

        int trueDmg = Mathf.Min(curHp, gm.stat.Get_Value(StatType.DMG, collision.GetComponent<Projectile>().stat.damage));
        Hit(trueDmg);
    }

    protected override void Destroy()
    {
        spawnManager.Spawn_Droptem_Ran(transform.position);
        spawnManager.Destroy_Box(this);
    }

    protected override void Drop(Vector2 pos)
    {
        transform.position = pos;
    }

    public void Hit(int dmg)
    {
        anim.SetTrigger("Hit");

        curHp -= dmg;
        spawnManager.Spawn_PopUpTxt(dmg, transform.position, false);

        if(curHp > 0)
        {
            anim.SetTrigger("Hit");
        }

        else
        {
            Destroy();
            Debug.Log("æ∆¿Ã≈∆ º“»Ø");
        }
    }
}
