using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Redcode.Pools;

public class StatUI_Player : MonoBehaviour, IPoolObject
{
    GameManager_Survivor gm;
    SpawnManager sm;
    UIManager um;
    SpriteData spriteData;

    public Image statIcon;
    public TextMeshProUGUI nameTxt, valueTxt;
    public StatID_Player statID;

    string explain;
    int min, max;

    public void OnCreatedInPool()
    {
        gm = GameManager_Survivor.Instance;
        sm = SpawnManager.Instance;
        um = UIManager.Instance;
        spriteData = SpriteData.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(StatData_Player data)
    {
        statID = data.ID;

        statIcon.sprite = spriteData.Export_StatSprite_Player(data.ID);
        nameTxt.text = $"{(data.isPercent ? "% " : "")}{data.name}";

        min = data.min;
        max = data.max;
        explain = data.explanation;

        AdjustUI();
    }

    public void AdjustUI()
    {
        int value = gm.stat.GetStat(statID, false);
        int defValue = gm.stat.GetStat(statID, true);

        if (defValue <= max && defValue >= min)
        {
            valueTxt.color = spriteData.Export_SignColor(value);
            valueTxt.text = value.ToString();
        }
        else
        {
            valueTxt.color = spriteData.Export_SignColor(defValue);
            valueTxt.text = $"{value} | {defValue}";
        }
    }

    public void OnClick()
    {
        sm.Spawn_InfoTxt(
            "",
            "",
            string.Format(explain, gm.stat.GetStat(statID, false)),
            um.playerIcon.rect,
            um.playerIcon.controller);
    }
}
