using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public StageType stage;
    public int tier; // 4´Â º¸½º
    public float speed;
    public AtkType type;
    public RuntimeAnimatorController[] animators;

    public RuntimeAnimatorController Export_RanAnim()
    {
        return animators[Random.Range(0, animators.Length)];
    }
}

public enum AtkType { Normal, Range, Charge, Area }