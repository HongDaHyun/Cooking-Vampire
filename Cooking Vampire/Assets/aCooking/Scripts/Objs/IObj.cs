using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooking;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public abstract class IObj : MonoBehaviour
{
    [Title("�� �����ϱ�")]
    public int objID;

    // ������Ʈ
    protected SpriteRenderer sr;
    protected SpriteData spriteData;
    protected Chef chef;

    protected virtual void Awake()
    {
        chef = GameManager_Cooking.Instance.chef;

        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        spriteData = SpriteData.Instance;
    }

    public abstract void Interact();
}
