using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_Move : MonoBehaviour
{
    SpriteRenderer sr;
    Animator anim;

    [HideInInspector] public Vector3 targetPos;
    public Point curPoint;
    public float curSpeed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        targetPos = new Vector3(6, 0, 0);
        StartCoroutine(MoveRoutine());
    }

    //public List<Point> PathFinding()
    //{
        
    //}

    public IEnumerator MoveRoutine()
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, curSpeed * Time.fixedDeltaTime);
            yield return null; // ���� �����ӱ��� ���
        }
        curSpeed = 0f;
    }

    private void LateUpdate()
    {
        // ������ �ִϸ��̼�
        anim.SetFloat("Speed", curSpeed);

        // ���� ��ȯ
        sr.flipX = transform.position.x > targetPos.x;
    }
}
