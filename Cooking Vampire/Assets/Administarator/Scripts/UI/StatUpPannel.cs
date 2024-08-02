using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpPannel : MonoBehaviour
{
    int id;
    public Image iconImg;
    public TextMeshProUGUI titleTxt, contentsTxt, levelTxt;

    GameManager_Survivor gm;
    WeaponController weaponController;
    UIManager uiManager;
    BtnManager btnManager;
    SpriteData spriteData;

    private void Awake()
    {
        gm = GameManager_Survivor.Instance;
        weaponController = gm.player.weaponController;
        uiManager = UIManager.Instance;
        btnManager = BtnManager.Instance;
        spriteData = SpriteData.Instance;
    }

    public void SetUI(Weapon weapon)
    {
        id = weapon.ID;

        iconImg.sprite = weapon.icon;

        titleTxt.text = weapon.title;
        contentsTxt.text = weapon.Export_LevelDiscription();
        levelTxt.color = spriteData.levelColor[weapon.lv];
        levelTxt.text = $"Lv.{weapon.lv}";
    }

    public void OnClick()
    {
        gm.playerLvCount--;
        weaponController.LevelUpWeapon(id);

        if (gm.playerLvCount <= 0)
        {
            btnManager.Tab(uiManager.lvUpPannel);
            btnManager.Resume();
        }
        else
            uiManager.Set_StatUpPannels_Ran();

    }
}
