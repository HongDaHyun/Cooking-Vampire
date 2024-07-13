using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Survivor : Singleton<GameManager_Survivor>
{
    [Title("���� ����")]
    public float MAX_GAMETIME;
    [ReadOnly] public float curGameTime;

    [Title("�÷��̾� ����")]
    public Player player;
    public int level;
    public int killCount;
    public int maxExp;
    [ReadOnly] public int exp;

    private void Update()
    {
        curGameTime += Time.deltaTime;

        if(curGameTime > MAX_GAMETIME)
        {
            curGameTime = MAX_GAMETIME;
            // Ŭ����
        }    
    }

    public void Player_GetExp(int amount)
    {
        exp += amount;

        if(exp >= maxExp)
            Player_LevelUp();
    }
    private void Player_LevelUp()
    {
        while (exp >= maxExp)
        {
            level++;
            exp -= maxExp;
            maxExp *= (int)Mathf.Pow(level, 2);
        }
    }
}