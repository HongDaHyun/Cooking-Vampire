using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUP_Btn : UP_Btn
{
    public StatID_Player ID;
    public int amount;

    public void SetBtn(StatID_Player statID)
    {
        ID = statID;
        TierType tier = gm.Get_Tier();

        // 데이터 설정
        StatData_Player statData = cm.Find_StatData_Player(statID);
        amount = cm.Find_StatData_PlayerLvUp(statID, tier);

        // UI 설정
        SetUI(spriteData.Export_StatSprite_Player(ID),
            statData.name,
            cm.Find_StatData_ContentText(amount, statData.name, statData.isPercent),
            dm.Get_Tier_Name(tier),
            "스탯",
            spriteData.Export_TierColor(tier));
    }

    public override void OnClick()
    {
        gm.stat.playerLvCount--;

        gm.stat.SetStat(ID, amount);

        if (gm.stat.playerLvCount <= 0)
            um.lvUpPannel.Active_Atk();
        else
            um.lvUpPannel.Reroll_StatUPs(um.lvUpPannel.statUps);
    }
}
