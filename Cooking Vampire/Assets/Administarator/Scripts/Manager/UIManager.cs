using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;

public class UIManager : Singleton<UIManager>
{
    [Title("서바이벌")]
    public Slider expSlider;
    public Slider hpSlider;
    public TextMeshProUGUI levelTxt, killTxt, timeTxt;
    public TextMeshProUGUI weapon0_Btn, weapon1_Btn;
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
        float maxHp = gm.maxHealth;

        float targetHP = curHp / maxHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, targetHP, Time.deltaTime * 5f);

        // TEST_BTN
        weapon0_Btn.text = gm.player.weaponController.availWeapons[0].isMax ?
            "Weapon 0\nLv.MAX" : $"Weapon 0\nLv.{gm.player.weaponController.availWeapons[0].lv}";
        weapon1_Btn.text = gm.player.weaponController.availWeapons[1].isMax ?
            "Weapon 1\nLv.MAX" : $"Weapon 1\nLv.{gm.player.weaponController.availWeapons[1].lv}";
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

    public string Export_UpdateString(UpdateType type, float amount)
    {
        switch (type)
        {
            case UpdateType.Count:
                return $"투사체 수 {(int)amount} 증가";
            case UpdateType.CoolTime:
                return $"쿨타임 {amount}초 감소";
            case UpdateType.ActiveTime:
                return $"지속시간 {amount}초 증가";
            case UpdateType.Damage:
                return $"공격력 {(int)amount} 증가";
            case UpdateType.Speed:
                return $"투사체 속도 {(int)amount}% 증가";
            case UpdateType.Per:
                return $"관통력 {(int)amount} 증가";
            default:
                return "";
        }
    }
}
