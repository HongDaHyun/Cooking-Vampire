using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUI_Player : MonoBehaviour
{
    public Image statIcon;
    public TextMeshProUGUI nameTxt, valueTxt;
    public StatID_Player statID;
    
    public void SetUI(StatData_Player data)
    {
        statID = data.ID;

        SpriteData spriteData = SpriteData.Instance;

        statIcon.sprite = spriteData.Export_StatSprite_Player(data.ID);
        nameTxt.text = data.name;
        valueTxt.text = ""; // 후에 제작
    }

    public void AdjustUI(int value)
    {
        valueTxt.text = value.ToString();
    }
}
