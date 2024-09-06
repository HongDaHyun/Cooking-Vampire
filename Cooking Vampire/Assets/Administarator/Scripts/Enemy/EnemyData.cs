using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Title("UI")]
    public string title;
    [TextArea] public string discription;

    [Title("ตฅภฬลอ")]
    public StageType stage;
    public float speed;
    public AtkType atkType;
    public EleType eleType;
    public RuntimeAnimatorController[] animators;

    public RuntimeAnimatorController Export_RanAnim()
    {
        return animators[Random.Range(0, animators.Length)];
    }
}

public enum AtkType { Normal, Range, Charge, Area }