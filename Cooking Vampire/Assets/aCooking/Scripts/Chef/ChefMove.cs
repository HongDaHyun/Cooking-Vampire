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
        if (!isExhaustion && value.isPressed && !chef.IsItem())
            curSpeed = runSpeed;
        else
            curSpeed = walkSpeed;
    }
    void OnInteract(InputValue value)
    {
        IObj nearObj = chef.chefScan.nearestObj;

        // 근처 상호작용 가능한 obj가 있다면,
        if (nearObj != null)
        {
            isInteract = value.isPressed;

            if (isInteract)
                nearObj.Interact();
        }
        // 근처 먼지가 없고, 아이템을 들고 있다면,
        else if (chef.chefScan.nearestDust == null && chef.IsItem())
        {
            chef.sm.Spawn_Dust(transform.position);
            chef.UseItem();
        }

    }

    public void CarryAnim(bool isCarry)
    {
        anim.SetBool("Carrying", isCarry);
    }
    public void DoingAnim(bool isDoing)
    {
        anim.SetBool("Doing", isDoing);
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
