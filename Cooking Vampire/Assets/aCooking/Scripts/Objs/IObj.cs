using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooking;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public abstract class IObj : MonoBehaviour
{
    protected SpawnManager sm;

    // ÄÄÆ÷³ÍÆ®
    [HideInInspector]public SpriteRenderer sr;
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
        sm = SpawnManager.Instance;
    }

    public abstract void Interact();
}
