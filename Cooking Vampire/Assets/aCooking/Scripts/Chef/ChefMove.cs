using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Cooking;

public class ChefMove : MonoBehaviour
{
    GameManager_Cooking gm;

    [ReadOnly] public Vector2 inputVec;
    [ReadOnly] public Vector2 inputLook;
    private float curSpeed = 2f;
    [ReadOnly] public float curStamina;
    [ReadOnly] public bool isInteract;
    private bool isExhaustion;

    private float walkSpeed = 2f, runSpeed = 5f;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        gm = GameManager_Cooking.Instance;

        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        curStamina = gm.chefStat.STAMINA;
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    void OnRun(InputValue value)
    {
        if (!isExhaustion && value.isPressed)
            curSpeed = runSpeed;
        else
            curSpeed = walkSpeed;
    }
    void OnInteract(InputValue value)
    {
        isInteract = value.isPressed;
        anim.SetBool("Doing", isInteract);
    }

    private void FixedUpdate()
    {
        if (curSpeed == runSpeed && inputVec != Vector2.zero && !isExhaustion)
        {
            curStamina -= 1f * Time.deltaTime; // 1�ʿ� 1�� ����

            // Ż��
            if(curStamina <= 0)
            {
                curStamina = 0;
                isExhaustion = true;
                curSpeed = walkSpeed;
            }
        }
        else
        {
            if (curStamina < gm.chefStat.STAMINA)
                curStamina += 1f * Time.deltaTime; // 1�ʿ� 1�� ȸ��
            else
            {
                curStamina = gm.chefStat.STAMINA;
                isExhaustion = false;
            }

        }
        if (isInteract)
            return;
        Vector2 nextVec = inputVec.normalized * curSpeed * Time.fixedDeltaTime;

        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        // ������ �ִϸ��̼�
        anim.SetFloat("Speed", inputVec.magnitude * curSpeed);

        // ���� ��ȯ
        if (inputVec.x != 0)
            sr.flipX = inputVec.x < 0;
    }
}
