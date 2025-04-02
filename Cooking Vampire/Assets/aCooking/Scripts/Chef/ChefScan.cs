using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChefScan : MonoBehaviour
{
    public LayerMask objLayer;
    private RaycastHit2D[] targets;

    [ReadOnly] public IObj nearestObj;

    Chef chef;

    private void Awake()
    {
        chef = GetComponent<Chef>();
    }

    private void FixedUpdate()
    {
        float range = chef.gm.chefStat.RANGE;

        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, objLayer);
        UpdateNearest();
    }

    private void UpdateNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        // nearestObj가 범위 밖이면 null로 설정
        if (result != null && Vector3.Distance(transform.position, result.position) <= chef.gm.chefStat.RANGE)
        {
            nearestObj = result.GetComponent<IObj>();
        }
        else
        {
            nearestObj = null;
        }
    }
}
