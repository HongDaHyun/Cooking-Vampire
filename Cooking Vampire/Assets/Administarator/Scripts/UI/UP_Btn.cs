using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UP_Btn : MonoBehaviour
{
    public Image iconImg;
    public TextMeshProUGUI titleTxt, contentsTxt, levelTxt, classTxt;

    protected GameManager_Survivor gm;
    protected CSVManager cm;
    protected UIManager um;
    protected BtnManager bm;
    protected DataManager dm;
    protected SpriteData spriteData;

    protected virtual void Awake()
    {
        gm = GameManager_Survivor.Instance;
        cm = CSVManager.Instance;
        um = UIManager.Instance;
        bm = BtnManager.Instance;
        dm = DataManager.Instance;
        spriteData = SpriteData.Instance;
    }

    protected void SetUI(Sprite sprite, string title, string contents, string level, string classStr, Color lvColor)
    {
        iconImg.sprite = sprite;

        titleTxt.text = title;
        contentsTxt.text = contents;
        levelTxt.text = level;
        levelTxt.color = lvColor;
        classTxt.text = classStr;
    }

    public abstract void OnClick();
}
