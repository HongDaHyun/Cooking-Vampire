using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Vampire;

public class AtkController : MonoBehaviour
{
    public Transform[] typeTrans;
    [ReadOnly] public Atk[] availAtks;
    DataManager dataManager;
    UIManager uiManager;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        dataManager = DataManager.Instance;
        uiManager = UIManager.Instance;

        SetPivot();
    }

    private void Start()
    {
        availAtks = typeTrans[(int)player.data.type].GetComponentsInChildren<Atk>(true);
        EquipBasic();
    }

    private void SetPivot()
    {
        switch (dataManager.curPlayer)
        {
            case PlayerType.Knight:
                transform.localPosition = new Vector2(0.05f, 0.5f);
                break;
            case PlayerType.Archer:
                break;
            case PlayerType.Ninja:
                break;
            case PlayerType.Magician:
                break;
        }
    }

    private void EquipBasic()
    {
        LevelUpAtk(player.data.baseWeaponID);
    }

    public void LevelUpAtk(int ID)
    {
        Atk atk = Find_Atk(ID);

        if (atk.isMax)
            return;
        else if (atk.lv == Atk.MAX_LV)
            availAtks[Find_Atk_Index(ID)].SetMax();
        else if (atk.lv == 0)
        {
            availAtks[Find_Atk_Index(ID)].SetEquip(uiManager);
            StartCoroutine(AtkRoutine(atk));
        }
        else
            availAtks[Find_Atk_Index(ID)].LevelUp();
    }
    private IEnumerator AtkRoutine(Atk atk)
    {
        atk.gameObject.SetActive(true);

        while(!player.isDead)
        {
            yield return atk.Active();

            // activeTime이 0보다 작으면 무한 유지
            if (atk.stat.activeT != AtkStat.X)
            {
                yield return new WaitForSeconds(player.gm.stat.Cal_AT(atk.stat.activeT));
                atk.gameObject.SetActive(false);
            }

            if (atk.isPet)
                yield break;
            yield return new WaitForSeconds(player.gm.stat.Cal_AS(atk.stat.atkSpeed));
        }
    }

    private Atk Find_Atk(int ID)
    {
        return Array.Find(availAtks, weapon => weapon.ID == ID);
    }
    private int Find_Atk_Index(int ID)
    {
        return Array.FindIndex(availAtks, weapon => weapon.ID == ID);
    }
}