using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public float TimeShoot = 0.4f;
    public bool HaveBullet = true;
    public GameObject MainCamera;
    public bool ShootButtonEnter = false;
    public Coroutine speedShoot;
    public void ShootButtonDown()
    {
        ShootButtonEnter = true;
    }
    
    public void ShootButtonUp()
    {
        ShootButtonEnter = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && HaveBullet && !MainCamera.GetComponent<SpawnEnemies>().isPause)
        {
            if (MainCamera.GetComponent<Music>().MusicIsPlay)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            StartCoroutine(Shooting());
        }
        
        if (ShootButtonEnter && HaveBullet && !MainCamera.GetComponent<SpawnEnemies>().isPause)
        {
            if (MainCamera.GetComponent<Music>().MusicIsPlay)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            StartCoroutine(Shooting());
        }

        if (ShootBonus.GetShootBonus)
        {
            ShootBonus.GetShootBonus = false;
            if (speedShoot != null)
            {
                StopCoroutine(speedShoot);
            }
            speedShoot = StartCoroutine(UpSpeedShoot());
        }
    }
    
    IEnumerator UpSpeedShoot()
    {
        TimeShoot = 0.2f;
        yield return new WaitForSeconds(5f);
        TimeShoot = 0.4f;
    }
    
    IEnumerator Shooting()
    {
        HaveBullet = false;
        Vector3 playerPos = transform.position;
        Vector3 spawnPos = new Vector3(playerPos.x, playerPos.y + 0.4f, playerPos.z);
        Instantiate(bullet, spawnPos, transform.rotation);
        yield return new WaitForSeconds(TimeShoot + TimeShoot / 10.0f / (TimeShoot + SpawnEnemies.Level / 20.0f) / 10.0f);
        HaveBullet = true;
    }
}
