using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour, IPoolObject
{
    [HideInInspector] public Weapon weapon;

    SpriteRenderer sr;
    BoxCollider2D col;

    public void OnCreatedInPool()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetProjectile(Sprite sprite, Weapon weapon)
    {
        this.weapon = weapon;
        transform.SetParent(weapon.transform);
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;

        sr.sprite = sprite;
        ReSetCollider();
    }

    private void ReSetCollider()
    {
        Vector2 spriteSize = sr.sprite.bounds.size;

        col.size = spriteSize;
        col.offset = sr.sprite.bounds.center;
    }
}
