using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon_Btn : MonoBehaviour
{
    public Image bg, icon;

    PlayerData playerData;

    private void Awake()
    {
        SetUI();
    }

    public void SetUI()
    {
        playerData = GameManager_Survivor.Instance.player.data;

        Color playerColor = playerData.personalColor;
        playerColor.a = 0.7f;
        bg.color = playerColor;

        icon.sprite = playerData.icon;
    }

    public void OnClick()
    {

    }
}
