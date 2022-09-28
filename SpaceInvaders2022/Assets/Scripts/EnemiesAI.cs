using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesAI : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            ++SpawnEnemies.Score;
            --SpawnEnemies.CountEnemy;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            SpawnEnemies.is_Live = -1;
        }
        
        if (other.gameObject.CompareTag(("RedZone")))
        {
            Destroy(gameObject);
        }
    }
}
