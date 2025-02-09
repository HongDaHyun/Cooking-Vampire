using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class SpawnManager : Singleton<SpawnManager>
{
    public List<Enemy> enemyList;

    private void Start()
    {
        // Spawn_Droptem(DataManager.Instance.droptemDatas[2], new Vector2(7, 0));
        //Spawn_Relic(DataManager.Instance.relicDatas[37], new Vector2(4, 0));
        // Spawn_Relic(DataManager.Instance.relicDatas[32], new Vector2(7, 0));
        //Spawn_Enemy(DataManager.Instance.enemyDatas[6], new Vector2(6, 1), 1f).enemyMove.enabled = false;
        //Spawn_Enemy(DataManager.Instance.enemyDatas[6], new Vector2(8, 0), 1f).enemyMove.enabled = false;
        //Spawn_Enemy(DataManager.Instance.enemyDatas[6], new Vector2(8, 2), 1f).enemyMove.enabled = false;
        //Spawn_Enemy(DataManager.Instance.enemyDatas[6], new Vector2(10, 1), 1f).enemyMove.enabled = false;
        //Spawn_IngredientDrop(0, new Vector2(5, 0));
        //Spawn_IngredientDrop(0, new Vector2(6, 0));
        //Spawn_IngredientDrop(0, new Vector2(7, 0));
        Spawn_Gem(10, new Vector2(10, 0));
    }

    #region enemyList
    public Enemy Find_EnemyList_Ran()
    {
        if (enemyList.Count == 0)
            return null;

        return enemyList[Random.Range(0, enemyList.Count)];
    }
    #endregion
    #region 타일
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
    #region 적
    public Enemy Spawn_Enemy(EnemyData data, Vector3 position, float size)
    {
        string enemyName = $"Enemy_{data.atkType}";

        if (data.atkType == AtkType.Boss)
            enemyName += $"_{data.title}";

        Enemy enemy = PoolManager.Instance.GetFromPool<Enemy>(enemyName);
        enemyList.Add(enemy);

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
        enemyList.Remove(enemy);

        PoolManager.Instance.TakeToPool<Enemy>(enemyName, enemy);
    }
    public void KillAll_Enemy()
    {
        foreach(Enemy enemy in enemyList)
        {
            enemy.Dead(true);
        }
    }
    #endregion
    #region 투사체
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
    #region 아이템
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
    public Relic Spawn_Relic_Ran(Vector2 pos)
    {
        DataManager dm = DataManager.Instance;
        RelicManager rm = RelicManager.Instance;

        List<RelicData> relicList = dm.relicDatas.Where(x => !rm.relicCollectors.Contains(x.ID)).ToList();

        if (relicList.Count == 0)
            return null;

        return Spawn_Relic(relicList[Random.Range(0, relicList.Count)], pos);
    }
    public Ingredient_Drop Spawn_IngredientDrop(int ID, Vector2 pos)
    {
        Ingredient_Drop ingredient = PoolManager.Instance.GetFromPool<Ingredient_Drop>("Ingredient_Drop");

        ingredient.SetIngredient(ID, pos);

        return ingredient;
    }
    #endregion
    #region 팝업텍스트
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
    #region 펫
    public Pet Spawn_Pet(string name, AtkStat _stat)
    {
        Pet pet = PoolManager.Instance.GetFromPool<Pet>(name);
        pet.stat = _stat;

        return pet;
    }
    #endregion
    #region 이펙트
    public Effect Spawn_Effect(string effectName, Vector2 pos, float size)
    {
        Effect effect = PoolManager.Instance.GetFromPool<Effect>($"Effect_{effectName}");
        effect.SetTrans(pos, size);

        return effect;
    }
    public Effect_Anim Spawn_Effect_Anim(RuntimeAnimatorController anim, Vector2 pos, float size)
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
        PoolManager.Instance.TakeToPool<Effect>(effect.name, effect);
    }
    public ChainThunder Spawn_ChainThunder(int amountToChain, Enemy enemy)
    {
        if (amountToChain <= 0)
            return null;
        ChainThunder thunder = PoolManager.Instance.GetFromPool<ChainThunder>("Effect_ChainThunder");
        thunder.SetChainThunder(amountToChain, enemy);

        return thunder;
    }
    public void Destroy_ChainThunder(ChainThunder chainThunder)
    {
        PoolManager.Instance.TakeToPool<ChainThunder>(chainThunder);
    }
    public Effect_Particle_Loop Spawn_Effect_Loop(string effectName, Transform parent, float size, float lifeTime)
    {
        Effect_Particle_Loop effect = PoolManager.Instance.GetFromPool<Effect_Particle_Loop>($"Effect_{effectName}");
        effect.SetEffect(parent, size, lifeTime);

        return effect;
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
