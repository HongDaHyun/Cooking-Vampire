using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int tier;
    public string weaponName;
    public PlayerType weaponType;
    public Sprite weaponSprite;
}
