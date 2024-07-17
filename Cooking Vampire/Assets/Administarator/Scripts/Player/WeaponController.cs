using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class WeaponController : MonoBehaviour
{
    [ReadOnly] public Weapon[] availWeapons;

    private void Awake()
    {
        availWeapons = GetComponentsInChildren<Weapon>(true);
    }

    public void LevelUpWeapon(int ID)
    {
        Weapon weapon = Find_Weapon(ID);

        if (weapon.isMax)
            return;

        if(weapon.lv == 0)
        {
            weapon.lv = 1;
            StartCoroutine(WeaponRoutine(weapon));

            return;
        }
        availWeapons[Find_Weapon_Index(ID)].LevelUp();
    }
    private IEnumerator WeaponRoutine(Weapon weapon)
    {
        weapon.gameObject.SetActive(true);

        while(true)
        {
            yield return weapon.Active();

            // activeTime이 0보다 작으면 무한 유지
            if (weapon.activeTime > 0f)
            {
                yield return new WaitForSeconds(weapon.activeTime);
                weapon.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(weapon.coolTime);
        }
    }

    private Weapon Find_Weapon(int ID)
    {
        return Array.Find(availWeapons, weapon => weapon.ID == ID);
    }
    private int Find_Weapon_Index(int ID)
    {
        return Array.FindIndex(availWeapons, weapon => weapon.ID == ID);
    }
}