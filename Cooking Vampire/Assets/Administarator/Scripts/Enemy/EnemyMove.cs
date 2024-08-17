using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D target;
    public bool isStop, isCharge;

    private void IsStop()
    {
        isStop = true;
    }
    private void IsMove()
    {
        isStop = false;
    }
    private void IsCharge_Stop()
    {
        IsMove();
        isCharge = false;
    }

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
        if (enemy.isDead || isStop || isCharge || enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
            return;

        Track();
    }

    private void LateUpdate()
    {
        if (enemy.isDead || isStop)
            return;

        enemy.sr.flipX = target.position.x < enemy.rigid.position.x;
    }

    public void ReSet()
    {
        isStop = false;
        isCharge = false;
        enemy.rigid.simulated = true;

        StopAllCoroutines();
        StartCoroutine($"{enemy.data.type}Move");
    }

    private void Track()
    {
        Vector2 dirVec = target.position - enemy.rigid.position;
        Vector2 nextVec = dirVec.normalized * enemy.stat.speed * Time.fixedDeltaTime;
        enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
        enemy.rigid.velocity = Vector2.zero;
    }
    private IEnumerator Charging()
    {
        isCharge = true;
        Vector2 dirVec = (target.position - enemy.rigid.position).normalized;

        while (isCharge)
        {
            Vector2 nextVec = dirVec.normalized * enemy.stat.speed * 7f * Time.fixedDeltaTime;
            enemy.rigid.MovePosition(enemy.rigid.position + nextVec);
            enemy.rigid.velocity = Vector2.zero;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator KnockBack()
    {
        yield return new WaitForFixedUpdate();

        Vector3 playerPos = player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        enemy.rigid.AddForce(dirVec.normalized * player.gm.stat.Get_Value(StatType.BACK, 1), ForceMode2D.Impulse);
    }    

    private IEnumerator NormalMove()
    {
        yield break;
    }
    private IEnumerator ChargeMove()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);

            enemy.anim.SetTrigger("Charge");
        }
    }
    private IEnumerator RangeMove()
    {
        float atkCool = 0f;

        yield return new WaitUntil(() => target);

        while(true)
        {
            atkCool += Time.fixedDeltaTime;

            if(Vector2.Distance(target.transform.position, transform.position) < 5f)
            {
                if(atkCool > 4f)
                {
                    enemy.anim.SetTrigger("Atk");
                    enemy.ShootRange(target.transform.position);
                    atkCool = 0f;
                    yield return new WaitUntil(() => !enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Atk"));
                }
                else
                {
                    IsStop();
                    enemy.anim.SetBool("IsStop", true);
                    yield return new WaitForSeconds(0.5f);
                    atkCool += 0.5f;
                }
            }
            else
            {
                IsMove();
                enemy.anim.SetBool("IsStop", false);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}