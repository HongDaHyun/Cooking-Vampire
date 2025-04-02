using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float defRange;
    public LayerMask[] atkLayers;
    public LayerMask itemLayer;

    public RaycastHit2D[] targets;
    private RaycastHit2D[] itemTargets;

    public Transform nearestTarget;

    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        float range = player.gm.stat.Cal_RAN();

        int targetCombineds = 0;
        foreach (LayerMask layer in atkLayers)
            targetCombineds |= layer;

        targets = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, targetCombineds);
        itemTargets = Physics2D.CircleCastAll(transform.position, range / 3, Vector2.zero, 0, itemLayer);
        DrainItem();
        UpdateNearest();
    }

    private void UpdateNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        // 범위를 벗어나면 null로 설정
        if (result != null && Vector3.Distance(transform.position, result.position) <= player.gm.stat.Cal_RAN())
        {
            nearestTarget = result;
        }
        else
        {
            nearestTarget = null;
        }
    }

    public Transform Export_RanTarget()
    {
        if (targets == null)
            return null;
        if (targets.Length == 0)
            return null;
        return targets[Random.Range(0, targets.Length)].transform;
    }

    private void DrainItem()
    {
        foreach(RaycastHit2D target in itemTargets)
        {
            Item item = target.transform.GetComponent<Item>();

            if(item.isActive)
                item.isDrain = true;
        }
    }
}
