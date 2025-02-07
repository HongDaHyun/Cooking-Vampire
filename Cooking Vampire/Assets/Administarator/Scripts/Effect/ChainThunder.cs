using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class ChainThunder : MonoBehaviour, IPoolObject
{
    SpawnManager sm;

    public Enemy parentEnemy;
    public int amountToChain;

    public ParticleSystem particle;

    public void OnCreatedInPool()
    {
        sm = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetChainThunder(int _amountToChain, Enemy enemy)
    {
        parentEnemy = enemy;

        amountToChain = _amountToChain;
        Vector2 startPos = enemy.transform.position + new Vector3(0, 0.5f);
        transform.position = startPos;

        Shoot_NearEnemy();
    }

    private void Shoot_NearEnemy()
    {
        // 1. 주변 적 찾기
        List<Enemy> nearEnemies = parentEnemy.Find_NearEnemy(EleType.Thunder);
        amountToChain--;

        if (nearEnemies.Count == 0 || amountToChain == 0)
        {
            amountToChain = 0;
            StartCoroutine(ShootRoutine(parentEnemy, parentEnemy));
        }
        else
        {
            Enemy tarEnemy = nearEnemies[Random.Range(0, nearEnemies.Count)];
            StartCoroutine(ShootRoutine(parentEnemy, tarEnemy));
        }
    }

    private IEnumerator ShootRoutine(Enemy startEnemy, Enemy tarEnemy)
    {
        if(startEnemy == tarEnemy)
        {
            yield return new WaitForSeconds(particle.main.duration);
            sm.Destroy_ChainThunder(this);
            yield break;
        }

        float lerpTime = 0f;
        Vector2 startPos = startEnemy.transform.position;
        Vector2 tarPos = tarEnemy.transform.position;

        yield return new WaitForFixedUpdate();

        while (Vector2.Distance(transform.position, tarPos) > 0.001f)
        {
            // Lerp 시간 증가 및 클램핑
            lerpTime = Mathf.Clamp01(lerpTime + Time.fixedDeltaTime * 8f);
            // Lerp를 통한 위치 계산
            transform.position = Vector2.Lerp(startPos, tarPos, lerpTime);

            yield return new WaitForFixedUpdate();
        }

        // 마지막 위치 보정
        tarEnemy.EleRoutine(EleType.Thunder, amountToChain);
        transform.position = tarPos;
        sm.Spawn_ChainThunder(amountToChain, tarEnemy);
        sm.Destroy_ChainThunder(this);
    }
}
