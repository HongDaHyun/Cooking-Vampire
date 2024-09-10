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

    public Enemy Spawn_Enemy(EnemyData data, Vector3 position)
    {
        Enemy enemy = PoolManager.Instance.GetFromPool<Enemy>($"Enemy_{data.atkType}");

        enemy.transform.position = position;
        enemy.SetEnemy(data);

        return enemy;
    }
    public void Destroy_Enemy(Enemy enemy)
    {
        PoolManager.Instance.TakeToPool<Enemy>(enemy.name, enemy);
    }

    public Projectile Spawn_Projectile(Sprite sprite, WeaponStat stat, Transform parent)
    {
        Projectile projectile = PoolManager.Instance.GetFromPool<Projectile>("Projectile");

        projectile.SetProjectile(sprite, stat, parent);

        return projectile;
    }
    public Projectile_Rigid Spawn_Projectile_Rigid(Sprite sprite, WeaponStat stat, Transform parent)
    {
        Projectile_Rigid projectile = PoolManager.Instance.GetFromPool<Projectile_Rigid>("Projectile_Rigid");

        projectile.SetProjectile(sprite, stat, parent);

        return projectile;
    }
    public Projectile_Enemy Spawn_Projectile_Enemy(Sprite sprite, float size, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Enemy projectile = PoolManager.Instance.GetFromPool<Projectile_Enemy>("Projectile_Enemy");

        WeaponStat dummy = new WeaponStat();
        dummy.size = size;
        projectile.SetProjectile(sprite, dummy, null);
        projectile.SetAnim(anim);
        projectile.transform.position = parent.position;

        return projectile;
    }
    public Projectile_Animation Spawn_Projectile_Anim(Sprite sprite, WeaponStat stat, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Animation projectile = PoolManager.Instance.GetFromPool<Projectile_Animation>("Projectile_Animation");

        projectile.SetProjectile(sprite, stat, parent);
        projectile.SetAnim(anim);

        return projectile;
    }
    public Projectile_Animation Spawn_Projectile_Trap(Sprite sprite, WeaponStat stat, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Animation projectile = PoolManager.Instance.GetFromPool<Projectile_Animation>("Projectile_Trap");

        projectile.SetProjectile(sprite, stat, parent);
        projectile.SetAnim(anim);

        return projectile;
    }
    public Projectile_Area Spawn_Projectile_Area(float size, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Area projectile = PoolManager.Instance.GetFromPool<Projectile_Area>("Projectile_Area");

        WeaponStat dummy = new WeaponStat();
        dummy.size = size;
        projectile.SetProjectile(null, dummy, null);
        projectile.SetAnim(anim);
        projectile.transform.position = parent.position;

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
    public Box Spawn_Box(Vector2 pos)
    {
        Box box = PoolManager.Instance.GetFromPool<Box>("Box");

        box.SetBox(pos);

        return box;
    }
    private Droptem Spawn_Droptem(DroptemData data, Vector2 pos)
    {
        Droptem droptem = PoolManager.Instance.GetFromPool<Droptem>("Droptem");

        droptem.SetDropItem(pos, data);

        return droptem;
    }
    public void Spawn_Droptem_Ran(Vector2 pos)
    {
        DataManager dm = DataManager.Instance;

        Spawn_Droptem(dm.Export_DroptemData_Ran(), pos);
    }
    public void Destroy_Item(Item item)
    {
        PoolManager.Instance.TakeToPool<Item>(item.name, item);
    }

    public TextMeshPro Spawn_PopUpTxt(int amount, Vector2 pos, bool isCrit)
    {
        SpriteData data = SpriteData.Instance;

        TextMeshPro text = PoolManager.Instance.GetFromPool<TextMeshPro>("PopUpTxt");
        text.transform.position = pos;

        text.text = amount.ToString();
        text.color = isCrit ? data.Export_Pallate(ColorType.Yellow) : data.Export_Pallate(ColorType.Red);

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

    public Pet Spawn_Pet(string name)
    {
        Pet pet = PoolManager.Instance.GetFromPool<Pet>(name);

        return pet;
    }

    public Effect Spawn_Effect(RuntimeAnimatorController anim, Vector2 pos, float size)
    {
        Effect effect = PoolManager.Instance.GetFromPool<Effect>("Effect");
        effect.SetEffect(anim, pos, size);

        return effect;
    }
    public void Destroy_Effect(Effect effect)
    {
        PoolManager.Instance.TakeToPool<Effect>(effect);
    }
}
