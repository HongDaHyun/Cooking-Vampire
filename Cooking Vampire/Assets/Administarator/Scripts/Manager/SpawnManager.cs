using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class SpawnManager : Singleton<SpawnManager>
{
    private void Start()
    {
        Spawn_Relic(DataManager.Instance.relicDatas[0], new Vector2(5, 0));
        Spawn_Relic(DataManager.Instance.relicDatas[1], new Vector2(7, 0));
        Spawn_Gem(10, new Vector2(10, 0));
    }

    #region Ÿ��
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
    #endregion
    #region ��
    public Enemy Spawn_Enemy(EnemyData data, Vector3 position, float size)
    {
        string enemyName = $"Enemy_{data.atkType}";

        if (data.atkType == AtkType.Boss)
            enemyName += $"_{data.title}";

        Enemy enemy = PoolManager.Instance.GetFromPool<Enemy>(enemyName);

        enemy.transform.position = position;
        enemy.SetEnemy(data, size);

        return enemy;
    }
    public Enemy Spawn_Enemy(string enemyName, Vector3 position, float size)
    {
        EnemyData data = DataManager.Instance.Export_EnemyData(enemyName);
        Enemy enemy = Spawn_Enemy(data, position, size);

        return enemy;
    }
    public void Destroy_Enemy(Enemy enemy)
    {
        string enemyName = enemy.data.atkType == AtkType.Boss ? $"Enemy_{enemy.data.atkType}_{enemy.data.title}" : enemy.name;

        PoolManager.Instance.TakeToPool<Enemy>(enemyName, enemy);
    }
    #endregion
    #region ����ü
    public Projectile Spawn_Projectile(Sprite sprite, AtkStat stat, Transform parent)
    {
        Projectile projectile = PoolManager.Instance.GetFromPool<Projectile>("Projectile");

        projectile.SetProjectile(sprite, stat, parent);

        return projectile;
    }
    public Projectile_Rigid Spawn_Projectile_Rigid(Sprite sprite, AtkStat stat, Transform parent, float gravity)
    {
        Projectile_Rigid projectile = PoolManager.Instance.GetFromPool<Projectile_Rigid>("Projectile_Rigid");

        projectile.SetProjectile(sprite, stat, parent);
        projectile.SetGravity(gravity);

        return projectile;
    }
    public Projectile_Enemy Spawn_Projectile_Enemy(Sprite sprite, float size, RuntimeAnimatorController anim, Vector2 position, float gravity)
    {
        Projectile_Enemy projectile = PoolManager.Instance.GetFromPool<Projectile_Enemy>("Projectile_Enemy");

        AtkStat dummy = new AtkStat();
        dummy.size = size;
        projectile.SetProjectile(sprite, dummy, null);
        projectile.SetGravity(gravity);
        projectile.SetAnim(anim);
        projectile.transform.position = position;

        return projectile;
    }
    public Projectile_Animation Spawn_Projectile_Anim(Sprite sprite, AtkStat stat, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Animation projectile = PoolManager.Instance.GetFromPool<Projectile_Animation>("Projectile_Animation");

        projectile.SetProjectile(sprite, stat, parent);
        projectile.SetAnim(anim);

        return projectile;
    }
    public Projectile_Animation Spawn_Projectile_Trap(Sprite sprite, AtkStat stat, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Animation projectile = PoolManager.Instance.GetFromPool<Projectile_Animation>("Projectile_Trap");

        projectile.SetProjectile(sprite, stat, parent);
        projectile.SetAnim(anim);

        return projectile;
    }
    public Projectile_Area Spawn_Projectile_Area(float size, RuntimeAnimatorController anim, Transform parent)
    {
        Projectile_Area projectile = PoolManager.Instance.GetFromPool<Projectile_Area>("Projectile_Area");

        AtkStat dummy = new AtkStat();
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
    #endregion
    #region ������
    private Gem Spawn_Gem(int unit, Vector2 pos)
    {
        if (unit <= 0)
            return null;

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
    public Relic Spawn_Relic(RelicData data, Vector2 pos)
    {
        Relic relic = PoolManager.Instance.GetFromPool<Relic>("Relic");

        relic.SetRelic(data, pos);

        return relic;
    }
    public void Destroy_Relic(Relic relic)
    {
        PoolManager.Instance.TakeToPool<Relic>("Relic", relic);
    }
    #endregion
    #region �˾��ؽ�Ʈ
    public PopUpTxt Spawn_PopUpTxt(string contents, PopUpType type, Vector2 pos)
    {
        PopUpTxt text = PoolManager.Instance.GetFromPool<PopUpTxt>("PopUpTxt");

        text.SetUI(contents, type, pos);

        return text;
    }
    public void Destroy_PopUpTxt(PopUpTxt popUp)
    {
        PoolManager.Instance.TakeToPool<PopUpTxt>(popUp.name, popUp);
    }
    #endregion
    #region ��
    public Pet Spawn_Pet(string name, AtkStat _stat)
    {
        Pet pet = PoolManager.Instance.GetFromPool<Pet>(name);
        pet.stat = _stat;

        return pet;
    }
    #endregion
    #region ����Ʈ
    public Effect_Anim Spawn_Effect(RuntimeAnimatorController anim, Vector2 pos, float size)
    {
        Effect_Anim effect = PoolManager.Instance.GetFromPool<Effect_Anim>("Effect_Anim");
        effect.SetEffect(anim, pos, size);

        return effect;
    }
    public Effect_X Spawn_Effect_X(string spawnName, Vector2 pos, float size)
    {
        Effect_X effect = PoolManager.Instance.GetFromPool<Effect_X>("Effect_X");
        effect.SetEffect(pos, size, spawnName);

        return effect;
    }
    public void Destroy_Effect(Effect effect)
    {
        PoolManager.Instance.TakeToPool<Effect>(effect);
    }
    #endregion
    #region UI
    public StatUI_Player Spawn_StatUI_Player(StatData_Player statData_Player)
    {
        StatUI_Player statUI = PoolManager.Instance.GetFromPool<StatUI_Player>("StatUI_Player");
        statUI.SetUI(statData_Player);

        return statUI;
    }
    public InfoTxt Spawn_InfoTxt(string title, string subTitle, string contents, RectTransform rect, InfoTxtController controller)
    {
        if (controller.isInfoing)
            return null;
        InfoTxt text = PoolManager.Instance.GetFromPool<InfoTxt>("InfoTxt");
        text.SetText(title, subTitle, contents, rect, controller);

        return text;
    }
    public void Destroy_InfoTxt(InfoTxt txt)
    {
        txt.transform.SetParent(null);
        PoolManager.Instance.TakeToPool<InfoTxt>(txt);
    }
    public RelicUI Spawn_RelicUI(RelicData data)
    {
        RelicUI relicUI = PoolManager.Instance.GetFromPool<RelicUI>("RelicUI");
        relicUI.SetUI(data);

        return relicUI;
    }
    #endregion
}
