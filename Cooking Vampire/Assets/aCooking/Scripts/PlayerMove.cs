using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class PlayerMove : MonoBehaviour
{
    [ReadOnly] public Vector2 inputVec;
    [ReadOnly] public Vector2 inputLook;
    private float curSpeed = 2f;

    private float walkSpeed = 2f, runSpeed = 5f;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(LookRoutine());
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    void OnRun(InputValue value)
    {
        curSpeed = value.isPressed ? runSpeed : walkSpeed;
    }
    IEnumerator LookRoutine()
    {
        if (inputVec != Vector2.zero)
            inputLook = inputVec;
        yield return new WaitForSeconds(0.1f);
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * curSpeed * Time.fixedDeltaTime;

        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        // 움직임 애니메이션
        anim.SetFloat("Speed", inputVec.magnitude * curSpeed);

        // 방향 전환
        if (inputVec.x != 0)
            sr.flipX = inputVec.x < 0;
    }
}
