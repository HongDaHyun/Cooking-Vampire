using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public Rigidbody2D target;
    public bool isStop, isForceStop;
    public bool isPattern;
    public float curSpeed;
    protected void IsStop()
    {
        isForceStop = true;
    }
    protected void IsMove()
    {
        isForceStop = false;
    }

    protected Enemy enemy;
    protected Player player;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    protected virtual void Start()
    {
        player = enemy.gm.player;
        target = player.GetComponent<Rigidbody2D>();
    }
    public virtual void ReSet()
    {
        isStop = false; isForceStop = false; isPattern = false;
        enemy.rigid.simulated = true;
        curSpeed = enemy.stat.speed;

        StopAllCoroutines();
        StartCoroutine(SpecialMoveRoutine());
    }

    protected virtual void FixedUpdate()
    {
        if (ChkStop())
            return;

        Track();
    }
    protected virtual void LateUpdate()
    {
        if (ChkStop())
            return;

        enemy.sr.flipX = target.position.x < enemy.rigid.position.x;
    }

    protected bool ChkStop()
    {
        if (enemy.isDead || isStop || isForceStop)
            return true;
        else
            return false;
    }

    protected virtual void Track()
    {
        Vector2 dirVec = target.position - enemy.rigid.position;
        Vector2 nextVec = dirVec.normalized * curSpeed * Time.fixedDeltaTime;
        enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
        enemy.rigid.velocity = Vector2.zero;
    }

    public IEnumerator KnockBack()
    {
        if (ChkStop())
            yield break;

        isStop = true;
        Vector3 playerPos = player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        enemy.rigid.velocity = Vector2.zero;
        enemy.rigid.AddForce(dirVec.normalized * player.gm.stat.Cal_BAK(), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);
        enemy.rigid.velocity = Vector2.zero;
        isStop = false;
    }

    protected abstract IEnumerator SpecialMoveRoutine();
}