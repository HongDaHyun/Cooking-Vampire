using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Vampire;

public class MoveController : MonoBehaviour
{
    [ReadOnly] public Vector2 inputVec;
    [ReadOnly] public Vector2 inputLook;

    Player player;
    GameManager_Survivor gm;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gm = GameManager_Survivor.Instance;
        player = GetComponent<Player>();
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (player.isDead)
            return;

        Vector2 nextVec = inputVec.normalized * gm.stat.Cal_SPE() * Time.fixedDeltaTime;

        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (player.isDead)
            return;

        // ������ �ִϸ��̼�
        anim.SetFloat("Speed", inputVec.magnitude);

        // ���� ��ȯ
        if (inputVec.x != 0)
            sr.flipX = inputVec.x < 0;
    }
}
