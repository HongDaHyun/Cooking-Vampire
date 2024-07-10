using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D target;

    Enemy enemy;
    Rigidbody2D rigid;
    SpriteRenderer sr;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        target = GameManager_Survivor.Instance.player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (enemy.isDead)
            return;

        Track();
    }

    private void LateUpdate()
    {
        if (enemy.isDead)
            return;

        sr.flipX = target.position.x < rigid.position.x;
    }

    private void Track()
    {
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * enemy.stat.speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
}
