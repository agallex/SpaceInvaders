using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    public int speedBonus = 3;
    static public bool GetSpeedBonus = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetSpeedBonus = true;
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag(("RedZone")))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speedBonus * Time.deltaTime);
    }
}
