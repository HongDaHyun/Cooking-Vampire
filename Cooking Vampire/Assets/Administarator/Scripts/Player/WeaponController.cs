using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class WeaponController : MonoBehaviour
{
    [ReadOnly] public Weapon[] availWeapons;

    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();

        SetAvailWeapons();
    }
    private void SetAvailWeapons()
    {
        availWeapons = Array.FindAll(transform.GetComponentsInChildren<Weapon>(true), weapon => weapon.isAvail(player.type));
    }

    public void EquipWeapon(int index)
    {
        Weapon weapon = availWeapons[index];

        if (weapon.lv > 0)
            return;

        weapon.lv = 1;
        StartCoroutine(WeaponRoutine(weapon));
    }
    public void LevelUpWeapon(int index)
    {
        availWeapons[index].LevelUp();
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
}