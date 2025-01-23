using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class ChainThunder : MonoBehaviour, IPoolObject
{
    SpawnManager sm;

    public LayerMask enemyLayer;
    public int amountToChain;
    public List<Enemy> nearEnemies;

    TrailRenderer trail;
    ParticleSystem particle;

    public void OnCreatedInPool()
    {
        sm = SpawnManager.Instance;
        particle = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
    }

    public void OnGettingFromPool()
    {
        nearEnemies.Clear();
    }

    public void SetChainThunder(int _amountToChain, Vector2 pos)
    {
        amountToChain = _amountToChain;
        Vector2 startPos = new Vector2(pos.x, pos.y + 0.5f);
        transform.position = startPos;

        Shoot_NearEnemy();
    }

    private void Shoot_NearEnemy()
    {
        // 1. 주변 적 찾기
        RaycastHit2D[] enemiesInRange = Physics2D.CircleCastAll(transform.position, 5f, Vector2.zero, 0, enemyLayer);

        foreach (RaycastHit2D enemy in enemiesInRange)
        {
            Enemy nearEnemy = enemy.collider.GetComponent<Enemy>();

            if (nearEnemy.Find_EleHit(EleType.Thunder).amount == 0 && nearEnemy != null)
                nearEnemies.Add(nearEnemy);
        }

        if (nearEnemies.Count == 0)
            sm.Destroy_ChainThunder(this);
        else
        {
            Enemy tarEnemy = nearEnemies[Random.Range(0, nearEnemies.Count)];

            tarEnemy.EleRoutine(EleType.Thunder, 1);

            amountToChain--;

            StartCoroutine(ShootRoutine(transform.position, tarEnemy));
        }
    }

    private IEnumerator ShootRoutine(Vector2 startPos, Enemy tarEnemy)
    {
        float lerpTime = 0f;
        Vector2 tarPos = tarEnemy.transform.position;

        tarPos += Vector2.up * 0.5f;

        yield return new WaitForFixedUpdate();

        while (Vector2.Distance(transform.position, tarPos) > 0.001f)
        {
            // Lerp 시간 증가 및 클램핑
            lerpTime = Mathf.Clamp01(lerpTime + Time.fixedDeltaTime * 8f);
            // Lerp를 통한 위치 계산
            transform.position = Vector2.Lerp(startPos, tarPos, lerpTime);
            Debug.Log(transform.position);

            yield return new WaitForFixedUpdate();
        }

        // 마지막 위치 보정
        transform.position = tarPos;
        sm.Spawn_ChainThunder(amountToChain, tarEnemy.transform.position);
    }
}
