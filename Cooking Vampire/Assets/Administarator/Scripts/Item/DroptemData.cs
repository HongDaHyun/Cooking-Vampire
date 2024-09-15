using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroptemData", menuName = "DroptemData")]
public class DroptemData : ScriptableObject
{
    public TierType tierType;
    public string droptemName; // string으로 구별
    public RuntimeAnimatorController anim;
}