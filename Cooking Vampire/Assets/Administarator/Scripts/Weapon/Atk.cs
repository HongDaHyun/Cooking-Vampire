using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Atk : MonoBehaviour
{
    public int ID;
    public const int MAX_LV = 5;

    [Title("UI")]
    public Sprite icon;
    public string title;
    [TextArea] public string discription;

    [Title("스탯")]
    public List<Sprite> projectileSprite;
    [ReadOnly] public int lv;
    [ReadOnly] public bool isMax;
    public AtkStat stat;
    public bool isPet;

    [Title("레벨")]
    public AtkStat_LevelUps[] atkStat_LvelUps = new AtkStat_LevelUps[MAX_LV];

    protected GameManager_Survivor gm;
    protected UIManager uiManager;
    protected DataManager dataManager;
    protected SpawnManager spawnManager;
    protected Player player;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();

        gm = GameManager_Survivor.Instance;
        dataManager = DataManager.Instance;
        spawnManager = SpawnManager.Instance;
        uiManager = UIManager.Instance;

        isMax = false;

        if (projectileSprite.Count == 0)
            projectileSprite.Add(dataManager.curWeapon.weaponSprite);
    }
    protected Sprite GetProjectileSprite()
    {
        if (projectileSprite.Count == 1)
            return projectileSprite[0];

        return projectileSprite[UnityEngine.Random.Range(0, projectileSprite.Count)];
    }

    public abstract IEnumerator Active();

    public void SetMax()
    {
        isMax = true;
        MaxLevel();
        uiManager.atkUIs[ID].SetBattery(lv);
    }
    public void SetEquip()
    {
        lv = 1;
    }
    public virtual void LevelUp()
    {
        foreach (AtkStat_LevelUp x in atkStat_LvelUps[lv - 1].atkPerLevels)
            stat.SetStat(x.ID, x.amount);

        lv++;
        uiManager.atkUIs[ID].SetBattery(lv);
    }
    
    protected abstract void MaxLevel();

    protected Projectile_Rigid FireDir(Vector3 dir)
    {
        Projectile_Rigid projectile = spawnManager.Spawn_Projectile_Rigid(GetProjectileSprite(), stat, transform, 0f);

        dir = dir.normalized;
        projectile.SetDir(dir);

        Transform projectTrans = projectile.transform;
        projectTrans.localPosition = Vector2.zero;
        projectTrans.localRotation = Quaternion.FromToRotation(Vector3.up, dir);

        return projectile;
    }
    protected Projectile_Rigid Fire(Vector3 targetPos)
    {
        Projectile_Rigid projectile = spawnManager.Spawn_Projectile_Rigid(GetProjectileSprite(), stat, transform, 0f);

        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        projectile.SetDir(dir);

        Transform projectTrans = projectile.transform;
        projectTrans.localPosition = Vector2.zero;
        projectTrans.localRotation = Quaternion.FromToRotation(Vector3.up, dir);

        return projectile;
    }
    protected Projectile_Rigid Fire_Nearest()
    {
        if (!player.scanner.nearestTarget)
            return Fire_Ran();

        Vector3 targetPos = player.scanner.nearestTarget.position;
        return Fire(targetPos);
    }
    protected Projectile_Rigid Fire_Ran()
    {
        Transform target = player.scanner.Export_RanTarget();
        if (!target)
            return Fire(RanPos());

        return Fire(target.position);
    }
    protected Vector2 RanPos()
    {
        Vector2 center = transform.position;

        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float radius = gm.stat.Cal_RAN();

        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}

public enum StatID_Atk { DMG, AS, AT, SPE, AMT, PER, SIZE }

[Serializable]
public class AtkStat
{
    public const int X = -1;

    public int dmg;
    public float atkSpeed;
    public float activeT;
    public float speed;
    public int amount;
    public int per;
    public float size;
    private Dictionary<StatID_Atk, Action<int>> statActions;

    public AtkStat()
    {
        statActions = new Dictionary<StatID_Atk, Action<int>>
        { 
            {StatID_Atk.DMG, x => dmg += Mathf.RoundToInt(dmg * x / 100f)},
            {StatID_Atk.AS, x => { if(atkSpeed != X) atkSpeed += atkSpeed * x / 100f; } },
            {StatID_Atk.AT, x => { if(activeT != X) activeT += activeT * x / 100f; } },
            {StatID_Atk.SPE, x => speed += speed * x / 100f},
            {StatID_Atk.AMT, x => amount += x},
            {StatID_Atk.PER, x => { if(per != X) per += x; } },
            {StatID_Atk.SIZE, x => size += size * x / 100f},
        };
    }

    public void SetStat(StatID_Atk id, int amount)
    {
        if (statActions.TryGetValue(id, out Action<int> action))
            action(amount);
        else
            throw new ArgumentException("Invalid StatID_Player", nameof(id));
    }
}
[Serializable]
public struct AtkStat_LevelUps
{
    public AtkStat_LevelUp[] atkPerLevels;
}
[Serializable]
public struct AtkStat_LevelUp
{
    public StatID_Atk ID;
    public int amount;
}

public enum WeaponSpriteType { Default, Projectile, Special }