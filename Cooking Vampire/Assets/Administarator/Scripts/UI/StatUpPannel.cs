using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpPannel : MonoBehaviour
{
    int id;
    public Image iconImg;
    public TextMeshProUGUI titleTxt, contentsTxt, levelTxt, classTxt;
    private BonusStat bonusStat;
    private bool isWeapon;

    GameManager_Survivor gm;
    WeaponController weaponController;
    UIManager uiManager;
    BtnManager btnManager;
    DataManager dataManager;
    SpriteData spriteData;

    private void Awake()
    {
        gm = GameManager_Survivor.Instance;
        weaponController = gm.player.weaponController;
        uiManager = UIManager.Instance;
        btnManager = BtnManager.Instance;
        spriteData = SpriteData.Instance;
        dataManager = DataManager.Instance;
    }

    public void SetUI(Weapon weapon)
    {
        isWeapon = true;
        id = weapon.ID;

        iconImg.sprite = weapon.icon;

        titleTxt.text = weapon.title;
        contentsTxt.text = weapon.Export_LevelDiscription();
        levelTxt.color = spriteData.pallates[weapon.lv].color;
        levelTxt.text = $"Lv.{weapon.lv}";
        classTxt.text = "¹«±â";
    }
    public void SetUI(int statID)
    {
        isWeapon = false;
        id = statID;
        TierType tier = gm.Get_Tier();

        bonusStat = new BonusStat();
        bonusStat.type = (StatType)id;
        bonusStat.Set_Amount(tier);

        iconImg.sprite = spriteData.statSprites[id];

        titleTxt.text = bonusStat.Get_Name();
        contentsTxt.text = $"{bonusStat.Get_Name()} {bonusStat.Get_Discription()}";
        levelTxt.color = spriteData.Export_TierColor(tier);
        levelTxt.text = dataManager.Get_Tier_Name(tier);
        classTxt.text = "½ºÅÈ";
    }

    public void OnClick()
    {
        gm.playerLvCount--;

        if (isWeapon)
            weaponController.LevelUpWeapon(id);
        else
            gm.Player_UpStat(bonusStat.type, (int)bonusStat.amount);

        if (gm.playerLvCount <= 0)
        {
            btnManager.Tab(uiManager.lvUpPannel);
            btnManager.Resume();
        }
        else
            uiManager.Set_StatUpPannels_Ran();

    }
}
