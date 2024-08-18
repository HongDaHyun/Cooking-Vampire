using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour, IPoolObject
{
    [HideInInspector] public WeaponStat stat;

    protected Player player;
    protected SpawnManager spawnManager;
    protected GameManager_Survivor gm;
    protected SpriteData spriteData;
    [HideInInspector] public SpriteRenderer sr;
    protected BoxCollider2D col;

    public virtual void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        player = GameManager_Survivor.Instance.player;
        spawnManager = SpawnManager.Instance;
        gm = GameManager_Survivor.Instance;
        spriteData = SpriteData.Instance;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public virtual void OnGettingFromPool()
    {
    }

    void Update()
    {
        Dead();
    }

    public virtual void SetProjectile(Sprite sprite, WeaponStat stat, Transform parent)
    {
        this.stat = stat;
        transform.SetParent(parent);
        float size = gm.stat.Get_Value(StatType.PRO_SIZE, stat.size);
        transform.localScale = new Vector2(size, size);

        SetSprite(sprite);
    }
    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
        ReSetCollider();
    }

    public IEnumerator Spin(Quaternion targetRot, float durTime, int spinCount)
    {
        sr.spriteSortPoint = SpriteSortPoint.Center;

        float elapsedTime = 0f;

        while (elapsedTime < durTime)
        {
            float progress = elapsedTime / durTime;

            float currentAngle = 360 * spinCount * progress;
            Quaternion spinRotation = Quaternion.Euler(0, 0, currentAngle);

            // Combine the spin rotation with the target rotation
            transform.rotation = spinRotation * targetRot;

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = targetRot;

        sr.spriteSortPoint = SpriteSortPoint.Pivot;
    }

    void Dead()
    {
        Transform target = player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 20f)
            spawnManager.Destroy_Projectile(this);
    }

    private void ReSetCollider()
    {
        Vector2 spriteSize = sr.sprite.bounds.size;

        col.size = spriteSize;
        col.offset = sr.sprite.bounds.center;
    }
}