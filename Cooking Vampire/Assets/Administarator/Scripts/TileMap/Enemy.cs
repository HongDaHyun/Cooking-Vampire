using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour, IPoolObject
{
    [ReadOnly] public int tier; // 1부터 시작
    [ReadOnly] public bool isDead;
    public EnemyStat stat;

    EnemyMove enemyMove;
    Animator anim;
    GameManager_Survivor gm;
    DataManager dataManager;

    public void OnCreatedInPool()
    {
        gm = GameManager_Survivor.Instance;
        dataManager = DataManager.Instance;
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetEnemy(int _tier)
    {
        tier = _tier;

        EnemyData data = dataManager.Export_EnemyData(tier);
        anim.runtimeAnimatorController = data.Export_RanAnim();

        SetStat(data);
    }

    private void SetStat(EnemyData data)
    {
        stat.maxHp = Mathf.CeilToInt(10 * Mathf.Min(1f, gm.curGameTime) * tier);
        stat.curHp = stat.maxHp;
        stat.speed = data.speed;
    }
}

[System.Serializable]
public struct EnemyStat
{
    public int curHp;
    public int maxHp;
    public float speed;
}