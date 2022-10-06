using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyBullet : MonoBehaviour
{
    public int speedBullet = 7;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speedBullet * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (--GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().Heart == 0)
            {
                GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().is_Live = -1;
            }
            GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().TextHeart.text = "x" +
                GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().Heart.ToString();
        }

        if (other.gameObject.CompareTag(("RedZone")))
        {
            Destroy(gameObject);
        }
        
    }
}
