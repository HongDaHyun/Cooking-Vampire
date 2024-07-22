using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;

public class UIManager : Singleton<UIManager>
{
    [Title("공용")]
    public GameObject raycastPannel;

    [Title("서바이벌")]
    public Slider expSlider;
    public Slider hpSlider;
    public TextMeshProUGUI levelTxt, killTxt, timeTxt;
    public TextMeshProUGUI[] weaponTest_Btn;
    public RectTransform lvUpPannel;

    private void LateUpdate()
    {
        Update_HUD();
    }

    private void Update_HUD()
    {
        GameManager_Survivor gm = GameManager_Survivor.Instance;

        // EXP_SLIDER
        float cur = gm.exp;
        float max = gm.maxExp;

        float targetValue = cur / max;
        expSlider.value = Mathf.Lerp(expSlider.value, targetValue, Time.deltaTime * 5f);

        // LV_TXT
        levelTxt.text = $"Lv.{gm.level:F0}";

        // KILL_TXT
        killTxt.text = $"x{gm.killCount:F0}";

        // TIMER_TXT
        float remainTime = gm.MAX_GAMETIME - gm.curGameTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timeTxt.text = $"{min:D2}:{sec:D2}";

        // HP_SLIDER
        float curHp = gm.health;
        float maxHp = gm.stat.maxHealth;

        float targetHP = curHp / maxHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, targetHP, Time.deltaTime * 5f);

        // TEST_BTN
        for (int i = 0; i < weaponTest_Btn.Length; i++)
        {
            weaponTest_Btn[i].text = gm.player.weaponController.availWeapons[i].isMax ?
                $"Weapon {i}\nLv.MAX" : $"Weapon {i}\nLv.{gm.player.weaponController.availWeapons[i].lv}";
        }
    }

    public void Set_StatUpPannels_Ran()
    {
        StatUpPannel[] pannels = lvUpPannel.GetComponentsInChildren<StatUpPannel>(true);
        List<Weapon> weapons = GameManager_Survivor.Instance.player.weaponController.availWeapons.ToList().FindAll(data => !data.isMax);

        foreach (StatUpPannel pannel in pannels)
        {
            bool isWeapon = Random.Range(0, 2) == 0; // 0 -> Weapon / 1 -> Etc...
            isWeapon = true; // test용

            if (isWeapon)
            {
                if (weapons.Count == 0 || weapons == null)
                {
                    // StatUpPannel_Passive(pannel)
                    // return;
                }

                int ranIndex = Random.Range(0, weapons.Count);
                Weapon weapon = weapons[ranIndex];
                weapons.RemoveAt(ranIndex);

                pannel.SetUI(weapon);
            }
            else
            {
                // StatUpPannel_Passive(pannel);
            }
        }
    }
}
