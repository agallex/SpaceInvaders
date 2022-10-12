using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBonus : MonoBehaviour
{
    public int speedBonus = 3;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().Heart < 3)
            {
                GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().Heart += 1;
                GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().TextHeart.text = "x" +
                    GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().Heart.ToString();
            }
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
