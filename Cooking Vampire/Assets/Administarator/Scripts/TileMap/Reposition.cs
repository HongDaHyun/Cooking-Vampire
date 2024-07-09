using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GameManager_Survivor.Instance.player;
    }

    private void Start()
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
                        transform.Translate(Vector3.right * dirX * 60);
                    if (diffY > 30)
                        transform.Translate(Vector3.up * dirY * 60);
                    break;
                case "Enemy":
                    break;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
