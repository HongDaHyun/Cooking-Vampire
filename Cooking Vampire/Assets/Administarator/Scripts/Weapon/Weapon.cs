using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int ID;

    [Title("UI")]
    public Sprite icon;
    public string title;
    [TextArea] public string discription;

    [Title("스탯")]
    public List<Sprite> projectileSprite;
    [ReadOnly] public int lv;
    [ReadOnly] public bool isMax;
    public WeaponStat stat;
    public bool isPet;

    [Title("레벨")]
    public WeaponLevel[] weaponPerLevels;

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

        SetStat();
    }
    private void SetStat()
    {
        int bonus = dataManager.curWeapon.tier;

        stat.damage += bonus;
        stat.speed += bonus;
    }
    protected Sprite GetProjectileSprite()
    {
        if (projectileSprite.Count == 1)
            return projectileSprite[0];

        return projectileSprite[Random.Range(0, projectileSprite.Count)];
    }

    public abstract IEnumerator Active();

    public virtual void LevelUp()
    {
        if(lv >=  weaponPerLevels.Length + 1)
        {
            isMax = true;
            MaxLevel();
            return;
        }

        foreach (BonusStat update in weaponPerLevels[lv - 1].updates)
            update.Update_Stat(ref stat);

        lv++;
    }
    
    protected abstract void MaxLevel();

    protected Projectile_Rigid FireDir(Vector3 dir)
    {
        Projectile_Rigid projectile = spawnManager.Spawn_Projectile_Rigid(GetProjectileSprite(), stat, 1, transform);

        dir = dir.normalized;
        projectile.SetDir(dir);

        Transform projectTrans = projectile.transform;
        projectTrans.localPosition = Vector2.zero;
        projectTrans.localRotation = Quaternion.FromToRotation(Vector3.up, dir);

        return projectile;
    }
    protected Projectile_Rigid Fire(Vector3 targetPos)
    {
        Projectile_Rigid projectile = spawnManager.Spawn_Projectile_Rigid(GetProjectileSprite(), stat, 1, transform);

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

        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = gm.stat.range;

        float x = center.x + radius * Mathf.Cos(angle);
        float y = center.y + radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public string Export_LevelDiscription()
    {
        if (lv == 0)
            return discription;
        else if(lv < weaponPerLevels.Length + 1)
        {
            string sum = "";

            BonusStat[] updates = weaponPerLevels[lv - 1].updates;

            foreach(BonusStat update in updates)
            {
                string element = "";

                element += update.Get_Discription();

                if (update.type != updates[updates.Length - 1].type)
                    element += "\n";

                sum += element;
            }

            return sum;
        }
        else
        {
            //Max
            return "MAX";
        }
    }
}

[System.Serializable]
public struct WeaponStat
{
    public int count;
    public float coolTime, activeTime;
    public int damage;
    public float speed;
    public int per; // 관통력
}

[System.Serializable]
public struct WeaponLevel
{
    public BonusStat[] updates;
}

public enum WeaponSpriteType { Default, Projectile, Special }