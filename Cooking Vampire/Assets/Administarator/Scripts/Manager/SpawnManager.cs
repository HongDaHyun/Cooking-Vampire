using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using TMPro;
using DG.Tweening;

public class SpawnManager : Singleton<SpawnManager>
{

    public SpriteRenderer Spawn_TileObj(Transform parentTrans)
    {
        SpriteRenderer obj = PoolManager.Instance.GetFromPool<SpriteRenderer>("TileObj");

        obj.transform.SetParent(parentTrans);

        return obj;
    }
    public void Destroy_TileObj(SpriteRenderer sr)
    {
        PoolManager.Instance.TakeToPool<SpriteRenderer>(sr);
    }

    public Enemy Spawn_Enemy(int[] spawnTier, Vector3 position)
    {
        Enemy enemy = PoolManager.Instance.GetFromPool<Enemy>("Enemy");

        enemy.transform.position = position;
        enemy.SetEnemy(spawnTier[Random.Range(0, spawnTier.Length)]);

        return enemy;
    }
    public void Destroy_Enemy(Enemy enemy)
    {
        PoolManager.Instance.TakeToPool<Enemy>(enemy.name, enemy);
    }

    public Projectile Spawn_Projectile(Sprite sprite, Weapon weapon, float size)
    {
        Projectile projectile = PoolManager.Instance.GetFromPool<Projectile>("Projectile");

        projectile.SetProjectile(sprite, weapon, size);

        return projectile;
    }
    public Projectile_Rigid Spawn_Projectile_Rigid(Sprite sprite, Weapon weapon, float size)
    {
        Projectile_Rigid projectile = PoolManager.Instance.GetFromPool<Projectile_Rigid>("Projectile_Rigid");

        projectile.SetProjectile(sprite, weapon, size);

        return projectile;
    }
    public Projectile_Animation Spawn_Projectile_Anim(Sprite sprite, Weapon weapon, RuntimeAnimatorController anim, float size)
    {
        Projectile_Animation projectile = PoolManager.Instance.GetFromPool<Projectile_Animation>("Projectile_Animation");

        projectile.SetProjectile(sprite, weapon, size);
        projectile.SetAnim(anim);

        return projectile;
    }
    public void Destroy_Projectile(Projectile projectile)
    {
        PoolManager.Instance.TakeToPool<Projectile>(projectile.name, projectile);
    }

    private Gem Spawn_Gem(int unit, Vector2 pos)
    {
        Gem gem = PoolManager.Instance.GetFromPool<Gem>("Gem");

        gem.SetGem(unit, pos);

        return gem;
    }
    public void Spawn_Gems(int amount, Vector2 pos)
    {
        GemSprite[] gemSprites = SpriteData.Instance.gemSprites;

        foreach (GemSprite gemSprite in gemSprites)
        {
            int unit = gemSprite.unit;
            int quotient = amount / unit;

            for (int j = 0; j < quotient; j++)
            {
                Spawn_Gem(unit, pos);
            }

            amount %= unit;

            if (amount == 0)
                break;
        }
    }
    public void Destroy_Gem(Gem gem)
    {
        PoolManager.Instance.TakeToPool<Gem>(gem.name, gem);
    }

    public TextMeshPro Spawn_PopUpTxt(int amount, Vector2 pos)
    {
        TextMeshPro text = PoolManager.Instance.GetFromPool<TextMeshPro>("PopUpTxt");
        text.transform.position = pos;

        text.text = amount.ToString();
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        Sequence popSeq = DOTween.Sequence().SetUpdate(true);
        popSeq.Append(text.transform.DOMoveY(pos.y + 1f, 0.5f))
            .Join(text.DOFade(0f, 1f).SetEase(Ease.InExpo))
            .OnComplete(() => Destroy_PopUpTxt(text));

        return text;
    }
    public void Destroy_PopUpTxt(TextMeshPro tmpro)
    {
        PoolManager.Instance.TakeToPool<TextMeshPro>(tmpro);
    }
}
