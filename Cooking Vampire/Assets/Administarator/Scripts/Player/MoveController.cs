using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class MoveController : MonoBehaviour
{
    [ReadOnly] public Vector2 inputVec;
    [ReadOnly] public Vector2 inputLook;
    public float defSpeed;

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
    }

    private void Start()
    {
        StartCoroutine(LookRoutine());
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    IEnumerator LookRoutine()
    {
        while(true)
        {
            if (inputVec != Vector2.zero)
                inputLook = inputVec;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * gm.stat.Get_SPEED(defSpeed) * Time.fixedDeltaTime;

        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        // 움직임 애니메이션
        anim.SetFloat("Speed", inputVec.magnitude);

        // 방향 전환
        if (inputVec.x != 0)
            sr.flipX = inputVec.x < 0;
    }
}
