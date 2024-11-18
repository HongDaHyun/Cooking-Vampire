using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtkUP_Btn : UP_Btn
{
    public int ID;

    AtkController atkController;

    protected override void Awake()
    {
        base.Awake();
        atkController = gm.player.atkController;
    }

    public void SetBtn(Atk atk)
    {
        ID = atk.ID;

        SetUI(atk.icon,
            atk.title,
            GetContent(atk.atkStat_LvelUps[atk.lv].atkPerLevels),
            $"Lv.{atk.lv}",
            "¹«±â",
            spriteData.pallates[atk.lv].color);
    }

    private string GetContent(AtkStat_LevelUp[] atkStat_LevelUps)
    {
        string str = "";
        int length = atkStat_LevelUps.Length;

        for (int i = 0; i < length; i++)
        {
            AtkStat_LevelUp lvUp = atkStat_LevelUps[i];
            StatData_Atk atkData = cm.Find_StatData_Atk(lvUp.ID);
            str += GetContent(lvUp.amount, atkData.name, atkData.isPercent);

            if (i < length - 1)
                str += "\n";
        }

        return str;
    }

    public override void OnClick()
    {
        atkController.LevelUpAtk(ID);

        um.lvUpPannel.UnTab(bm);
    }
}
