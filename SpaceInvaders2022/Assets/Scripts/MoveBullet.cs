using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public int speedBullet = 7;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speedBullet * Time.deltaTime);
    }
}
