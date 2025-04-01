using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Redcode.Pools;
using Vampire;

public class StatUI_Player : MonoBehaviour, IPoolObject
{
    GameManager_Survivor gm;
    SpawnManager sm;
    UIManager um;
    SpriteData spriteData;

    public Image statIcon;
    public TextMeshProUGUI nameTxt, valueTxt;

    StatData_Player data;
    string explainTxt;

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

    public void SetUI(StatData_Player _data)
    {
        data = _data;

        statIcon.sprite = spriteData.Export_StatSprite_Player(data.ID);
        nameTxt.text = $"{(data.isPercent ? "% " : "")}{data.name}";

        AdjustUI();
    }

    public void AdjustUI()
    {
        int value = gm.stat.GetStat(data.ID, false);
        int defValue = gm.stat.GetStat(data.ID, true);

        AdjustExplain(value);

        if (defValue <= data.max && defValue >= data.min)
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
    public void AdjustExplain(int value)
    {
        string colorTag = spriteData.Export_ColorTag(spriteData.Export_SignColor(value));
        string percent = data.isPercent ? "%" : "";

        switch (data.ID)
        {
            default:
                string value1 = $"<color={colorTag}>{value}{percent}</color>";
                explainTxt = string.Format(data.explanation, value1);
                break;
            case StatID_Player.HPREG:
                value1 = $"<color={colorTag}>{gm.stat.Cal_HPREG_Cool()}</color>";
                explainTxt = string.Format(data.explanation, value1);
                break;
        }
    }

    public void OnClick()
    {
        sm.Spawn_InfoTxt(
            "",
            "",
            explainTxt,
            um.playerIcon.rect,
            um.playerIcon.controller);
    }
}
