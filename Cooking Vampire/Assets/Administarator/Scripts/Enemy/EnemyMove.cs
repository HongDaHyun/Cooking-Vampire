using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public Rigidbody2D target;
    public bool isStop;
    protected void IsStop()
    {
        isStop = true;
    }
    protected void IsMove()
    {
        isStop = false;
    }

    protected Enemy enemy;
    protected Player player;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        player = enemy.gm.player;
        target = player.GetComponent<Rigidbody2D>();
    }
    public virtual void ReSet()
    {
        isStop = false;
        enemy.rigid.simulated = true;

        StopAllCoroutines();
        StartCoroutine(SpecialMoveRoutine());
    }

    protected virtual void FixedUpdate()
    {
        if (enemy.isDead || isStop || enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            return;

        Track();
    }
    protected virtual void LateUpdate()
    {
        if (enemy.isDead || isStop || enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            return;

        enemy.sr.flipX = target.position.x < enemy.rigid.position.x;
    }

    protected virtual void Track()
    {
        Vector2 dirVec = target.position - enemy.rigid.position;
        Vector2 nextVec = dirVec.normalized * enemy.stat.speed * Time.fixedDeltaTime;
        enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
        enemy.rigid.velocity = Vector2.zero;
    }

    public IEnumerator KnockBack()
    {
        yield return new WaitForFixedUpdate();

        Vector3 playerPos = player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        enemy.rigid.AddForce(dirVec.normalized * player.gm.stat.Get_Value(StatType.BACK, 1), ForceMode2D.Impulse);
    }

    protected abstract IEnumerator SpecialMoveRoutine();
}