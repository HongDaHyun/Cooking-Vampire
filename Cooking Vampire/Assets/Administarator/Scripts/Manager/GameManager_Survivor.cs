using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Survivor : Singleton<GameManager_Survivor>
{
    [Title("게임 관리")]
    public float MAX_GAMETIME;
    [ReadOnly] public float curGameTime;
    public int playerLvCount;

    [Title("플레이어 정보")]
    public Player player;
    public int maxHealth;
    public int level;
    public int killCount;
    public int maxExp;
    [ReadOnly] public int exp;
    [ReadOnly] public int health;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        curGameTime += Time.deltaTime;

        if(curGameTime > MAX_GAMETIME)
        {
            curGameTime = MAX_GAMETIME;
            // 클리어
        }    
    }

    public void Player_GainExp(int amount)
    {
        exp += amount;

        if(exp >= maxExp)
            Player_LevelUp();
    }
    private void Player_LevelUp()
    {
        UIManager um = UIManager.Instance;

        BtnManager.Instance.Tab(um.lvUpPannel);
        um.Set_StatUpPannels_Ran();

        do
        {
            playerLvCount++;
            level++;
            exp -= maxExp;
            maxExp *= (int)Mathf.Pow(level, 2);
        }
        while (exp >= maxExp);
    }
}