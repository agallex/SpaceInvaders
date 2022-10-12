using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase.Auth;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class EnemiesAI : MonoBehaviour
{
    public GameObject HeartBonus;
    public GameObject ShootBonus;
    public GameObject SpeedBonus;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            ++SpawnEnemies.Score;
            --SpawnEnemies.CountEnemy;
            Random rnd = new Random();
            int digit = rnd.Next(0, 150);
            if (digit == 0)
            {
                var heartBonus = Instantiate(HeartBonus, transform.position, transform.rotation);
            }
            if (digit == 1 || digit == 2)
            {
                var heartBonus = Instantiate(ShootBonus, transform.position, transform.rotation);
            }
            if (digit == 3 || digit == 4)
            {
                var heartBonus = Instantiate(SpeedBonus, transform.position, transform.rotation);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().is_Live = -1;
        }
        
        if (other.gameObject.CompareTag(("RedZone")))
        {
            Destroy(gameObject);
        }
    }
}
