using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public int speedBullet = 10;
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speedBullet * Time.deltaTime, 0);
    }
}
