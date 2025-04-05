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
    [ReadOnly] public bool isInteract, isCarry;
    private bool isExhaustion;

    Chef chef;
    private float walkSpeed = 2f, runSpeed = 5f;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        gm = GameManager_Cooking.Instance;

        chef = GetComponent<Chef>();
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
        if (!isExhaustion && value.isPressed && !isCarry)
            curSpeed = runSpeed;
        else
            curSpeed = walkSpeed;
    }
    void OnInteract(InputValue value)
    {
        IObj nearObj = chef.chefScan.nearestObj;

        if (nearObj == null)
            return;

        isInteract = value.isPressed;

        switch(nearObj.objID)
        {
            case 0: // 오븐
                if (nearObj.GetComponent<Oven>().ingredients.Count <= 0 && chef.ingredientInven == 0)
                    return;
                break;
            case 1: // 오크 통
                Oak oak = nearObj.GetComponent<Oak>();

                if (oak.amount <= 0 || (chef.ingredientInven != 0 && oak.ingredientID != chef.ingredientInven))
                    return;
                break;
            case 2:
                if (chef.ingredientInven != 0 && chef.ingredientInven != 8)
                    return;
                break;
        }

        if(nearObj.isPickable && isInteract)
        {
            isCarry = !isCarry;
            anim.SetBool("Carrying", isCarry);
            nearObj.Doing();
        }
        else if(!nearObj.isPickable)
        {
            anim.SetBool("Doing", isInteract);
            nearObj.Doing();
        }
    }

    private void FixedUpdate()
    {
        if (curSpeed == runSpeed && inputVec != Vector2.zero && !isExhaustion)
        {
            curStamina -= 1f * Time.deltaTime; // 1초에 1씩 감소

            // 탈진
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
                curStamina += 1f * Time.deltaTime; // 1초에 1씩 회복
            else
            {
                curStamina = gm.chefStat.STAMINA;
                isExhaustion = false;
            }

        }

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
