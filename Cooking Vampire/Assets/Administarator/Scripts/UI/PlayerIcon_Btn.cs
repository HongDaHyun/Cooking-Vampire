using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon_Btn : MonoBehaviour
{
    public Image bg, icon;

    SpawnManager sm;
    RectTransform rect;
    InfoTxtController controller;
    PlayerData playerData;

    private void Awake()
    {
        sm = SpawnManager.Instance;
        rect = GetComponent<RectTransform>();
        controller = GetComponent<InfoTxtController>();
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
        string contents = string.Join("\n", playerData.crystals.Select(crystal => crystal.Export_Explain()));

        sm.Spawn_InfoTxt(
            playerData.title,
            "Á÷¾÷",
            contents,
            rect,
            controller
            );
    }
}
