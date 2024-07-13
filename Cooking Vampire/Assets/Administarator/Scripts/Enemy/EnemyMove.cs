using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D target;
    public bool isStop;

    Enemy enemy;
    Player player;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        player = enemy.gm.player;
        target = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (enemy.isDead || isStop || enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            return;

        Track();
    }

    private void LateUpdate()
    {
        if (enemy.isDead)
            return;

        enemy.sr.flipX = target.position.x < enemy.rigid.position.x;
    }

    public void ReSet()
    {
        isStop = false;
        enemy.rigid.simulated = true;
    }

    private void Track()
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
        enemy.rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }    
}
