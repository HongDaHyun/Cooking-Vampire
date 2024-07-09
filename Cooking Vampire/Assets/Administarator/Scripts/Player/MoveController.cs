using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class MoveController : MonoBehaviour
{
    [Title("����", "�Ŀ� ���� ����")]
    public float speed;

    [ReadOnly]
    public Vector2 inputVec;

    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        // ������ �ִϸ��̼�
        anim.SetFloat("Speed", inputVec.magnitude);

        // ���� ��ȯ
        if (inputVec.x != 0)
            sr.flipX = inputVec.x < 0;
    }
}
