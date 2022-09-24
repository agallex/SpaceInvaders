using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public float TimeShoot = 0.3f;
    private bool StartCoroutineShooting = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !StartCoroutineShooting)
        {
            StartCoroutine(Shooting());
        }
    }

    IEnumerator Shooting()
    {
        StartCoroutineShooting = true;
        Vector3 playerPos = transform.position;
        Vector3 spawnPos = new Vector3(playerPos.x, playerPos.y + 0.4f, playerPos.z);
        Instantiate(bullet, spawnPos, transform.rotation);
        yield return new WaitForSeconds(TimeShoot);
        StartCoroutineShooting = false;
    }
}
