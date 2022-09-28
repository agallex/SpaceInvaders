using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public float TimeShoot = 0.3f;
    public bool HaveBullet = true;
    public Coroutine CoroutinePlayerShoot;
    public GameObject MainCamera;
    public void ShootOnClick ()
    {
        if (HaveBullet && !MainCamera.GetComponent<SpawnEnemies>().isPause)
        {
            StartCoroutine(Shooting());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && HaveBullet && !GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().isPause)
        {
            CoroutinePlayerShoot = StartCoroutine(Shooting());
        }

        if (MainCamera.GetComponent<SpawnEnemies>().isRestart)
        {
            MainCamera.GetComponent<SpawnEnemies>().isRestart = false;
            if (CoroutinePlayerShoot != null)
            {
                StopCoroutine(CoroutinePlayerShoot);
                HaveBullet = true;
            }
        }
    }

    IEnumerator Shooting()
    {
        HaveBullet = false;
        Vector3 playerPos = transform.position;
        Vector3 spawnPos = new Vector3(playerPos.x, playerPos.y + 0.4f, playerPos.z);
        Instantiate(bullet, spawnPos, transform.rotation);
        yield return new WaitForSeconds(TimeShoot);
        HaveBullet = true;
    }
}
