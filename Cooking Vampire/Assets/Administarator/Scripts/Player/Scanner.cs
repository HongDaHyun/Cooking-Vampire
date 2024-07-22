using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask itemLayer;

    public RaycastHit2D[] targets;
    private RaycastHit2D[] itemTargets;

    public Transform nearestTarget;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, player.gm.stat.range, Vector2.zero, 0, enemyLayer);
        itemTargets = Physics2D.CircleCastAll(transform.position, player.gm.stat.range / 3, Vector2.zero, 0, itemLayer);
        DrainGem();
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

        nearestTarget = result;
    }

    public Transform Export_RanTarget()
    {
        if (targets == null)
            return null;
        if (targets.Length == 0)
            return null;
        return targets[Random.Range(0, targets.Length)].transform;
    }

    private void DrainGem()
    {
        foreach(RaycastHit2D target in itemTargets)
        {
            Item gem = target.transform.GetComponent<Item>();
            gem.isDrain = true;
        }
    }
}
