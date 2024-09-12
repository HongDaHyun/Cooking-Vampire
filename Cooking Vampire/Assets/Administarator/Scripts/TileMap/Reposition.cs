using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Player player;
    Enemy enemy;

    private void Awake()
    {
        player = GameManager_Survivor.Instance.player;

        if (transform.CompareTag("Enemy"))
            enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        StartCoroutine(RePosRoutine());
    }

    private IEnumerator RePosRoutine()
    {
        while(true)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 myPos = transform.position;

            float dirX = playerPos.x - myPos.x;
            float dirY = playerPos.y - myPos.y;

            float diffX = Mathf.Abs(dirX);
            float diffY = Mathf.Abs(dirY);

            dirX = dirX > 0 ? 1 : -1;
            dirY = dirY > 0 ? 1 : -1;

            switch (transform.tag)
            {
                case "Ground":
                    if (diffX > 30)
                    {
                        transform.Translate(Vector3.right * dirX * 60);
                        GetComponent<TileMap>().ArrangeObj();
                    }
                    if (diffY > 30)
                    {
                        transform.Translate(Vector3.up * dirY * 60);
                        GetComponent<TileMap>().ArrangeObj();
                    }
                    break;
                case "Enemy":
                    if (enemy.isDead)
                        break;

                    if (diffX > 20)
                        transform.Translate(Vector3.right * dirX * 40 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
                    if (diffY > 20)
                        transform.Translate(Vector3.up * dirY * 40 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
                    break;
                case "Item":
                case "Item_Box":
                    if (diffX > 30 || diffY > 30)
                        SpawnManager.Instance.Destroy_Item(transform.GetComponent<Item>());
                    break;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
