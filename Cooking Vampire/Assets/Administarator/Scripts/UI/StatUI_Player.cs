using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Redcode.Pools;

public class StatUI_Player : MonoBehaviour, IPoolObject
{
    GameManager_Survivor gm;
    SpriteData spriteData;

    public Image statIcon;
    public TextMeshProUGUI nameTxt, valueTxt;
    public StatID_Player statID;

    public void OnCreatedInPool()
    {
        gm = GameManager_Survivor.Instance;
        spriteData = SpriteData.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(StatData_Player data)
    {
        statID = data.ID;

        SpriteData spriteData = SpriteData.Instance;

        statIcon.sprite = spriteData.Export_StatSprite_Player(data.ID);
        nameTxt.text = $"{(data.isPercent ? "% " : "")}{data.name}";
        AdjustUI();
    }

    public void AdjustUI()
    {
        int value = gm.stat.GetStat(statID);

        valueTxt.color = spriteData.Export_SignColor(value);
        valueTxt.text = gm.stat.GetStat(statID).ToString();
    }
}
