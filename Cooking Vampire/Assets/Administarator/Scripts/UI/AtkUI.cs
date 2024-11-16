using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtkUI : MonoBehaviour
{
    public Image icon, lvBattery;

    public void SetUI(Sprite iconSprite, int level)
    {
        gameObject.SetActive(true);

        icon.sprite = iconSprite;
        SetBattery(level);
    }

    public void SetBattery(int level)
    {
        lvBattery.sprite = SpriteData.Instance.battery_Sprites[level - 1];
    }
}
