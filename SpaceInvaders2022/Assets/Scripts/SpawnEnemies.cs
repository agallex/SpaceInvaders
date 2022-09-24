using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;

    public static int Score = 0;

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString();
    }
}
