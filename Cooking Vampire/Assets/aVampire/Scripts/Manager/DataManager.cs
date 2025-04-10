using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using Vampire;

public class DataManager : Singleton<DataManager>
{
    [Title("저장 데이터")]
    public StageType curStage;
    public PlayerType curPlayer;
    public WeaponData curWeapon;
    public int coin;
    public List<int> ingredientInventory;

    [Title("정적 데이터")]
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public WeaponData[] weaponDatas;
    public DroptemData[] droptemDatas;
    public RelicData[] relicDatas;
    public IngredientData[] ingredientDatas;
    public List<CookItemData> cookItemLists;

    private void Start()
    {
        SetCookItem();
    }

    private void SetCookItem()
    {
        foreach(IngredientData data in ingredientDatas)
        {
            CookItemData item = ScriptableObject.CreateInstance<CookItemData>();
            item.Init(data.ID, data.sprites[2]);
            cookItemLists.Add(item);
        }
        int length = ingredientDatas.Length;
    }

    public void EarnCoin(int amount)
    {
        coin += amount;
    }

    #region 플레이어
    public PlayerData Export_PlayerData()
    {
        return Array.Find(playerDatas, data => data.type == curPlayer);
    }
    #endregion
    #region 적
    public EnemyData Export_EnemyData(string enemyName)
    {
        return Array.Find(enemyDatas, data => data.title == enemyName);
    }
    public EnemyData Export_BossData(StageType stage)
    {
        return Array.Find(enemyDatas, data => data.atkType == AtkType.Boss && Array.Exists(data.stage, x => x == stage));
    }
    #endregion
    #region 무기
    public WeaponData Export_WeaponData(PlayerType type, int tier)
    {
        return Array.Find(weaponDatas, data => data.weaponType == type && data.tier == tier);
    }
    #endregion
    #region 아이템
    public DroptemData Export_DroptemData(string _name)
    {
        return Array.Find(droptemDatas, data => data.droptemName == _name);
    }
    public DroptemData Export_DroptemData_Ran()
    {
        TierType ranTier = GameManager_Survivor.Instance.stat.Get_Tier();

        DroptemData[] dropArray = Array.FindAll(droptemDatas, data => data.tierType == ranTier);
        
        // 예외 처리
        if (dropArray.Length == 0 || dropArray == null)
            return droptemDatas[0];

        return dropArray[UnityEngine.Random.Range(0, dropArray.Length)];
    }
    #endregion
    #region 유물
    public RelicData Export_RelicData(int ID)
    {
        return Array.Find(relicDatas, data => data.ID == ID);
    }
    public ref RelicData Export_RelicData_Ref(int ID)
    {
        return ref relicDatas[Array.FindIndex(relicDatas, data => data.ID == ID)];
    }
    #endregion
    #region 재료
    public IngredientData Export_IngredientData(int ID)
    {
        return Array.Find(ingredientDatas, data => data.ID == ID);
    }
    public void Gain_Ingredient(int ID, int amount)
    {
        for (int i = 0; i < amount; i++)
            ingredientInventory.Add(ID);
    }
    public void Lose_Ingredient(int ID)
    {
        ingredientInventory.Remove(ID);
    }
    public void Count_Ingredient(int ID)
    {
        ingredientInventory.Count(x => x == ID);
    }
    #endregion
    #region 요리템
    public CookItemData Export_CookItemData(int ID)
    {
        return cookItemLists.Find(data => data.ID == ID);
    }
    #endregion

    #region ETC
    public string Get_Tier_Name(TierType tier)
    {
        switch(tier)
        {
            case TierType.Common:
                return "일반";
            case TierType.Rare:
                return "레어";
            case TierType.Epic:
                return "에픽";
            case TierType.Legend:
                return "레전드";
            default:
                return "";
        }
    }
    public bool Get_Ran(int percent)
    {
        int ranID = UnityEngine.Random.Range(1, 101);

        if (ranID <= percent)
            return true;
        else
            return false;
    }
    #endregion
}

public enum TierType { Common = 0, Rare, Epic, Legend }
public enum StageType { Grass = 0, Cave, Swarm, Forest }