using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class StatUpPannel : MonoBehaviour
{
    int id, amount; // amount 현재 스탯에만 사용중(후에 수정)
    public Image iconImg;
    public TextMeshProUGUI titleTxt, contentsTxt, levelTxt, classTxt;
    private bool isAtk;

    GameManager_Survivor gm;
    CSVManager cm;
    AtkController atkController;
    UIManager uiManager;
    BtnManager btnManager;
    DataManager dataManager;
    SpriteData spriteData;

    private void Awake()
    {
        gm = GameManager_Survivor.Instance;
        cm = CSVManager.Instance;
        atkController = gm.player.atkController;
        uiManager = UIManager.Instance;
        btnManager = BtnManager.Instance;
        spriteData = SpriteData.Instance;
        dataManager = DataManager.Instance;
    }

    public void SetUI(Atk atk)
    {
        isAtk = true;
        id = atk.ID;

        iconImg.sprite = atk.icon;

        titleTxt.text = atk.title;
        contentsTxt.text = GetContent(atk.atkStat_LvelUps[atk.lv].atkPerLevels);
        levelTxt.color = spriteData.pallates[atk.lv].color;
        levelTxt.text = $"Lv.{atk.lv}";
        classTxt.text = "무기";
    }
    public void SetUI(StatID_Player statID)
    {
        isAtk = false;
        id = (int)statID;
        TierType tier = gm.Get_Tier();

        StatData_Player statData = cm.Find_StatData_Player(statID);
        amount = cm.Find_StatData_PlayerLvUp(statID, tier);

        iconImg.sprite = spriteData.statSprites[id];

        titleTxt.text = statData.name;
        contentsTxt.text = GetContent(amount, statData.name);
        levelTxt.color = spriteData.Export_TierColor(tier);
        levelTxt.text = dataManager.Get_Tier_Name(tier);
        classTxt.text = "스탯";
    }

    private string GetContent(int amount, string statName)
    {
        return $"-> <color={spriteData.Export_ColorTag(spriteData.Export_SignColor(amount > 0))}>{amount}</color> {statName}";
    }
    private string GetContent(AtkStat_LevelUp[] atkStat_LevelUps)
    {
        string str = "";
        int length = atkStat_LevelUps.Length;

        for (int i = 0; i < length; i++)
        {
            AtkStat_LevelUp lvUp = atkStat_LevelUps[i];
            str += GetContent(lvUp.amount, cm.Find_StatData_Atk(lvUp.ID).name);

            if (i < length - 1)
                str += "\n";
        }

        return str;
    }

    public void OnClick()
    {
        gm.playerLvCount--;

        if (isAtk)
            atkController.LevelUpAtk(id);
        else
            gm.stat.SetStat((StatID_Player)id, amount);

        if (gm.playerLvCount <= 0)
        {
            btnManager.Tab(uiManager.lvUpPannel.transform);
            btnManager.Resume();
        }
        else
            uiManager.lvUpPannel.Set_StatUpPannels_Ran();

    }
}
